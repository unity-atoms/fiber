using System;
using UnityEngine.UIElements;
using Fiber;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static InteractiveElement CreateInteractiveElement(
            this BaseComponent component,
            Ref<VisualElement> _ref = null,
            ISignal<bool> isDisabled = null,
            Action onPress = null
        )
        {
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
                    // Need to specify useTrickleDown in order to catch the event for buttons
                    // See this thread for more info: https://forum.unity.com/threads/pointerdownevent-on-buttons-not-working.1211238/
                }, useTrickleDown: TrickleDown.TrickleDown);
                interactiveElement.Ref.Current.RegisterCallback<PointerUpEvent>(evt =>
                {
                    if (interactiveElement.IsPressed.Value)
                    {
                        interactiveElement.IsPressed.Value = false;
                        if (onPress != null && (isDisabled == null || !isDisabled.Get()))
                        {
                            onPress();
                        }
                    }
                });
                return null;
            });

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