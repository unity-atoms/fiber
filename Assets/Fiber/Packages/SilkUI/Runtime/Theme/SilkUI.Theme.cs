using Signals;

namespace SilkUI
{
    public class Theme : BaseSignal<Theme>
    {
        public string FallbackRole;
        public ColorTokenCollection Color;
        public TypographyTokens Typography;
        public SpacingTokens Spacing;
        public IconTokens Icon;
        public BreakpointTokens Breakpoints;

        public Theme(
            string fallbackRole,
            ColorTokenCollection color,
            TypographyTokens typography,
            int spacingBaseline = 4,
            float borderWidthDefault = BorderWidthTokens.DEFAULT,
            float borderWidthThick = BorderWidthTokens.THICK,
            float borderWidthThin = BorderWidthTokens.THIN,
            float textOutlineDefault = TextOutlineTokens.DEFAULT,
            float textOutlineEmphasis = TextOutlineTokens.EMPHASIS,
            float textOutlineLight = TextOutlineTokens.LIGHT,
            IconTokens icon = null,
            BreakpointTokens breakpoints = null
        )
        {
            FallbackRole = fallbackRole;
            Color = color;
            Color.RegisterDependent(this);
            Typography = typography;
            Typography.RegisterDependent(this);
            Spacing = new SpacingTokens(
                baseline: spacingBaseline,
                borderWidthDefault: borderWidthDefault,
                borderWidthThick: borderWidthThick,
                borderWidthThin: borderWidthThin,
                textOutlineDefault: textOutlineDefault,
                textOutlineEmphasis: textOutlineEmphasis,
                textOutlineLight: textOutlineLight
            );
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
    }
}