using System;
using Fiber.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

namespace Fiber.DragAndDrop
{

    public static partial class BaseComponentExtensions
    {
        public static DragAndDropProviderComponent DragAndDropProvider(
            this BaseComponent component,
            VirtualBody children
        )
        {
            return new DragAndDropProviderComponent(
                children: children
            );
        }

        public static void CreateDraggable(
            this BaseComponent component,
            Ref<VisualElement> _ref
        )
        {
            var context = component.GetContext<DragAndDropContext>();
            component.CreateEffect(() =>
            {
                context.RegisterDraggable(_ref.Current);
                return () => context.UnregisterDraggable(_ref.Current);
            });
        }
    }

    public class DragAndDropContext
    {
        private class Draggable
        {
            public VisualElement Element { get; private set; }
            public EventCallback<PointerDownEvent> OnPointerDown { get; private set; }
            public EventCallback<PointerMoveEvent> OnPointerMove { get; private set; }
            public EventCallback<PointerUpEvent> OnPointerUp { get; private set; }
            public EventCallback<PointerCaptureOutEvent> OnPointerCaptureOut { get; private set; }

            public Draggable(
                VisualElement element,
                EventCallback<PointerDownEvent> onPointerDown,
                EventCallback<PointerMoveEvent> onPointerMove,
                EventCallback<PointerUpEvent> onPointerUp,
                EventCallback<PointerCaptureOutEvent> onPointerCaptureOut
            )
            {
                Element = element;
                OnPointerDown = onPointerDown;
                OnPointerMove = onPointerMove;
                OnPointerUp = onPointerUp;
                OnPointerCaptureOut = onPointerCaptureOut;
            }

            public void Register()
            {
                Element.RegisterCallback(OnPointerDown);
                Element.RegisterCallback(OnPointerMove);
                Element.RegisterCallback(OnPointerUp);
                Element.RegisterCallback(OnPointerCaptureOut);
            }

            public void Unregister()
            {
                Element.UnregisterCallback(OnPointerDown);
                Element.UnregisterCallback(OnPointerMove);
                Element.UnregisterCallback(OnPointerUp);
                Element.UnregisterCallback(OnPointerCaptureOut);
            }
        }

        private Vector3 _targetStartPosition;
        private Vector3 _pointerStartPosition;
        private VisualElement _draggedElement;
        private List<Draggable> _draggables = new();

        public void RegisterDraggable(VisualElement element)
        {
            int itemIndex = _draggables.Count;
            VisualElement tempElement = null;

            var draggable = new Draggable(
                element: element,
                onPointerDown: (PointerDownEvent evt) =>
                {
                    _pointerStartPosition = evt.position;

                    Vector3 targetFlexedWorldBound = element.worldBound.center;
                    Vector3 targetAbsoluteWorldBound = element.parent.worldBound.center;

                    Vector3 deltaTargetWorldBound = targetFlexedWorldBound - targetAbsoluteWorldBound;

                    _targetStartPosition = deltaTargetWorldBound;
                    element.transform.position = _targetStartPosition;

                    _draggedElement = element;
                    element.CapturePointer(evt.pointerId);
                    element.style.position = Position.Absolute;

                    var width = element.resolvedStyle.width;
                    var height = element.resolvedStyle.height;
                    var marginLeft = element.resolvedStyle.marginLeft;
                    var marginTop = element.resolvedStyle.marginTop;
                    var marginRight = element.resolvedStyle.marginRight;
                    var marginBottom = element.resolvedStyle.marginBottom;

                    tempElement ??= new VisualElement();
                    tempElement.style.width = width;
                    tempElement.style.height = height;
                    tempElement.style.marginLeft = marginLeft;
                    tempElement.style.marginTop = marginTop;
                    tempElement.style.marginRight = marginRight;
                    tempElement.style.marginBottom = marginBottom;
                    tempElement.style.backgroundColor = Color.red;

                    element.parent.Insert(itemIndex, tempElement);

                    element.MoveToBack();
                    // var currentIndex = element.parent.IndexOf(element);
                    // var targetIndex = element.parent.childCount;
                    // if (currentIndex > targetIndex)
                    // {
                    //     var currentElementAtIndex = element.parent.ElementAt(targetIndex);
                    //     element.PlaceBehind(currentElementAtIndex);
                    // }
                    // else if (currentIndex < targetIndex)
                    // {
                    //     var currentElementBeforeIndex = element.parent.ElementAt(targetIndex - 1);
                    //     element.PlaceInFront(currentElementBeforeIndex);
                    // }
                },
                onPointerMove: (PointerMoveEvent evt) =>
                {
                    if (_draggedElement == element && element.HasPointerCapture(evt.pointerId))
                    {
                        var delta = evt.position - _pointerStartPosition;
                        element.transform.position = new Vector2(_targetStartPosition.x + delta.x, _targetStartPosition.y + delta.y);
                    }
                },
                onPointerUp: (PointerUpEvent evt) =>
                {
                    if (_draggedElement == element && element.HasPointerCapture(evt.pointerId))
                    {
                        _draggedElement.ReleasePointer(evt.pointerId);
                        _draggedElement.transform.position = Vector3.zero;
                        _draggedElement = null;
                        element.style.position = Position.Relative;
                        tempElement.parent.Remove(tempElement);
                        element.MoveToIndex(itemIndex);
                    }
                },
                onPointerCaptureOut: (PointerCaptureOutEvent evt) =>
                {
                    if (_draggedElement == element)
                    {
                        _draggedElement.transform.position = Vector3.zero;
                        _draggedElement = null;
                        element.style.position = Position.Relative;
                        tempElement.parent.Remove(tempElement);
                        element.MoveToIndex(itemIndex);
                    }
                }
            );
            _draggables.Add(draggable);
            draggable.Register();
        }

        public void UnregisterDraggable(VisualElement element)
        {
            for (var i = 0; i < _draggables.Count; i++)
            {
                if (_draggables[i].Element == element)
                {
                    _draggables[i].Unregister();
                    _draggables.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class DragAndDropProviderComponent : BaseComponent
    {

        public DragAndDropProviderComponent(VirtualBody children) : base(children) { }

        public override VirtualBody Render()
        {
            return F.ContextProvider(
                value: new DragAndDropContext(),
                children: Children
            );
        }
    }

}