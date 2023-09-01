using System;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.Theme;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static IconButtonComponent IconButton(
            this BaseComponent component,
            SignalProp<string> iconName,
            Action onPress,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new IconButtonComponent(
                iconName: iconName,
                onPress: onPress,
                role: role,
                variant: variant,
                style: style,
                forwardRef: forwardRef
            );
        }
    }

    public class IconButtonComponent : BaseComponent
    {
        private readonly SignalProp<string> _iconName;
        private readonly Action _onPress;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;

        public IconButtonComponent(
            SignalProp<string> iconName,
            Action onPress,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            _iconName = iconName;
            _onPress = onPress;
            _role = role;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
        }
        public override VirtualNode Render()
        {
            var interactiveElement = F.CreateInteractiveElement(_ref: _forwardRef, isDisabled: null, onPress: _onPress);

            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateIconButton != null)
            {
                return overrideVisualComponents.CreateIconButton(
                    iconName: _iconName,
                    onPress: _onPress,
                    interactiveRef: interactiveElement,
                    role: _role,
                    variant: _variant,
                    style: _style,
                    forwardRef: _forwardRef
                );
            }

            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);
            var color = themeStore.Color(
                role: role,
                elementType: ElementType.Text,
                isPressed: interactiveElement.IsPressed,
                isHovered: interactiveElement.IsHovered,
                isSelected: null,
                variant: _variant
            );
            var backgroundColor = themeStore.Color(
                role: role,
                elementType: ElementType.Background,
                isPressed: interactiveElement.IsPressed,
                isHovered: interactiveElement.IsHovered,
                isSelected: null,
                variant: _variant
            );

            return F.Icon(
                forwardRef: interactiveElement.Ref,
                iconName: _iconName,
                style: new Style(
                    mergedStyle: _style,
                    color: color,
                    backgroundColor: backgroundColor,
                    width: themeStore.Spacing(6),
                    height: themeStore.Spacing(6),
                    borderTopRightRadius: new Length(50, LengthUnit.Percent),
                    borderTopLeftRadius: new Length(50, LengthUnit.Percent),
                    borderBottomRightRadius: new Length(50, LengthUnit.Percent),
                    borderBottomLeftRadius: new Length(50, LengthUnit.Percent)
                )
            );
        }
    }
}
