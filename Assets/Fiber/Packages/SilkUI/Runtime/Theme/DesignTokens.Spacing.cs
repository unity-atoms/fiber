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
            float borderWidthThick = BorderWidthTokens.THICK,
            float borderWidthThin = BorderWidthTokens.THIN,
            float textOutlineDefault = TextOutlineTokens.DEFAULT,
            float textOutlineEmphasis = TextOutlineTokens.EMPHASIS,
            float textOutlineLight = TextOutlineTokens.LIGHT
        )
        {
            Baseline = new(baseline);
            Baseline.RegisterDependent(this);
            BorderWidth = new(borderWidthDefault, borderWidthThick, borderWidthThin);
            BorderWidth.RegisterDependent(this);
            TextOutline = new(textOutlineDefault, textOutlineEmphasis, textOutlineLight);
            TextOutline.RegisterDependent(this);
        }
        ~SpacingTokens()
        {
            Baseline.UnregisterDependent(this);
        }

        public override SpacingTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
    }

    public enum BorderWidthType
    {
        Default = 0,
        Thick = 1,
        Thin = 2,
        None = 3
    }
    public class BorderWidthTokens : BaseSignal<BorderWidthTokens>
    {
        public const float DEFAULT = 0.25f;
        public const float THICK = 0.5f;
        public const float THIN = 0.125f;

        // Multiplied by SpacingTokens.Baseline to get the actual value
        public Signal<float> Default { get; private set; }
        // Multiplied by SpacingTokens.Baseline to get the actual value
        public Signal<float> Thick { get; private set; }
        // Multiplied by SpacingTokens.Baseline to get the actual value
        public Signal<float> Thin { get; private set; }
        private StaticSignal<float> _none = new(0f);

        public BorderWidthTokens(float @default = DEFAULT, float thick = THICK, float thin = THIN)
        {
            Default = new(@default);
            Default.RegisterDependent(this);
            Thick = new(thick);
            Thick.RegisterDependent(this);
            Thin = new(thin);
            Thin.RegisterDependent(this);
        }

        ~BorderWidthTokens()
        {
            Default.UnregisterDependent(this);
            Thick.UnregisterDependent(this);
            Thin.UnregisterDependent(this);
        }

        public BaseSignal<float> GetBorderWidthSignal(BorderWidthType borderWidthType)
        {
            switch (borderWidthType)
            {
                case BorderWidthType.Default:
                    return Default;
                case BorderWidthType.Thick:
                    return Thick;
                case BorderWidthType.Thin:
                    return Thin;
                case BorderWidthType.None:
                    return _none;
                default:
                    return Default;
            }
        }

        public override BorderWidthTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
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
        public const float DEFAULT = 0.25f;
        public const float EMPHASIS = 0.5f;
        public const float LIGHT = 0.125f;

        // Multiplied by SpacingTokens.Baseline to get the actual value
        public Signal<float> Default { get; private set; }
        // Multiplied by SpacingTokens.Baseline to get the actual value
        public Signal<float> Emphasis { get; private set; }
        // Multiplied by SpacingTokens.Baseline to get the actual value
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