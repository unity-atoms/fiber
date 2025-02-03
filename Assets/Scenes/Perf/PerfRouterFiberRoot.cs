using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.Router;
using Fiber.UIElements;
using Fiber.Suite;
using Signals;
using SilkUI;
using Fiber.Cursed;

public class PerfRouterFiberRoot : MonoBehaviour
{
    public static class ROUTES
    {
        public const string ROOT = "root";
        public const string PAGE1 = "page1";
        public const string PAGE2 = "page2";
    }

    public static readonly RouteDefinition ROUTER_TREE = new RouteDefinition(
        id: ROUTES.ROOT,
        isLayoutRoute: true,
        component: new SimpleRouteComponent(new RootComponent()),
        children: new List<RouteDefinition>()
        {
            new RouteDefinition(
                id: ROUTES.PAGE1,
                isLayoutRoute: false,
                component: new ActiveRouteComponent(component: new Page1())
            ),
            new RouteDefinition(
                id: ROUTES.PAGE2,
                isLayoutRoute: false,
                component: new ActiveRouteComponent(component: new Page2())
            )
        }
    );


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
            var router = new Router(ROUTER_TREE).Navigate(ROUTES.PAGE1);
            _fiber.Render(new RouterProvider(router));
        }
    }

    void OnDisable()
    {
        if (_fiber != null && _fiber.IsMounted)
        {
            _fiber.Unmount(immediatelyExecuteRemainingWork: true);
        }
    }

    public class RootComponent : BaseComponent
    {
        public override VirtualBody Render()
        {
            return F.CursorManager(
                cursorDefinitionsStore: new Store<ShallowSignalList<CursorDefinition>>(),
                children: F.Outlet()
            );
        }
    }

    public class Page1 : BaseComponent
    {
        public override VirtualBody Render()
        {
            return F.UIRoot(
                name: "Page1",
                children: F.View(
                    usageHints: UsageHints.DynamicTransform,
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
                            onClick: (e) =>
                            {
                                var router = C<Router>();
                                router.Navigate(ROUTES.PAGE2);
                                Debug.Break();
                            }
                        )
                    )
                )
            );
        }
    }

    public class Page2 : BaseComponent
    {
        public override VirtualBody Render()
        {
            return F.UIRoot(
                name: "Page2",
                children: F.View(
                    usageHints: UsageHints.DynamicTransform,
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
                            onClick: (e) =>
                            {
                                var router = C<Router>();
                                router.Navigate(ROUTES.PAGE1);
                            }
                        )
                    )
                )
            );
        }
    }
}


