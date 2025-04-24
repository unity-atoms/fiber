using System;

namespace Fiber.Router
{
    [Serializable]
    public struct ModalRouteDefinition
    {
        public string Id;
        public BaseRouteComponent Component;

        public ModalRouteDefinition(string id, BaseRouteComponent component)
        {
            Id = id;
            Component = component;
        }
    }
}