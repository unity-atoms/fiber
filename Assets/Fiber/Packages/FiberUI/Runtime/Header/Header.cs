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
                            flexGrow: 0,
                            flexShrink: 0,
                            backgroundColor: themeStore.Color(role, ElementType.Background, _variant)
                        )
                    )
                )
            );
        }
    }
}