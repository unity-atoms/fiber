using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkHeaderComponent SilkHeader(
            this BaseComponent component,
                List<VirtualNode> children,
                string role = THEME_CONSTANTS.INHERIT_ROLE,
                string variant = null,
                Style style = new()
        )
        {
            return new SilkHeaderComponent(
                children: children,
                role: role,
                variant: variant,
                style: style
            );
        }

        public static SilkHeaderItemGroupComponent SilkHeaderItemGroup(
            this BaseComponent component,
            List<VirtualNode> children,
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
        private readonly string _variant;
        private readonly Style _style;

        public SilkHeaderComponent(
            List<VirtualNode> children,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        ) : base(children)
        {
            _role = role;
            _variant = variant;
            _style = style;
        }

        public override VirtualNode Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateHeaderContainer != null)
            {
                return overrideVisualComponents.CreateHeaderContainer(
                    children: children,
                    role: _role,
                    variant: _variant,
                    style: _style
                );
            }

            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);

            return F.RoleProvider(
                role: role,
                children: F.Children(
                    F.View(
                        children: children,
                        style: new Style(
                            mergedStyle: _style,
                            width: new Length(100, LengthUnit.Percent),
                            height: themeStore.Spacing(14),
                            paddingBottom: themeStore.Spacing(2),
                            paddingTop: themeStore.Spacing(2),
                            paddingLeft: themeStore.Spacing(4),
                            paddingRight: themeStore.Spacing(4),
                            backgroundColor: themeStore.Color(role, ElementType.Background, _variant),
                            display: DisplayStyle.Flex,
                            flexGrow: 0,
                            flexShrink: 0,
                            flexDirection: FlexDirection.Row,
                            alignItems: Align.Center,
                            justifyContent: Justify.SpaceBetween
                        )
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
            List<VirtualNode> children,
            Justify justifyContent = Justify.FlexStart,
            Style style = new()
        ) : base(children)
        {
            _justifyContent = justifyContent;
            _style = style;
        }

        public override VirtualNode Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateHeaderItemGroup != null)
            {
                return overrideVisualComponents.CreateHeaderItemGroup(
                    children: children,
                    justifyContent: _justifyContent,
                    style: _style
                );
            }

            return F.View(
                children: children,
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