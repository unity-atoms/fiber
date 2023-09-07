using Signals;
using UnityEngine.UIElements;
using UnityEngine;

namespace Fiber.UI
{
    public class TypographyTokens : BaseSignal<TypographyTokens>
    {
        public TypographyTypeTokens Heading1 { get; private set; }
        public TypographyTypeTokens Heading2 { get; private set; }
        public TypographyTypeTokens Heading3 { get; private set; }
        public TypographyTypeTokens Heading4 { get; private set; }
        public TypographyTypeTokens Heading5 { get; private set; }
        public TypographyTypeTokens Heading6 { get; private set; }
        public TypographyTypeTokens Subtitle1 { get; private set; }
        public TypographyTypeTokens Subtitle2 { get; private set; }
        public TypographyTypeTokens Body1 { get; private set; }
        public TypographyTypeTokens Body2 { get; private set; }
        public TypographyTypeTokens Button { get; private set; }
        public TypographyTypeTokens Caption { get; private set; }
        public TypographyTypeTokens Overline { get; private set; }

        public TypographyTokens(
            TypographyTypeTokens heading1 = null,
            TypographyTypeTokens heading2 = null,
            TypographyTypeTokens heading3 = null,
            TypographyTypeTokens heading4 = null,
            TypographyTypeTokens heading5 = null,
            TypographyTypeTokens heading6 = null,
            TypographyTypeTokens subtitle1 = null,
            TypographyTypeTokens subtitle2 = null,
            TypographyTypeTokens body1 = null,
            TypographyTypeTokens body2 = null,
            TypographyTypeTokens button = null,
            TypographyTypeTokens caption = null,
            TypographyTypeTokens overline = null
        )
        {
            Heading1 = heading1;
            Heading1?.RegisterDependent(this);
            Heading2 = heading2;
            Heading2?.RegisterDependent(this);
            Heading3 = heading3;
            Heading3?.RegisterDependent(this);
            Heading4 = heading4;
            Heading4?.RegisterDependent(this);
            Heading5 = heading5;
            Heading5?.RegisterDependent(this);
            Heading6 = heading6;
            Heading6?.RegisterDependent(this);
            Subtitle1 = subtitle1;
            Subtitle1?.RegisterDependent(this);
            Subtitle2 = subtitle2;
            Subtitle2?.RegisterDependent(this);
            Body1 = body1;
            Body1?.RegisterDependent(this);
            Body2 = body2;
            Body2?.RegisterDependent(this);
            Button = button;
            Button?.RegisterDependent(this);
            Caption = caption;
            Caption?.RegisterDependent(this);
            Overline = overline;
            Overline?.RegisterDependent(this);
        }
        ~TypographyTokens()
        {
            Heading1?.UnregisterDependent(this);
            Heading2?.UnregisterDependent(this);
            Heading3?.UnregisterDependent(this);
            Heading4?.UnregisterDependent(this);
            Heading5?.UnregisterDependent(this);
            Heading6?.UnregisterDependent(this);
            Subtitle1?.UnregisterDependent(this);
            Subtitle2?.UnregisterDependent(this);
            Body1?.UnregisterDependent(this);
            Body2?.UnregisterDependent(this);
            Button?.UnregisterDependent(this);
            Caption?.UnregisterDependent(this);
            Overline?.UnregisterDependent(this);
        }

        public override TypographyTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        public TypographyTypeTokens GetTypographyTypeTokens(TypographyType typographyType)
        {
            return typographyType switch
            {
                TypographyType.Heading1 => Heading1,
                TypographyType.Heading2 => Heading2,
                TypographyType.Heading3 => Heading3,
                TypographyType.Heading4 => Heading4,
                TypographyType.Heading5 => Heading5,
                TypographyType.Heading6 => Heading6,
                TypographyType.Subtitle1 => Subtitle1,
                TypographyType.Subtitle2 => Subtitle2,
                TypographyType.Body1 => Body1,
                TypographyType.Body2 => Body2,
                TypographyType.Button => Button,
                TypographyType.Caption => Caption,
                TypographyType.Overline => Overline,
                _ => null,
            };
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
    }

    public enum TypographyType
    {
        Heading1 = 0,
        Heading2 = 1,
        Heading3 = 2,
        Heading4 = 3,
        Heading5 = 4,
        Heading6 = 5,
        Subtitle1 = 6,
        Subtitle2 = 7,
        Body1 = 8,
        Body2 = 9,
        Button = 10,
        Caption = 11,
        Overline = 12,
    }

    public class TypographyTypeTokens : BaseSignal<TypographyTypeTokens>
    {
        public Signal<StyleFont> Font;
        public Signal<StyleLength> FontSize;
        public Signal<StyleEnum<FontStyle>> FontStyle;

        public TypographyTypeTokens(
            Font font,
            int fontSize,
            FontStyle fontStyle
        )
        {
            Font = new(new(font));
            Font.RegisterDependent(this);
            FontSize = new(fontSize);
            FontSize.RegisterDependent(this);
            FontStyle = new(fontStyle);
            FontStyle.RegisterDependent(this);
        }
        ~TypographyTypeTokens()
        {
            Font.UnregisterDependent(this);
            FontSize.UnregisterDependent(this);
            FontStyle.UnregisterDependent(this);
        }

        public override TypographyTypeTokens Get() => this;
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