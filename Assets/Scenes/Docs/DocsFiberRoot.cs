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
            return new DocsThemes.Provider(children: F.Children(
                F.UIDocument(
                    name: "DocsDocument",
                    children: F.Children(new TreeView.Container(role: "neutral", children: F.Children(
                        new TreeView.Item(text: "Item 1"),
                        new TreeView.Item(text: "Item 2"),
                        new TreeView.Item(text: "Item 3")
                    )))
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
