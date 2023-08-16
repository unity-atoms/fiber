using UnityEngine.UIElements;
using Signals;
using System;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static InteractiveRef CreateInteractiveRef(
            this BaseComponent component,
            BaseSignal<bool> isDisabled = null,
            Action onPress = null
        )
        {
            var interactiveRef = new InteractiveRef(
                isDisabled: isDisabled
            );

            component.CreateEffect(() =>
            {
                interactiveRef.Current.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    interactiveRef.IsHovered.Value = true;
                });
                interactiveRef.Current.RegisterCallback<MouseLeaveEvent>(evt =>
                {
                    interactiveRef.IsHovered.Value = false;
                    interactiveRef.IsPressed.Value = false;
                });
                interactiveRef.Current.RegisterCallback<PointerDownEvent>(evt =>
                {
                    interactiveRef.IsPressed.Value = true;
                });
                interactiveRef.Current.RegisterCallback<PointerUpEvent>(evt =>
                {
                    if (interactiveRef.IsPressed.Value)
                    {
                        interactiveRef.IsPressed.Value = false;
                        if (onPress != null && (isDisabled == null || !isDisabled.Get()))
                        {
                            onPress();
                        }
                    }
                });
                return null;
            });

            return interactiveRef;
        }
    }

    public class InteractiveRef : Ref<VisualElement>
    {
        public Signal<bool> IsHovered { get; private set; }
        public Signal<bool> IsPressed { get; private set; }
        // IsDisabled will most likely be derived from external state, but can be handled interanlly
        public BaseSignal<bool> IsDisabled { get; private set; }

        public InteractiveRef(
            BaseSignal<bool> isDisabled = null
        )
        {
            IsHovered = new Signal<bool>();
            IsPressed = new Signal<bool>();
            IsDisabled = isDisabled;
        }
    }
}