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

        public static DraggableComponent Draggable(
            this BaseComponent component,
            VirtualBody children,
            Ref<VisualElement> draggableRef
        )
        {
            return new DraggableComponent(
                children: children,
                draggableRef: draggableRef
            );
        }
    }

    public class DraggableContext
    {
        public Ref<VisualElement> DraggableRef { get; private set; }
        public DraggableContext(
            Ref<VisualElement> draggableRef
        )
        {
            DraggableRef = draggableRef;
        }
    }

    public class DraggableComponent : BaseComponent
    {
        private readonly Ref<VisualElement> _draggableRef;
        public DraggableComponent(
            VirtualBody children,
            Ref<VisualElement> draggableRef
        ) : base(children)
        {
            _draggableRef = draggableRef;
        }

        public override VirtualBody Render()
        {
            var context = F.GetContext<DragAndDropContext>();
            F.CreateEffect(() =>
            {
                context.RegisterDraggable(_draggableRef.Current);
                return () => context.UnregisterDraggable(_draggableRef.Current);
            });

            return F.ContextProvider(
                value: new DraggableContext(_draggableRef),
                children: Children
            );
        }
    }

    public class DragAndDropContext
    {
        private class DraggableElement
        {
            public VisualElement Element { get; private set; }
            public EventCallback<PointerDownEvent> OnPointerDown { get; private set; }
            public EventCallback<PointerMoveEvent> OnPointerMove { get; private set; }
            public EventCallback<PointerUpEvent> OnPointerUp { get; private set; }
            public EventCallback<PointerCaptureOutEvent> OnPointerCaptureOut { get; private set; }

            public DraggableElement(
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
        private List<DraggableElement> _draggableElements = new();

        public void RegisterDraggable(VisualElement element)
        {
            int itemIndex = _draggableElements.Count;
            VisualElement tempElement = null;

            var draggable = new DraggableElement(
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
            _draggableElements.Add(draggable);
            draggable.Register();
        }

        public void UnregisterDraggable(VisualElement element)
        {
            for (var i = 0; i < _draggableElements.Count; i++)
            {
                if (_draggableElements[i].Element == element)
                {
                    _draggableElements[i].Unregister();
                    _draggableElements.RemoveAt(i);
                    break;
                }
            }
        }

        public void RegisterDragHandle(Ref<VisualElement> dragHandle)
        {

        }

        public void UnregisterDragHandle(Ref<VisualElement> dragHandle)
        {

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