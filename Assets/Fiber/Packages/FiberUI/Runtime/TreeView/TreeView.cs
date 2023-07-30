using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fiber.UIElements;

namespace Fiber.UI
{
    public class TreeView
    {
        public class Container : BaseComponent
        {
            public string _role;
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

                return F.View(
                    className: F.ClassName("fiber-treeView-container"),
                    style: new Style(
                        backgroundColor: theme.DesignTokens["neutral"].Background.Regular.Default,
                        borderRightColor: theme.DesignTokens["neutral"].Border.Regular.Default,
                        borderRightWidth: 2
                    ),
                    children: F.Children(F.Text(text: "Test", style: new Style(color: theme.DesignTokens["neutral"].Text.Regular.Default)))
                );
            }
        }
    }
}

