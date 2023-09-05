using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static IconComponent Icon(
            this BaseComponent component,
            SignalProp<string> iconName,
            IconSize size = IconSize.Medium,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new IconComponent(
                iconName: iconName,
                size: size,
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
        private readonly IconSize _size;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;

        public IconComponent(
            SignalProp<string> iconName,
            IconSize size = IconSize.Medium,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            _iconName = iconName;
            _size = size;
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
            var fontAwesomeSolid = Resources.Load<Font>("Fonts/FontAwesome/fontawesome-solid");
            var fontAwesomeBrands = Resources.Load<Font>("Fonts/FontAwesome/fontawesome-brands");

            var iconNameSignal = F.WrapSignalProp(_iconName);
            var iconUnicode = CreateComputedSignal((iconName) => FontAwesomeUtils.IconNameToUnicode(iconName), iconNameSignal);
            var font = CreateComputedSignal((iconName) => FontAwesomeUtils.IsBrandsIcon(iconName) ? new StyleFont(fontAwesomeBrands) : new StyleFont(fontAwesomeSolid), iconNameSignal);

            return F.Text(
                _ref: _forwardRef,
                text: iconUnicode,
                style: new Style(
                    mergedStyle: _style,
                    color: color,
                    unityFont: font,
                    unityFontDefinition: StyleKeyword.None,
                    unityTextAlign: TextAnchor.MiddleCenter,
                    fontSize: themeStore.Icon(_size)
                )
            );
        }
    }
}