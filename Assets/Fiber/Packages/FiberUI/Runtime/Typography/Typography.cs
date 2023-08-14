
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.DesignTokens;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static TypographyComponent Typography(
            this BaseComponent component,
                TypographyType type,
                SignalProp<string> text = new(),
                string role = Constants.INHERIT_ROLE,
                string variant = null,
                Style style = new()
        )
        {
            return new TypographyComponent(
                type: type,
                text: text,
                role: role,
                variant: variant,
                style: style
            );
        }
    }
    public class TypographyComponent : BaseComponent
    {
        private readonly TypographyType _type;
        private SignalProp<string> _text;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;

        public TypographyComponent(
            TypographyType type,
            SignalProp<string> text = new(),
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        )
        {
            _type = type;
            _text = text;
            _role = role;
            _variant = variant;
            _style = style;
        }

        public override VirtualNode Render()
        {
            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);

            return F.Text(
                text: _text,
                style: new Style(
                    mergedStyle: _style,
                    unityFont: themeStore.Font(_type),
                    unityFontDefinition: StyleKeyword.None,
                    fontSize: themeStore.FontSize(_type),
                    unityFontStyle: themeStore.FontStyle(_type),
                    color: themeStore.Color(role, ElementType.Text, _variant)
                )
            );
        }
    }
}