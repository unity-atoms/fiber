using UnityEngine;
using UnityEngine.UIElements;
using Signals;
using Fiber.Theme;

namespace Fiber.UIElements
{
    public static partial class BaseComponentExtensions
    {
        public static DebugWindowScalingComponent DebugWindowScaling(
            this BaseComponent component
        )
        {
            return new DebugWindowScalingComponent();
        }
    }

    // TODO: Implement a generic solution for debug windows, which are toggelable via a shortcut keys and draggable by pointer.
    public class DebugWindowScalingComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var uiRootContext = C<UIRootContext>();
            var scalingContext = C<ScalingContext>();
            var screenSizeSignal = scalingContext.ScreenSizeSignal;
            var rootDimensionsSignal = new Signal<Rect>();

            F.CreateEffect(() =>
            {
                void UpdateDimensionsSignal(GeometryChangedEvent e)
                {
                    rootDimensionsSignal.Value = e.newRect;
                }
                uiRootContext.RootRef.Current.RegisterCallback((EventCallback<GeometryChangedEvent>)UpdateDimensionsSignal);
                return () => uiRootContext.RootRef.Current.UnregisterCallback((EventCallback<GeometryChangedEvent>)UpdateDimensionsSignal);
            });

            var scalingDebugString = F.CreateComputedSignal((screenSize, rootDimensions) =>
            {
                return $"ScreenSize: {screenSize.PixelWidth}x{screenSize.PixelHeight} @ {screenSize.DPI}dpi ({screenSize.DPWidth}x{screenSize.DPHeight}dp) \nRoot dimensions: {rootDimensions.width}x{rootDimensions.height} \nRuntimePlatform {Application.platform} Screen.dpi {Screen.dpi}";
            }, screenSizeSignal, rootDimensionsSignal);

            var themeStore = C<ThemeStore>();
            var textColor = themeStore.Color(role: "debug", ElementType.Text);
            var backgroundColor = themeStore.Color(role: "debug", ElementType.Background);

            return F.Text(
                text: scalingDebugString,
                style: new Style(
                    display: DisplayStyle.Flex,
                    position: Position.Absolute,
                    right: 0,
                    bottom: 0,
                    color: textColor,
                    backgroundColor: backgroundColor,
                    fontSize: 12
                )
            );
        }
    }
}