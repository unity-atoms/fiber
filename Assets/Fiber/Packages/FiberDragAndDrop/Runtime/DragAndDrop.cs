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
        public static DragAndDropProviderComponent DragAndDropProvider(
            this BaseComponent component,
            VirtualBody children
        )
        {
            return new DragAndDropProviderComponent(
                children: children
            );
        }

        public static DroppableComponent Droppable(
            this BaseComponent component,
            VirtualBody children,
            Style style = new()
        )
        {
            return new DroppableComponent(
                children: children,
                style: style
            );
        }

        public static DraggableComponent Draggable(
            this BaseComponent component,
            VirtualBody children,
            Style style = new(),
            bool isDragHandle = false
        )
        {
            return new DraggableComponent(
                children: children,
                style: style,
                isDragHandle: isDragHandle
            );
        }

        public static DragHandleComponent DragHandle(
            this BaseComponent component,
            VirtualBody children
        )
        {
            return new DragHandleComponent(
                children: children
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

    public class DragAndDropListItem : BaseComponent
    {
        public override VirtualBody Render()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DragHandleComponent : BaseComponent
    {
        public DragHandleComponent(
            VirtualBody children
        ) : base(children)
        {
        }

        public override VirtualBody Render()
        {
            var dragHandleRef = new Ref<VisualElement>();
            var context = F.GetContext<DragAndDropContext>();
            var dragableContext = F.GetContext<DraggableContext>();
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

    public class DraggableComponent : BaseComponent
    {
        private readonly bool _isDragHandle;
        private readonly Style _style;
        private Action _onDragStart;
        private Action _onDragEnd;
        private Action _onDragMove;

        public DraggableComponent(
            VirtualBody children,
            Style style = new(),
            bool isDragHandle = false,
            Action onDragStart = null,
            Action onDragEnd = null,
            Action onDragMove = null
        ) : base(children)
        {
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
            var context = F.GetContext<DragAndDropContext>();
            var destinationId = new Signal<string>(null);

            F.CreateEffect(() =>
            {
                context.RegisterDraggable(
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
                value: new DraggableContext(draggableRef),
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

    public class DroppableComponent : BaseComponent
    {
        private readonly Style _style;
        public DroppableComponent(
            VirtualBody children,
            Style style = new()
        ) : base(children)
        {
            _style = style;
        }

        public override VirtualBody Render()
        {
            var droppableRef = new Ref<VisualElement>();
            var context = F.GetContext<DragAndDropContext>();
            F.CreateEffect(() =>
            {
                context.RegisterDroppable(droppableRef);
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

    public class DragAndDropContext
    {
        private class DragHandle
        {
            public Ref<VisualElement> Ref { get; private set; }
            public Draggable Draggable { get; private set; }
            public DragAndDropContext Context { get; private set; }

            public DragHandle(
                Ref<VisualElement> _ref,
                Draggable draggable,
                DragAndDropContext context
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
                if (Context.DraggedElement != null)
                {
                    return;
                }

                Context.PointerStartPosition = evt.position;

                Context.DraggedElement = Draggable.Ref.Current;
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

                Draggable.OnDragStart?.Invoke();
            }

            private void OnPointerMove(PointerMoveEvent evt)
            {
                if (Context.DraggedElement == Draggable.Ref.Current && Ref.Current.HasPointerCapture(evt.pointerId))
                {
                    var deltaMousePos = evt.position - Context.PointerStartPosition;
                    Draggable.Context.PortalDestinationRef.Current.transform.position = new Vector2(
                        Context.TargetStartPosition.x + deltaMousePos.x,
                        Context.TargetStartPosition.y + deltaMousePos.y
                    );
                    Draggable.OnDragMove?.Invoke();
                }
            }

            private void OnPointerUp(PointerUpEvent evt)
            {
                if (Context.DraggedElement == Draggable.Ref.Current && Ref.Current.HasPointerCapture(evt.pointerId))
                {
                    Ref.Current.ReleasePointer(evt.pointerId);
                    Release();
                }
            }

            private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
            {
                if (Context.DraggedElement == Draggable.Ref.Current)
                {
                    Release();
                }
            }

            private void Release()
            {
                Context.DraggedElement.transform.position = Vector3.zero;
                Context.DraggedElement = null;
                Draggable.DestinationIdSignal.Value = null;
                Draggable.PositionSignal.Value = Position.Relative;
                Draggable.OnDragEnd?.Invoke();
            }
        }

        private class Draggable
        {
            public Ref<VisualElement> Ref { get; private set; }
            private readonly bool _isDragHandle;
            public Ref<int> IndexRef { get; private set; }
            private List<DragHandle> DragHandles { get; set; } = new();
            public Vector3 TargetStartPosition;
            public Vector3 PointerStartPosition;
            public VisualElement TempElement;
            public VisualElement DraggedElement;
            public DragAndDropContext Context { get; private set; }
            public Action OnDragStart { get; private set; }
            public Action OnDragEnd { get; private set; }
            public Action OnDragMove { get; private set; }
            public Signal<StyleEnum<Position>> PositionSignal { get; private set; }
            public Signal<string> DestinationIdSignal { get; set; }

            public Draggable(
                Ref<VisualElement> _ref,
                DragAndDropContext context,
                bool isDragHandle,
                Signal<StyleEnum<Position>> positionSignal,
                Signal<string> destinationIdSignal,
                Action onDragStart,
                Action onDragEnd,
                Action onDragMove
            )
            {
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


            public void RegisterDragHandle(Ref<VisualElement> dragHandle, DragAndDropContext context)
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

        private class Droppable
        {
            public Ref<VisualElement> Ref { get; private set; }
            public DragAndDropContext Context { get; private set; }
            public Droppable(
                Ref<VisualElement> _ref,
                DragAndDropContext context
            )
            {
                Ref = _ref;
                Context = context;
            }
        }

        public string PortalDestinationId { get; private set; }
        public Ref<VisualElement> PortalDestinationRef { get; private set; }
        public Vector3 TargetStartPosition { get; set; }
        public Vector3 PointerStartPosition { get; set; }
        public VisualElement DraggedElement { get; set; }
        private List<Draggable> _draggables = new();
        private List<Droppable> _droppables = new();

        public DragAndDropContext(
            string portalDestinationId,
            Ref<VisualElement> portalDestinationRef = null
        )
        {
            PortalDestinationId = portalDestinationId;
            PortalDestinationRef = portalDestinationRef;
        }

        public void RegisterDroppable(Ref<VisualElement> droppableRef)
        {
            var droppable = new Droppable(
                _ref: droppableRef,
                context: this
            );
            _droppables.Add(droppable);
        }

        public void UnregisterDroppable(Ref<VisualElement> droppableRef)
        {
            for (var i = 0; i < _droppables.Count; i++)
            {
                if (_droppables[i].Ref == droppableRef)
                {
                    _droppables.RemoveAt(i);
                    break;
                }
            }
        }

        public void RegisterDraggable(
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
                _ref: draggableRef,
                context: this,
                positionSignal: positionSignal,
                destinationIdSignal: destinationIdSignal,
                isDragHandle: isDragHandle,
                onDragStart: onDragStart,
                onDragEnd: onDragEnd,
                onDragMove: onDragMove
            );
            _draggables.Add(draggable);
        }

        public void UnregisterDraggable(Ref<VisualElement> draggableRef)
        {
            for (var i = 0; i < _draggables.Count; i++)
            {
                if (_draggables[i].Ref == draggableRef)
                {
                    _draggables[i].Unregister();
                    _draggables.RemoveAt(i);
                    break;
                }
            }
        }

        public void RegisterDragHandle(Ref<VisualElement> draggableRef, Ref<VisualElement> dragHandle)
        {
            for (var i = 0; i < _draggables.Count; i++)
            {
                if (_draggables[i].Ref == draggableRef)
                {
                    _draggables[i].RegisterDragHandle(dragHandle, this);
                }
            }
        }

        public void UnregisterDragHandle(Ref<VisualElement> draggableRef, Ref<VisualElement> dragHandle)
        {
            for (var i = 0; i < _draggables.Count; i++)
            {
                if (_draggables[i].Ref == draggableRef)
                {
                    _draggables[i].UnregisterDragHandle(dragHandle);
                }
            }
        }
    }

    public class DragAndDropProviderComponent : BaseComponent
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
                value: new DragAndDropContext(portalDestinationId, portalDestinationRef),
                children: F.Nodes(
                    F.Fragment(Children),
                    F.UIPortalDestination(id: portalDestinationId, forwardRef: portalDestinationRef)
                )
            );
        }
    }

}