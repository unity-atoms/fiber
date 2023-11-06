using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.Router;
using Fiber.UIElements;
using Fiber.Suite;
using Signals;
using SilkUI;

public class DocsFiberRoot : MonoBehaviour
{
    [SerializeField]
    private PanelSettings _panelSettings;
    private FiberSuite _fiber;
    [SerializeField]
    private Router _router;

    void OnEnable()
    {
        if (_fiber == null)
        {
            _fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _panelSettings, globals: new() { });
        }
        if (!_fiber.IsMounted)
        {
            _router = new Router(DocsRouting.ROUTER_TREE).Navigate(DocsRouting.ROUTES.LANDING);
            _fiber.Render(new DocsRouting.RouterProvider(_router));
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
