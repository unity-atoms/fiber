using Signals;
using FiberUtils;

namespace SilkUI
{
    public static class THEME_CONSTANTS
    {
        public const string INHERIT = "inherit";
    }


    public class Theme : BaseSignal<Theme>
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

        public ColorModifiers GetColorModifiers(string role, ElementType elementType, string subRole, string variant = null)
        {
            Element element = null;
            if (Color.ContainsKey(role))
            {
                // Prefer sub role if it exists
                if (subRole != null && Color[role].SubRoles.ContainsKey(subRole) && !Color[role].SubRoles[subRole].IsElementEmpty(elementType))
                {
                    element = Color[role].SubRoles[subRole].GetElement(elementType);
                }

                // Fallback to role if sub role doesn't exist or element is empty for the sub role
                if (element == null)
                {
                    element = Color[role].GetElement(elementType);
                }
            }

            // If no element was found, fallback to the fallback role
            if (element == null)
            {
                element = Color[FallbackRole].GetElement(elementType);
            }

            // If a variant isn't present, fallback to the default element colors
            var colorModifiers = variant != null && element.Variants.ContainsKey(variant) ? element.Variants[variant] : element;
            return colorModifiers;
        }
    }
}