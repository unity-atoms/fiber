using System;
using System.Collections.Generic;
using Signals;
using FiberUtils;

namespace Fiber.Router
{
    public static class Pooling
    {
        public static ObjectPool<OutletComponent> OutletComponentPool { get; private set; } = new(InitialCapacityConstants.XS, null, preload: false);
        public static ListPool<ModalRoute> ModalRouteListPool { get; private set; } = new(InitialCapacityConstants.XS, preload: false);
    }

    public static class BaseComponentExtensions
    {
        public static RouterProvider RouterProvider(
            this BaseComponent component,
            Router router
        )
        {
            // No need to pool RouterProvider since it's a one-time use component
            return new RouterProvider(router);
        }

        public static OutletComponent Outlet(this BaseComponent component)
        {
            return Pooling.OutletComponentPool.Get();
        }

        private static Dictionary<string, BaseSignal<bool>> _isRouteSignals = new();
        public static BaseSignal<bool> GetIsRouteSignal(this BaseComponent component, string id)
        {
            if (_isRouteSignals.ContainsKey(id))
            {
                return _isRouteSignals[id];
            }

            var router = component.C<Router>();
            var isRouteSignal = component.F.CreateComputedSignal((currentRoute) =>
            {
                return currentRoute.Id == id;
            }, router.CurrentRouteSignal);

            _isRouteSignals.Add(id, isRouteSignal);

            return isRouteSignal;
        }
    }

    [Serializable]
    public class Router : BaseSignal<Router>
    {
        private class CurrentRouteSignal_Implementation : ComputedSignal<IList<Route>, Route>
        {
            public CurrentRouteSignal_Implementation(ShallowSignalList<Route> routeStack) : base(routeStack) { }
            protected override Route Compute(IList<Route> routeStack)
            {
                if (routeStack.Count == 0)
                {
                    return Route.Empty();
                }
                var route = routeStack[routeStack.Count - 1];
                return route;
            }
        }

        private class CurrentModalSignal_Implementation : ComputedSignal<Route, ModalRoute>
        {
            public CurrentModalSignal_Implementation(BaseSignal<Route> currentRoute) : base(currentRoute) { }
            protected override ModalRoute Compute(Route currentRoute)
            {
                var modalRoute = currentRoute.PeekModal();
                return modalRoute;
            }
        }

        public RouteDefinition RouterTree { get; private set; }
        public ShallowSignalList<Route> RouteStack;
        public BaseSignal<Route> CurrentRouteSignal;
        public BaseSignal<ModalRoute> CurrentModalSignal;
        private readonly ISignal _parent;

        public Router(RouteDefinition routerTree, ISignal parent = null)
        {
            RouterTree = routerTree;
            RouteStack = new(5, this);
            if (parent != null)
            {
                _parent = parent;
                RegisterDependent(parent);
            }
            CurrentRouteSignal = new CurrentRouteSignal_Implementation(RouteStack);
            CurrentModalSignal = new CurrentModalSignal_Implementation(CurrentRouteSignal);
        }

        ~Router()
        {
            UnregisterDependent(_parent);
        }

        public Router Navigate(string path, string stringValue = default, int intValue = default)
        {
            var currentRoute = RouteStack.Count != 0 ? RouteStack[RouteStack.Count - 1] : Route.Empty();
            while (!currentRoute.IsEmpty() && !GetRouteDefinition(currentRoute.Id).HasNoneLayoutRouteDecedent(path))
            {
                RouteStack.RemoveAt(RouteStack.Count - 1);
                currentRoute = RouteStack.Count != 0 ? RouteStack[RouteStack.Count - 1] : Route.Empty();
            }

            PushRoute(path, stringValue, intValue);

            return this;
        }

        private RouteDefinition PeekRouteDefinition()
        {
            if (RouteStack.Count == 0)
            {
                return RouteDefinition.Empty();
            }

            return GetRouteDefinition(RouteStack[RouteStack.Count - 1].Id);
        }

        public RouteDefinition GetRouteDefinition(string id)
        {
            return GetRouteDefinitionRecursively(id, RouterTree);
        }

        private RouteDefinition GetRouteDefinitionRecursively(string id, RouteDefinition currentDefinition)
        {
            if (currentDefinition.Id == id)
            {
                return currentDefinition;
            }

            for (var i = 0; currentDefinition.Children != null && i < currentDefinition.Children.Count; i++)
            {
                var definition = GetRouteDefinitionRecursively(id, currentDefinition.Children[i]);
                if (!definition.IsEmpty())
                {
                    return definition;
                }
            }

            return RouteDefinition.Empty();
        }

        private void PushIntermediateLayoutRoutes(string id)
        {
            var peekedRouteDefinition = PeekRouteDefinition();
            var startingRouteDefinition = peekedRouteDefinition.IsEmpty() ? RouterTree : peekedRouteDefinition;

            PushIntermediateLayoutRoutesRecursively(id, peekedRouteDefinition, RouteStack.Count, startingRouteDefinition);
        }

        private bool PushIntermediateLayoutRoutesRecursively(string id, RouteDefinition peekedRouteDefinition, int stackCountBefore, RouteDefinition currentDefinition)
        {
            if (currentDefinition.IsEmpty())
            {
                return false;
            }

            if (currentDefinition.Id == id)
            {
                return true;
            }

            for (var i = 0; currentDefinition.Children != null && i < currentDefinition.Children.Count; i++)
            {
                var child = currentDefinition.Children[i];
                if (PushIntermediateLayoutRoutesRecursively(id, peekedRouteDefinition, stackCountBefore, child))
                {
                    if (peekedRouteDefinition.IsEmpty() || peekedRouteDefinition.Id != currentDefinition.Id)
                    {
                        RouteStack.Insert(stackCountBefore, new Route(currentDefinition.Id, isLayoutRoute: true, modals: Pooling.ModalRouteListPool.Get()));
                    }
                    return true;
                }
            }

            return false;
        }

        private Router PushRoute(string id, string stringValue = default, int intValue = default)
        {
            var peekedRouteDefinition = PeekRouteDefinition();
            var topOfStackDefinition = peekedRouteDefinition.IsEmpty() ? RouterTree : peekedRouteDefinition;
            if (!topOfStackDefinition.HasNoneLayoutRouteDecedent(id))
            {
                throw new Exception($"Route {id} is not a decedent of {topOfStackDefinition.Id}");
            }

            PushIntermediateLayoutRoutes(id);
            RouteStack.Add(new Route(id, isLayoutRoute: false, modals: Pooling.ModalRouteListPool.Get(), stringValue, intValue));
            NotifySignalUpdate();
            return this;
        }


        private Router PopRoute()
        {
            if (RouteStack.Count == 0)
            {
                return this;
            }

            var index = RouteStack.Count - 1;
            var route = RouteStack[index];
            Pooling.ModalRouteListPool.Release(route.Modals);
            RouteStack.RemoveAt(index);

            // Pop intermediate layout routes
            while (RouteStack.Count > 0 && RouteStack[RouteStack.Count - 1].IsLayoutRoute)
            {
                index = RouteStack.Count - 1;
                route = RouteStack[index];
                Pooling.ModalRouteListPool.Release(route.Modals);
                RouteStack.RemoveAt(index);
            }

            NotifySignalUpdate();
            return this;
        }

        private Router PopRoute(string backToPath)
        {
            while (RouteStack.Count > 0 && RouteStack[RouteStack.Count - 1].Id != backToPath)
            {
                PopRoute();
            }

            return this;
        }

        public Router PushModal(string id, string stringValue = default, int intValue = default)
        {
            for (var i = RouteStack.Count - 1; i >= 0; i--)
            {
                var route = RouteStack[i];
                var definition = GetRouteDefinition(route.Id);

                for (var j = 0; definition.Modals != null && j < definition.Modals.Count; ++j)
                {
                    var modalDefinition = definition.Modals[j];
                    if (modalDefinition.Id == id)
                    {
                        route.PushModal(id, stringValue, intValue);
                        return this;
                    }
                }
            }

            return this;
        }

        public Router PopModal(string id)
        {
            for (var i = RouteStack.Count - 1; i >= 0; i--)
            {
                var route = RouteStack[i];
                var definition = GetRouteDefinition(route.Id);

                for (var j = 0; definition.Modals != null && j < definition.Modals.Count; ++j)
                {
                    var modalDefinition = definition.Modals[j];
                    if (modalDefinition.Id == id)
                    {
                        route.PopModal(id);
                        return this;
                    }
                }
            }

            return this;
        }

        public bool IsModalShowing(string id)
        {
            for (var i = RouteStack.Count - 1; i >= 0; i--)
            {
                var route = RouteStack[i];

                if (route.IsModalPushed(id))
                {
                    return true;
                }
            }

            return false;
        }

        public Route PeekRoute()
        {
            // Top route will always be a none layout route
            return RouteStack.Count == 0 ? Route.Empty() : RouteStack[RouteStack.Count - 1];
        }

        public ModalRoute PeekModal()
        {
            for (var i = RouteStack.Count - 1; i >= 0; i--)
            {
                var route = RouteStack[i];
                if (route.Modals.Count > 0)
                {
                    return route.Modals[^1];
                }
            }

            return ModalRoute.Empty();
        }

        public override Router Get() => this;
    }

    public class RouterProvider : BaseComponent
    {
        private RouteDefinition _routeDefinition;
        private readonly Router _router;
        private readonly int _currentStackIndex;


        public RouterProvider(Router router) : base()
        {
            if (!router.RouterTree.IsLayoutRoute)
            {
                throw new ArgumentException("The root route must be a layout route.");
            }

            _routeDefinition = router.RouterTree;
            _router = router;
            _currentStackIndex = 0;
        }

        private RouterProvider(RouteDefinition routeDefinition, Router router, int currentStackIndex) : base()
        {
            _routeDefinition = routeDefinition;
            _router = router;
            _currentStackIndex = currentStackIndex;
        }

        private class RouteAtCurrentIndexSignal : ComputedSignal<Router, Route>
        {
            private int _currentStackIndex;
            public RouteAtCurrentIndexSignal(Router router, int currentStackIndex) : base(router)
            {
                _currentStackIndex = currentStackIndex;
            }
            protected override Route Compute(Router router)
            {
                if (router.RouteStack.Count <= _currentStackIndex)
                {
                    return Route.Empty();
                }
                var route = router.RouteStack[_currentStackIndex];
                return route;
            }
        }

        private class IsMatchSignal : ComputedSignal<Route, bool>
        {
            private readonly RouteDefinition _routeDefinition;
            private readonly int _currentStackIndex;
            public IsMatchSignal(RouteDefinition routeDefinition, int currentStackIndex, RouteAtCurrentIndexSignal routeAtCurrentIndexSignal) : base(routeAtCurrentIndexSignal)
            {
                _routeDefinition = routeDefinition;
                _currentStackIndex = currentStackIndex;
            }
            protected override bool Compute(Route routeAtCurrentIndex)
            {
                // Special case for the root layout route
                if (_currentStackIndex == 0 && _routeDefinition.IsLayoutRoute)
                {
                    return true;
                }
                else if (routeAtCurrentIndex.IsEmpty())
                {
                    return false;
                }

                return _routeDefinition.Id == routeAtCurrentIndex.Id;
            }
        }
        private class ShowModalSignal : ComputedSignal<Route, bool, bool>
        {
            private string _id;
            public ShowModalSignal(BaseSignal<Route> routeSignal, BaseSignal<bool> isMatchSignal, string id) : base(routeSignal, isMatchSignal)
            {
                _id = id;
            }
            protected override bool Compute(Route route, bool isMatch)
            {
                if (!isMatch || route.IsEmpty())
                {
                    return false;
                }

                for (var i = 0; i < route.Modals.Count; ++i)
                {
                    var modal = route.Modals[i];
                    if (modal.Id == _id)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public override VirtualBody Render()
        {
            // Render component
            var routeSignal = new RouteAtCurrentIndexSignal(_router, _currentStackIndex);
            var isMatchSignal = new IsMatchSignal(_routeDefinition, _currentStackIndex, routeSignal);

            var component = _routeDefinition.Component;
            component.ShowSignal = isMatchSignal;
            var children = Nodes(component);

            // Render subpaths
            var subPaths = Nodes();
            for (var i = 0; _routeDefinition.Children != null && i < _routeDefinition.Children.Count; ++i)
            {
                var childDefinition = _routeDefinition.Children[i];
                var childRouterProvider = new RouterProvider(childDefinition, _router, currentStackIndex: _currentStackIndex + 1);
                subPaths.Add(childRouterProvider);
            }

            // Render modals
            for (var i = 0; _routeDefinition.Modals != null && i < _routeDefinition.Modals.Count; ++i)
            {
                var modal = _routeDefinition.Modals[i];
                var showModalSignal = new ShowModalSignal(routeSignal, isMatchSignal, modal.Id);

                var modalComponent = modal.Component;
                modalComponent.ShowSignal = showModalSignal;

                children.Add(modalComponent);
            }

            var outletProvider = new OutletProvider(
                virtualBody: subPaths,
                children: children
            );

            return ContextProvider(
                value: _router,
                children: _routeDefinition.Wrapper != null ?
                    _routeDefinition.Wrapper.Invoke(outletProvider) : outletProvider
            );
        }
    }
}