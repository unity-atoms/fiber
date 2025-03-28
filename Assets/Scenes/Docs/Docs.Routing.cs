using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.Router;
using SilkUI;
using Signals;
using Fiber.Cursed;

public static class DocsRouting
{
    public static class ROUTES
    {
        // Layout routes
        public const string ROOT = "root";

        // Path routes
        public const string LANDING = "landing";
        public const string DOCS = "docs";
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

        public override VirtualBody Render()
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
        public override VirtualBody Render()
        {
            return F.ScalingProvider(
                children: F.OverrideVisualComponentsProvider(
                    overrideVisualComponents: new OverrideVisualComponents(),
                    children: new DocsThemes.Provider(
                        children: F.CursorManager(
                            cursorDefinitionsStore: new Store<ShallowSignalList<CursorDefinition>>(),
                            children: F.UIRoot(
                                name: "DocsRootDocument",
                                children: new DocsDrawerContextProviderComponent(
                                    new MainLayoutComponent()
                                )
                            )
                        )
                    )
                )
            );
        }
    }

    public class SideMenuComponent : BaseComponent
    {
        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var backgroundColor = themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Background, DocsThemes.SUBROLE.DEEP);
            var deepBorderColor = themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Border, DocsThemes.SUBROLE.DEEP);

            return F.Mount(
                when: themeStore.IsMediumScreen,
                children: F.View(
                    style: new Style(
                        width: 280,
                        height: new Length(100, LengthUnit.Percent),
                        backgroundColor: themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Background, DocsThemes.SUBROLE.DEEP),
                        borderRightColor: deepBorderColor,
                        borderTopColor: deepBorderColor,
                        borderBottomColor: deepBorderColor,
                        borderRightWidth: themeStore.BorderWidth(),
                        borderTopWidth: themeStore.BorderWidth(),
                        borderBottomWidth: themeStore.BorderWidth(),
                        flexShrink: 0,
                        flexGrow: 0
                    ),
                    children: new DocsTreeViewComponent()
                )
            );
        }
    }

    public class MainLayoutComponent : BaseComponent
    {
        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();

            return F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    backgroundColor: C<ThemeStore>().Color(DocsThemes.ROLE.NEUTRAL, ElementType.Background),
                    height: new Length(100, LengthUnit.Percent),
                    width: new Length(100, LengthUnit.Percent),
                    flexDirection: FlexDirection.Column,
                    position: Position.Relative
                ),
                children: F.Nodes(
                    new DocsHeaderComponent(),
                    F.Outlet()
                )
            );
        }
    }

    public class DocsRootComponent : BaseComponent
    {
        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();

            return F.Nodes(
                F.View(
                    style: new Style(
                        display: DisplayStyle.Flex,
                        alignItems: Align.Stretch,
                        flexDirection: FlexDirection.RowReverse,
                        flexGrow: 1,
                        flexShrink: 1
                    ),
                    children: F.Nodes(
                        F.View(
                            style: new Style(
                                backgroundColor: themeStore.Color(DocsThemes.ROLE.NEUTRAL, ElementType.Background),
                                minHeight: new Length(100, LengthUnit.Percent),
                                flexShrink: 1,
                                flexGrow: 1,
                                minWidth: new Length(100, LengthUnit.Pixel),
                                paddingLeft: themeStore.Spacing(7),
                                paddingRight: themeStore.Spacing(7)
                            ),
                            children: F.Outlet()
                        ),
                        new SideMenuComponent()
                    )
                ),
                new DocsDrawerComponent()
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
                id: ROUTES.LANDING,
                isLayoutRoute: false,
                component: new SimpleRouteComponent(component: new DocsLandingPageComponent())
            ),
            new RouteDefinition(
                id: ROUTES.DOCS,
                isLayoutRoute: true,
                component: new SimpleRouteComponent(component: new DocsRootComponent()),
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
                        isLayoutRoute: true,
                        component: new SimpleRouteComponent(component: new OutletComponent(isPooled: false)),
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

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();

            return F.SilkTypography(
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
        private readonly Router _router;
        public RouterProvider(Router router)
        {
            _router = router;
        }
        public override VirtualBody Render()
        {
            return F.RouterProvider(_router);
        }
    }
}