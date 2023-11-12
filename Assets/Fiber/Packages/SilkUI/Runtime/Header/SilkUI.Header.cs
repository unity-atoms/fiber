using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkHeaderComponent SilkHeader(
            this BaseComponent component,
                VirtualBody children,
                string role = THEME_CONSTANTS.INHERIT,
                string subRole = THEME_CONSTANTS.INHERIT,
                SignalProp<string> variant = new(),
                Style style = new()
        )
        {
            return new SilkHeaderComponent(
                children: children,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style
            );
        }

        public static SilkHeaderItemGroupComponent SilkHeaderItemGroup(
            this BaseComponent component,
            VirtualBody children,
            Justify justifyContent = Justify.FlexStart,
            Style style = new()
        )
        {
            return new SilkHeaderItemGroupComponent(
                children: children,
                justifyContent: justifyContent,
                style: style
            );
        }
    }

    public class SilkHeaderComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly Style _style;

        public SilkHeaderComponent(
            VirtualBody children,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new()
        ) : base(children)
        {
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
        }

        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateHeaderContainer != null)
            {
                return overrideVisualComponents.CreateHeaderContainer(
                    children: Children,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant,
                    style: _style
                );
            }

            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            return F.RoleProvider(
                role: role,
                subRole: subRole,
                children: F.View(
                    children: Children,
                    style: new Style(
                        mergedStyle: _style,
                        width: new Length(100, LengthUnit.Percent),
                        height: themeStore.Spacing(14),
                        paddingBottom: themeStore.Spacing(2),
                        paddingTop: themeStore.Spacing(2),
                        paddingLeft: themeStore.Spacing(4),
                        paddingRight: themeStore.Spacing(4),
                        backgroundColor: themeStore.Color(role, ElementType.Background, subRole, _variant),
                        display: DisplayStyle.Flex,
                        flexGrow: 0,
                        flexShrink: 0,
                        flexDirection: FlexDirection.Row,
                        alignItems: Align.Center,
                        justifyContent: Justify.SpaceBetween
                    )
                )
            );
        }
    }

    public class SilkHeaderItemGroupComponent : BaseComponent
    {
        private readonly Style _style;
        private readonly Justify _justifyContent;

        public SilkHeaderItemGroupComponent(
            VirtualBody children,
            Justify justifyContent = Justify.FlexStart,
            Style style = new()
        ) : base(children)
        {
            _justifyContent = justifyContent;
            _style = style;
        }

        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateHeaderItemGroup != null)
            {
                return overrideVisualComponents.CreateHeaderItemGroup(
                    children: Children,
                    justifyContent: _justifyContent,
                    style: _style
                );
            }

            return F.View(
                children: Children,
                style: new Style(
                    mergedStyle: _style,
                    display: DisplayStyle.Flex,
                    flexGrow: 1,
                    flexShrink: 1,
                    flexDirection: FlexDirection.Row,
                    alignItems: Align.Center,
                    justifyContent: _justifyContent,
                    height: new Length(100, LengthUnit.Percent)
                )
            );
        }
    }
}