using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.Router;
using Fiber.UIElements;
using Fiber.Suite;
using Signals;
using SilkUI;
using Fiber.Cursed;

public class PerfFiberRoot : MonoBehaviour
{
    [SerializeField]
    private PanelSettings _panelSettings;
    private FiberSuite _fiber;

    void OnEnable()
    {
        if (_fiber == null)
        {
            _fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _panelSettings, globals: new() { });
        }
        if (!_fiber.IsMounted)
        {
            _fiber.Render(new PerfRootComponent());
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

public class PerfRootComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        var visibleState = F.CreateSignal(0);

        // var showPage1 = F.CreateComputedSignal((visibleState) => visibleState == 0, visibleState);
        // var showPage2 = F.CreateComputedSignal((visibleState) => visibleState == 1, visibleState);

        return F.CursorManager(
            cursorDefinitionsStore: new Store<ShallowSignalList<CursorDefinition>>(),
            children: F.Nodes(
                F.UIRoot(
                    name: "PerfRootDocument",
                    children: new PerfPage1(visibleState)
                ),
                F.UIRoot(
                    name: "PerfRootDocument2",
                    children: new PerfPage2(visibleState)
                )
            )
        );

        // return F.CursorManager(
        //     cursorDefinitionsStore: new Store<ShallowSignalList<CursorDefinition>>(),
        //     children: F.UIRoot(
        //         name: "PerfRootDocument",
        //         children: F.Nodes(
        //             F.Visible(
        //                 when: showPage1,
        //                 children: new PerfPage1(visibleState)
        //             ),
        //             F.Visible(
        //                 when: showPage2,
        //                 children: new PerfPage2(visibleState)
        //             )
        //         )
        //     )
        // );
    }
}

public class PerfPage1 : BaseComponent
{
    private readonly Signal<int> _visibleState;

    public PerfPage1(Signal<int> visibleState)
    {
        _visibleState = visibleState;
    }

    public override VirtualBody Render()
    {
        var translate = F.CreateComputedSignal<int, StyleTranslate>((visibleState) => visibleState == 0 ? new Translate(0f, 0f) : new Translate(-5000f, -5000f), _visibleState);
        var opacity = F.CreateComputedSignal<int, StyleFloat>((visibleState) => visibleState == 0 ? 1f : 0f, _visibleState);
        var pickingMode = F.CreateComputedSignal<int, PickingMode>((visibleState) => visibleState == 0 ? PickingMode.Position : PickingMode.Ignore, _visibleState);

        return F.View(
            style: new Style(
                // translate: translate,
                opacity: opacity
            ),
            pickingMode: pickingMode,
            usageHints: UsageHints.DynamicTransform,
            // style: new Style(
            //     display: DisplayStyle.Flex,
            //     width: new Length(100, LengthUnit.Percent),
            //     alignItems: Align.FlexStart,
            //     flexDirection: FlexDirection.Column
            // ),
            children: F.Nodes(
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Text(text: "Page 1"),
                F.Fragment(F.Nodes(
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2"),
                    F.Text(text: "Page 1.2")
                )),
                F.Button(
                    text: "Go to Page 2",
                    pickingMode: pickingMode,
                    onClick: (e) =>
                    {
                        _visibleState.Value = 1;
                    }
                )
            )
        );
    }
}

public class PerfPage2 : BaseComponent
{
    private readonly Signal<int> _visibleState;

    public PerfPage2(Signal<int> visibleState)
    {
        _visibleState = visibleState;
    }

    public override VirtualBody Render()
    {
        var translate = F.CreateComputedSignal<int, StyleTranslate>((visibleState) => visibleState == 1 ? new Translate(0f, 0f) : new Translate(-5000f, -5000f), _visibleState);
        var opacity = F.CreateComputedSignal<int, StyleFloat>((visibleState) => visibleState == 1 ? 1f : 0f, _visibleState);
        var pickingMode = F.CreateComputedSignal<int, PickingMode>((visibleState) => visibleState == 1 ? PickingMode.Position : PickingMode.Ignore, _visibleState);

        return F.View(
            style: new Style(
                // translate: translate,
                opacity: opacity
            ),
            pickingMode: pickingMode,
            usageHints: UsageHints.DynamicTransform,
            // style: new Style(
            //     display: DisplayStyle.Flex,
            //     width: new Length(100, LengthUnit.Percent),
            //     alignItems: Align.FlexStart,
            //     flexDirection: FlexDirection.Column
            // ),
            children: F.Nodes(
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Text(text: "Page 2"),
                F.Fragment(F.Nodes(
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2"),
                    F.Text(text: "Page 2.2")
                )),
                F.Button(
                    text: "Go to Page 1",
                    // pickingMode: pickingMode,
                    onClick: (e) =>
                    {
                        _visibleState.Value = 0;
                    }
                )
            )
        );
    }
}

