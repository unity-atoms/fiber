using Fiber;
using Fiber.UIElements;
using Fiber.UI;

public class DocsIntroductionPageComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();

        return F.Fragment(F.Children(
            F.Typography(
                text: "Introduction",
                type: TypographyType.Heading1,
                style: new Style(
                    marginTop: themeStore.Spacing(3),
                    marginBottom: themeStore.Spacing(3)
                )
            ),
            F.Typography(
                text: "Fiber is a declarative library for creating games in Unity. It is derived and inspired by web libraries such as React and Solid.",
                type: TypographyType.Body1,
                style: new Style(
                    marginBottom: themeStore.Spacing(3)
                )
            )
        ));
    }
}
