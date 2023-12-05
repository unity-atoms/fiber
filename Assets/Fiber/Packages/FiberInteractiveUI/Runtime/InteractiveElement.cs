using System;
using UnityEngine.UIElements;
using Signals;
using Fiber.Cursed;
using FiberUtils;
using UnityEngine;

namespace Fiber.InteractiveUI
{
    public struct InteractiveCursorTypes
    {
        public CursorType OnHover;
        public CursorType OnPressedDown;
        public CursorType OnDisabled;

        public InteractiveCursorTypes(CursorType onHover, CursorType onPressedDown, CursorType onDisabled)
        {
            OnHover = onHover;
            OnPressedDown = onPressedDown;
            OnDisabled = onDisabled;
        }

        public static InteractiveCursorTypes Default = new(
            CursorType.Pointer,
            CursorType.Pointer,
            CursorType.NotAllowed
        );

        public readonly bool IsEmpty()
        {
            return OnHover == CursorType.Default && OnPressedDown == CursorType.Default && OnDisabled == CursorType.Default;
        }
    }

    public struct PointerData
    {
        public Vector3 Position;
        public int PointerId;
        private EventBase _evt;

        public PointerData(Vector3 position, int pointerId, EventBase evt = null)
        {
            Position = position;
            PointerId = pointerId;
            _evt = evt;
        }

        public void StopImmediatePropagation()
        {
            _evt?.StopImmediatePropagation();
        }
    }

    public static partial class BaseComponentExtensions
    {
        private static IntIdGenerator _idGenerator = new IntIdGenerator();
        private static readonly float MAX_CLICK_HOLD_TIME = 0.15f;

        public static InteractiveElement CreateInteractiveElement(
            this BaseComponent component,
            Ref<VisualElement> _ref = null,
            ISignal<bool> isDisabled = null,
            Action<PointerData> onPressUp = null,
            Action<PointerData> onPressDown = null,
            Action<PointerData> onClick = null,
            InteractiveCursorTypes cursorType = new()
        )
        {
            if (cursorType.IsEmpty())
            {
                cursorType = InteractiveCursorTypes.Default;
            }

            var interactiveElement = new InteractiveElement(
                _ref: _ref,
                isDisabled: isDisabled
            );

            // Keep track of how long time ago pointer was down in order to detect
            // abortion of clicks (holding for too long) and detection of press down.
            var isClicking = false;
            var clickStartTime = -1f;
            PointerData pointerDataLastPointerDown = default;
            if (onClick != null)
            {
                component.CreateUpdateEffect((deltaTime) =>
                {
                    if (isClicking)
                    {
                        if (MAX_CLICK_HOLD_TIME + clickStartTime <= Time.unscaledTime)
                        {
                            isClicking = false;
                            if (onPressDown != null)
                            {
                                onPressDown(pointerDataLastPointerDown);
                            }
                        }
                    }
                });
            }

            component.CreateEffect(() =>
            {
                interactiveElement.Ref.Current.RegisterCallback<PointerEnterEvent>(evt =>
                {
                    interactiveElement.IsHovered.Value = true;
                });
                interactiveElement.Ref.Current.RegisterCallback<PointerLeaveEvent>(evt =>
                {
                    interactiveElement.IsHovered.Value = false;
                    interactiveElement.IsPressed.Value = false;
                });
                interactiveElement.Ref.Current.RegisterCallback<PointerDownEvent>(evt =>
                {
                    var isEnabled = isDisabled == null || !isDisabled.Get();

                    interactiveElement.IsPressed.Value = true;
                    if (isEnabled)
                    {
                        if (onClick != null)
                        {
                            isClicking = true;
                            clickStartTime = Time.unscaledTime;
                            pointerDataLastPointerDown = new PointerData(evt.position, evt.pointerId, evt);
                        }
                        else if (onPressDown != null)
                        {
                            onPressDown(new PointerData(evt.position, evt.pointerId, evt));
                        }
                    }
                    // Need to specify useTrickleDown in order to catch the event for buttons
                    // See this thread for more info: https://forum.unity.com/threads/pointerdownevent-on-buttons-not-working.1211238/
                }, useTrickleDown: TrickleDown.TrickleDown);
                interactiveElement.Ref.Current.RegisterCallback<PointerUpEvent>(evt =>
                {
                    if (interactiveElement.IsPressed.Value)
                    {
                        var isEnabled = isDisabled == null || !isDisabled.Get();
                        interactiveElement.IsPressed.Value = false;
                        if (isClicking)
                        {
                            isClicking = false;
                            if (isEnabled)
                            {
                                onClick(new PointerData(evt.position, evt.pointerId, evt));
                            }
                        }
                        else if (onPressUp != null && isEnabled)
                        {
                            onPressUp(new PointerData(evt.position, evt.pointerId, evt));
                        }
                    }
                });
                if (onClick != null)
                {
                    interactiveElement.Ref.Current.RegisterCallback<PointerMoveEvent>(evt =>
                    {
                        if (isClicking)
                        {
                            isClicking = false;
                            if (onPressDown != null)
                            {
                                onPressDown(new PointerData(evt.position, evt.pointerId, evt));
                            }
                        }
                    });
                }
                return null;
            });

            var cursorManager = component.C<CursorManager>();
            var id = _idGenerator.NextId();
            component.CreateEffect((isHovered, isPressed, isDisabled) =>
            {
                if (isDisabled)
                {
                    cursorManager.WishCursorUIElements(id, cursorType.OnDisabled, interactiveElement.Ref.Current);
                }
                else if (isHovered)
                {
                    cursorManager.WishCursorUIElements(id, cursorType.OnHover, interactiveElement.Ref.Current);
                }
                else if (isPressed)
                {
                    cursorManager.WishCursorUIElements(id, cursorType.OnPressedDown, interactiveElement.Ref.Current);
                }
                else
                {
                    cursorManager.UnwishCursor(id);
                }

                return () =>
                {
                    cursorManager.UnwishCursor(id);
                };
            }, interactiveElement.IsHovered, interactiveElement.IsPressed, interactiveElement.IsDisabled ?? new StaticSignal<bool>(false));

            return interactiveElement;
        }
    }

    public class InteractiveElement
    {
        public Ref<VisualElement> Ref { get; private set; }
        public Signal<bool> IsHovered { get; private set; }
        public Signal<bool> IsPressed { get; private set; }
        // IsDisabled will most likely be derived from external state, but can be handled interanlly
        public ISignal<bool> IsDisabled { get; private set; }

        public InteractiveElement(
            Ref<VisualElement> _ref = null,
            ISignal<bool> isDisabled = null
        )
        {
            Ref = _ref ?? new Ref<VisualElement>();
            IsHovered = new Signal<bool>();
            IsPressed = new Signal<bool>();
            IsDisabled = isDisabled;
        }
    }
}