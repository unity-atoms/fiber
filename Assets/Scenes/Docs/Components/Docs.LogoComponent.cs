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
    private readonly EventCallback<ClickEvent> _onClick;
    private readonly Style _style;
    public DocsLogoComponent(
        SignalProp<DocsLogoSize> size,
        EventCallback<ClickEvent> onClick = null,
        Style style = new()
    )
    {
        _size = size;
        _onClick = onClick;
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

    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();
        var sizeSignal = F.WrapSignalProp(_size);
        var imgSize = F.CreateComputedSignal(GetImgSize, sizeSignal);
        var fontSize = F.CreateComputedSignal(GetFontSize, sizeSignal);

        return F.View(
            style: new Style(
                mergedStyle: _style,
                display: DisplayStyle.Flex,
                flexDirection: FlexDirection.Row,
                alignItems: Align.Center
            ),
            children: F.Children(
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
                        color: themeStore.Color(DocsThemes.ROLES.NEUTRAL, ElementType.Text),
                        unityTextAlign: TextAnchor.MiddleCenter
                    )
                )
            ),
            onClick: _onClick
        );
    }
}