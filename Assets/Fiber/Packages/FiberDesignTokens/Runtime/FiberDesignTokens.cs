using System.Collections.Generic;
using Signals;
using UnityEngine.UIElements;
using UnityEngine;

namespace Fiber.DesignTokens
{
    public class TypographyTokensCollection : SignalDictionary<string, TypographyTokens> { }

    public class TypographyTokens : BaseSignal<TypographyTokens>
    {
        public Signal<StyleFont> Font;
        public Signal<StyleLength> FontSize;
        public Signal<StyleEnum<FontStyle>> FontStyle;

        public TypographyTokens(
            Font font,
            int fontSize,
            FontStyle fontStyle
        )
        {
            Font = new(new(font));
            Font.RegisterParent(this);
            FontSize = new(fontSize);
            FontSize.RegisterParent(this);
            FontStyle = new(fontStyle);
            FontStyle.RegisterParent(this);
        }

        public override TypographyTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    public class SpacingTokens : BaseSignal<SpacingTokens>
    {
        public Signal<int> Baseline;

        public SpacingTokens(int baseline = 4)
        {
            Baseline = new(baseline);
            Baseline.RegisterParent(this);
        }

        public override SpacingTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    public class ColorTokenCollection : SignalDictionary<string, Role> { }

    public struct ColorToken
    {
        public Color Value;
        public bool IsInitialized;

        public ColorToken(Color color)
        {
            Value = color;
            IsInitialized = true;
        }

        public ColorToken(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color color);
            Value = color;
            IsInitialized = true;
        }

        public static implicit operator ColorToken(Color color)
        {
            return new ColorToken(color);
        }

        public static implicit operator ColorToken(string hex)
        {
            return new ColorToken(hex);
        }
    }

    public enum ElementType
    {
        Background = 0,
        Border = 1,
        Text = 2,
    }

    public class Role : BaseSignal<Role>
    {
        public Element Background;
        public Element Border;
        public Element Text;

        public Role(
            Element background = null,
            Element border = null,
            Element text = null
        )
        {
            Background = background ?? new();
            Background.RegisterParent(this);
            Border = border ?? new();
            Border.RegisterParent(this);
            Text = text ?? new();
            Text.RegisterParent(this);
        }

        public override Role Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        public Element GetElement(ElementType elementType)
        {
            return elementType switch
            {
                ElementType.Background => Background,
                ElementType.Border => Border,
                ElementType.Text => Text,
                _ => null,
            };
        }
    }

    public class Element : ColorModifiers
    {
        public SignalDictionary<string, ColorModifiers> Variants;

        public Element(
            ColorToken @default = default,
            ColorToken selected = default,
            ColorToken focused = default,
            ColorToken hovered = default,
            ColorToken pressed = default,
            ColorToken disabled = default,
            SignalDictionary<string, ColorModifiers> variants = null
        ) : base(
            @default: @default,
            selected: selected,
            focused: focused,
            hovered: hovered,
            pressed: pressed,
            disabled: disabled
        )
        {
            Variants = variants ?? new();
        }
    }

    public class Modifiers<T> : BaseSignal<Modifiers<T>>
    {
        public Signal<T> Default;
        public Signal<T> Selected;
        public Signal<T> Focused;
        public Signal<T> Hovered;
        public Signal<T> Pressed;
        public Signal<T> Disabled;

        public Modifiers(
            T @default = default,
            T selected = default,
            T focused = default,
            T hovered = default,
            T pressed = default,
            T disabled = default
        )
        {
            Default = new Signal<T>(@default ?? default, this);
            Default.RegisterParent(this);
            Selected = new Signal<T>(selected ?? default, this);
            Selected.RegisterParent(this);
            Focused = new Signal<T>(focused ?? default, this);
            Focused.RegisterParent(this);
            Hovered = new Signal<T>(hovered ?? default, this);
            Hovered.RegisterParent(this);
            Pressed = new Signal<T>(pressed ?? default, this);
            Pressed.RegisterParent(this);
            Disabled = new Signal<T>(disabled ?? default, this);
            Disabled.RegisterParent(this);
        }

        public override Modifiers<T> Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    public class ColorModifiers : Modifiers<StyleColor>
    {
        public ColorModifiers(
            ColorToken @default = default,
            ColorToken selected = default,
            ColorToken focused = default,
            ColorToken hovered = default,
            ColorToken pressed = default,
            ColorToken disabled = default
        ) : base(
            @default: @default.IsInitialized ? new(@default.Value) : new(StyleKeyword.Null),
            selected: selected.IsInitialized ? new(selected.Value) : new(StyleKeyword.Null),
            focused: focused.IsInitialized ? new(focused.Value) : new(StyleKeyword.Null),
            hovered: hovered.IsInitialized ? new(hovered.Value) : new(StyleKeyword.Null),
            pressed: pressed.IsInitialized ? new(pressed.Value) : new(StyleKeyword.Null),
            disabled: disabled.IsInitialized ? new(disabled.Value) : new(StyleKeyword.Null)
        )
        { }
    }
}