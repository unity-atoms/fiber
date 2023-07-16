// A copy of the example in Fiber.md

using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.GameObjects;
using Signals;

namespace Fiber.Suite
{
    public class CounterExample : MonoBehaviour
    {
        [SerializeField] private PanelSettings _defaultPanelSettings;
        public class CounterComponent : BaseComponent
        {
            public override VirtualNode Render()
            {
                var count = new Signal<int>(0);

                return F.UIDocument(
                    children: F.Children(
                        F.Button(text: "Increment", onClick: (e) => { count.Value += 1; }),
                        F.Text(text: new IntToStringSignal(count))
                    )
                );
            }
        }

        void Start()
        {
            var fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _defaultPanelSettings);
            fiber.Render(new CounterComponent());
        }
    }

    public class PhysicsObjectComponent : BaseComponent
    {
        BaseSignal<bool> IsKinematicSignal;
        public PhysicsObjectComponent(BaseSignal<bool> isKinematicSignal)
        {
            IsKinematicSignal = isKinematicSignal;
        }
        public override VirtualNode Render()
        {
            var _ref = new Ref<GameObject>();
            CreateEffect((isKinematic) =>
            {
                _ref.Current.GetComponent<Rigidbody>().isKinematic = isKinematic;
                return null;
            }, IsKinematicSignal, runOnMount: true);
            return F.GameObject(_ref: _ref, getInstance: (parentGo) =>
            {
                var go = new GameObject();
                go.AddComponent<Rigidbody>();
                return go;
            });
        }
    }
}