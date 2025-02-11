using Signals;

namespace SilkUI
{
    public class SpacingTokens : BaseSignal<SpacingTokens>
    {
        public const int DEFAULT_BASELINE = 4;
        public Signal<int> Baseline { get; private set; }
        public BorderWidthTokens BorderWidth { get; private set; }
        public TextOutlineTokens TextOutline { get; private set; }

        public SpacingTokens(
            int baseline = DEFAULT_BASELINE,
            float borderWidthDefault = BorderWidthTokens.DEFAULT,
            float borderWidthThin = BorderWidthTokens.THIN,
            float borderWidthThick = BorderWidthTokens.THICK,
            float borderWidthThicker = BorderWidthTokens.THICKER,
            float borderWidthThickest = BorderWidthTokens.THICKEST,
            float textOutlineDefault = TextOutlineTokens.DEFAULT,
            float textOutlineEmphasis = TextOutlineTokens.EMPHASIS,
            float textOutlineLight = TextOutlineTokens.LIGHT
        )
        {
            Baseline = new(baseline);
            Baseline.RegisterDependent(this);
            BorderWidth = new(@default: borderWidthDefault, thin: borderWidthThin, thick: borderWidthThick, thicker: borderWidthThicker, thickest: borderWidthThickest);
            BorderWidth.RegisterDependent(this);
            TextOutline = new(textOutlineDefault, textOutlineEmphasis, textOutlineLight);
            TextOutline.RegisterDependent(this);
        }
        ~SpacingTokens()
        {
            Baseline.UnregisterDependent(this);
        }

        public override SpacingTokens Get() => this;
    }

    public enum BorderWidthType
    {
        Default = 0,
        Thin = 1,
        Thick = 2,
        Thicker = 3,
        Thickest = 4,
        None = -1
    }
    public class BorderWidthTokens : BaseSignal<BorderWidthTokens>
    {
        public const float DEFAULT = 1f;
        public const float THIN = 0.5f;
        public const float THICK = 2f;
        public const float THICKER = 4f;
        public const float THICKEST = 8f;

        public Signal<float> Default { get; private set; }
        public Signal<float> Thin { get; private set; }
        public Signal<float> Thick { get; private set; }
        public Signal<float> Thicker { get; private set; }
        public Signal<float> Thickest { get; private set; }

        private StaticSignal<float> _none = new(0f);

        public BorderWidthTokens(
            float @default = DEFAULT,
            float thin = THIN,
            float thick = THICK,
            float thicker = THICKER,
            float thickest = THICKEST
        )
        {
            Default = new(@default);
            Default.RegisterDependent(this);
            Thin = new(thin);
            Thin.RegisterDependent(this);
            Thick = new(thick);
            Thick.RegisterDependent(this);
            Thicker = new(thicker);
            Thicker.RegisterDependent(this);
            Thickest = new(thickest);
            Thickest.RegisterDependent(this);
        }

        ~BorderWidthTokens()
        {
            Default.UnregisterDependent(this);
            Thin.UnregisterDependent(this);
            Thick.UnregisterDependent(this);
            Thicker.UnregisterDependent(this);
            Thickest.UnregisterDependent(this);
        }

        public BaseSignal<float> GetBorderWidthSignal(BorderWidthType borderWidthType)
        {
            switch (borderWidthType)
            {
                case BorderWidthType.Default:
                    return Default;
                case BorderWidthType.Thin:
                    return Thin;
                case BorderWidthType.Thick:
                    return Thick;
                case BorderWidthType.Thicker:
                    return Thicker;
                case BorderWidthType.Thickest:
                    return Thickest;
                case BorderWidthType.None:
                    return _none;
                default:
                    return Default;
            }
        }

        public override BorderWidthTokens Get() => this;
    }

    public enum TextOutlineType
    {
        Default = 0,
        Emphasis = 1,
        Light = 2,
        None = 3
    }
    public class TextOutlineTokens : BaseSignal<TextOutlineTokens>
    {
        public const float DEFAULT = 1f;
        public const float EMPHASIS = 2f;
        public const float LIGHT = 0.5f;

        public Signal<float> Default { get; private set; }
        public Signal<float> Emphasis { get; private set; }
        public Signal<float> Light { get; private set; }
        private StaticSignal<float> _none = new(0f);

        public TextOutlineTokens(float @default = DEFAULT, float emphasis = EMPHASIS, float light = LIGHT)
        {
            Default = new(@default);
            Default.RegisterDependent(this);
            Emphasis = new(emphasis);
            Emphasis.RegisterDependent(this);
            Light = new(light);
            Light.RegisterDependent(this);
        }
        ~TextOutlineTokens()
        {
            Default.UnregisterDependent(this);
            Emphasis.UnregisterDependent(this);
            Light.UnregisterDependent(this);
        }

        public BaseSignal<float> GetTextOutlineSignal(TextOutlineType textOutlineType)
        {
            switch (textOutlineType)
            {
                case TextOutlineType.Default:
                    return Default;
                case TextOutlineType.Emphasis:
                    return Emphasis;
                case TextOutlineType.Light:
                    return Light;
                case TextOutlineType.None:
                    return _none;
                default:
                    return Default;
            }
        }

        public override TextOutlineTokens Get() => this;
    }
}