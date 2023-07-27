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
                // var styles = C<Styles>();

                return F.View(
                    className: F.ClassName("fiber-treeView-container"),
                    children: children
                );
            }
        }
    }
}

