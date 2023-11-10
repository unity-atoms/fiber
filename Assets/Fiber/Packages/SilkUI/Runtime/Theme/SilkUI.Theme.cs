using Signals;
using FiberUtils;

namespace SilkUI
{
    public static class THEME_CONSTANTS
    {
        public const string INHERIT = "inherit";
    }


    public class Theme : BaseSignal<Theme>, IGetName
    {
        public string Name;
        public string FallbackRole;
        public ColorTokenCollection Color;
        public TypographyTokens Typography;
        public SpacingTokens Spacing;
        public IconTokens Icon;
        public BreakpointTokens Breakpoints;

        public Theme(
            string name,
            string fallbackRole,
            ColorTokenCollection color,
            TypographyTokens typography,
            SpacingTokens spacing,
            IconTokens icon = null,
            BreakpointTokens breakpoints = null
        )
        {
            Name = name;
            FallbackRole = fallbackRole;
            Color = color;
            Color.RegisterDependent(this);
            Typography = typography;
            Typography.RegisterDependent(this);
            Spacing = spacing;
            Spacing.RegisterDependent(this);
            Icon = icon ?? new IconTokens();
            Icon.RegisterDependent(this);
            Breakpoints = breakpoints ?? new BreakpointTokens();
            Breakpoints.RegisterDependent(this);
        }
        ~Theme()
        {
            Color.UnregisterDependent(this);
            Typography.UnregisterDependent(this);
            Spacing.UnregisterDependent(this);
            Icon.UnregisterDependent(this);
            Breakpoints.UnregisterDependent(this);
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

        public string GetName() => Name;
    }
}