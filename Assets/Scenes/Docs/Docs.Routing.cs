using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.Router;
using Fiber.UI;
using Signals;

public static class DocsRouting
{
    public static class ROUTES
    {
        // Layout routes
        public const string ROOT = "root";

        // Path routes
        public const string INTRODUCTION = "introduction";
        public const string INSTALLATION = "installation";
    }

    public class RootComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            return new DocsThemes.Provider(children: F.Children(
                F.UIDocument(
                    name: "DocsRootDocument",
                    children: F.Children(new MainLayoutComponent())
                )
            ));
        }
    }

    public class MainLayoutComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var theme = C<ThemeStore>().Get();

            var router = C<Router>();
            var selectedItemId = F.CreateComputedSignal((router) =>
            {
                var route = router.PeekRoute();
                return route?.Id;
            }, router);

            return F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    height: new Length(100, LengthUnit.Percent),
                    width: new Length(100, LengthUnit.Percent),
                    alignItems: Align.Stretch,
                    flexDirection: FlexDirection.Row
                ),
                children: F.Children(
                    F.View(
                        style: new Style(
                            minWidth: 100,
                            maxWidth: 240,
                            width: new Length(25, LengthUnit.Percent),
                            height: new Length(100, LengthUnit.Percent),
                            backgroundColor: theme.DesignTokens[DocsThemes.ROLES.DEEP_NEUTRAL].Background.Default,
                            borderRightColor: theme.DesignTokens[DocsThemes.ROLES.DEEP_NEUTRAL].Border.Default,
                            borderRightWidth: 1,
                            flexShrink: 0,
                            flexGrow: 0
                        ),
                        children: F.Children(F.TreeViewContainer(
                            role: DocsThemes.ROLES.DEEP_NEUTRAL,
                            selectedItemId: selectedItemId,
                            onItemIdSelected: (string id) =>
                            {
                                router.Navigate(id);
                            },
                            children: F.Children(
                                F.TreeViewItem(id: ROUTES.INTRODUCTION, label: $"Introduction"),
                                F.TreeViewItem(id: ROUTES.INSTALLATION, label: $"Installation")
                            )
                        ))
                    ),
                    F.View(
                        style: new Style(
                            backgroundColor: theme.DesignTokens[DocsThemes.ROLES.NEUTRAL].Background.Default,
                            minHeight: new Length(100, LengthUnit.Percent),
                            flexShrink: 1,
                            flexGrow: 1,
                            minWidth: new Length(100, LengthUnit.Pixel)
                        ),
                        children: F.Children(F.Outlet())
                    )
                )
            );
        }
    }

    public static readonly RouteDefinition ROUTER_TREE = new RouteDefinition(
        id: ROUTES.ROOT,
        isLayoutRoute: true,
        component: new SimpleRouteComponent(new RootComponent()),
        children: new List<RouteDefinition>()
        {
            new RouteDefinition(
                id: ROUTES.INTRODUCTION,
                isLayoutRoute: false,
                component: new SimpleRouteComponent(component: new IntroductionComponent())
            ),
            new RouteDefinition(
                id: ROUTES.INSTALLATION,
                isLayoutRoute: false,
                component: new SimpleRouteComponent(component: new InstallationComponent())
            ),
        }
    );

    public class IntroductionComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var theme = C<ThemeStore>().Get();

            return F.Text(
                text: "Introduction",
                style: new Style(
                    color: theme.DesignTokens[DocsThemes.ROLES.NEUTRAL].Text.Default,
                    fontSize: 36,
                    paddingLeft: 28,
                    paddingTop: 12,
                    paddingRight: 12,
                    paddingBottom: 28
                )
            );
        }
    }

    public class InstallationComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var theme = C<ThemeStore>().Get();

            return F.Text(
                text: "Installation",
                style: new Style(
                    color: theme.DesignTokens[DocsThemes.ROLES.NEUTRAL].Text.Default,
                    fontSize: 36,
                    paddingLeft: 28,
                    paddingTop: 12,
                    paddingRight: 12,
                    paddingBottom: 28
                )
            );
        }
    }

    public class RouterProvider : BaseComponent
    {
        public override VirtualNode Render()
        {
            return F.RouterProvider(new Router(ROUTER_TREE).Navigate(ROUTES.INTRODUCTION));
        }
    }
}