using System;
using System.Collections.Generic;
using Signals;

namespace Fiber.Router
{
    public static class BaseComponentExtensions
    {
        public static RouterProvider RouterProvider(
            this BaseComponent component,
            Router router
        )
        {
            return new RouterProvider(router);
        }

        public static OutletComponent Outlet(this BaseComponent component)
        {
            return new OutletComponent();
        }
    }

    [Serializable]
    public class Router : BaseSignal<Router>
    {
        public class ModalRoute
        {
            public string Id { get; private set; }
            public ModalRoute(string id)
            {
                Id = id;
            }
        }

        public class ModalRoute<C> : ModalRoute
        {
            public C Context { get; private set; }

            public ModalRoute(string id, C context) : base(id)
            {
                Context = context;
            }
        }

        [Serializable]
        public class Route : BaseSignal
        {
            public string Id;
            public bool IsLayoutRoute { get; private set; }
            public List<ModalRoute> Modals { get; private set; }

            public Route(string id, bool isLayoutRoute)
            {
                Id = id;
                IsLayoutRoute = isLayoutRoute;
                Modals = new();
            }

            public override bool IsDirty(byte otherDirtyBit)
            {
                return DirtyBit != otherDirtyBit;
            }

            public void PushModal(string id)
            {
                if (IsModalPushed(id))
                {
                    throw new Exception($"Modal with id {id} is already pushed");
                }
                // TODO: Pool routes
                Modals.Add(new ModalRoute(id));
                NotifySignalUpdate();
            }

            public void PushModal<C>(string id, C context)
            {
                if (IsModalPushed(id))
                {
                    throw new Exception($"Modal with id {id} is already pushed");
                }

                // TODO: Pool routes
                Modals.Add(new ModalRoute<C>(id, context));
                NotifySignalUpdate();
            }

            public void PopModal()
            {
                Modals.RemoveAt(Modals.Count - 1);
                NotifySignalUpdate();
            }

            public void PopModal(string id)
            {
                if (!IsModalPushed(id))
                {
                    throw new Exception($"Modal with id {id} is not pushed");
                }

                for (var i = 0; i < Modals.Count; i++)
                {
                    if (Modals[i].Id == id)
                    {
                        Modals.RemoveAt(i);
                        break;
                    }
                }
                NotifySignalUpdate();
            }

            public ModalRoute PeekModal()
            {
                return Modals.Count == 0 ? null : Modals[Modals.Count - 1];
            }

            public bool IsModalPushed(string id)
            {
                for (var i = 0; Modals != null && i < Modals.Count; i++)
                {
                    if (Modals[i].Id == id)
                    {
                        return true;
                    }
                }
                return false;
            }

            protected override sealed void OnNotifySignalUpdate()
            {
                _dirtyBit++;
            }
        }

        public class Route<C> : Route
        {
            public C Context { get; private set; }

            public Route(string path, C context) : base(path, isLayoutRoute: false)
            {
                Context = context;
            }
        }

        private class CurrentRouteSignal_Implementation : ComputedSignal<IList<Route>, Route>
        {
            public CurrentRouteSignal_Implementation(SignalList<Route> routeStack) : base(routeStack) { }
            protected override Route Compute(IList<Route> routeStack)
            {
                if (routeStack.Count == 0)
                {
                    return null;
                }
                var route = routeStack[routeStack.Count - 1];
                return route;
            }

            protected override bool ShouldSetDirty(Route newValue, Route previousValue) => newValue?.Id != previousValue?.Id;
        }

        private class CurrentModalSignal_Implementation : ComputedSignal<Route, ModalRoute>
        {
            public CurrentModalSignal_Implementation(BaseSignal<Route> currentRoute) : base(currentRoute) { }
            protected override ModalRoute Compute(Route currentRoute)
            {
                var modalRoute = currentRoute.PeekModal();
                return modalRoute;
            }

            protected override bool ShouldSetDirty(ModalRoute newValue, ModalRoute previousValue) => newValue?.Id != previousValue?.Id;
        }

        public RouteDefinition RouterTree { get; private set; }
        public SignalList<Route> RouteStack;
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

        public Router Navigate(string path)
        {
            var currentRoute = RouteStack.Count != 0 ? RouteStack[RouteStack.Count - 1] : null;
            while (currentRoute != null && !GetRouteDefinition(currentRoute.Id).HasNoneLayoutRouteDecedent(path))
            {
                RouteStack.RemoveAt(RouteStack.Count - 1);
                currentRoute = RouteStack.Count != 0 ? RouteStack[RouteStack.Count - 1] : null;
            }

            PushRoute(path);

            return this;
        }

        public Router Navigate<C>(string path, C context)
        {
            var currentRoute = RouteStack.Count != 0 ? RouteStack[RouteStack.Count - 1] : null;
            while (currentRoute != null && !GetRouteDefinition(currentRoute.Id).HasNoneLayoutRouteDecedent(path))
            {
                RouteStack.RemoveAt(RouteStack.Count - 1);
                currentRoute = RouteStack.Count != 0 ? RouteStack[RouteStack.Count - 1] : null;
            }

            PushRoute<C>(path, context);
            return this;
        }

        private RouteDefinition PeekRouteDefinition()
        {
            if (RouteStack.Count == 0)
            {
                return null;
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
                if (definition != null)
                {
                    return definition;
                }
            }

            return null;
        }

        private void PushIntermediateLayoutRoutes(string id)
        {
            var peekedRouteDefinition = PeekRouteDefinition();
            var startingRouteDefinition = peekedRouteDefinition ?? RouterTree;

            PushIntermediateLayoutRoutesRecursively(id, peekedRouteDefinition, RouteStack.Count, startingRouteDefinition);
        }

        private bool PushIntermediateLayoutRoutesRecursively(string id, RouteDefinition peekedRouteDefinition, int stackCountBefore, RouteDefinition currentDefinition)
        {
            if (currentDefinition == null)
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
                    if (peekedRouteDefinition == null || peekedRouteDefinition.Id != currentDefinition.Id)
                    {
                        // TODO: Pool routes
                        RouteStack.Insert(stackCountBefore, new Route(currentDefinition.Id, isLayoutRoute: true));
                    }
                    return true;
                }
            }

            return false;
        }

        private Router PushRoute(string path)
        {
            var topOfStackDefinition = PeekRouteDefinition() ?? RouterTree;
            if (!topOfStackDefinition.HasNoneLayoutRouteDecedent(path))
            {
                throw new Exception($"Route {path} is not a decedent of {topOfStackDefinition.Id}");
            }

            PushIntermediateLayoutRoutes(path);
            // TODO: Pool routes
            RouteStack.Add(new Route(path, isLayoutRoute: false));
            NotifySignalUpdate();
            return this;
        }

        private Router PushRoute<C>(string path, C context)
        {
            PushIntermediateLayoutRoutes(path);
            // TODO: Pool routes
            RouteStack.Add(new Route<C>(path, context));
            NotifySignalUpdate();
            return this;
        }

        private Router PopRoute()
        {
            if (RouteStack.Count == 0)
            {
                return this;
            }
            RouteStack.RemoveAt(RouteStack.Count - 1);

            // Pop intermediate layout routes
            while (RouteStack.Count > 0 && RouteStack[RouteStack.Count - 1].GetType() == typeof(Route))
            {
                RouteStack.RemoveAt(RouteStack.Count - 1);
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

        public Router PushModal(string id)
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
                        route.PushModal(id);
                        return this;
                    }
                }
            }

            return this;
        }

        public Router PushModal<C>(string id, C context)
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
                        route.PushModal<C>(id, context);
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
            return RouteStack.Count == 0 ? null : RouteStack[RouteStack.Count - 1];
        }

        public ModalRoute PeekModal()
        {
            for (var i = RouteStack.Count - 1; i >= 0; i--)
            {
                var route = RouteStack[i];
                if (route.Modals.Count > 0)
                {
                    return route.Modals[route.Modals.Count - 1];
                }
            }

            return null;
        }

        public override sealed bool IsDirty(byte otherDirtyBit)
        {
            return otherDirtyBit != DirtyBit;
        }
        public override Router Get() => this;

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
    }

    public abstract class BaseRouteComponent : BaseComponent
    {
        public BaseSignal<bool> ShowSignal { get; set; }
        public BaseRouteComponent() : base() { }
    }

    public class SimpleRouteComponent : BaseRouteComponent
    {
        public VirtualNode Component { get; private set; }
        public SimpleRouteComponent(VirtualNode component) : base()
        {
            Component = component;
        }

        public override VirtualBody Render()
        {
            return Mount(
                when: ShowSignal,
                children: Component
            );
        }
    }

    public class KeepMountedRouteComponent : BaseRouteComponent
    {
        public VirtualNode Component { get; private set; }
        public KeepMountedRouteComponent(VirtualNode component, string debug = null) : base()
        {
            Component = component;
        }
        private class HasBeenTrueOnceSignal : ComputedSignal<bool, bool>
        {
            private bool _hasBeenTrueOnce = false;
            public HasBeenTrueOnceSignal(BaseSignal<bool> baseSignal) : base(baseSignal) { }

            protected override bool Compute(bool currentValue)
            {
                if (_hasBeenTrueOnce)
                {
                    return true;
                }

                if (currentValue)
                {
                    _hasBeenTrueOnce = true;
                }

                return currentValue;
            }
        }
        public override VirtualBody Render()
        {
            var hasBeenTrueOnce = new HasBeenTrueOnceSignal(ShowSignal);
            return Mount(
                when: hasBeenTrueOnce,
                children: Active(ShowSignal, Component)
            );
        }
    }

    public class RouteDefinition
    {
        public string Id { get; private set; }
        // A layout route is a route that participates in the layout nesting.
        // However, we don't need to care about layout routes when we are 
        // navigating, since layout routes are automatically added and removed
        // from the route stack. Translated to the web, a layout route would
        // not add a segment to the path / URL.
        public bool IsLayoutRoute { get; private set; }
        public BaseRouteComponent Component { get; private set; }
        public Func<VirtualBody, BaseComponent> Wrapper { get; private set; }
        public List<RouteDefinition> Children { get; private set; }
        public List<ModalRouteDefinition> Modals { get; private set; }
        public RouteDefinition(
            string id,
            bool isLayoutRoute,
            BaseRouteComponent component,
            Func<VirtualBody, BaseComponent> wrapper = null,
            List<RouteDefinition> children = null,
            List<ModalRouteDefinition> modals = null
        )
        {
            Id = id;
            IsLayoutRoute = isLayoutRoute;
            Component = component;
            Wrapper = wrapper;
            Children = children;
            Modals = modals;
        }

        public virtual VirtualNode Render(BaseSignal<Router.Route> routeAtCurrentIndexSignal, BaseSignal<bool> isMatchSignal)
        {
            Component.ShowSignal = isMatchSignal;
            return Component;
        }

        // Checks if there is a none layout route as a decedent of this route.
        // A decedent is only considered if there are no none layout routes
        // in between in the router tree.
        public bool HasNoneLayoutRouteDecedent(string id)
        {
            for (var i = 0; Children != null && i < Children.Count; i++)
            {
                if (HasNoneLayoutRouteDecedent(id, Children[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasNoneLayoutRouteDecedent(string id, RouteDefinition definition)
        {
            var isLayoutRoute = definition.IsLayoutRoute;
            if (definition.Id == id && !isLayoutRoute)
            {
                return true;
            }
            else if (!isLayoutRoute)
            {
                return false;
            }

            for (var i = 0; definition.Children != null && i < definition.Children.Count; i++)
            {
                var childDefinition = definition.Children[i];
                if (HasNoneLayoutRouteDecedent(id, childDefinition))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class RouteDefinition<C> : RouteDefinition
    {
        public RouteDefinition(
            string id,
            BaseRouteComponent component,
            Func<VirtualBody, BaseComponent> wrapper = null,
            List<RouteDefinition> children = null,
            List<ModalRouteDefinition> modals = null
        ) : base(id, isLayoutRoute: false, component, wrapper, children, modals)
        {
        }


        protected class ContextSignal : ComputedSignal<Router.Route, C>
        {
            public ContextSignal(BaseSignal<Router.Route> routeAtCurrentIndexSignal)
                : base(routeAtCurrentIndexSignal)
            {
            }
            protected override C Compute(Router.Route baseRoute)
            {
                if (baseRoute == null)
                {
                    return default(C);
                }
                var route = baseRoute as Router.Route<C>;
                return route.Context;
            }
        }

        public override VirtualNode Render(BaseSignal<Router.Route> routeAtCurrentIndexSignal, BaseSignal<bool> isMatchSignal)
        {
            var contextSignal = new ContextSignal(routeAtCurrentIndexSignal);
            Component.ShowSignal = isMatchSignal;

            return new ContextProvider<BaseSignal<C>>(
                value: contextSignal,
                children: Component
            );
        }
    }

    public class ModalRouteDefinition
    {
        public string Id { get; private set; }
        public BaseRouteComponent Component { get; private set; }

        public ModalRouteDefinition(string id, BaseRouteComponent component)
        {
            Id = id;
            Component = component;
        }

        public virtual VirtualNode CreateContextProvider(Router.ModalRoute route, List<VirtualNode> children)
        {
            return null;
        }
    }

    public class ModalRouteDefinition<C> : ModalRouteDefinition
    {
        public ModalRouteDefinition(string id, BaseRouteComponent component) : base(id, component)
        {
        }

        public override VirtualNode CreateContextProvider(Router.ModalRoute baseModalRoute, List<VirtualNode> children)
        {
            var modalRoute = baseModalRoute as Router.ModalRoute<C>;
            return new Fiber.ContextProvider<C>(
                value: modalRoute.Context,
                children: children
            );
        }
    }

    public class OutletContext
    {
        public VirtualBody VirtualBody { get; set; }
    }

    public class OutletProvider : Component<VirtualBody>
    {
        public OutletProvider(VirtualBody virtualBody, VirtualBody children) : base(virtualBody, children) { }
        public override VirtualBody Render()
        {
            return ContextProvider(
                value: new OutletContext() { VirtualBody = Props },
                children: Children
            );
        }
    }

    public class OutletComponent : BaseComponent
    {
        public override VirtualBody Render()
        {
            var outletContext = GetContext<OutletContext>();
            return outletContext.VirtualBody;
        }
    }

    public class RouterProvider : BaseComponent
    {
        private RouteDefinition _routeDefinition;
        private Router _router;
        private int _currentStackIndex;


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

        private class RouteAtCurrentIndexSignal : ComputedSignal<Router, Router.Route>
        {
            private int _currentStackIndex;
            public RouteAtCurrentIndexSignal(Router routeStack, int currentStackIndex) : base(routeStack)
            {
                _currentStackIndex = currentStackIndex;
            }
            protected override Router.Route Compute(Router routeStack)
            {
                if (routeStack.RouteStack.Count <= _currentStackIndex)
                {
                    return null;
                }
                var route = routeStack.RouteStack[_currentStackIndex];
                return route;
            }
        }

        private class IsMatchSignal : ComputedSignal<Router.Route, bool>
        {
            private readonly RouteDefinition _routeDefinition;
            private readonly int _currentStackIndex;
            public IsMatchSignal(RouteDefinition routeDefinition, int currentStackIndex, RouteAtCurrentIndexSignal routeAtCurrentIndexSignal) : base(routeAtCurrentIndexSignal)
            {
                _routeDefinition = routeDefinition;
                _currentStackIndex = currentStackIndex;
            }
            protected override bool Compute(Router.Route routeAtCurrentIndex)
            {
                // Special case for the root layout route
                if (_currentStackIndex == 0 && _routeDefinition.IsLayoutRoute)
                {
                    return true;
                }
                else if (routeAtCurrentIndex == null)
                {
                    return false;
                }

                return _routeDefinition.Id == routeAtCurrentIndex.Id;
            }
        }
        private class ShowModalSignal : ComputedSignal<Router.Route, bool, bool>
        {
            private string _id;
            public ShowModalSignal(BaseSignal<Router.Route> routeSignal, BaseSignal<bool> isMatchSignal, string id) : base(routeSignal, isMatchSignal)
            {
                _id = id;
            }
            protected override bool Compute(Router.Route route, bool isMatch)
            {
                if (!isMatch || route == null)
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
            var children = Nodes(
                _routeDefinition.Render(routeSignal, isMatchSignal)
            );

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