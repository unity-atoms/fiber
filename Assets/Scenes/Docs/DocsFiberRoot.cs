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

    public class DocsAppComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            return F.Fragment(children: F.Children(
                F.UIDocument(
                    name: "DocsDocument",
                    children: F.Children(new TreeView.Container(F.Children(), role: "neutral"))
                )
            ));
        }
    }

    void Start()
    {
        var fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _panelSettings, globals: new()
        {
        });
        fiber.Render(new DocsAppComponent());
    }
}
