using Signals;

namespace Fiber.Router
{
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

    public class ActiveRouteComponent : BaseRouteComponent
    {
        public VirtualNode Component { get; private set; }
        public ActiveRouteComponent(VirtualNode component) : base()
        {
            Component = component;
        }
        public override VirtualBody Render()
        {
            return Active(ShowSignal, Component);
        }
    }

    public class KeepMountedRouteComponent : BaseRouteComponent
    {
        public VirtualNode Component { get; private set; }
        public KeepMountedRouteComponent(VirtualNode component) : base()
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

}