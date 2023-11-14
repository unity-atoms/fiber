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
            var draggable = new Draggable(
                element: element,
                onPointerDown: (PointerDownEvent evt) =>
                {
                    _targetStartPosition = element.transform.position;
                    _pointerStartPosition = evt.position;
                    _draggedElement = element;
                    element.CapturePointer(evt.pointerId);
                },
                onPointerMove: (PointerMoveEvent evt) =>
                {
                    if (_draggedElement == element && element.HasPointerCapture(evt.pointerId))
                    {
                        var delta = evt.position - _pointerStartPosition;

                        element.transform.position = new Vector2(_targetStartPosition.x + delta.x, _targetStartPosition.y + delta.y);
                        // element.transform.position = new Vector2(
                        //     Mathf.Clamp(_targetStartPosition.x + delta.x, 0, element.panel.visualTree.worldBound.width),
                        //     Mathf.Clamp(_targetStartPosition.y + delta.y, 0, element.panel.visualTree.worldBound.height));
                    }
                },
                onPointerUp: (PointerUpEvent evt) =>
                {
                    if (_draggedElement == element && element.HasPointerCapture(evt.pointerId))
                    {
                        _draggedElement.ReleasePointer(evt.pointerId);
                        _draggedElement.transform.position = _targetStartPosition;
                        _draggedElement = null;
                    }
                },
                onPointerCaptureOut: (PointerCaptureOutEvent evt) =>
                {
                    if (_draggedElement == element)
                    {
                        _draggedElement.transform.position = _targetStartPosition;
                        _draggedElement = null;
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