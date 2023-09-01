using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.Router;
using Fiber.UI;
using Fiber.Theme;
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
        public const string PACKAGES = "packages";
        public const string ROUTER = "router";
        public const string UI = "ui";
    }

    // TODO: Remove. For testing purposes only
    private class IconOverrideComponent : BaseComponent
    {
        private readonly Ref<VisualElement> _forwardRef;
        public IconOverrideComponent(
            Ref<VisualElement> forwardRef
        )
        {
            _forwardRef = forwardRef;
        }

        public override VirtualNode Render()
        {
            return F.Text(_ref: _forwardRef, style: new Style(
                height: 20,
                width: 20,
                backgroundColor: Color.red
            ));
        }
    }

    public class RootComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            return F.ScalingProvider(
                children: F.Children(F.OverrideVisualComponentsProvider(
                    overrideVisualComponents: new OverrideVisualComponents(),
                    children: F.Children(new DocsThemes.Provider(
                        children: F.Children(
                            F.UIRoot(
                                name: "DocsRootDocument",
                                children: F.Children(new MainLayoutComponent())
                            )
                        )
                    ))
                ))
            );
        }
    }

    public class MainLayoutComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var router = C<Router>();
            var selectedItemId = F.CreateComputedSignal((router) =>
            {
                var route = router.PeekRoute();
                return route?.Id;
            }, router);

            var expandedItemIds = F.CreateComputedSignalList<Router, string>((router, list) =>
            {
                for (var i = 0; i < router.RouteStack.Count; ++i)
                {
                    var route = router.RouteStack[i];
                    list.Add(route.Id);
                }
                return list;
            }, router);

            var themeStore = C<ThemeStore>();
            var backgroundColor = themeStore.Color(DocsThemes.ROLES.DEEP_NEUTRAL, ElementType.Background);
            var deepBorderColor = themeStore.Color(DocsThemes.ROLES.DEEP_NEUTRAL, ElementType.Border);


            return F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    height: new Length(100, LengthUnit.Percent),
                    width: new Length(100, LengthUnit.Percent),
                    flexDirection: FlexDirection.Column,
                    position: Position.Relative
                ),
                children: F.Children(
                    new DocsHeaderComponent(),
                    F.View(
                        style: new Style(
                            display: DisplayStyle.Flex,
                            alignItems: Align.Stretch,
                            flexDirection: FlexDirection.Row,
                            flexGrow: 1,
                            flexShrink: 1
                        ),
                        children: F.Children(
                            F.View(
                                style: new Style(
                                    minWidth: 100,
                                    maxWidth: 240,
                                    width: new Length(25, LengthUnit.Percent),
                                    height: new Length(100, LengthUnit.Percent),
                                    backgroundColor: themeStore.Color(DocsThemes.ROLES.DEEP_NEUTRAL, ElementType.Background),
                                    borderRightColor: deepBorderColor,
                                    borderTopColor: deepBorderColor,
                                    borderBottomColor: deepBorderColor,
                                    borderRightWidth: 1,
                                    borderTopWidth: 1,
                                    borderBottomWidth: 1,
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
                                    expandedItemIds: expandedItemIds,
                                    children: F.Children(
                                        F.TreeViewItem(id: ROUTES.INTRODUCTION, label: $"Introduction"),
                                        F.TreeViewItem(id: ROUTES.INSTALLATION, label: $"Installation"),
                                        F.TreeViewItem(id: ROUTES.PACKAGES, label: $"Packages", children: F.Children(
                                            F.TreeViewItem(id: ROUTES.ROUTER, label: $"Router"),
                                            F.TreeViewItem(id: ROUTES.UI, label: $"UI")
                                        ))
                                    )
                                ))
                            ),
                            F.View(
                                style: new Style(
                                    backgroundColor: themeStore.Color(DocsThemes.ROLES.NEUTRAL, ElementType.Background),
                                    minHeight: new Length(100, LengthUnit.Percent),
                                    flexShrink: 1,
                                    flexGrow: 1,
                                    minWidth: new Length(100, LengthUnit.Pixel),
                                    paddingLeft: themeStore.Spacing(7),
                                    paddingRight: themeStore.Spacing(7)
                                ),
                                children: F.Children(F.Outlet())
                            )
                        )
                    ),
                    F.DebugWindowScaling()
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
                component: new SimpleRouteComponent(component: new DocsIntroductionPageComponent())
            ),
            new RouteDefinition(
                id: ROUTES.INSTALLATION,
                isLayoutRoute: false,
                component: new SimpleRouteComponent(component: new HeadingComponent("Installation"))
            ),
            new RouteDefinition(
                id: ROUTES.PACKAGES,
                isLayoutRoute: false,
                component: new SimpleRouteComponent(component: new DocsPackagesPageComponent()),
                children: new List<RouteDefinition>()
                {
                    new RouteDefinition(
                        id: ROUTES.ROUTER,
                        isLayoutRoute: false,
                        component: new SimpleRouteComponent(component: new HeadingComponent("Router"))
                    ),
                    new RouteDefinition(
                        id: ROUTES.UI,
                        isLayoutRoute: false,
                        component: new SimpleRouteComponent(component: new HeadingComponent("UI"))
                    ),
                }
            ),
        }
    );

    public class HeadingComponent : BaseComponent
    {
        private readonly string _text;
        public HeadingComponent(string text)
        {
            _text = text;
        }

        public override VirtualNode Render()
        {
            var themeStore = C<ThemeStore>();

            return F.Typography(
                text: _text,
                type: TypographyType.Heading1,
                style: new Style(
                    marginTop: themeStore.Spacing(3),
                    marginBottom: themeStore.Spacing(3)
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