using Signals;
using UnityEngine.UIElements;
using UnityEngine;

namespace SilkUI
{
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
        TextOutline = 3,
        Overlay = 4,
        Outline = 5,
        Shine = 6,
        Gloom = 7
    }

    public class Role : BaseSignal<Role>
    {
        public Element Background;
        public Element Border;
        public Element Text;
        public Element TextOutline;
        public Element Overlay;
        public Element Outline;
        public Element Shine;
        public Element Gloom;
        public SignalDictionary<string, Role> SubRoles;

        public Role(
            Element background = null,
            Element border = null,
            Element text = null,
            Element textOutline = null,
            Element overlay = null,
            Element outline = null,
            Element shine = null,
            Element gloom = null,
            SignalDictionary<string, Role> subRoles = null
        )
        {
            Background = background ?? new();
            Background.RegisterDependent(this);
            Border = border ?? new();
            Border.RegisterDependent(this);
            Text = text ?? new();
            Text.RegisterDependent(this);
            TextOutline = textOutline ?? new();
            TextOutline.RegisterDependent(this);
            Overlay = overlay ?? new();
            Overlay.RegisterDependent(this);
            Outline = outline ?? new();
            Outline.RegisterDependent(this);
            Shine = shine ?? new();
            Shine.RegisterDependent(this);
            Gloom = gloom ?? new();
            Gloom.RegisterDependent(this);

            SubRoles = subRoles ?? new();
            SubRoles.RegisterDependent(this);
        }

        ~Role()
        {
            Background.UnregisterDependent(this);
            Border.UnregisterDependent(this);
            Text.UnregisterDependent(this);
            TextOutline.UnregisterDependent(this);
            Overlay.UnregisterDependent(this);
            Outline.UnregisterDependent(this);
            Shine.UnregisterDependent(this);
            Gloom.UnregisterDependent(this);

            SubRoles.UnregisterDependent(this);
        }

        public override Role Get() => this;
        public Element GetElement(ElementType elementType)
        {
            return elementType switch
            {
                ElementType.Background => Background,
                ElementType.Border => Border,
                ElementType.Text => Text,
                ElementType.TextOutline => TextOutline,
                ElementType.Overlay => Overlay,
                ElementType.Outline => Outline,
                ElementType.Shine => Shine,
                ElementType.Gloom => Gloom,
                _ => null,
            };
        }

        public bool IsElementEmpty(ElementType elementType)
        {
            return GetElement(elementType).IsEmpty();
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
            Variants.RegisterDependent(this);
        }

        ~Element()
        {
            Variants.UnregisterDependent(this);
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
            Default.RegisterDependent(this);
            Selected = new Signal<T>(selected ?? default, this);
            Selected.RegisterDependent(this);
            Focused = new Signal<T>(focused ?? default, this);
            Focused.RegisterDependent(this);
            Hovered = new Signal<T>(hovered ?? default, this);
            Hovered.RegisterDependent(this);
            Pressed = new Signal<T>(pressed ?? default, this);
            Pressed.RegisterDependent(this);
            Disabled = new Signal<T>(disabled ?? default, this);
            Disabled.RegisterDependent(this);
        }

        ~Modifiers()
        {
            Default.UnregisterDependent(this);
            Selected.UnregisterDependent(this);
            Focused.UnregisterDependent(this);
            Hovered.UnregisterDependent(this);
            Pressed.UnregisterDependent(this);
            Disabled.UnregisterDependent(this);
        }

        public override Modifiers<T> Get() => this;
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

        public bool IsEmpty()
        {
            return Default.Value == StyleKeyword.Null
                && Selected.Value == StyleKeyword.Null
                && Focused.Value == StyleKeyword.Null
                && Hovered.Value == StyleKeyword.Null
                && Pressed.Value == StyleKeyword.Null
                && Disabled.Value == StyleKeyword.Null;
        }
    }
}