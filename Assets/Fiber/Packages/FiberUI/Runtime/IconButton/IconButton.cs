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
            Style style = new()
        )
        {
            return new IconButtonComponent(
                type: type,
                onPress: onPress,
                role: role,
                variant: variant,
                style: style
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

        public IconButtonComponent(
            SignalProp<string> type,
            Action onPress,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        )
        {
            _type = type;
            _onPress = onPress;
            _role = role;
            _variant = variant;
            _style = style;
        }
        public override VirtualNode Render()
        {
            var interactiveRef = F.CreateInteractiveRef<TextElement>(isDisabled: null, onPress: _onPress);

            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateIconButton != null)
            {
                return overrideVisualComponents.CreateIconButton(
                    type: _type,
                    onPress: _onPress,
                    interactiveRef: interactiveRef,
                    role: _role,
                    variant: _variant,
                    style: _style
                );
            }

            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);
            var color = themeStore.Color(
                role: role,
                elementType: ElementType.Text,
                isPressed: interactiveRef.IsPressed,
                isHovered: interactiveRef.IsHovered,
                isSelected: null,
                variant: _variant
            );
            var backgroundColor = themeStore.Color(
                role: role,
                elementType: ElementType.Background,
                isPressed: interactiveRef.IsPressed,
                isHovered: interactiveRef.IsHovered,
                isSelected: null,
                variant: _variant
            );

            return F.Icon(
                forwardRef: interactiveRef,
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
