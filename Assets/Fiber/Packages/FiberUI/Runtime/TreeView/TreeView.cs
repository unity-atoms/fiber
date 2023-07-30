using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

namespace Fiber.UI
{
    public class TreeView
    {
        public class Container : BaseComponent
        {
            private string _role;
            public Container(
                List<VirtualNode> children,
                string role = Constants.INHERIT_ROLE
            ) : base(children)
            {
                _role = role;
            }

            public override VirtualNode Render()
            {
                var theme = C<ThemeStore>().Get();
                var role = F.GetRole(_role);

                return F.RoleProvider(role: _role,
                    children: F.Children(F.View(
                        style: new Style(
                            backgroundColor: theme.DesignTokens[role].Background.Default,
                            borderRightColor: theme.DesignTokens[role].Border.Default,
                            borderRightWidth: 2,
                            height: 100,
                            width: 100
                        ),
                        children: children
                    ))
                );
            }
        }

        public class Item : BaseComponent
        {
            private SignalProp<string> _text;
            private string _role;

            public Item(
                SignalProp<string> text,
                List<VirtualNode> children = null,
                string role = Constants.INHERIT_ROLE
            ) : base(children)
            {
                _text = text;
                _role = role;
            }

            public override VirtualNode Render()
            {
                var _ref = new Ref<VisualElement>();
                var theme = C<ThemeStore>().Get();
                var role = F.GetRole(_role);
                var isHovered = new Signal<bool>(false);

                CreateEffect(() =>
                {
                    _ref.Current.RegisterCallback<MouseEnterEvent>(evt =>
                    {
                        isHovered.Value = true;
                    });
                    _ref.Current.RegisterCallback<MouseLeaveEvent>(evt =>
                    {
                        isHovered.Value = false;
                    });
                    return null;
                });

                var color = CreateComputedSignal<bool, Theme, StyleColor>((isHovered, theme) =>
                {
                    return isHovered && theme.DesignTokens[role].Text.Hovered.Get().keyword != StyleKeyword.Null ?
                        theme.DesignTokens[role].Text.Hovered.Get() : theme.DesignTokens[role].Text.Default.Get();
                }, isHovered, theme);

                var backgroundColor = CreateComputedSignal<bool, Theme, StyleColor>((isHovered, theme) =>
                {
                    return isHovered && theme.DesignTokens[role].Background.Hovered.Get().keyword != StyleKeyword.Null ?
                        theme.DesignTokens[role].Background.Hovered.Get() : theme.DesignTokens[role].Background.Default.Get();
                }, isHovered, theme);

                return F.View(
                    _ref: _ref,
                    style: new Style(backgroundColor: backgroundColor),
                    children: F.Children(F.Text(
                        text: _text,
                        style: new Style(color: color)
                    ))
                );
            }
        }
    }
}

