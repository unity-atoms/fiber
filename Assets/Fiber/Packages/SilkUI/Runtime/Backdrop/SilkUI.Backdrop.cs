using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkBackdropComponent SilkBackdrop(
            this BaseComponent component,
            VirtualBody children = default,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            EventCallback<ClickEvent> onClick = null
        )
        {
            return new SilkBackdropComponent(
                children: children,
                role: role,
                subRole: subRole,
                variant: variant,
                onClick: onClick
            );
        }
    }

    public class SilkBackdropComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly EventCallback<ClickEvent> _onClick;
        public SilkBackdropComponent(
            VirtualBody children = default,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            EventCallback<ClickEvent> onClick = null
        ) : base(children)
        {
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _onClick = onClick;
        }
        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateBackdrop != null)
            {
                return overrideVisualComponents.CreateBackdrop(
                    children: Children,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant,
                    onClick: _onClick
                );
            }

            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);
            var backgroundColor = themeStore.Color(role: role, ElementType.Overlay, subRole: subRole, variant: _variant);

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
                children: Children
            );
        }
    }

}