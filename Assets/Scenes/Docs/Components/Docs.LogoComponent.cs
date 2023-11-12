using System;
using Fiber;
using SilkUI;
using Fiber.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Signals;

public enum DocsLogoSize
{
    Small = 0,
    Medium = 1,
    Large = 2,
    XL = 3
}

public class DocsLogoComponent : BaseComponent
{
    private readonly SignalProp<DocsLogoSize> _size;
    private readonly Action _onPress;
    private readonly Style _style;
    public DocsLogoComponent(
        SignalProp<DocsLogoSize> size,
        Action onPress = null,
        Style style = new()
    )
    {
        _size = size;
        _onPress = onPress;
        _style = style;
    }

    StyleLength GetImgSize(DocsLogoSize size)
    {
        return size switch
        {
            DocsLogoSize.Small => 32,
            DocsLogoSize.Medium => 64,
            DocsLogoSize.Large => 96,
            DocsLogoSize.XL => 128,
            _ => throw new System.Exception("Invalid logo size")
        };
    }

    StyleLength GetFontSize(DocsLogoSize size)
    {
        return size switch
        {
            DocsLogoSize.Small => 24,
            DocsLogoSize.Medium => 46,
            DocsLogoSize.Large => 68,
            DocsLogoSize.XL => 92,
            _ => throw new System.Exception("Invalid logo size")
        };
    }

    public override VirtualBody Render()
    {
        var themeStore = C<ThemeStore>();
        var sizeSignal = F.WrapSignalProp(_size);
        var imgSize = F.CreateComputedSignal(GetImgSize, sizeSignal);
        var fontSize = F.CreateComputedSignal(GetFontSize, sizeSignal);

        var isInteractive = _onPress != null;
        var interactiveElement = isInteractive ? F.CreateInteractiveElement(isDisabled: null, onPress: _onPress) : null;

        return F.View(
            _ref: interactiveElement?.Ref,
            style: new Style(
                mergedStyle: _style,
                display: DisplayStyle.Flex,
                flexDirection: FlexDirection.Row,
                alignItems: Align.Center
            ),
            children: F.Nodes(
                F.Image(
                    style: new Style(
                        height: StyleKeyword.Auto,
                        maxWidth: imgSize,
                        marginRight: C<ThemeStore>().Spacing(1)
                    ),
                    image: Resources.Load<Texture2D>("Images/fiber-logo-b-128"),
                    scaleMode: ScaleMode.ScaleToFit
                ),
                F.Text(
                    text: "Fiber",
                    style: new Style(
                        fontSize: fontSize,
                        unityFont: Resources.Load<Font>("Fonts/Pacifico/Pacifico-Regular"),
                        unityFontDefinition: StyleKeyword.None,
                        color: themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Text),
                        unityTextAlign: TextAnchor.MiddleCenter
                    )
                )
            )
        );
    }
}