using System;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using Signals;
using Fiber.UIElements;
using FiberUtils;

namespace Fiber.DragAndDrop
{
    public static partial class BaseComponentExtensions
    {
        public static DragAndDropProviderComponent<T> DragAndDropProvider<T>(
            this BaseComponent component,
            VirtualBody children
        )
        {
            return new DragAndDropProviderComponent<T>(
                children: children
            );
        }

        public static DroppableComponent<T> Droppable<T>(
            this BaseComponent component,
            T value,
            VirtualBody children,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new DroppableComponent<T>(
                children: children,
                value: value,
                style: style,
                forwardRef: forwardRef
            );
        }

        public static DraggableComponent<T> Draggable<T>(
            this BaseComponent component,
            VirtualBody children,
            T value,
            Style style = new(),
            bool isDragHandle = false,
            Action onDragStart = null,
            Action onDragEnd = null,
            Action onDragMove = null
        )
        {
            return new DraggableComponent<T>(
                children: children,
                value: value,
                style: style,
                isDragHandle: isDragHandle,
                onDragStart: onDragStart,
                onDragEnd: onDragEnd,
                onDragMove: onDragMove
            );
        }

        public static DragHandleComponent<T> DragHandle<T>(
            this BaseComponent component,
            VirtualBody children
        )
        {
            return new DragHandleComponent<T>(
                children: children
            );
        }
    }

    public class DraggableContext<T>
    {
        public T Value { get; private set; }
        public Ref<VisualElement> DraggableRef { get; private set; }
        public DraggableContext(
            T value,
            Ref<VisualElement> draggableRef
        )
        {
            Value = value;
            DraggableRef = draggableRef;
        }
    }

    public class DragAndDropListItem<T> : BaseComponent
    {
        public override VirtualBody Render()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DragHandleComponent<T> : BaseComponent
    {
        public DragHandleComponent(
            VirtualBody children
        ) : base(children)
        {
        }

        public override VirtualBody Render()
        {
            var dragHandleRef = new Ref<VisualElement>();
            var context = F.GetContext<DragAndDropContext<T>>();
            var dragableContext = F.GetContext<DraggableContext<T>>();
            F.CreateEffect(() =>
            {
                context.RegisterDragHandle(dragableContext.DraggableRef, dragHandleRef);
                return () =>
                {
                    context.UnregisterDragHandle(dragableContext.DraggableRef, dragHandleRef);
                };
            });

            return F.View(
                _ref: dragHandleRef,
                children: Children
            );
        }
    }

    public class DraggableComponent<T> : BaseComponent
    {
        private readonly T _value;
        private readonly bool _isDragHandle;
        private readonly Style _style;
        private Action _onDragStart;
        private Action _onDragEnd;
        private Action _onDragMove;

        public DraggableComponent(
            VirtualBody children,
            T value,
            Style style = new(),
            bool isDragHandle = false,
            Action onDragStart = null,
            Action onDragEnd = null,
            Action onDragMove = null
        ) : base(children)
        {
            _value = value;
            _style = style;

            if (!_style.Position.IsEmpty)
            {
#if UNITY_EDITOR
                Debug.LogWarning("DraggableComponent: Position style is reserved for internal use. Ignoring style.");
#endif
                // OPEN POINT: Maybe we want to add a non-destructive merge to Style?
                // If we support it in the future we should use it in the Render method below
                // instead of just overwriting the styles like this.
                _style = new();
            }

            _isDragHandle = isDragHandle;
        }

        public override VirtualBody Render()
        {
            var position = new Signal<StyleEnum<Position>>(Position.Relative);
            var parentRef = new Ref<VisualElement>();
            var draggableRef = new Ref<VisualElement>();
            var context = F.GetContext<DragAndDropContext<T>>();
            var destinationId = new Signal<string>(null);

            F.CreateEffect(() =>
            {
                context.RegisterDraggable(
                    value: _value,
                    draggableRef: draggableRef,
                    isDragHandle: _isDragHandle,
                    positionSignal: position,
                    destinationIdSignal: destinationId,
                    onDragStart: _onDragStart,
                    onDragEnd: _onDragEnd,
                    onDragMove: _onDragMove
                );
                return () =>
                {
                    context.UnregisterDraggable(draggableRef);
                };
            });

            F.CreateEffect(() =>
            {
                EventCallback<GeometryChangedEvent> onGeometryChanged = null;
                onGeometryChanged = new((e) =>
                {
                    var resolvedStyle = parentRef.Current.resolvedStyle;
                    parentRef.Current.style.width = resolvedStyle.width;
                    parentRef.Current.style.height = resolvedStyle.height;
                    draggableRef.Current.UnregisterCallback(onGeometryChanged);
                });
                draggableRef.Current.RegisterCallback(onGeometryChanged);
                return () =>
                {
                    draggableRef.Current.UnregisterCallback(onGeometryChanged);
                };
            });

            return F.ContextProvider(
                value: new DraggableContext<T>(_value, draggableRef),
                children: F.View(
                    _ref: parentRef,
                    style: new Style(
                        paddingBottom: 0,
                        paddingLeft: 0,
                        paddingRight: 0,
                        paddingTop: 0,
                        marginBottom: 0,
                        marginLeft: 0,
                        marginRight: 0,
                        marginTop: 0,
                        borderBottomWidth: 0,
                        borderLeftWidth: 0,
                        borderRightWidth: 0,
                        borderTopWidth: 0
                    ),
                    children: F.Portal(
                        destinationId: destinationId,
                        children: F.View(
                            _ref: draggableRef,
                            style: new Style(
                                mergedStyle: _style,
                                position: position
                            ),
                            children: Children
                        )
                    )
                )
            );
        }
    }

    public class DroppableComponent<T> : BaseComponent
    {
        private readonly T _value;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;
        public DroppableComponent(
            VirtualBody children,
            T value,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        ) : base(children)
        {
            _value = value;
            _style = style;
            _forwardRef = forwardRef;
        }

        public override VirtualBody Render()
        {
            var droppableRef = _forwardRef ?? new Ref<VisualElement>();
            var context = F.GetContext<DragAndDropContext<T>>();
            F.CreateEffect(() =>
            {
                context.RegisterDroppable(_value, droppableRef);
                return () =>
                {
                    context.UnregisterDroppable(droppableRef);
                };
            });

            return F.View(
                _ref: droppableRef,
                style: _style,
                children: Children
            );
        }
    }

    public class DragAndDropContext<T>
    {
        private class DragHandle
        {
            public Ref<VisualElement> Ref { get; private set; }
            public Draggable Draggable { get; private set; }
            public DragAndDropContext<T> Context { get; private set; }

            public DragHandle(
                Ref<VisualElement> _ref,
                Draggable draggable,
                DragAndDropContext<T> context
            )
            {
                Ref = _ref;
                Draggable = draggable;
                Context = context;
            }

            public void RegisterCallbacks()
            {
                Ref.Current.RegisterCallback<PointerDownEvent>(OnPointerDown);
                Ref.Current.RegisterCallback<PointerMoveEvent>(OnPointerMove);
                Ref.Current.RegisterCallback<PointerUpEvent>(OnPointerUp);
                Ref.Current.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
            }

            public void UnregisterCallbacks()
            {
                Ref.Current.UnregisterCallback<PointerDownEvent>(OnPointerDown);
                Ref.Current.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
                Ref.Current.UnregisterCallback<PointerUpEvent>(OnPointerUp);
                Ref.Current.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
            }

            private void OnPointerDown(PointerDownEvent evt)
            {
                if (Context.CurrentlyDragged.Value != null)
                {
                    return;
                }

                Context.PointerStartPosition = evt.position;

                Context.CurrentlyDragged.Value = Draggable;
                Ref.Current.CapturePointer(evt.pointerId);

                // The portal destination transition might not happen directly, but we want to keep the dragged
                // element's position. We do that by: 
                // 1) Making the parent of the dragged element always be kept in placen with the same dimensions.
                // 2) Absolute position the portal destination, set it's size to the same as that of the draggable,
                //  and move it to the exact same location as the draggable.

                // Absolute position the dragged element and change its parent using the portal destination id.
                Draggable.PositionSignal.Value = Position.Absolute;
                Draggable.DestinationIdSignal.Value = Draggable.Context.PortalDestinationId;

                // Get the width, height and world position of the draggable
                var draggableElement = Draggable.Ref.Current;
                var draggableWidth = draggableElement.resolvedStyle.width;
                var draggableHeight = draggableElement.resolvedStyle.height;
                var draggableWorldPosition = draggableElement.worldBound.center;

                // Place portal destination at the same position as the dragged element and make it the same size
                var portalDestination = Draggable.Context.PortalDestinationRef.Current;
                var portalWorldPosition = portalDestination.worldBound.center;
                var portalDestinationStartPosition = (Vector3)draggableWorldPosition - (Vector3)portalWorldPosition + portalDestination.transform.position;
                Context.TargetStartPosition = portalDestinationStartPosition;
                portalDestination.transform.position = portalDestinationStartPosition;
                portalDestination.style.width = draggableWidth;
                portalDestination.style.height = draggableHeight;

                Context.ClosestDroppable.Value = GetClosestDroppable();

                Draggable.OnDragStart?.Invoke();
            }

            private void OnPointerMove(PointerMoveEvent evt)
            {
                if (Context.CurrentlyDragged.Value == Draggable && Ref.Current.HasPointerCapture(evt.pointerId))
                {
                    var portalDestination = Draggable.Context.PortalDestinationRef.Current;
                    var deltaMousePos = evt.position - Context.PointerStartPosition;
                    var newPos = new Vector2(
                        Context.TargetStartPosition.x + deltaMousePos.x,
                        Context.TargetStartPosition.y + deltaMousePos.y
                    );
                    portalDestination.transform.position = newPos;

                    var closestDroppable = GetClosestDroppable();
                    if (closestDroppable != Context.ClosestDroppable.Value)
                    {
                        Context.ClosestDroppable.Value = closestDroppable;
                    }

                    Draggable.OnDragMove?.Invoke();
                }
            }

            private void OnPointerUp(PointerUpEvent evt)
            {
                if (Context.CurrentlyDragged.Value == Draggable && Ref.Current.HasPointerCapture(evt.pointerId))
                {
                    Ref.Current.ReleasePointer(evt.pointerId);
                    Release();
                }
            }

            private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
            {
                if (Context.CurrentlyDragged.Value == Draggable)
                {
                    Release();
                }
            }

            private Droppable GetClosestDroppable()
            {
                var portalDestination = Draggable.Context.PortalDestinationRef.Current;
                Droppable droppable = null;

                Vector3 smallestDelta = Vector2.one * float.MaxValue;
                for (var i = 0; i < Context.Droppables.Count; ++i)
                {
                    var currentDroppable = Context.Droppables[i];
                    var droppableWorldPos = currentDroppable.Ref.Current.worldBound.center;
                    var delta = droppableWorldPos - portalDestination.worldBound.center;

                    if (delta.magnitude < smallestDelta.magnitude)
                    {
                        smallestDelta = delta;
                        droppable = currentDroppable;
                    }
                }

                return droppable;
            }

            private void Release()
            {
                Context.CurrentlyDragged.Value.Ref.Current.transform.position = Vector3.zero;
                Context.CurrentlyDragged.Value = null;
                Context.ClosestDroppable.Value = null;
                Draggable.DestinationIdSignal.Value = null;
                Draggable.PositionSignal.Value = Position.Relative;
                Draggable.OnDragEnd?.Invoke();
            }
        }

        public class Draggable
        {
            public T Value { get; private set; }
            public Ref<VisualElement> Ref { get; private set; }
            private readonly bool _isDragHandle;
            private List<DragHandle> DragHandles { get; set; } = new();
            public DragAndDropContext<T> Context { get; private set; }
            public Action OnDragStart { get; private set; }
            public Action OnDragEnd { get; private set; }
            public Action OnDragMove { get; private set; }
            public Signal<StyleEnum<Position>> PositionSignal { get; private set; }
            public Signal<string> DestinationIdSignal { get; set; }

            public Draggable(
                T value,
                Ref<VisualElement> _ref,
                DragAndDropContext<T> context,
                bool isDragHandle,
                Signal<StyleEnum<Position>> positionSignal,
                Signal<string> destinationIdSignal,
                Action onDragStart,
                Action onDragEnd,
                Action onDragMove
            )
            {
                Value = value;
                Ref = _ref;
                Context = context;
                _isDragHandle = isDragHandle;
                PositionSignal = positionSignal;
                DestinationIdSignal = destinationIdSignal;

                if (_isDragHandle)
                {
                    RegisterDragHandle(_ref, context);
                }

                OnDragStart = onDragStart;
                OnDragEnd = onDragEnd;
                OnDragMove = onDragMove;
            }

            public void Unregister()
            {
                for (var i = DragHandles.Count - 1; i >= 0; --i)
                {
                    DragHandles[i].UnregisterCallbacks();
                    DragHandles.RemoveAt(i);
                }
            }


            public void RegisterDragHandle(Ref<VisualElement> dragHandle, DragAndDropContext<T> context)
            {
                var dragHandleElement = new DragHandle(
                    _ref: dragHandle,
                    draggable: this,
                    context: context
                );
                dragHandleElement.RegisterCallbacks();
                DragHandles.Add(dragHandleElement);
            }

            public void UnregisterDragHandle(Ref<VisualElement> dragHandle)
            {
                for (var i = 0; i < DragHandles.Count; ++i)
                {
                    if (DragHandles[i].Ref == dragHandle)
                    {
                        DragHandles[i].UnregisterCallbacks();
                        DragHandles.RemoveAt(i);
                        break;
                    }

                }
            }
        }

        public class Droppable
        {
            public T Value { get; private set; }
            public Ref<VisualElement> Ref { get; private set; }
            public DragAndDropContext<T> Context { get; private set; }
            public Droppable(
                T value,
                Ref<VisualElement> _ref,
                DragAndDropContext<T> context
            )
            {
                Value = value;
                Ref = _ref;
                Context = context;
            }
        }

        public string PortalDestinationId { get; private set; }
        public Ref<VisualElement> PortalDestinationRef { get; private set; }
        public Vector3 TargetStartPosition { get; set; }
        public Vector3 PointerStartPosition { get; set; }
        public Signal<Draggable> CurrentlyDragged { get; set; } = new();
        public List<Draggable> Draggables { get; private set; } = new();
        public List<Droppable> Droppables = new();
        public Signal<Droppable> ClosestDroppable { get; private set; } = new();

        public DragAndDropContext(
            string portalDestinationId,
            Ref<VisualElement> portalDestinationRef = null
        )
        {
            PortalDestinationId = portalDestinationId;
            PortalDestinationRef = portalDestinationRef;
        }

        public void RegisterDroppable(T value, Ref<VisualElement> droppableRef)
        {
            var droppable = new Droppable(
                value: value,
                _ref: droppableRef,
                context: this
            );
            Droppables.Add(droppable);
        }

        public void UnregisterDroppable(Ref<VisualElement> droppableRef)
        {
            for (var i = 0; i < Droppables.Count; i++)
            {
                if (Droppables[i].Ref == droppableRef)
                {
                    Droppables.RemoveAt(i);
                    break;
                }
            }
        }

        public void RegisterDraggable(
            T value,
            Ref<VisualElement> draggableRef,
            bool isDragHandle,
            Signal<StyleEnum<Position>> positionSignal,
            Signal<string> destinationIdSignal,
            Action onDragStart,
            Action onDragEnd,
            Action onDragMove
        )
        {
            var draggable = new Draggable(
                value: value,
                _ref: draggableRef,
                context: this,
                positionSignal: positionSignal,
                destinationIdSignal: destinationIdSignal,
                isDragHandle: isDragHandle,
                onDragStart: onDragStart,
                onDragEnd: onDragEnd,
                onDragMove: onDragMove
            );
            Draggables.Add(draggable);
        }

        public void UnregisterDraggable(Ref<VisualElement> draggableRef)
        {
            for (var i = 0; i < Draggables.Count; i++)
            {
                if (Draggables[i].Ref == draggableRef)
                {
                    Draggables[i].Unregister();
                    Draggables.RemoveAt(i);
                    break;
                }
            }
        }

        public void RegisterDragHandle(Ref<VisualElement> draggableRef, Ref<VisualElement> dragHandle)
        {
            for (var i = 0; i < Draggables.Count; i++)
            {
                if (Draggables[i].Ref == draggableRef)
                {
                    Draggables[i].RegisterDragHandle(dragHandle, this);
                }
            }
        }

        public void UnregisterDragHandle(Ref<VisualElement> draggableRef, Ref<VisualElement> dragHandle)
        {
            for (var i = 0; i < Draggables.Count; i++)
            {
                if (Draggables[i].Ref == draggableRef)
                {
                    Draggables[i].UnregisterDragHandle(dragHandle);
                }
            }
        }
    }

    public class DragAndDropProviderComponent<T> : BaseComponent
    {
        private readonly string PORTAL_DESTINATION_BASE_ID = "drag-and-drop-provider";
        private static IntIdGenerator _intIdGenerator = new();
        public DragAndDropProviderComponent(VirtualBody children) : base(children) { }

        public override VirtualBody Render()
        {
            var id = _intIdGenerator.NextId();
            var portalDestinationId = PORTAL_DESTINATION_BASE_ID + id;
            var portalDestinationRef = new Ref<VisualElement>();

            return F.ContextProvider(
                value: new DragAndDropContext<T>(portalDestinationId, portalDestinationRef),
                children: F.Nodes(
                    F.Fragment(Children),
                    F.UIPortalDestination(id: portalDestinationId, forwardRef: portalDestinationRef)
                )
            );
        }
    }

}