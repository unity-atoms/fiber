using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using Signals;

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
            Ref<VisualElement> draggableRef,
            int initialIndex,
            bool isDragHandle = false
        )
        {
            return new DraggableComponent(
                children: children,
                draggableRef: draggableRef,
                initialIndex: initialIndex,
                isDragHandle: isDragHandle
            );
        }

        public static DragHandleComponent DragHandle(
            this BaseComponent component,
            VirtualBody children,
            Ref<VisualElement> dragHandleRef
        )
        {
            return new DragHandleComponent(
                children: children,
                dragHandleRef: dragHandleRef
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

    public class DragHandleComponent : BaseComponent
    {
        private readonly Ref<VisualElement> _dragHandleRef;
        public DragHandleComponent(
            VirtualBody children,
            Ref<VisualElement> dragHandleRef
        ) : base(children)
        {
            _dragHandleRef = dragHandleRef;
        }

        public override VirtualBody Render()
        {
            var context = F.GetContext<DragAndDropContext>();
            var dragableContext = F.GetContext<DraggableContext>();
            F.CreateEffect(() =>
            {
                context.RegisterDragHandle(dragableContext.DraggableRef, _dragHandleRef);
                return () =>
                {
                    context.UnregisterDragHandle(dragableContext.DraggableRef, _dragHandleRef);
                };
            });

            return F.ContextProvider(
                value: new DraggableContext(_dragHandleRef),
                children: Children
            );
        }
    }

    public class DraggableComponent : BaseComponent
    {
        private readonly Ref<VisualElement> _draggableRef;
        private readonly int _initialIndex;
        private readonly bool _isDragHandle;
        public DraggableComponent(
            VirtualBody children,
            Ref<VisualElement> draggableRef,
            int initialIndex,
            bool isDragHandle = false
        ) : base(children)
        {
            _draggableRef = draggableRef;
            _initialIndex = initialIndex;
            _isDragHandle = isDragHandle;
        }

        public override VirtualBody Render()
        {
            var context = F.GetContext<DragAndDropContext>();
            F.CreateEffect(() =>
            {
                context.RegisterDraggable(_draggableRef, _initialIndex, _isDragHandle);
                return () =>
                {
                    context.UnregisterDraggable(_draggableRef);
                };
            });

            return F.ContextProvider(
                value: new DraggableContext(_draggableRef),
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
            private bool _isDragging;

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
                Context.PointerStartPosition = evt.position;

                var draggableElement = Draggable.Ref.Current;
                Vector3 targetFlexedWorldBound = draggableElement.worldBound.center;
                Vector3 targetAbsoluteWorldBound = draggableElement.parent.worldBound.center;

                Vector3 deltaTargetWorldBound = targetFlexedWorldBound - targetAbsoluteWorldBound;

                Context.TargetStartPosition = deltaTargetWorldBound;
                draggableElement.transform.position = deltaTargetWorldBound;

                Context.DraggedElement = Draggable.Ref.Current;
                Ref.Current.CapturePointer(evt.pointerId);
                draggableElement.style.position = Position.Absolute;

                var width = draggableElement.resolvedStyle.width;
                var height = draggableElement.resolvedStyle.height;
                var marginLeft = draggableElement.resolvedStyle.marginLeft;
                var marginTop = draggableElement.resolvedStyle.marginTop;
                var marginRight = draggableElement.resolvedStyle.marginRight;
                var marginBottom = draggableElement.resolvedStyle.marginBottom;

                Draggable.TempElement ??= new VisualElement();
                Draggable.TempElement.style.width = width;
                Draggable.TempElement.style.height = height;
                Draggable.TempElement.style.marginLeft = marginLeft;
                Draggable.TempElement.style.marginTop = marginTop;
                Draggable.TempElement.style.marginRight = marginRight;
                Draggable.TempElement.style.marginBottom = marginBottom;
                Draggable.TempElement.style.backgroundColor = Color.red;

                draggableElement.parent.Insert(Draggable.IndexRef.Current, Draggable.TempElement);
                draggableElement.MoveToBack();

                _isDragging = true;
            }

            private void OnPointerMove(PointerMoveEvent evt)
            {
                if (Context.DraggedElement == Draggable.Ref.Current && Ref.Current.HasPointerCapture(evt.pointerId) && _isDragging)
                {
                    var delta = evt.position - Context.PointerStartPosition;
                    Draggable.Ref.Current.transform.position = new Vector2(Context.TargetStartPosition.x + delta.x, Context.TargetStartPosition.y + delta.y);
                }
            }

            private void OnPointerUp(PointerUpEvent evt)
            {
                if (Context.DraggedElement == Draggable.Ref.Current && Ref.Current.HasPointerCapture(evt.pointerId) && _isDragging)
                {
                    Ref.Current.ReleasePointer(evt.pointerId);
                    Release();
                }
            }

            private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
            {
                if (Context.DraggedElement == Draggable.Ref.Current && _isDragging)
                {
                    Release();
                }
            }

            private void Release()
            {
                Context.DraggedElement.transform.position = Vector3.zero;
                Context.DraggedElement = null;
                Draggable.Ref.Current.style.position = Position.Relative;
                Draggable.TempElement.parent.Remove(Draggable.TempElement);
                Draggable.Ref.Current.MoveToIndex(Draggable.IndexRef.Current);
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

            public Draggable(
                Ref<VisualElement> _ref,
                DragAndDropContext context,
                int initialIndex,
                bool isDragHandle
            )
            {
                Ref = _ref;
                Context = context;
                IndexRef = new Ref<int>(initialIndex);
                _isDragHandle = isDragHandle;

                if (_isDragHandle)
                {
                    RegisterDragHandle(_ref, context);
                }
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

        public Vector3 TargetStartPosition { get; set; }
        public Vector3 PointerStartPosition { get; set; }
        public VisualElement DraggedElement { get; set; }
        private List<Draggable> _draggables = new();

        public void RegisterDraggable(Ref<VisualElement> draggableRef, int initialIndex, bool isDragHandle = false)
        {
            var draggable = new Draggable(
                _ref: draggableRef,
                context: this,
                initialIndex: initialIndex,
                isDragHandle: isDragHandle
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