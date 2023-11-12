using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkDrawerComponent SilkDrawer(
            this BaseComponent component,
            VirtualBody children,
            BaseSignal<bool> isOpen,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            DrawerPosition position = DrawerPosition.Left,
            Style style = new(),
            float outsideScreenPercentage = 100f
        )
        {
            return new SilkDrawerComponent(
                children: children,
                role: role,
                subRole: subRole,
                isOpen: isOpen,
                variant: variant,
                position: position,
                style: style,
                outsideScreenPercentage: outsideScreenPercentage
            );
        }
    }

    public enum DrawerPosition
    {
        Left = 0,
        Right = 1
    }

    public class SilkDrawerComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly BaseSignal<bool> _isOpen;
        private readonly SignalProp<string> _variant;
        private readonly DrawerPosition _position;
        private readonly Style _style;
        private readonly float _outsideScreenPercentage;
        public SilkDrawerComponent(
            VirtualBody children,
            BaseSignal<bool> isOpen,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            DrawerPosition position = DrawerPosition.Left,
            Style style = new(),
            float outsideScreenPercentage = 100f
        ) : base(children)
        {
            _role = role;
            _subRole = subRole;
            _isOpen = isOpen;
            _variant = variant;
            _position = position;
            _style = style;
            _outsideScreenPercentage = outsideScreenPercentage;
        }

        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateDrawer != null)
            {
                return overrideVisualComponents.CreateDrawer(
                    children: Children,
                    isOpen: _isOpen,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant,
                    position: _position,
                    style: _style,
                    outsideScreenPercentage: _outsideScreenPercentage
                );
            }

            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);
            var backgroundColor = themeStore.Color(role, ElementType.Background, subRole, _variant);
            var borderColor = themeStore.Color(role, ElementType.Border, subRole, _variant);
            var translate = F.CreateComputedSignal<bool, StyleTranslate>((isOpen) =>
            {
                return isOpen ?
                    new Translate(new Length(0, LengthUnit.Percent), new Length(0, LengthUnit.Pixel)) :
                    new Translate(new Length(_position == DrawerPosition.Left ? -_outsideScreenPercentage : _outsideScreenPercentage, LengthUnit.Percent), new Length(0, LengthUnit.Pixel));
            }, _isOpen);

            return F.View(
                style: new Style(
                    mergedStyle: _style,
                    position: Position.Absolute,
                    top: 0,
                    left: _position == DrawerPosition.Left ? 0 : StyleKeyword.Initial,
                    right: _position == DrawerPosition.Right ? 0 : StyleKeyword.Initial,
                    bottom: 0,
                    translate: translate,
                    transitionProperty: new List<StylePropertyName>() { new("translate") },
                    transitionDuration: new List<TimeValue>() { new(0.2f, TimeUnit.Second) },
                    width: new Length(80, LengthUnit.Percent),
                    backgroundColor: backgroundColor,
                    borderRightColor: borderColor,
                    borderLeftColor: borderColor,
                    borderTopColor: borderColor,
                    borderBottomColor: borderColor,
                    borderRightWidth: themeStore.BorderWidth(_position == DrawerPosition.Left ? BorderWidthType.Default : BorderWidthType.None),
                    borderLeftWidth: themeStore.BorderWidth(_position == DrawerPosition.Right ? BorderWidthType.Default : BorderWidthType.None),
                    borderTopWidth: themeStore.BorderWidth(),
                    borderBottomWidth: themeStore.BorderWidth()
                ),
                pickingMode: PickingMode.Position,
                children: Children
            );
        }
    }
}