using Signals;
using Fiber.UIElements;

namespace Fiber.UI
{
    public class IsSmallScreenSignal : ComputedSignal<BreakpointTokens, ScreenSize, bool>
    {
        public IsSmallScreenSignal(BreakpointTokens breakpointTokens, ScreenSizeSignal screenSizeSignal) : base(breakpointTokens, screenSizeSignal) { }
        protected override bool Compute(BreakpointTokens breakpointTokens, ScreenSize screenSize) => breakpointTokens.Small.Get() <= screenSize.DPWidth;
    }
    public class IsMediumScreenSignal : ComputedSignal<BreakpointTokens, ScreenSize, bool>
    {
        public IsMediumScreenSignal(BreakpointTokens breakpointTokens, ScreenSizeSignal screenSizeSignal) : base(breakpointTokens, screenSizeSignal) { }
        protected override bool Compute(BreakpointTokens breakpointTokens, ScreenSize screenSize) => breakpointTokens.Medium.Get() <= screenSize.DPWidth;
    }
    public class IsLargeScreenSignal : ComputedSignal<BreakpointTokens, ScreenSize, bool>
    {
        public IsLargeScreenSignal(BreakpointTokens breakpointTokens, ScreenSizeSignal screenSizeSignal) : base(breakpointTokens, screenSizeSignal) { }
        protected override bool Compute(BreakpointTokens breakpointTokens, ScreenSize screenSize) => breakpointTokens.Large.Get() <= screenSize.DPWidth;
    }
    public class IsXLScreenSignal : ComputedSignal<BreakpointTokens, ScreenSize, bool>
    {
        public IsXLScreenSignal(BreakpointTokens breakpointTokens, ScreenSizeSignal screenSizeSignal) : base(breakpointTokens, screenSizeSignal) { }
        protected override bool Compute(BreakpointTokens breakpointTokens, ScreenSize screenSize) => breakpointTokens.XL.Get() <= screenSize.DPWidth;
    }

    public enum Breakpoint
    {
        Base = 0,
        Small = 1,
        Medium = 2,
        Large = 3,
        XL = 4
    }
    public class BreakpointSignal : ComputedSignal<BreakpointTokens, ScreenSize, Breakpoint>
    {
        public BreakpointSignal(BreakpointTokens breakpointTokens, ScreenSizeSignal screenSizeSignal) : base(breakpointTokens, screenSizeSignal) { }
        protected override Breakpoint Compute(BreakpointTokens breakpointTokens, ScreenSize screenSize)
        {
            if (breakpointTokens.XL.Get() <= screenSize.DPWidth)
            {
                return Breakpoint.XL;
            }
            else if (breakpointTokens.Large.Get() <= screenSize.DPWidth)
            {
                return Breakpoint.Large;
            }
            else if (breakpointTokens.Medium.Get() <= screenSize.DPWidth)
            {
                return Breakpoint.Medium;
            }
            else if (breakpointTokens.Small.Get() <= screenSize.DPWidth)
            {
                return Breakpoint.Small;
            }
            return Breakpoint.Base;
        }
    }

    public struct ResponsiveProp<T>
    {
        public T Base;
        public T Small;
        public T Medium;
        public T Large;
        public T XL;

        public ResponsiveProp(T @base, T small, T medium, T large, T xl)
        {
            Base = @base;
            Small = small;
            Medium = medium;
            Large = large;
            XL = xl;
        }

        public readonly T Get(Breakpoint breakpoint)
        {
            return breakpoint switch
            {
                Breakpoint.Base => Base,
                Breakpoint.Small => Small,
                Breakpoint.Medium => Medium,
                Breakpoint.Large => Large,
                Breakpoint.XL => XL,
                _ => throw new System.Exception("Invalid breakpoint")
            };
        }
    }

    public class ResponsiveSignal<T> : ComputedSignal<Breakpoint, T>
    {
        private readonly ResponsiveProp<T> _responsiveProp;
        public ResponsiveSignal(BreakpointSignal breakpointSignal, ResponsiveProp<T> responsiveProp) : base(breakpointSignal)
        {
            _responsiveProp = responsiveProp;
        }
        protected override T Compute(Breakpoint breakpoint)
        {
            return _responsiveProp.Get(breakpoint);
        }
    }

    public class BreakpointTokens : BaseSignal<BreakpointTokens>
    {
        public Signal<float> Small;
        public Signal<float> Medium;
        public Signal<float> Large;
        public Signal<float> XL;

        public BreakpointTokens(
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

        public override BreakpointTokens Get() => this;
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