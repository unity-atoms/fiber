using Signals;

namespace Fiber.UI
{
    // public class IsSmallSignal : ComputedSignal<BreakpointsTokens, ScreenSizeSignal, bool>
    // {
    //     public IsSmallSignal(BreakpointsTokens breakpointsTokens, ScreenSizeSignal screenSizeSignal)
    //         : base(breakpointsTokens, screenSizeSignal)
    //     {
    //         breakpointsTokens.Small.RegisterDependent(this);
    //     }

    //     protected override bool Compute(BreakpointsTokens breakpointsTokens, ScreenSize screenSize)
    //     {
    //         return breakpointsTokens.Small.Get() >= screenSize.DPWidth;
    //     }
    // }

    public class BreakpointsTokens : BaseSignal<BreakpointsTokens>
    {
        public Signal<float> Small;
        public Signal<float> Medium;
        public Signal<float> Large;
        public Signal<float> XL;

        // TODO: Computed signals for IsSmall, IsMedium, IsLarge, IsXL

        public BreakpointsTokens(
            float small = 640,
            float medium = 768,
            float large = 1024,
            float xl = 1280
        )
        {
            Small = new(small);
            Small.RegisterDependent(this);
            Medium = new(medium);
            Medium.RegisterDependent(this);
            Large = new(large);
            Large.RegisterDependent(this);
            XL = new(xl);
            XL.RegisterDependent(this);
        }

        public override BreakpointsTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
    }
}