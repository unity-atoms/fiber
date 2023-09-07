using Fiber;
using Fiber.UIElements;
using Fiber.UI;
using UnityEngine.UIElements;

public class DocsLandingPageComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();
        var logoSize = F.CreateComputedSignal((isSmall) => !isSmall ? DocsLogoSize.Large : DocsLogoSize.XL, themeStore.IsSmallScreen);
        var typographyType = F.CreateComputedSignal((isSmall) => !isSmall ? TypographyType.Heading3 : TypographyType.Heading2, themeStore.IsSmallScreen);

        return F.View(
            style: new Style(
                width: new Length(100, LengthUnit.Percent),
                height: new Length(100, LengthUnit.Percent),
                backgroundColor: themeStore.Color(DocsThemes.ROLES.NEUTRAL, ElementType.Background),
                paddingLeft: themeStore.Spacing(7),
                paddingRight: themeStore.Spacing(7),
                paddingTop: themeStore.Spacing(7),
                paddingBottom: themeStore.Spacing(7),
                alignItems: Align.Center,
                justifyContent: Justify.Center
            ),
            children: F.Children(
                new DocsLogoComponent(size: logoSize),
                F.Typography(
                    text: "A declarative library for Unity",
                    type: typographyType
                )
            )
        );
    }
}
