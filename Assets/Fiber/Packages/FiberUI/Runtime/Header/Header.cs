using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.DesignTokens;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static HeaderComponent Header(
            this BaseComponent component,
                List<VirtualNode> children,
                string role = Constants.INHERIT_ROLE,
                string variant = null,
                Style style = new()
        )
        {
            return new HeaderComponent(
                children: children,
                role: role,
                variant: variant,
                style: style
            );
        }

        public static HeaderItemGroupComponent HeaderItemGroup(
            this BaseComponent component,
            List<VirtualNode> children,
            Justify justifyContent = Justify.FlexStart,
            Style style = new()
        )
        {
            return new HeaderItemGroupComponent(
                children: children,
                justifyContent: justifyContent,
                style: style
            );
        }
    }

    public class HeaderComponent : BaseComponent
    {
        private readonly Style _style;
        private readonly string _role;
        private readonly string _variant;

        public HeaderComponent(
            List<VirtualNode> children,
            string role = Constants.INHERIT_ROLE,
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
                            height: themeStore.Spacing(13),
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

    public class HeaderItemGroupComponent : BaseComponent
    {
        private readonly Style _style;
        private readonly Justify _justifyContent;

        public HeaderItemGroupComponent(
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
            return F.View(
                children: children,
                style: new Style(
                    mergedStyle: _style,
                    display: DisplayStyle.Flex,
                    flexGrow: 1,
                    flexShrink: 1,
                    flexDirection: FlexDirection.Row,
                    alignItems: Align.Center,
                    justifyContent: _justifyContent
                )
            );
        }
    }
}