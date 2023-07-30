using System.Collections.Generic;
using Signals;
using UnityEngine.UIElements;
using UnityEngine;

namespace Fiber.DesignTokens
{
    public class TokenCollection : SignalDictionary<string, Role> { }

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
            Border = border ?? new();
            Text = text ?? new();
        }

        public override Role Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    public class Element : ColorModifiers
    {
        public SignalDictionary<string, ColorModifiers> Variants;

        public Element(
            ColorToken @default = default(ColorToken),
            ColorToken selected = default(ColorToken),
            ColorToken focused = default(ColorToken),
            ColorToken hovered = default(ColorToken),
            ColorToken pressed = default(ColorToken),
            ColorToken disabled = default(ColorToken),
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
            T @default = default(T),
            T selected = default(T),
            T focused = default(T),
            T hovered = default(T),
            T pressed = default(T),
            T disabled = default(T)
        )
        {
            Default = new Signal<T>(@default ?? default(T), this);
            Selected = new Signal<T>(selected ?? default(T), this);
            Focused = new Signal<T>(focused ?? default(T), this);
            Hovered = new Signal<T>(hovered ?? default(T), this);
            Pressed = new Signal<T>(pressed ?? default(T), this);
            Disabled = new Signal<T>(disabled ?? default(T), this);
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
            ColorToken @default = default(ColorToken),
            ColorToken selected = default(ColorToken),
            ColorToken focused = default(ColorToken),
            ColorToken hovered = default(ColorToken),
            ColorToken pressed = default(ColorToken),
            ColorToken disabled = default(ColorToken)
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