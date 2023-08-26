using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using UnityEngine.InputSystem;
using Signals;
using FiberUtils;

namespace Fiber.Suite
{
    public class FiberSuiteTest : MonoBehaviour
    {
        [SerializeField]
        private PanelSettings _defaultPanelSettings;

        private FiberSuite _fiber;

        void Start()
        {
            _fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _defaultPanelSettings);
            _fiber.Render(new Test());
        }

        void Update()
        {
            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                if (_fiber.IsMounted)
                {
                    _fiber.Unmount();
                }
                else
                {
                    _fiber.Render(new Test());
                }
            }
        }

        public class TrafficLightComponent : BaseComponent
        {
            public class IsGreenSignal : ComputedSignal<string, bool>
            {
                public IsGreenSignal(Signal<string> textSignal) : base(textSignal) { }
                protected override bool Compute(string text)
                {
                    return text == "green";
                }
            }

            public class IsYellowSignal : ComputedSignal<string, bool>
            {
                public IsYellowSignal(Signal<string> textSignal) : base(textSignal) { }
                protected override bool Compute(string text)
                {
                    return text == "yellow";
                }
            }

            public override VirtualNode Render()
            {
                var textSignal = new Signal<string>("red");
                var isGreenSignal = new IsGreenSignal(textSignal);
                var isYellowSignal = new IsYellowSignal(textSignal);

                return (
                    F.View(
                        children: Children(
                            F.TextField(
                                style: new Style(
                                    color: Color.black,
                                    fontSize: 20
                                ),
                                name: "TrafficLight_TextField",
                                value: textSignal,
                                onChange: (e) =>
                                {
                                    textSignal.Value = e.newValue;
                                }
                            ),
                            Switch(
                                fallback: F.Text(text: "Red", style: new Style(color: Color.red)),
                                children: Children(
                                    Match(when: isGreenSignal, children: Children(F.Text(text: "Green", style: new Style(color: Color.green)))),
                                    Match(when: isYellowSignal, children: Children(F.Text(text: "Yellow", style: new Style(color: Color.yellow))))
                                )
                            )
                        )
                    )
                );
            }
        }

        public class Test : BaseComponent
        {
            private class TodoItem
            {
                public int Id { get; set; }
                public string Text { get; set; }
                public bool IsDone { get; set; }
                private static IntIdGenerator idGenerator = new IntIdGenerator();

                public TodoItem(string text, bool isDone)
                {
                    Id = idGenerator.NextId();
                    Text = Id.ToString();
                    IsDone = isDone;
                }
            }

            public override VirtualNode Render()
            {
                var showSingal = new Signal<bool>(false);
                var showNestedSingal = new Signal<bool>(false);
                var valueSignal = new Signal<string>("0");
                var todoItemsSignal = new ShallowSignalList<TodoItem>(new ShallowSignalList<TodoItem>());

                return F.UIDocument(
                    name: "FiberTestUIDocument",
                    children: Children(
                        F.View(
                            style: new Style(
                                backgroundColor: new Color(1f, 0f, 0f, 1f)
                            ),
                            pickingMode: PickingMode.Position,
                            name: "FiberTestRoot",
                            children: Children(
                                F.TextField(
                                    style: new Style(
                                        color: Color.black,
                                        fontSize: 20
                                    ),
                                    name: "TextField",
                                    value: valueSignal,
                                    onChange: (e) =>
                                    {
                                        valueSignal.Value = e.newValue;
                                    }
                                ),
                                F.Button(
                                    style: new Style(
                                        color: Color.black,
                                        fontSize: 20
                                    ),
                                    name: "AddButton",
                                    text: "Add",
                                    onClick: (e) =>
                                    {
                                        var index = int.Parse(valueSignal.Value);
                                        todoItemsSignal.Insert(index, new TodoItem("TODO", false));
                                        valueSignal.Value = "0";
                                    }
                                ),
                                F.Button(
                                    style: new Style(
                                        color: Color.black,
                                        fontSize: 20
                                    ),
                                    name: "RemoveButton",
                                    text: "Remove",
                                    onClick: (e) =>
                                    {
                                        if (todoItemsSignal.Count > 0)
                                        {
                                            var index = int.Parse(valueSignal.Value);
                                            todoItemsSignal.RemoveAt(index);
                                            valueSignal.Value = "0";
                                        }
                                    }
                                ),
                                F.Button(
                                    style: new Style(
                                        color: Color.black,
                                        fontSize: 20
                                    ),
                                    name: "RandomReorder",
                                    text: "Reorder",
                                    onClick: (e) =>
                                    {
                                        for (var i = 0; i < todoItemsSignal.Count; ++i)
                                        {
                                            var randomIndex = UnityEngine.Random.Range(0, todoItemsSignal.Count);
                                            var temp = todoItemsSignal[randomIndex];
                                            todoItemsSignal[randomIndex] = todoItemsSignal[i];
                                            todoItemsSignal[i] = temp;
                                        }
                                    }
                                ),
                                F.Button(
                                    style: new Style(
                                        color: Color.black,
                                        fontSize: 20
                                    ),
                                    name: "AddAndRemoveButton",
                                    text: "Add and remove",
                                    onClick: (e) =>
                                    {
                                        if (todoItemsSignal.Count > 0)
                                        {
                                            todoItemsSignal.Insert(0, new TodoItem("TODO", false));
                                            todoItemsSignal.RemoveAt(todoItemsSignal.Count - 1);
                                            valueSignal.Value = "0";
                                        }
                                    }
                                ),
                                For<TodoItem, ShallowSignalList<TodoItem>, int>(
                                    each: todoItemsSignal,
                                    children: (item, i) =>
                                    {
                                        return (item.Id, For<TodoItem, ShallowSignalList<TodoItem>, int>(
                                            each: todoItemsSignal,
                                            children: (item, i) =>
                                            {
                                                return (item.Id, F.Text(text: item.Text));
                                            }
                                        ));
                                    }
                                ),
                                F.Button(
                                    style: new Style(
                                        color: Color.black,
                                        fontSize: 20
                                    ),
                                    name: "ToggleButton",
                                    text: "Toggle",
                                    onClick: (e) =>
                                    {
                                        showSingal.Value = !showSingal.Value;
                                    }
                                ),
                                Mount(
                                    when: showSingal,
                                    children: Children(
                                         F.Button(
                                            style: new Style(
                                                color: Color.black,
                                                fontSize: 20
                                            ),
                                            name: "NestedToggle",
                                            text: "Nested Toggle",
                                            onClick: (e) =>
                                            {
                                                showNestedSingal.Value = !showNestedSingal.Value;
                                            }
                                        ),
                                        Mount(
                                            when: showNestedSingal,
                                            children: Children(
                                                F.Text(text: "Nested Text")
                                            )
                                        )
                                    )
                                ),
                                new TrafficLightComponent()
                            )
                        )
                    )
                );
            }
        }
    }
}