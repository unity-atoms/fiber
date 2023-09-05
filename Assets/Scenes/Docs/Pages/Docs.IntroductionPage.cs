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
            ),
            F.ListItem(text: "<b>Declarative</b> - Define what you want for particular state instead of defining how you want to create it."),
            F.ListItem(text: "<b>Component based</b> - Create self contained components that can be reused in different contexts."),
            F.ListItem(text: "<b>Reactive</b> - Signals are reactive primitives that makes it possible for Fiber to only update what needs to be updated."),
            F.ListItem(text: "<b>Extendable</b> - Fiber is built to be extendable. Create your own renderer extension if there something that you natively are missing. "),
            F.ListItem(text: "<b>More than UI</b> - Fiber is not only for UI. It can be used to declare anything in your game, eg. any game object in your scene.")
        ));
    }
}
