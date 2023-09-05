using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static DrawerComponent Drawer(
            this BaseComponent component,
            List<VirtualNode> children,
            string role,
            BaseSignal<bool> isOpen,
            DrawerPosition position = DrawerPosition.Left,
            Style style = new()
        )
        {
            return new DrawerComponent(
                children: children,
                role: role,
                isOpen: isOpen,
                position: position,
                style: style
            );
        }
    }

    public enum DrawerPosition
    {
        Left = 0,
        Right = 1
    }

    public class DrawerComponent : BaseComponent
    {
        private readonly string _role;
        private readonly BaseSignal<bool> _isOpen;
        private readonly DrawerPosition _position;
        private readonly Style _style;
        public DrawerComponent(
            List<VirtualNode> children,
            string role,
            BaseSignal<bool> isOpen,
            DrawerPosition position = DrawerPosition.Left,
            Style style = new()
        ) : base(children)
        {
            _role = role;
            _isOpen = isOpen;
            _position = position;
            _style = style;
        }

        public override VirtualNode Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateDrawer != null)
            {
                return overrideVisualComponents.CreateDrawer(
                    children: children,
                    role: _role,
                    isOpen: _isOpen,
                    position: _position,
                    style: _style
                );
            }

            var themeStore = C<ThemeStore>();
            var backgroundColor = themeStore.Color(_role, ElementType.Background);
            var borderColor = themeStore.Color(_role, ElementType.Border);
            var translate = F.CreateComputedSignal<bool, StyleTranslate>((isOpen) =>
            {
                return isOpen ?
                    new Translate(new Length(0, LengthUnit.Percent), new Length(0, LengthUnit.Pixel)) :
                    new Translate(new Length(_position == DrawerPosition.Left ? -100 : 100, LengthUnit.Percent), new Length(0, LengthUnit.Pixel));
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
                    borderRightWidth: _position == DrawerPosition.Left ? 1 : 0,
                    borderLeftWidth: _position == DrawerPosition.Right ? 1 : 0,
                    borderTopWidth: 1,
                    borderBottomWidth: 1
                ),
                pickingMode: PickingMode.Position,
                children: children
            );
        }
    }
}