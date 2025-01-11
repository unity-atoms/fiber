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
            Action onDragMove = null,
            Ref<VisualElement> wrapperRef = null,
            Action<PointerData> onClick = null
        )
        {
            return new DraggableComponent<T>(
                children: children,
                value: value,
                style: style,
                isDragHandle: isDragHandle,
                onDragStart: onDragStart,
                onDragEnd: onDragEnd,
                onDragMove: onDragMove,
                wrapperRef: wrapperRef,
                onClick: onClick
            );
        }

        public static DragHandleComponent<T> DragHandle<T>(
            this BaseComponent component,
            VirtualBody children,
            Style style = new(),
            DragHandlePointerMode pointerMode = DragHandlePointerMode.HoldToDrag,
            Action<PointerData> onClick = null
        )
        {
            return new DragHandleComponent<T>(
                children: children,
                style: style,
                onClick: onClick,
                pointerMode: pointerMode
            );
        }

        public static DragHandleElement<T> CreateDragHandleElement<T>(
            this BaseComponent component,
            DraggableContext<T> draggableContext,
            Ref<VisualElement> _ref = null,
            Action<PointerData> onClick = null,
            DragHandlePointerMode pointerMode = DragHandlePointerMode.HoldToDrag
        )
        {
            var dndContext = component.GetContext<DragAndDropContext<T>>();
            // int timeoutId = -1;

            _ref = _ref ?? new Ref<VisualElement>();
            var interactiveDragHandle = component.CreateInteractiveElement(
                _ref: _ref,
                cursorType: new InteractiveCursorTypes(
                    onHover: CursorType.Grab,
                    onPressedDown: CursorType.Grabbing,
                    onDisabled: CursorType.NotAllowed
                ),
                onClick: onClick,
                onPressDown: (evt) =>
                {
                    if (pointerMode == DragHandlePointerMode.HoldToDrag && !dndContext.IsDragging())
                    {
                        dndContext.StartDrag(draggable: draggableContext.DraggableRef.Current, dragHandle: _ref.Current, evt.Position, evt.PointerId);
                    }
                },
                onPressUp: (evt) =>
                {
                    if (pointerMode == DragHandlePointerMode.PressToDrag && !dndContext.IsDragging())
                    {
                        dndContext.StartDrag(draggable: draggableContext.DraggableRef.Current, dragHandle: _ref.Current, evt.Position, evt.PointerId);
                        evt.StopImmediatePropagation();
                    }
                }
            );

            var dragHandleElement = new DragHandleElement<T>(
                _ref: _ref,
                interactiveElement: interactiveDragHandle,
                dragAndDropContext: dndContext,
                draggableContext: draggableContext
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
            Func<ItemType, int, BaseSignal<bool>, VirtualNode> renderItem,
            Func<ItemType, int, KeyType> createItemKey,
            IEqualityComparer<ItemType> itemComparer = null,
            DragAndDropListAnimationType animationType = DragAndDropListAnimationType.Linear,
            bool isItemDragHandle = true,
            Action<int, int> moveItem = null,
            Action<ItemType> onClick = null
        )
        {
            return new DragAndDropListComponent<ItemType, KeyType>(
                items: items,
                renderItem: renderItem,
                createItemKey: createItemKey,
                itemComparer: itemComparer,
                animationType: animationType,
                isItemDragHandle: isItemDragHandle,
                moveItem: moveItem,
                onClick: onClick
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

    public class DragAndDropListComponent<ItemType, KeyType> : BaseComponent
    {
        private readonly ISignalList<ItemType> _items;
        private readonly Func<ItemType, int, BaseSignal<bool>, VirtualNode> _renderItem;
        private readonly Func<ItemType, int, KeyType> _createItemKey;
        private readonly IEqualityComparer<ItemType> _itemComparer;
        private readonly DragAndDropListAnimationType _animationType;
        private readonly bool _isItemDragHandle;
        private readonly Action<int, int> _moveItem;
        private readonly Action<ItemType> _onClick;

        public DragAndDropListComponent(
            ISignalList<ItemType> items,
            Func<ItemType, int, BaseSignal<bool>, VirtualNode> renderItem,
            Func<ItemType, int, KeyType> createItemKey,
            IEqualityComparer<ItemType> itemComparer = null,
            DragAndDropListAnimationType animationType = DragAndDropListAnimationType.Linear,
            bool isItemDragHandle = true,
            Action<int, int> moveItem = null,
            Action<ItemType> onClick = null
        ) : base(VirtualBody.Empty)
        {
            _items = items;
            _renderItem = renderItem;
            _createItemKey = createItemKey;
            _itemComparer = itemComparer ?? EqualityComparer<ItemType>.Default;
            _animationType = animationType;
            _isItemDragHandle = isItemDragHandle;
            _moveItem = moveItem;
            _onClick = onClick;
        }

        public override VirtualBody Render()
        {
            return F.DragAndDropProvider<ItemType>(
                children: new DragAndDropListInner(
                    items: _items,
                    renderItem: _renderItem,
                    createItemKey: _createItemKey,
                    itemComparer: _itemComparer,
                    animationType: _animationType,
                    isItemDragHandle: _isItemDragHandle,
                    moveItem: _moveItem,
                    onClick: _onClick
                )
            );
        }

        private class DragAndDropListInner : BaseComponent
        {
            private readonly ISignalList<ItemType> _items;
            private readonly Func<ItemType, int, BaseSignal<bool>, VirtualNode> _renderItem;
            private readonly Func<ItemType, int, KeyType> _createItemKey;
            private readonly IEqualityComparer<ItemType> _itemComparer;
            private readonly DragAndDropListAnimationType _animationType;
            private readonly bool _isItemDragHandle;
            private readonly Action<int, int> _moveItem;
            private readonly Action<ItemType> _onClick;

            public DragAndDropListInner(
                ISignalList<ItemType> items,
                Func<ItemType, int, BaseSignal<bool>, VirtualNode> renderItem,
                Func<ItemType, int, KeyType> createItemKey,
                IEqualityComparer<ItemType> itemComparer,
                DragAndDropListAnimationType animationType,
                bool isItemDragHandle,
                Action<int, int> moveItem,
                Action<ItemType> onClick
            ) : base(VirtualBody.Empty)
            {
                _items = items;
                _renderItem = renderItem;
                _createItemKey = createItemKey;
                _itemComparer = itemComparer;
                _animationType = animationType;
                _isItemDragHandle = isItemDragHandle;
                _moveItem = moveItem;
                _onClick = onClick;
            }

            public override VirtualBody Render()
            {
                var dndContext = C<DragAndDropContext<ItemType>>();

                // Effect to change position of item when closer to another droppable.
                F.CreateEffect((closestDroppable, currentlyDragged) =>
                {
                    if (closestDroppable != null && currentlyDragged != null && !_itemComparer.Equals(currentlyDragged.Value, closestDroppable.Value))
                    {
                        var items = _items.Get();
                        var closestDroppableIndex = items.IndexOf(closestDroppable.Value);
                        var currentlyDraggedIndex = items.IndexOf(currentlyDragged.Value);

                        if (_moveItem != null)
                        {
                            _moveItem(currentlyDraggedIndex, closestDroppableIndex);
                        }
                        else
                        {
                            items.Remove(currentlyDragged.Value);
                            items.Insert(closestDroppableIndex, currentlyDragged.Value);
                        }
                    }
                    return null;
                }, dndContext.ClosestDroppable, dndContext.CurrentlyDragged);

                return F.For(
                    each: _items,
                    createItemKey: _createItemKey,
                    renderItem: (item, index) =>
                    {
                        var isDraggedSignal = F.CreateComputedSignal((currentlyDragged) =>
                        {
                            return currentlyDragged != null && currentlyDragged.Value != null && _itemComparer.Equals(currentlyDragged.Value, item);
                        }, dndContext.CurrentlyDragged);
                        var child = _renderItem(item, index, isDraggedSignal);
                        return new DragAndDropListItemComponent(
                            item: item,
                            children: child,
                            animationType: _animationType,
                            isItemDragHandle: _isItemDragHandle,
                            onClick: _onClick != null ? (e) =>
                            {
                                _onClick.Invoke(item);
                            }
                        : null
                        );
                    }
                );
            }
        }

        private class DragAndDropListItemComponent : BaseComponent
        {
            private readonly ItemType _item;
            private readonly DragAndDropListAnimationType _animationType;
            private readonly bool _isItemDragHandle;
            private readonly Action<PointerData> _onClick;

            public DragAndDropListItemComponent(
                VirtualBody children,
                ItemType item,
                DragAndDropListAnimationType animationType,
                bool isItemDragHandle,
                Action<PointerData> onClick
            ) : base(children)
            {
                _item = item;
                _animationType = animationType;
                _isItemDragHandle = isItemDragHandle;
                _onClick = onClick;
            }

            public override VirtualBody Render()
            {
                var dndContext = C<DragAndDropContext<ItemType>>();
                var droppableRef = new Ref<VisualElement>();

                if (_animationType != DragAndDropListAnimationType.None)
                {
                    // Handler that inverts changes of the droppable's position from flexbox in order to be able to animate it (see hook further down)
                    var enabledTimeRef = new Ref<float>(Time.fixedUnscaledTime);
                    EventCallback<GeometryChangedEvent> onGeometryChanged = new((e) =>
                    {
                        // Prevent animations when:
                        // 1) Item is first mounted
                        // 2) The full list is re-enabled
                        if (enabledTimeRef.Current + 1f > Time.fixedUnscaledTime)
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
                    F.CreateOnEnableEffect((enabled) =>
                    {
                        if (enabled)
                        {
                            enabledTimeRef.Current = Time.fixedUnscaledTime;
                            droppableRef.Current.RegisterCallback(onGeometryChanged);
                        }
                        else
                        {
                            droppableRef.Current.UnregisterCallback(onGeometryChanged);
                        }
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

                var isFirstDrag = true;
                var droppableWrapperRef = new Ref<VisualElement>();

                return F.Droppable(
                    forwardRef: droppableRef,
                    value: _item,
                    children: F.Draggable(
                        onClick: _onClick,
                        wrapperRef: droppableWrapperRef,
                        value: _item,
                        isDragHandle: _isItemDragHandle,
                        children: Children,
                        onDragStart: () =>
                        {
                            // This is a hack since we currently are not supporting suspense in Fiber.
                            // If we supported suspense we would have set width and height when the suspension
                            // was resolved. Now we wait until first drag to calculate it, which works fine. 
                            // The reason why we want to keep the height is that we re-parent the dragged element
                            // when dragging it, but we still want to keep the dimenstions of the container, which
                            // is not dragged around. 
                            if (isFirstDrag)
                            {
                                isFirstDrag = false;
                                var resolvedStyle = droppableWrapperRef.Current.resolvedStyle;
                                droppableWrapperRef.Current.style.width = resolvedStyle.width;
                                droppableWrapperRef.Current.style.height = resolvedStyle.height;
                            }
                        }
                    )
                );
            }
        }
    }

    public class DragHandleComponent<T> : BaseComponent
    {
        private readonly Style _style;
        private readonly DragHandlePointerMode _pointerMode;
        private readonly Action<PointerData> _onClick;
        public DragHandleComponent(
            VirtualBody children,
            Style style = new(),
            DragHandlePointerMode pointerMode = DragHandlePointerMode.HoldToDrag,
            Action<PointerData> onClick = null
        ) : base(children)
        {
            _style = style;
            _pointerMode = pointerMode;
            _onClick = onClick;
        }

        public override VirtualBody Render()
        {
            var draggableContext = F.GetContext<DraggableContext<T>>();
            var dragHandleElement = F.CreateDragHandleElement<T>(draggableContext: draggableContext, pointerMode: _pointerMode, onClick: _onClick);

            return F.ContextProvider(
                value: dragHandleElement,
                children: F.View(
                    _ref: dragHandleElement.Ref,
                    style: _style,
                    children: Children
                )
            );
        }
    }

    public class DragHandleElement<T>
    {
        public Ref<VisualElement> Ref { get; private set; }
        public InteractiveElement InteractiveElement { get; private set; }
        public DragAndDropContext<T> DragAndDropContext { get; private set; }
        public DraggableContext<T> DraggableContext { get; private set; }

        public DragHandleElement(
            Ref<VisualElement> _ref,
            InteractiveElement interactiveElement,
            DragAndDropContext<T> dragAndDropContext,
            DraggableContext<T> draggableContext
        )
        {
            Ref = _ref ?? new Ref<VisualElement>();
            InteractiveElement = interactiveElement;
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
        private readonly Action _onDragStart;
        private readonly Action _onDragEnd;
        private readonly Action _onDragMove;
        private readonly Action<PointerData> _onClick;
        private readonly Ref<VisualElement> _wrapperRef;

        public DraggableComponent(
            VirtualBody children,
            T value,
            Style style = new(),
            bool isDragHandle = false,
            DragHandlePointerMode dragHandlePointerMode = DragHandlePointerMode.HoldToDrag,
            Action onDragStart = null,
            Action onDragEnd = null,
            Action onDragMove = null,
            Action<PointerData> onClick = null,
            Ref<VisualElement> wrapperRef = null
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

            _onDragStart = onDragStart;
            _onDragEnd = onDragEnd;
            _onDragMove = onDragMove;
            _onClick = onClick;
            _wrapperRef = wrapperRef;

            _isDragHandle = isDragHandle;
        }

        public override VirtualBody Render()
        {
            var position = new Signal<StyleEnum<Position>>(Position.Relative);
            var wrapperRef = _wrapperRef ?? new Ref<VisualElement>();
            var dndContext = F.GetContext<DragAndDropContext<T>>();
            var destinationId = new Signal<string>(null);

            var draggableElementRef = new Ref<VisualElement>();
            var draggableRef = new Ref<DragAndDropContext<T>.Draggable>();
            var draggableContext = new DraggableContext<T>(draggableRef);
            DragHandleElement<T> dragHandleElement = _isDragHandle ? F.CreateDragHandleElement<T>(draggableContext: draggableContext, _ref: draggableElementRef, pointerMode: _dragHandlePointerMode, onClick: _onClick) : null;

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

            return F.ContextProvider(
                value: draggableContext,
                children: F.ContextProvider(
                    value: dragHandleElement,
                    children: F.View(
                        _ref: wrapperRef,
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

            // Get position of draggable and portal destination
            var draggableElement = draggable.Ref.Current;
            var draggableWorldPosition = draggableElement.worldBound.position;

            var portalDestination = PortalDestinationRef.Current;
            var portalWorldPosition = portalDestination.worldBound.position;

            // Place portal destination at the same position as the dragged element
            var portalDestinationStartPosition = (Vector3)draggableWorldPosition - (Vector3)portalWorldPosition + portalDestination.transform.position;
            TargetStartPosition = portalDestinationStartPosition;
            portalDestination.transform.position = portalDestinationStartPosition;

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
            var dragged = CurrentlyDragged.Value.Ref.Current;
            Droppable droppable = null;

            Vector3 smallestDelta = Vector2.one * float.MaxValue;
            for (var i = 0; i < Droppables.Count; ++i)
            {
                var currentDroppable = Droppables[i];
                var droppableWorldPos = currentDroppable.Ref.Current.worldBound.center;
                var delta = droppableWorldPos - dragged.worldBound.center;

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
            var portalDestinationId = $"{PORTAL_DESTINATION_BASE_ID}-{typeof(T)}-{id}";
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