using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.Suite;
using Signals;
using SilkUI;

public class DocsFiberRoot : MonoBehaviour
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
            _fiber.Render(new DocsRouting.RouterProvider());
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
