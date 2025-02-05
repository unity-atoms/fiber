namespace Fiber.Router
{
    public struct ModalRouteDefinition
    {
        public string Id { get; private set; }
        public BaseRouteComponent Component { get; private set; }

        public ModalRouteDefinition(string id, BaseRouteComponent component)
        {
            Id = id;
            Component = component;
        }
    }
}