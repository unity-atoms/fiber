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
        public static SilkButtonComponent SilkButton(
            this BaseComponent component,
            VirtualBody children,
            string role,
            string subRole = null,
            SignalProp<string> variant = new(),
            Style style = new(),
            Action<PointerData> onPress = null,
            ISignal<bool> isDisabled = null
        )
        {
            return new SilkButtonComponent(
                children: children,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
                onPress: onPress,
                isDisabled: isDisabled
            );
        }
    }

    public class SilkButtonComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly Style _style;
        private readonly Action<PointerData> _onPress;
        private readonly ISignal<bool> _isDisabled;

        public SilkButtonComponent(
            VirtualBody children,
            string role,
            string subRole = null,
            SignalProp<string> variant = new(),
            Style style = new(),
            Action<PointerData> onPress = null,
            ISignal<bool> isDisabled = null
        ) : base(children)
        {
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _onPress = onPress;
            _isDisabled = isDisabled;
        }

        public override VirtualBody Render()
        {
            var interactiveElement = F.CreateInteractiveElement(isDisabled: _isDisabled, onPressUp: _onPress);

            var themeStore = C<ThemeStore>();
            var backgroundColor = themeStore.Color(
                role: _role,
                subRole: _subRole,
                elementType: ElementType.Background,
                interactiveElement: interactiveElement,
                variant: _variant
            );
            var borderColor = themeStore.Color(
                role: _role,
                subRole: _subRole,
                elementType: ElementType.Border,
                interactiveElement: interactiveElement,
                variant: _variant
            );
            var textColor = themeStore.Color(
                role: _role,
                subRole: _subRole,
                elementType: ElementType.Text,
                interactiveElement: interactiveElement,
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