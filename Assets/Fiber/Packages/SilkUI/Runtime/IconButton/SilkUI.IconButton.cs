using System;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;
using Fiber.InteractiveUI;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkIconButtonComponent SilkIconButton(
            this BaseComponent component,
            SignalProp<string> iconName,
            Action<PointerData> onPress,
            ISignal<bool> isDisabled = null,
            IconSize size = IconSize.Medium,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new SilkIconButtonComponent(
                iconName: iconName,
                onPress: onPress,
                isDisabled: isDisabled,
                size: size,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
                forwardRef: forwardRef
            );
        }
    }

    public class SilkIconButtonComponent : BaseComponent
    {
        private readonly SignalProp<string> _iconName;
        private readonly Action<PointerData> _onPress;
        private readonly ISignal<bool> _isDisabled;
        private readonly IconSize _size;
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;

        public SilkIconButtonComponent(
            SignalProp<string> iconName,
            Action<PointerData> onPress,
            ISignal<bool> isDisabled = null,
            IconSize size = IconSize.Medium,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            _iconName = iconName;
            _onPress = onPress;
            _isDisabled = isDisabled;
            _size = size;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
        }

        private float GetDimensionSpacing()
        {
            switch (_size)
            {
                case IconSize.Tiny:
                    return 4;
                case IconSize.Small:
                    return 6;
                case IconSize.Large:
                    return 8;
                case IconSize.XL:
                    return 10;
                case IconSize.Medium:
                default:
                    return 6;
            }
        }

        public override VirtualBody Render()
        {
            var interactiveElement = F.CreateInteractiveElement(_ref: _forwardRef, isDisabled: _isDisabled, onPressUp: _onPress);

            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateIconButton != null)
            {
                return overrideVisualComponents.CreateIconButton(
                    iconName: _iconName,
                    onPress: _onPress,
                    interactiveRef: interactiveElement,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant,
                    style: _style,
                    forwardRef: _forwardRef
                );
            }

            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            var color = themeStore.Color(
                role: role,
                elementType: ElementType.Text,
                interactiveElement: interactiveElement,
                subRole: subRole,
                variant: _variant
            );
            var backgroundColor = themeStore.Color(
                role: role,
                elementType: ElementType.Background,
                interactiveElement: interactiveElement,
                subRole: subRole,
                variant: _variant
            );

            return F.SilkIcon(
                forwardRef: interactiveElement.Ref,
                iconName: _iconName,
                size: _size,
                style: new Style(
                    mergedStyle: _style,
                    color: color,
                    backgroundColor: backgroundColor,
                    width: themeStore.Spacing(GetDimensionSpacing()),
                    height: themeStore.Spacing(GetDimensionSpacing()),
                    borderTopRightRadius: new Length(50, LengthUnit.Percent),
                    borderTopLeftRadius: new Length(50, LengthUnit.Percent),
                    borderBottomRightRadius: new Length(50, LengthUnit.Percent),
                    borderBottomLeftRadius: new Length(50, LengthUnit.Percent)
                ),
                interactiveElement: interactiveElement
            );
        }
    }
}
