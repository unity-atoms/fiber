using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.InteractiveUI;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkIconComponent SilkIcon(
            this BaseComponent component,
            SignalProp<string> iconName,
            IconSize size = IconSize.Medium,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null,
            InteractiveElement interactiveElement = null
        )
        {
            return new SilkIconComponent(
                iconName: iconName,
                size: size,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
                forwardRef: forwardRef,
                interactiveElement: interactiveElement
            );
        }
    }

    public class SilkIconComponent : BaseComponent
    {
        private readonly SignalProp<string> _iconName;
        private readonly IconSize _size;
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;
        private readonly InteractiveElement _interactiveElement;

        public SilkIconComponent(
            SignalProp<string> iconName,
            IconSize size = IconSize.Medium,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null,
            InteractiveElement interactiveElement = null
        )
        {
            _iconName = iconName;
            _size = size;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
            _interactiveElement = interactiveElement;
        }
        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateIcon != null)
            {
                return overrideVisualComponents.CreateIcon(
                    iconName: _iconName,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant,
                    style: _style,
                    forwardRef: _forwardRef
                );
            }

            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            var color = themeStore.Color(role: role, elementType: ElementType.Text, interactiveElement: _interactiveElement, subRole: subRole, variant: _variant);
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
                    fontSize: themeStore.IconSize(_size)
                )
            );
        }
    }
}