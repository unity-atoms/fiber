
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
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
        private string _type;
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
            var theme = C<ThemeStore>().Get();
            var role = F.GetRole(_role);

            return F.Text(
                text: _text,
                style: new Style(
                    unityFont: theme.Typography[_type].Font,
                    fontSize: theme.Typography[_type].FontSize,
                    unityFontStyle: theme.Typography[_type].FontStyle,
                    color: theme.Color[role].Text.Default
                )
            );
        }
    }
}