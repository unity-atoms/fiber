using System.Collections.Generic;
using UnityEngine;
using Fiber.Suite;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using SilkUI;
using Signals;
using Fiber.DragAndDrop;
using FiberUtils;

public class DragAndDropExample : MonoBehaviour
{
    public static class Colors
    {
        public static ColorToken Dark = new("#212424");
        public static ColorToken Gray = new("#373B3B");
        public static ColorToken NeonBlue = new("#20B8C1");
        public static ColorToken NeonYellow = new("#FBEC37");
        public static ColorToken NeonRed = new("#FA0F38");
    }

    [SerializeField] private PanelSettings _defaultPanelSettings;

    private const string DEBUG_ROLE = "debug";

    public class ItemComponent : BaseComponent
    {
        private readonly int _id;
        private readonly BaseSignal<bool> _isDraggedSignal;
        public ItemComponent(int id, BaseSignal<bool> isDraggedSignal) : base()
        {
            _id = id;
            _isDraggedSignal = isDraggedSignal;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var dragHandleElement = C<DragHandleElement<int>>();
            var delayedIsDragged = new Signal<bool>(false);
            F.CreateEffect((isDragged) =>
            {
                var timeoutId = MonoBehaviourHelper.Instance.SetTimeout(() =>
                {
                    delayedIsDragged.Value = isDragged;
                }, 0.025f);
                return () =>
                {
                    MonoBehaviourHelper.Instance.ClearTimeout(timeoutId);
                };
            }, _isDraggedSignal);

            var borderColor = F.CreateComputedSignal<bool, bool, StyleColor>((isDragged, isHovered) =>
            {
                if (isDragged)
                {
                    return Colors.NeonYellow.Value;
                }
                else if (isHovered)
                {
                    return Colors.NeonBlue.Value;
                }

                return Colors.Gray.Value;
            }, delayedIsDragged, dragHandleElement.InteractiveElement.IsHovered);
            var textColor = F.CreateComputedSignal<bool, bool, StyleColor>((isDragged, isHovered) =>
            {
                if (isDragged)
                {
                    return Colors.NeonYellow.Value;
                }
                else if (isHovered)
                {
                    return Colors.NeonBlue.Value;
                }

                return Colors.NeonBlue.Value;
            }, delayedIsDragged, dragHandleElement.InteractiveElement.IsHovered);
            var statusTextBackgroundColor = F.CreateComputedSignal<bool, bool, StyleColor>((isDragged, isHovered) =>
            {
                if (isDragged)
                {
                    return Colors.NeonYellow.Value;
                }
                else if (isHovered)
                {
                    return Colors.NeonBlue.Value;
                }

                return Colors.Gray.Value;
            }, delayedIsDragged, dragHandleElement.InteractiveElement.IsHovered);
            var statusText = F.CreateComputedSignal((isDragged, isHovered) =>
            {
                if (isDragged)
                {
                    return "DRAGGING";
                }
                else if (isHovered)
                {
                    return "HOVERED";
                }

                return "IDLE";
            }, delayedIsDragged, dragHandleElement.InteractiveElement.IsHovered);
            var firaMono = Resources.Load<Font>("FiraMono/FiraMonoNerdFont-Regular");

            var dragDecaleSize = F.CreateComputedSignal<bool, StyleLength>((isDragged) =>
            {
                if (isDragged)
                {
                    return new Length(90, LengthUnit.Percent);
                }

                return new Length(40, LengthUnit.Percent);
            }, delayedIsDragged);

            var dragDecaleRotate = F.CreateComputedSignal<bool, StyleRotate>((isDragged) =>
            {
                if (isDragged)
                {
                    return new Rotate(45f + 1f * 360f);
                }

                return new Rotate(0f);
            }, delayedIsDragged);

            return F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    flexDirection: FlexDirection.Row,
                    justifyContent: Justify.Center,
                    alignItems: Align.Center
                ),
                children: F.Nodes(
                    F.View(
                        style: new Style(
                            position: Position.Absolute,
                            width: dragDecaleSize,
                            height: dragDecaleSize,
                            backgroundColor: Colors.Dark.Value,
                            transitionProperty: new List<StylePropertyName>() { new("all") },
                            transitionDuration: new List<TimeValue>() { new(0.5f, TimeUnit.Second) },
                            borderRightColor: Colors.NeonRed.Value,
                            borderLeftColor: Colors.NeonRed.Value,
                            borderTopColor: Colors.NeonRed.Value,
                            borderBottomColor: Colors.NeonRed.Value,
                            borderRightWidth: 4,
                            borderLeftWidth: 4,
                            borderTopWidth: 4,
                            borderBottomWidth: 4,
                            rotate: dragDecaleRotate
                        )
                    ),
                    F.View(
                        pickingMode: PickingMode.Position,
                        style: new Style(
                            transitionProperty: new List<StylePropertyName>() { new("all") },
                            transitionDuration: new List<TimeValue>() { new(0.3f, TimeUnit.Second) },
                            width: 88,
                            height: 88,
                            backgroundColor: Colors.Dark.Value,
                            borderRightColor: borderColor,
                            borderLeftColor: borderColor,
                            borderTopColor: borderColor,
                            borderBottomColor: borderColor,
                            borderRightWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            borderLeftWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            borderTopWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            borderBottomWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            marginRight: themeStore.Spacing(1),
                            marginBottom: themeStore.Spacing(1),
                            marginTop: themeStore.Spacing(1),
                            marginLeft: themeStore.Spacing(1),
                            display: DisplayStyle.Flex,
                            justifyContent: Justify.Center,
                            alignItems: Align.Center
                        ),
                        children: F.Nodes(
                            F.Text(
                                text: $"{_id}",
                                style: new Style(
                                    transitionProperty: new List<StylePropertyName>() { new("all") },
                                    transitionDuration: new List<TimeValue>() { new(0.3f, TimeUnit.Second) },
                                    color: textColor,
                                    unityFont: firaMono,
                                    unityFontDefinition: StyleKeyword.None,
                                    unityTextAlign: TextAnchor.MiddleCenter,
                                    fontSize: 32
                                )
                            ),
                            F.Text(
                                text: statusText,
                                style: new Style(
                                    transitionProperty: new List<StylePropertyName>() { new("all") },
                                    transitionDuration: new List<TimeValue>() { new(0.3f, TimeUnit.Second) },
                                    backgroundColor: statusTextBackgroundColor,
                                    color: Colors.Dark.Value,
                                    unityFont: firaMono,
                                    unityFontDefinition: StyleKeyword.None,
                                    unityTextAlign: TextAnchor.MiddleCenter,
                                    fontSize: 9,
                                    paddingBottom: 1,
                                    paddingLeft: 1,
                                    paddingRight: 1,
                                    paddingTop: 1,
                                    position: Position.Absolute,
                                    bottom: 0,
                                    right: 0
                                )
                            )
                        )
                    )
                )
            );
        }
    }

    public class ExampleContainer : BaseComponent
    {
        public override VirtualBody Render()
        {
            var items = new ShallowSignalList<int>(new List<int> { 1, 2, 3, 4, 5 });

            return F.SilkUIProvider(
                children: F.UIDocument(
                    children: F.View(
                        style: new Style(
                            flexDirection: FlexDirection.Column,
                            justifyContent: Justify.Center,
                            alignItems: Align.Center,
                            width: new Length(100, LengthUnit.Percent),
                            height: new Length(100, LengthUnit.Percent),
                            backgroundColor: Colors.Dark.Value
                        ),
                        children: F.Nodes(
                            F.View(
                                style: new Style(
                                    display: DisplayStyle.Flex,
                                    flexDirection: FlexDirection.Row,
                                    justifyContent: Justify.Center,
                                    alignItems: Align.Center,
                                    width: new Length(100, LengthUnit.Percent),
                                    height: new Length(120, LengthUnit.Pixel)
                                ),
                                children: F.DragAndDropList(
                                    items: items,
                                    createItemKey: (item, index) => item,
                                    renderItem: (item, index, isDraggedSignal) =>
                                    {
                                        return new ItemComponent(item, isDraggedSignal);
                                    },
                                    animationType: DragAndDropListAnimationType.Linear,
                                    isItemDragHandle: true
                                )
                            ),
                            F.Text(
                                text: "Fiber drag and drop demo",
                                style: new Style(
                                    color: Colors.NeonBlue.Value,
                                    unityFont: Resources.Load<Font>("FiraMono/FiraMonoNerdFont-Regular"),
                                    unityFontDefinition: StyleKeyword.None,
                                    unityTextAlign: TextAnchor.MiddleCenter,
                                    fontSize: 32
                                )
                            )
                        )
                    )
                )
            );
        }
    }

    private class ExampleRoot : BaseComponent
    {
        public ExampleRoot() : base() { }

        public override VirtualBody Render()
        {
            return F.SilkUIProvider(
                children: new ExampleContainer()
            );
        }
    }

    void Start()
    {
        var fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _defaultPanelSettings);
        fiber.Render(new ExampleRoot());
    }
}
