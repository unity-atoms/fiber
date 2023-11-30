using System;
using UnityEngine.UIElements;
using Signals;
using Fiber.Cursed;
using FiberUtils;

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

    public static partial class BaseComponentExtensions
    {

        private static IntIdGenerator _idGenerator = new IntIdGenerator();

        public static InteractiveElement CreateInteractiveElement(
            this BaseComponent component,
            Ref<VisualElement> _ref = null,
            ISignal<bool> isDisabled = null,
            Action<PointerUpEvent> onPressUp = null,
            Action<PointerDownEvent> onPressDown = null,
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

            component.CreateEffect(() =>
            {
                interactiveElement.Ref.Current.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    interactiveElement.IsHovered.Value = true;
                });
                interactiveElement.Ref.Current.RegisterCallback<MouseLeaveEvent>(evt =>
                {
                    interactiveElement.IsHovered.Value = false;
                    interactiveElement.IsPressed.Value = false;
                });
                interactiveElement.Ref.Current.RegisterCallback<PointerDownEvent>(evt =>
                {
                    interactiveElement.IsPressed.Value = true;
                    if (onPressDown != null && (isDisabled == null || !isDisabled.Get()))
                    {
                        onPressDown(evt);
                    }
                    // Need to specify useTrickleDown in order to catch the event for buttons
                    // See this thread for more info: https://forum.unity.com/threads/pointerdownevent-on-buttons-not-working.1211238/
                }, useTrickleDown: TrickleDown.TrickleDown);
                interactiveElement.Ref.Current.RegisterCallback<PointerUpEvent>(evt =>
                {
                    if (interactiveElement.IsPressed.Value)
                    {
                        interactiveElement.IsPressed.Value = false;
                        if (onPressUp != null && (isDisabled == null || !isDisabled.Get()))
                        {
                            onPressUp(evt);
                        }
                    }
                });
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