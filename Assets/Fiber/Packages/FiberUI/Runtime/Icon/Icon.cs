using System;
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
        public static IconComponent Icon(
            this BaseComponent component,
            SignalProp<string> type,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        )
        {
            return new IconComponent(
                type: type,
                role: role,
                variant: variant,
                style: style
            );
        }
    }

    public class IconComponent : BaseComponent
    {
        private readonly SignalProp<string> _type;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;

        public IconComponent(
            SignalProp<string> type,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        )
        {
            _type = type;
            _role = role;
            _variant = variant;
            _style = style;
        }
        public override VirtualNode Render()
        {
            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);
            var color = themeStore.Color(role, ElementType.Text, _variant);
            var fontAwesome = Resources.Load<Font>("Fonts/FontAwesome/fontawesome-solid");

            var typeSignal = F.WrapSignalProp(_type);
            var iconUnicode = CreateComputedSignal((type) =>
            {
                return type switch
                {
                    "chevron-up" => '\uf077'.ToString(),
                    "chevron-right" => '\uf054'.ToString(),
                    "chevron-down" => '\uf078'.ToString(),
                    "chevron-left" => '\uf053'.ToString(),
                    "moon" => '\uf186'.ToString(),
                    "sun" => '\uf185'.ToString(),
                    _ => null,
                };
            }, typeSignal);

            return F.Text(
                text: iconUnicode,
                style: new Style(
                    mergedStyle: _style,
                    color: color,
                    unityFont: fontAwesome,
                    unityFontDefinition: StyleKeyword.None,
                    fontSize: 12
                )
            );
        }
    }
}