
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
                string type,
                SignalProp<string> text = new(),
                string role = Constants.INHERIT_ROLE,
                string variant = null
        )
        {
            return new TypographyComponent(
                type: type,
                text: text,
                role: role,
                variant: variant
            );
        }
    }
    public class TypographyComponent : BaseComponent
    {
        private readonly string _type;
        private SignalProp<string> _text;
        private readonly string _role;
        private readonly string _variant;

        public TypographyComponent(
            string type,
            SignalProp<string> text = new(),
            string role = Constants.INHERIT_ROLE,
            string variant = null
        )
        {
            _type = type;
            _text = text;
            _role = role;
            _variant = variant;
        }

        public override VirtualNode Render()
        {
            var themeStore = C<ThemeStore>();
            var role = F.GetRole(_role);

            return F.Text(
                text: _text,
                style: new Style(
                    unityFont: themeStore.Font(_type),
                    fontSize: themeStore.FontSize(_type),
                    unityFontStyle: themeStore.FontStyle(_type),
                    color: themeStore.Color(role, ElementType.Text, _variant)
                )
            );
        }
    }
}