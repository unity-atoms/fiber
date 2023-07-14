using System;
using UnityEngine;
using Fiber;
using Fiber.GameObjects;
using Fiber.Suite;
using Signals;

public class RotatingCubesExample : MonoBehaviour
{
    [Serializable]
    public class Materials
    {
        public Material CubeDefault;
        public Material CubeHovered;
    }

    [SerializeField]
    private Materials _materials;

    public class CubeComponent : BaseComponent
    {
        private Vector3 _position;

        public CubeComponent(Vector3 position)
        {
            _position = position;
        }

        public override VirtualNode Render()
        {
            var _ref = new Ref<GameObject>();
            F.CreateUpdateEffect((deltaTime) =>
            {
                _ref.Current.transform.Rotate(new Vector3(25, 25, 25) * deltaTime);
            });

            var isHovered = new Signal<bool>(false);
            var clicked = new Signal<bool>(false);

            return F.GameObject(
                name: "Cube",
                _ref: _ref,
                position: _position,
                localScale: F.CreateComputedSignal((clicked) => clicked ? Vector3.one * 1.5f : Vector3.one, clicked),
                primitiveType: PrimitiveType.Cube,
                children: F.Children(
                    F.GameObjectPointerEvents(
                        onClick: () => { clicked.Value = !clicked.Value; },
                        onPointerEnter: () => { isHovered.Value = true; },
                        onPointerExit: () => { isHovered.Value = false; }
                    ),
                    F.MeshRenderer(
                        material: F.CreateComputedSignal((isHovered) => isHovered ?
                            G<Materials>().CubeHovered : G<Materials>().CubeDefault,
                            isHovered
                        )
                    )
                )
            );
        }
    }

    public class RotatingCubesComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            return F.GameObjectPointerEventsManager(F.Children(
                new CubeComponent(new Vector3(1.2f, 0, 0)),
                new CubeComponent(new Vector3(-1.2f, 0, 0))
            ));
        }
    }

    void Start()
    {
        var fiber = new FiberSuite(rootGameObject: gameObject, globals: new()
        {
            { typeof(Materials), _materials }
        });
        fiber.Render(new RotatingCubesComponent());
    }
}
