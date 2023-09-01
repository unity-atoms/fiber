using UnityEngine;
using UnityEngine.UIElements;
using Signals;
using Fiber.Theme;

namespace Fiber.UIElements
{
    // TODO: Implement a generic solution for debug windows, which are toggelable via a shortcut keys and draggable by pointer.
    public class DebugScalingWindowComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var scalingContext = C<ScalingContext>();
            var screenSizeSignal = scalingContext.ScreenSizeSignal;
            var rootDimenstionsSignal = new Signal<Rect>();

            var scalingDebugString = F.CreateComputedSignal((screenSize, rootDimensions) =>
            {
                return $"ScreenSize: {screenSize.PixelWidth}x{screenSize.PixelHeight} @ {screenSize.DPI}dpi ({screenSize.DPWidth}x{screenSize.DPHeight}dp) \nRoot dimensions: {rootDimensions.width}x{rootDimensions.height} \nRuntimePlatform {Application.platform} Screen.dpi {Screen.dpi}";
            }, screenSizeSignal, rootDimenstionsSignal);

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