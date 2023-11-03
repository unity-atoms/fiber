using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkBackdropComponent SilkBackdrop(
            this BaseComponent component,
            VirtualBody children = default,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            EventCallback<ClickEvent> onClick = null
        )
        {
            return new SilkBackdropComponent(
                children: children,
                role: role,
                variant: variant,
                onClick: onClick
            );
        }
    }

    public class SilkBackdropComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _variant;
        private readonly EventCallback<ClickEvent> _onClick;
        public SilkBackdropComponent(
            VirtualBody children = default,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            EventCallback<ClickEvent> onClick = null
        ) : base(children)
        {
            _role = role;
            _variant = variant;
            _onClick = onClick;
        }
        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateBackdrop != null)
            {
                return overrideVisualComponents.CreateBackdrop(
                    children: children,
                    role: _role,
                    variant: _variant,
                    onClick: _onClick
                );
            }

            var themeStore = C<ThemeStore>();
            var backgroundColor = themeStore.Color(role: _role, ElementType.Overlay, variant: _variant);

            return F.View(
                style: new Style(
                    position: Position.Absolute,
                    top: 0,
                    left: 0,
                    right: 0,
                    bottom: 0,
                    backgroundColor: backgroundColor
                ),
                pickingMode: PickingMode.Position,
                onClick: _onClick,
                children: children
            );
        }
    }

}