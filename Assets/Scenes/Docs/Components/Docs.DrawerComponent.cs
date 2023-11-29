using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using SilkUI;
using Fiber.Router;
using Signals;

public class DocsDrawerComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        var router = C<Router>();
        var themeStore = C<ThemeStore>();
        var drawerContext = C<DocsDrawerContext>();

        return F.Mount(
            when: new NegatedBoolSignal(themeStore.IsMediumScreen),
            children: F.Nodes(
                F.Visible(
                    when: drawerContext.IsOpen,
                    children: F.SilkBackdrop(
                        onClick: (e) =>
                        {
                            drawerContext.IsOpen.Value = false;
                        }
                    )
                ),
                F.SilkDrawer(
                    role: DocsThemes.ROLE.NEUTRAL,
                    subRole: DocsThemes.SUBROLE.DEEP,
                    isOpen: drawerContext.IsOpen,
                    children: F.View(
                            style: new Style(
                                display: DisplayStyle.Flex,
                                flexDirection: FlexDirection.Column
                            ),
                            children: F.Nodes(
                                F.View(
                                    style: new Style(
                                        display: DisplayStyle.Flex,
                                        flexDirection: FlexDirection.Row,
                                        alignItems: Align.Center,
                                        justifyContent: Justify.SpaceBetween,
                                        height: themeStore.Spacing(14),
                                        paddingBottom: themeStore.Spacing(2),
                                        paddingTop: themeStore.Spacing(2),
                                        paddingLeft: themeStore.Spacing(4),
                                        paddingRight: themeStore.Spacing(4),
                                        backgroundColor: themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Background),
                                        borderBottomWidth: themeStore.BorderWidth(),
                                        borderBottomColor: themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Border)
                                    ),
                                    children: F.Nodes(
                                        new DocsLogoComponent(
                                            size: DocsLogoSize.Small,
                                            onPress: (evt) =>
                                            {
                                                router.Navigate(DocsRouting.ROUTES.LANDING);
                                            }
                                        ),
                                        F.SilkIconButton(
                                            iconName: "xmark",
                                            onPress: (evt) =>
                                            {
                                                drawerContext.IsOpen.Value = false;
                                            }
                                        )
                                    )
                                ),
                                new DocsTreeViewComponent()
                            )
                    ),
                    position: DrawerPosition.Left
                )
            )
        );
    }
}

public class DocsDrawerContext
{
    public Signal<bool> IsOpen { get; private set; }

    public DocsDrawerContext(Signal<bool> isOpen)
    {
        IsOpen = isOpen;
    }
}

public class DocsDrawerContextProviderComponent : BaseComponent
{
    public DocsDrawerContextProviderComponent(VirtualBody children = default) : base(children) { }

    public override VirtualBody Render()
    {
        var isOpen = new Signal<bool>(false);

        return F.ContextProvider(
            value: new DocsDrawerContext(isOpen),
            children: Children
        );
    }
}
