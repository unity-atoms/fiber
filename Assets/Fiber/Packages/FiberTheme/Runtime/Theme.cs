using Signals;

namespace Fiber.Theme
{
    public class Theme : BaseSignal<Theme>
    {
        public string FallbackRole;
        public ColorTokenCollection Color;
        public TypographyTokens Typography;
        public SpacingTokens Spacing;
        public BreakpointsTokens Breakpoints;

        public Theme(
            string fallbackRole,
            ColorTokenCollection color,
            TypographyTokens typography,
            int spacingBaseline = 4,
            BreakpointsTokens breakpoints = null
        )
        {
            FallbackRole = fallbackRole;
            Color = color;
            Color.RegisterDependent(this);
            Typography = typography;
            Typography.RegisterDependent(this);
            Spacing = new SpacingTokens(spacingBaseline);
            Spacing.RegisterDependent(this);
            Breakpoints = breakpoints ?? new BreakpointsTokens();
            Breakpoints.RegisterDependent(this);
        }

        public override Theme Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }

        public ColorModifiers GetColorModifiers(string role, ElementType elementType, string variant = null)
        {
            role = Color.ContainsKey(role) ? role : FallbackRole;
            var element = Color[role].GetElement(elementType);
            var colorModifiers = variant != null && element.Variants.ContainsKey(variant) ? element.Variants[variant] : element;
            return colorModifiers;
        }
    }
}