using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fiber;
using Fiber.GameObjects;
using Fiber.UIElements;
using Fiber.Suite;
using Signals;
using Fiber.UI;

public class DocsFiberRoot : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UIElements.PanelSettings _panelSettings;
    private FiberSuite _fiber;

    public class DocsAppComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var _children = new List<VirtualNode>();
            for (var i = 0; i < 10; i++)
            {
                _children.Add(new TreeView.Item(id: i.ToString(), label: $"Item {i}"));
            }

            return new DocsThemes.Provider(children: F.Children(
                F.UIDocument(
                    name: "DocsDocument",
                    children: F.Children(
                        new TreeView.Container(
                            role: "neutral",
                            children: F.Children(
                                new TreeView.Item(id: "1", label: $"Item 1"),
                                new TreeView.Item(id: "2", label: $"Item 2", children: F.Children(
                                    new TreeView.Item(id: "2.1", label: $"Item 2.1"),
                                    new TreeView.Item(id: "2.2", label: $"Item 2.2"),
                                    new TreeView.Item(id: "2.3", label: $"Item 2.3")
                                )),
                                new TreeView.Item(id: "3", label: $"Item 3")
                            )
                        )
                    )
                )
            ));
        }
    }

    void OnEnable()
    {
        if (_fiber == null)
        {
            _fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _panelSettings, globals: new()
            {
            });
        }
        if (!_fiber.IsMounted)
        {
            _fiber.Render(new DocsAppComponent());
        }
    }

    void OnDisable()
    {
        if (_fiber != null && _fiber.IsMounted)
        {
            _fiber.Unmount(immediatelyExecuteRemainingWork: true);
        }
    }
}
