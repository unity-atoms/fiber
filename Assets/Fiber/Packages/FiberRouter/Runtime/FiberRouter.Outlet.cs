using FiberUtils;

namespace Fiber.Router
{
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
        public static ObjectPool<OutletComponent> Pool { get; private set; } = new(InitialCapacityConstants.XS, null, preload: false);

        public override VirtualBody Render()
        {
            var outletContext = GetContext<OutletContext>();
            return outletContext.VirtualBody;
        }

        public sealed override void Dispose()
        {
            Pool.TryRelease(this);
        }
    }
}