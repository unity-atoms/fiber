using System;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using Signals;
using Fiber.UIElements;
using FiberUtils;
using Fiber.Cursed;
using Fiber.InteractiveUI;

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
            VirtualBody children,
            Style style = new(),
            DragHandlePointerMode pointerMode = DragHandlePointerMode.HoldToDrag
        )
        {
            return new DragHandleComponent<T>(
                children: children,
                style: style,
                pointerMode: pointerMode
            );
        }

        public static DragHandleElement<T> CreateDragHandleElement<T>(
            this BaseComponent component,
            Ref<VisualElement> _ref = null,
            DragHandlePointerMode pointerMode = DragHandlePointerMode.HoldToDrag
        )
        {
            var dndContext = component.GetContext<DragAndDropContext<T>>();
            var draggableContext = component.GetContext<DraggableContext<T>>();

            _ref = _ref ?? new Ref<VisualElement>();
            var dragHandleElement = new DragHandleElement<T>(
                _ref: _ref,
                dragAndDropContext: dndContext,
                draggableContext: draggableContext
            );

            var interactiveDragHandle = component.CreateInteractiveElement(
                _ref: _ref,
                cursorType: new InteractiveCursorTypes(
                    onHover: CursorType.Grab,
                    onPressedDown: CursorType.Grabbing,
                    onDisabled: CursorType.NotAllowed
                ),
                onPressDown: (evt) =>
                {
                    if (pointerMode == DragHandlePointerMode.HoldToDrag && !dndContext.IsDragging())
                    {
                        dndContext.StartDrag(draggable: draggableContext.DraggableRef.Current, dragHandle: _ref.Current, evt.position, evt.pointerId);
                    }
                },
                onPressUp: (evt) =>
                {
                    if (pointerMode == DragHandlePointerMode.PressToDrag && !dndContext.IsDragging())
                    {
                        dndContext.StartDrag(draggable: draggableContext.DraggableRef.Current, dragHandle: _ref.Current, evt.position, evt.pointerId);
                        evt.StopImmediatePropagation();
                    }
                }
            );

            component.CreateEffect(() =>
            {
                _ref.Current.RegisterCallback<PointerMoveEvent>(dragHandleElement.OnPointerMove);
                _ref.Current.RegisterCallback<PointerCaptureOutEvent>(dragHandleElement.OnPointerCaptureOut);
                _ref.Current.RegisterCallback<PointerUpEvent>(dragHandleElement.OnPointerUp);
                return () =>
                {
                    _ref.Current.UnregisterCallback<PointerMoveEvent>(dragHandleElement.OnPointerMove);
                    _ref.Current.UnregisterCallback<PointerCaptureOutEvent>(dragHandleElement.OnPointerCaptureOut);
                    _ref.Current.UnregisterCallback<PointerUpEvent>(dragHandleElement.OnPointerUp);
                };
            });

            return dragHandleElement;
        }

        public static DragAndDropListComponent<ItemType, KeyType> DragAndDropList<ItemType, KeyType>(
            this BaseComponent component,
            ISignalList<ItemType> items,
            Func<ItemType, int, BaseSignal<bool>, ValueTuple<KeyType, VirtualNode>> children,
            DragAndDropListAnimationType animationType = DragAndDropListAnimationType.Linear,
            bool isItemDragHandle = true
        ) where ItemType : IEquatable<ItemType>
        {
            return new DragAndDropListComponent<ItemType, KeyType>(
                items: items,
                children: children,
                animationType: animationType,
                isItemDragHandle: isItemDragHandle
            );
        }
    }

    public enum DragHandlePointerMode
    {
        HoldToDrag = 0,
        PressToDrag = 1
    }

    public class DraggableContext<T>
    {
        public Ref<DragAndDropContext<T>.Draggable> DraggableRef { get; private set; }
        public DraggableContext(
            Ref<DragAndDropContext<T>.Draggable> draggableRef
        )
        {
            DraggableRef = draggableRef;
        }
    }

    public enum DragAndDropListAnimationType
    {
        None,
        Linear
    }

    public class DragAndDropListComponent<ItemType, KeyType> : BaseComponent where ItemType : IEquatable<ItemType>
    {
        private readonly ISignalList<ItemType> _items;
        private readonly Func<ItemType, int, BaseSignal<bool>, ValueTuple<KeyType, VirtualNode>> _children;
        private readonly DragAndDropListAnimationType _animationType;
        private readonly bool _isItemDragHandle;

        public DragAndDropListComponent(
            ISignalList<ItemType> items,
            Func<ItemType, int, BaseSignal<bool>, ValueTuple<KeyType, VirtualNode>> children,
            DragAndDropListAnimationType animationType = DragAndDropListAnimationType.Linear,
            bool isItemDragHandle = true
        ) : base(VirtualBody.Empty)
        {
            _items = items;
            _children = children;
            _animationType = animationType;
            _isItemDragHandle = isItemDragHandle;
        }

        public override VirtualBody Render()
        {
            return F.DragAndDropProvider<ItemType>(
                children: new DragAndDropListInner(
                    items: _items,
                    children: _children,
                    animationType: _animationType,
                    isItemDragHandle: _isItemDragHandle
                )
            );
        }

        private class DragAndDropListInner : BaseComponent
        {
            private readonly ISignalList<ItemType> _items;
            private readonly Func<ItemType, int, BaseSignal<bool>, ValueTuple<KeyType, VirtualNode>> _children;
            private readonly DragAndDropListAnimationType _animationType;
            private readonly bool _isItemDragHandle;

            public DragAndDropListInner(
                ISignalList<ItemType> items,
                Func<ItemType, int, BaseSignal<bool>, ValueTuple<KeyType, VirtualNode>> children,
                DragAndDropListAnimationType animationType,
                bool isItemDragHandle
            ) : base(VirtualBody.Empty)
            {
                _items = items;
                _children = children;
                _animationType = animationType;
                _isItemDragHandle = isItemDragHandle;
            }

            public override VirtualBody Render()
            {
                var dndContext = C<DragAndDropContext<ItemType>>();

                // Effect to change position of item when closer to another droppable.
                F.CreateEffect((closestDroppable, currentlyDragged) =>
                {
                    if (closestDroppable != null && currentlyDragged != null && !currentlyDragged.Value.Equals(closestDroppable.Value))
                    {
                        var items = _items.Get();
                        var closestDroppableIndex = items.IndexOf(closestDroppable.Value);
                        var currentlyDraggedIndex = items.IndexOf(currentlyDragged.Value);
                        items.Remove(currentlyDragged.Value);
                        items.Insert(closestDroppableIndex, currentlyDragged.Value);
                    }
                    return null;
                }, dndContext.ClosestDroppable, dndContext.CurrentlyDragged);

                return F.For(
                    each: _items,
                    children: (item, index) =>
                    {
                        var isDraggedSignal = F.CreateComputedSignal((currentlyDragged) =>
                        {
                            return currentlyDragged != null && currentlyDragged.Value != null && currentlyDragged.Value.Equals(item);
                        }, dndContext.CurrentlyDragged);
                        var (key, child) = _children(item, index, isDraggedSignal);
                        return (key, new DragAndDropListItemComponent(
                            item: item,
                            children: child,
                            animationType: _animationType,
                            isItemDragHandle: _isItemDragHandle
                        ));
                    }
                );
            }
        }

        private class DragAndDropListItemComponent : BaseComponent
        {
            private readonly ItemType _item;
            private readonly DragAndDropListAnimationType _animationType;
            private readonly bool _isItemDragHandle;

            public DragAndDropListItemComponent(
                VirtualBody children,
                ItemType item,
                DragAndDropListAnimationType animationType,
                bool isItemDragHandle
            ) : base(children)
            {
                _item = item;
                _animationType = animationType;
                _isItemDragHandle = isItemDragHandle;
            }

            public override VirtualBody Render()
            {
                var dndContext = C<DragAndDropContext<int>>();
                var droppableRef = new Ref<VisualElement>();

                if (_animationType != DragAndDropListAnimationType.None)
                {
                    // Effect that inverts changes of the droppable's position from flexbox in order to be able to animate it (see next hook)
                    F.CreateEffect(() =>
                    {
                        var mountTime = Time.fixedUnscaledTime;
                        EventCallback<GeometryChangedEvent> onGeometryChanged = new((e) =>
                        {
                            if (mountTime + 1f > Time.fixedUnscaledTime)
                            {
                                return;
                            }

                            var oldCenter = e.oldRect.center;
                            var newCenter = e.newRect.center;
                            if (oldCenter != newCenter)
                            {
                                var delta = oldCenter - newCenter;
                                droppableRef.Current.transform.position = (Vector3)delta;
                            }
                        });
                        droppableRef.Current.RegisterCallback(onGeometryChanged);
                        return () =>
                        {
                            droppableRef.Current.UnregisterCallback(onGeometryChanged);
                        };
                    });

                    // Very simple linear animation of transform that always moves towards 0,0
                    F.CreateEffect(() =>
                    {
                        void Update(float deltaTime)
                        {
                            var currentPos = droppableRef.Current.transform.position;
                            if (currentPos == Vector3.zero)
                            {
                                return;
                            }

                            var newPosition = currentPos - 500f * deltaTime * currentPos.normalized;
                            var overreachedX = currentPos.x > 0 && newPosition.x < 0 || currentPos.x < 0 && newPosition.x > 0;
                            var overreachedY = currentPos.y > 0 && newPosition.y < 0 || currentPos.y < 0 && newPosition.y > 0;
                            droppableRef.Current.transform.position = overreachedX || overreachedY ? Vector3.zero : newPosition;
                        }
                        var updateLoopSubId = MonoBehaviourHelper.AddOnUpdateHandler(Update);
                        return () =>
                        {
                            MonoBehaviourHelper.RemoveOnUpdateHandler(updateLoopSubId);
                        };
                    });
                }

                return F.Droppable(
                    forwardRef: droppableRef,
                    value: _item,
                    children: F.Draggable(
                        value: _item,
                        isDragHandle: _isItemDragHandle,
                        children: Children
                    )
                );
            }
        }
    }

    public class DragHandleComponent<T> : BaseComponent
    {
        private readonly Style _style;
        private readonly DragHandlePointerMode _pointerMode;
        public DragHandleComponent(
            VirtualBody children,
            Style style = new(),
            DragHandlePointerMode pointerMode = DragHandlePointerMode.HoldToDrag
        ) : base(children)
        {
            _style = style;
            _pointerMode = pointerMode;
        }

        public override VirtualBody Render()
        {
            var dragHandleElement = F.CreateDragHandleElement<T>(pointerMode: _pointerMode);

            return F.View(
                _ref: dragHandleElement.Ref,
                style: _style,
                children: Children
            );
        }
    }

    public class DragHandleElement<T>
    {
        public Ref<VisualElement> Ref { get; private set; }
        public DragAndDropContext<T> DragAndDropContext { get; private set; }
        public DraggableContext<T> DraggableContext { get; private set; }

        public DragHandleElement(
            Ref<VisualElement> _ref,
            DragAndDropContext<T> dragAndDropContext,
            DraggableContext<T> draggableContext
        )
        {
            Ref = _ref ?? new Ref<VisualElement>();
            DragAndDropContext = dragAndDropContext;
            DraggableContext = draggableContext;
        }

        public void OnPointerMove(PointerMoveEvent evt)
        {
            var draggable = DraggableContext.DraggableRef.Current;
            if (DragAndDropContext.CurrentlyDragged.Value == draggable && Ref.Current.HasPointerCapture(evt.pointerId))
            {
                DragAndDropContext.MoveDragged(evt.position);
            }
        }

        public void OnPointerCaptureOut(PointerCaptureOutEvent evt)
        {
            var draggable = DraggableContext.DraggableRef.Current;
            if (DragAndDropContext.CurrentlyDragged.Value == draggable)
            {
                DragAndDropContext.EndDrag();
            }
        }

        public void OnPointerUp(PointerUpEvent evt)
        {
            var draggable = DraggableContext.DraggableRef.Current;
            if (DragAndDropContext.CurrentlyDragged.Value == draggable)
            {
                DragAndDropContext.EndDrag();
            }
        }
    }

    public class DraggableComponent<T> : BaseComponent
    {
        private readonly T _value;
        private readonly bool _isDragHandle;
        private readonly DragHandlePointerMode _dragHandlePointerMode;
        private readonly Style _style;
        private Action _onDragStart;
        private Action _onDragEnd;
        private Action _onDragMove;

        public DraggableComponent(
            VirtualBody children,
            T value,
            Style style = new(),
            bool isDragHandle = false,
            DragHandlePointerMode dragHandlePointerMode = DragHandlePointerMode.HoldToDrag,
            Action onDragStart = null,
            Action onDragEnd = null,
            Action onDragMove = null
        ) : base(children)
        {
            _value = value;
            _style = style;
            _isDragHandle = isDragHandle;
            _dragHandlePointerMode = dragHandlePointerMode;

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
            var dndContext = F.GetContext<DragAndDropContext<T>>();
            var destinationId = new Signal<string>(null);

            var draggableElementRef = new Ref<VisualElement>();
            DragHandleElement<T> dragHandleElement = _isDragHandle ? F.CreateDragHandleElement<T>(draggableElementRef, _dragHandlePointerMode) : null;
            var draggableRef = new Ref<DragAndDropContext<T>.Draggable>();

            F.CreateEffect(() =>
            {
                var draggable = dndContext.RegisterDraggable(
                    value: _value,
                    draggableRef: draggableElementRef,
                    positionSignal: position,
                    destinationIdSignal: destinationId,
                    onDragStart: _onDragStart,
                    onDragEnd: _onDragEnd,
                    onDragMove: _onDragMove
                );
                draggableRef.Current = draggable;

                return () =>
                {
                    dndContext.UnregisterDraggable(draggable);
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
                    draggableElementRef.Current.UnregisterCallback(onGeometryChanged);
                });
                draggableElementRef.Current.RegisterCallback(onGeometryChanged);
                return () =>
                {
                    draggableElementRef.Current.UnregisterCallback(onGeometryChanged);
                };
            });

            return F.ContextProvider(
                value: new DraggableContext<T>(draggableRef),
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
                            _ref: draggableElementRef,
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
        public class Draggable
        {
            public T Value { get; private set; }
            public Ref<VisualElement> Ref { get; private set; }
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
                PositionSignal = positionSignal;
                DestinationIdSignal = destinationIdSignal;

                OnDragStart = onDragStart;
                OnDragEnd = onDragEnd;
                OnDragMove = onDragMove;
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
        public Signal<VisualElement> CurrentDragHandle { get; set; } = new();
        private int CurrentPointerId { get; set; }
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

        public bool IsDragging()
        {
            return CurrentlyDragged.Value != null;
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

        public Draggable RegisterDraggable(
            T value,
            Ref<VisualElement> draggableRef,
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
                onDragStart: onDragStart,
                onDragEnd: onDragEnd,
                onDragMove: onDragMove
            );
            Draggables.Add(draggable);
            return draggable;
        }

        public void UnregisterDraggable(Draggable draggable)
        {
            Draggables.Remove(draggable);
        }

        // OPEN POINT: When implementing focus input (gamepad / keyboard), then we might want to break 
        // this out into several different methods, one for the base functionality, one for pointer 
        // specific functionality, and one for gamepad / keyboard.
        public bool StartDrag(Draggable draggable, VisualElement dragHandle, Vector3 pointerPosition, int pointerId)
        {
            if (IsDragging())
            {
                return false;
            }

            PointerStartPosition = pointerPosition;

            CurrentlyDragged.Value = draggable;
            CurrentDragHandle.Value = dragHandle;
            CurrentPointerId = pointerId;
            dragHandle.CapturePointer(pointerId);

            // The portal destination transition might not happen directly, but we want to keep the dragged
            // element's position. We do that by: 
            // 1) Making the parent of the dragged element always be kept in placen with the same dimensions.
            // 2) Absolute position the portal destination, set it's size to the same as that of the draggable,
            //  and move it to the exact same location as the draggable.

            // Absolute position the dragged element and change its parent using the portal destination id.
            draggable.PositionSignal.Value = Position.Absolute;
            draggable.DestinationIdSignal.Value = PortalDestinationId;

            // Get the width, height and world position of the draggable
            var draggableElement = draggable.Ref.Current;
            var draggableWidth = draggableElement.resolvedStyle.width;
            var draggableHeight = draggableElement.resolvedStyle.height;
            var draggableWorldPosition = draggableElement.worldBound.center;

            // Place portal destination at the same position as the dragged element and make it the same size
            var portalDestination = PortalDestinationRef.Current;
            var portalWorldPosition = portalDestination.worldBound.center;
            var portalDestinationStartPosition = (Vector3)draggableWorldPosition - (Vector3)portalWorldPosition + portalDestination.transform.position;
            TargetStartPosition = portalDestinationStartPosition;
            portalDestination.transform.position = portalDestinationStartPosition;
            portalDestination.style.width = draggableWidth;
            portalDestination.style.height = draggableHeight;

            ClosestDroppable.Value = GetClosestDroppable();

            draggable.OnDragStart?.Invoke();

            return true;
        }

        public void EndDrag()
        {
            Release();
        }

        public void MoveDragged(Vector3 pointerPosition)
        {
            var deltaPointerPos = pointerPosition - PointerStartPosition;
            var portalDestination = PortalDestinationRef.Current;
            var newPos = new Vector2(
                TargetStartPosition.x + deltaPointerPos.x,
                TargetStartPosition.y + deltaPointerPos.y
            );
            portalDestination.transform.position = newPos;

            var closestDroppable = GetClosestDroppable();
            if (closestDroppable != ClosestDroppable.Value)
            {
                ClosestDroppable.Value = closestDroppable;
            }

            CurrentlyDragged.Value.OnDragMove?.Invoke();
        }

        private Droppable GetClosestDroppable()
        {
            var portalDestination = PortalDestinationRef.Current;
            Droppable droppable = null;

            Vector3 smallestDelta = Vector2.one * float.MaxValue;
            for (var i = 0; i < Droppables.Count; ++i)
            {
                var currentDroppable = Droppables[i];
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
            if (!IsDragging())
            {
                Debug.LogWarning($"DragAndDropContext: Trying to release draggable, but no draggable is currently being dragged.");
                return;
            }

            CurrentDragHandle.Value.ReleasePointer(CurrentPointerId);
            var draggable = CurrentlyDragged.Value;
            draggable.DestinationIdSignal.Value = null;
            draggable.PositionSignal.Value = Position.Relative;

            CurrentlyDragged.Value.Ref.Current.transform.position = Vector3.zero;
            CurrentlyDragged.Value = null;
            ClosestDroppable.Value = null;
            CurrentDragHandle.Value = null;
            CurrentPointerId = -1;

            draggable.OnDragEnd?.Invoke();
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