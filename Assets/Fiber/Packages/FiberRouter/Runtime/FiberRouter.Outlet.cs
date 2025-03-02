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
        private readonly bool _isPooled = false;
        public OutletComponent() : base()
        {
            _isPooled = true;
        }
        public OutletComponent(bool isPooled) : base()
        {
            _isPooled = isPooled;
        }
        public override VirtualBody Render()
        {
            var outletContext = GetContext<OutletContext>();
            return outletContext.VirtualBody;
        }

        public sealed override void Dispose()
        {
            if (_isPooled)
            {
                Pooling.OutletComponentPool.Release(this);
            }
        }
    }
}