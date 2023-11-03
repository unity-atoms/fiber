using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkButtonComponent SilkButton(
            this BaseComponent component,
            VirtualBody children,
            string role,
            string variant = null,
            Style style = new(),
            Action onPress = null
        )
        {
            return new SilkButtonComponent(
                children: children,
                role: role,
                variant: variant,
                style: style,
                onPress: onPress
            );
        }
    }

    public class SilkButtonComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;
        private readonly Action _onPress;

        public SilkButtonComponent(
            VirtualBody children,
            string role,
            string variant = null,
            Style style = new(),
            Action onPress = null
        ) : base(children)
        {
            _role = role;
            _variant = variant;
            _style = style;
            _onPress = onPress;
        }

        public override VirtualBody Render()
        {
            var interactiveElement = F.CreateInteractiveElement(isDisabled: null, onPress: _onPress);

            var themeStore = C<ThemeStore>();
            var backgroundColor = themeStore.Color(
                role: _role,
                elementType: ElementType.Background,
                isPressed: interactiveElement.IsPressed,
                isHovered: interactiveElement.IsHovered,
                variant: _variant
            );
            var borderColor = themeStore.Color(
                role: _role,
                elementType: ElementType.Border,
                isPressed: interactiveElement.IsPressed,
                isHovered: interactiveElement.IsHovered,
                variant: _variant
            );
            var textColor = themeStore.Color(
                role: _role,
                elementType: ElementType.Text,
                isPressed: interactiveElement.IsPressed,
                isHovered: interactiveElement.IsHovered,
                variant: _variant
            );

            return F.Button(
                _ref: interactiveElement.Ref,
                style: new Style(
                    mergedStyle: _style,
                    backgroundColor: backgroundColor,
                    borderTopColor: borderColor,
                    borderLeftColor: borderColor,
                    borderRightColor: borderColor,
                    borderBottomColor: borderColor,
                    borderBottomLeftRadius: themeStore.Spacing(1),
                    borderBottomRightRadius: themeStore.Spacing(1),
                    borderTopLeftRadius: themeStore.Spacing(1),
                    borderTopRightRadius: themeStore.Spacing(1),
                    borderTopWidth: themeStore.BorderWidth(),
                    borderLeftWidth: themeStore.BorderWidth(),
                    borderRightWidth: themeStore.BorderWidth(),
                    borderBottomWidth: themeStore.BorderWidth(),
                    color: textColor,
                    unityFont: themeStore.Font(TypographyType.Button),
                    unityFontDefinition: StyleKeyword.None,
                    fontSize: themeStore.FontSize(TypographyType.Button),
                    unityFontStyle: themeStore.FontStyle(TypographyType.Button),
                    paddingTop: themeStore.Spacing(2),
                    paddingBottom: themeStore.Spacing(2),
                    paddingLeft: themeStore.Spacing(6),
                    paddingRight: themeStore.Spacing(6)
                ),
                children: Children,
                pickingMode: PickingMode.Position
            );
        }
    }
}