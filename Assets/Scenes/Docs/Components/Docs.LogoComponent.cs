using Fiber;
using Fiber.UI;
using Fiber.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DocsLogoComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        return F.View(
            style: new Style(
                display: DisplayStyle.Flex,
                flexDirection: FlexDirection.Row,
                height: new Length(100, LengthUnit.Percent),
                alignItems: Align.Center
            ),
            children: F.Children(
                F.Image(
                    style: new Style(
                        height: new Length(32, LengthUnit.Pixel),
                        width: new Length(32, LengthUnit.Pixel),
                        marginRight: C<ThemeStore>().Spacing(1)
                    ),
                    image: Resources.Load<Texture2D>("Images/fiber-logo-b-128"),
                    scaleMode: ScaleMode.ScaleToFit
                ),
                F.Typography(
                    text: "Fiber",
                    type: TypographyType.Logo
                )
            )
        );
    }
}