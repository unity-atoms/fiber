using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using SilkUI;
using Fiber.Router;
using Signals;

public class DocsHeaderComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        var router = C<Router>();
        var themeStore = C<ThemeStore>();
        var drawerContext = C<DocsDrawerContext>();
        var iconName = new Signal<string>("sun");
        var isLandingPageRoute = F.CreateComputedSignal((r) => r.PeekRoute().Id == DocsRouting.ROUTES.LANDING, router);
        var smallScreenNotLandingPage = F.CreateComputedSignal((isLandingPageRoute, isMediumScreen) => !isLandingPageRoute && !isMediumScreen, isLandingPageRoute, themeStore.IsMediumScreen);

        return F.SilkHeader(children: F.Nodes(
            F.SilkHeaderItemGroup(justifyContent: Justify.FlexStart, children: F.Nodes(
                F.Visible(
                    when: smallScreenNotLandingPage,
                    children: F.SilkIconButton(
                        iconName: "bars", onPress: () =>
                        {
                            drawerContext.IsOpen.Value = true;
                        },
                        style: new Style(marginRight: themeStore.Spacing(2))
                    )
                ),
                new DocsLogoComponent(
                    size: DocsLogoSize.Small,
                    onPress: () =>
                    {
                        router.Navigate(DocsRouting.ROUTES.LANDING);
                    }
                )
            )),
            F.SilkHeaderItemGroup(justifyContent: Justify.FlexEnd, children: F.Nodes(
                F.SilkIconButton(variant: "github", iconName: "github", onPress: () =>
                {
                    Application.OpenURL("https://github.com/unity-atoms/fiber");
                }),
                F.SilkIconButton(variant: "discord", iconName: "discord", onPress: () =>
                {
                    Application.OpenURL("https://discord.gg/Jw2hRhEB");
                }),
                F.SilkIconButton(variant: "sun", iconName: iconName, onPress: () =>
                {
                    if (themeStore.Value == DocsThemes.LIGHT_THEME)
                    {
                        themeStore.Value = DocsThemes.DARK_THEME;
                        iconName.Value = "sun";
                    }
                    else
                    {
                        themeStore.Value = DocsThemes.LIGHT_THEME;
                        iconName.Value = "moon";
                    }
                })
            ))
        ));
    }
}