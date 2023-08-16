using System;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.DesignTokens;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static IconButtonComponent IconButton(
            this BaseComponent component,
            SignalProp<string> type,
            Action onPress,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new IconButtonComponent(
                type: type,
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
        private readonly SignalProp<string> _type;
        private readonly Action _onPress;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;

        public IconButtonComponent(
            SignalProp<string> type,
            Action onPress,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            _type = type;
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
                    type: _type,
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
                type: _type,
                style: new Style(
                    mergedStyle: _style,
                    color: color,
                    backgroundColor: backgroundColor,
                    paddingLeft: themeStore.Spacing(2),
                    paddingRight: themeStore.Spacing(2),
                    paddingTop: themeStore.Spacing(2),
                    paddingBottom: themeStore.Spacing(2),
                    borderTopRightRadius: new Length(50, LengthUnit.Percent),
                    borderTopLeftRadius: new Length(50, LengthUnit.Percent),
                    borderBottomRightRadius: new Length(50, LengthUnit.Percent),
                    borderBottomLeftRadius: new Length(50, LengthUnit.Percent)
                )
            );
        }
    }
}
