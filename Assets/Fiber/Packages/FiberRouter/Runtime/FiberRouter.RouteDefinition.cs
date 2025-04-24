using System;
using System.Collections.Generic;

namespace Fiber.Router
{
    [Serializable]
    public struct RouteDefinition
    {
        public string Id;

        // A layout route is a route that participates in the layout nesting.
        // However, we don't need to care about layout routes when we are 
        // navigating, since layout routes are automatically added and removed
        // from the route stack. Translated to the web, a layout route would
        // not add a segment to the path / URL.
        public bool IsLayoutRoute;
        public BaseRouteComponent Component;
        public Func<VirtualBody, BaseComponent> Wrapper;
        public List<RouteDefinition> Children;
        public List<ModalRouteDefinition> Modals;
        public RouteDefinition(
            string id,
            bool isLayoutRoute,
            BaseRouteComponent component,
            Func<VirtualBody, BaseComponent> wrapper = default,
            List<RouteDefinition> children = default,
            List<ModalRouteDefinition> modals = default
        )
        {
            Id = id;
            IsLayoutRoute = isLayoutRoute;
            Component = component;
            Wrapper = wrapper;
            Children = children;
            Modals = modals;
        }

        // Checks if there is a none layout route as a decedent of this route.
        // A decedent is only considered if there are no none layout routes
        // in between in the router tree.
        public readonly bool HasNoneLayoutRouteDecedent(string id)
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

        public readonly bool IsEmpty()
        {
            return Id == default && IsLayoutRoute == default && Component == default;
        }

        public static RouteDefinition Empty() => new(default, default, default);
    }
}