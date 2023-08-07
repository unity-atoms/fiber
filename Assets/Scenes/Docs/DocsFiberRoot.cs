using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.GameObjects;
using Fiber.UIElements;
using Fiber.Suite;
using Signals;
using Fiber.UI;

public class DocsFiberRoot : MonoBehaviour
{
    [SerializeField]
    private PanelSettings _panelSettings;
    private FiberSuite _fiber;

    public class DocsAppComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            return new DocsThemes.Provider(children: F.Children(
                F.UIDocument(
                    name: "DocsDocument",
                    children: F.Children(
                        new DocsContentComponent()
                    )
                )
            ));
        }
    }

    public class DocsContentComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var theme = C<ThemeStore>().Get();
            var neutral = "neutral";
            var deepNeutral = "deepNeutral";

            return F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    height: new Length(100, LengthUnit.Percent),
                    width: new Length(100, LengthUnit.Percent),
                    alignItems: Align.Stretch,
                    flexDirection: FlexDirection.Row
                ),
                children: F.Children(
                    F.View(
                        style: new Style(
                            minWidth: 100,
                            maxWidth: 240,
                            width: new Length(25, LengthUnit.Percent),
                            height: new Length(100, LengthUnit.Percent),
                            backgroundColor: theme.DesignTokens[deepNeutral].Background.Default,
                            borderRightColor: theme.DesignTokens[deepNeutral].Border.Default,
                            borderRightWidth: 1,
                            flexShrink: 0,
                            flexGrow: 0
                        ),
                        children: F.Children(new TreeViewComponent.Container(
                            role: deepNeutral,
                            children: F.Children(
                                new TreeViewComponent.Item(id: "1", label: $"Item 1"),
                                new TreeViewComponent.Item(id: "2", label: $"Item 2", children: F.Children(
                                    new TreeViewComponent.Item(id: "2.1", label: $"Item 2.1"),
                                    new TreeViewComponent.Item(id: "2.2", label: $"Item 2.2", children: F.Children(
                                        new TreeViewComponent.Item(id: "2.2.1", label: $"Item 2.2.1 Item 2.2.1 Item 2.2.1"),
                                        new TreeViewComponent.Item(id: "2.2.2", label: $"Item 2.2.2")
                                    )),
                                    new TreeViewComponent.Item(id: "2.3", label: $"Item 2.3")
                                )),
                                new TreeViewComponent.Item(id: "3", label: $"Item 3")
                            )
                        ))
                    ),
                    F.View(
                        style: new Style(
                            backgroundColor: theme.DesignTokens[neutral].Background.Default,
                            minHeight: new Length(100, LengthUnit.Percent),
                            flexShrink: 1,
                            flexGrow: 1,
                            minWidth: new Length(100, LengthUnit.Pixel)
                        ),
                        children: F.Children(
                            F.Text(
                                style: new Style(
                                    color: theme.DesignTokens[neutral].Text.Default,
                                    fontSize: 36,
                                    paddingLeft: 18,
                                    paddingTop: 18,
                                    paddingRight: 18,
                                    paddingBottom: 18
                                ),
                                text: "Content"
                            )
                        )
                    )
                )
            );
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
