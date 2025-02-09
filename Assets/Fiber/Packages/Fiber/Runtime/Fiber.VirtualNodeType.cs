namespace Fiber
{
    public enum VirtualNodeType
    {
        CustomComponent = 0,
        Context = 1,
        Fragment = 2,
        EnableComponent = 3,
        VisibleComponent = 4,
        ActiveComponent = 5,
        MountComponent = 6,
        ForComponent = 7,
        SwitchComponent = 8,
        MatchComponent = 9,
        PortalComponent = 10,
        PortalDestinationComponent = 11,
        RendererComponentSpecialHandling = 12,
        Null = 13,
    }

    public static class VirtualNodeTypeExtensions
    {
        public static bool IsUsedByFiberAfterMount(this VirtualNodeType nodeType)
        {
            return nodeType == VirtualNodeType.VisibleComponent ||
                   nodeType == VirtualNodeType.PortalDestinationComponent ||
                   nodeType == VirtualNodeType.Context ||
                   nodeType == VirtualNodeType.EnableComponent ||
                   nodeType == VirtualNodeType.RendererComponentSpecialHandling;
        }

        public static bool IsBuiltInComponent(this VirtualNodeType nodeType)
        {
            return nodeType != VirtualNodeType.CustomComponent;
        }
    }
}