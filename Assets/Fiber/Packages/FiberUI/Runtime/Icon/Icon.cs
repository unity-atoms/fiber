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
            SignalProp<string> iconName,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new IconComponent(
                iconName: iconName,
                role: role,
                variant: variant,
                style: style,
                forwardRef: forwardRef
            );
        }
    }

    public class IconComponent : BaseComponent
    {
        private readonly SignalProp<string> _iconName;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;

        public IconComponent(
            SignalProp<string> iconName,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            _iconName = iconName;
            _role = role;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
        }
        public override VirtualNode Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateIcon != null)
            {
                return overrideVisualComponents.CreateIcon(
                    iconName: _iconName,
                    role: _role,
                    variant: _variant,
                    style: _style,
                    forwardRef: _forwardRef
                );
            }

            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);
            var color = themeStore.Color(role, ElementType.Text, _variant);
            var fontAwesome = Resources.Load<Font>("Fonts/FontAwesome/fontawesome-solid");

            var iconNameSignal = F.WrapSignalProp(_iconName);
            var iconUnicode = CreateComputedSignal((iconName) => FontAwesomeUtils.IconNameToUnicode(iconName), iconNameSignal);

            return F.Text(
                _ref: _forwardRef,
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