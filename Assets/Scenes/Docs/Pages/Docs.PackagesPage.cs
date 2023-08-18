using Fiber;
using Fiber.UIElements;
using Fiber.Router;
using Fiber.UI;
using Fiber.DesignTokens;

public class DocsPackagesPageComponent : BaseComponent
{
    public class Content : BaseComponent
    {
        public override VirtualNode Render()
        {
            var themeStore = C<ThemeStore>();

            return F.Fragment(F.Children(
                F.Typography(
                    text: "Packages",
                    type: TypographyType.Heading1,
                    style: new Style(
                        marginTop: themeStore.Spacing(3),
                        marginBottom: themeStore.Spacing(3)
                    )
                )
            ));
        }
    }

    public override VirtualNode Render()
    {
        var router = C<Router>();
        var isExactMatch = F.CreateComputedSignal((router) =>
        {
            var peedkedRoute = router.PeekRoute();
            return peedkedRoute.Id == DocsRouting.ROUTES.PACKAGES;
        }, router);

        return F.Switch(
            fallback: F.Outlet(),
            children: Children(
                Match(when: isExactMatch, children: Children(new Content()))
            )
        );
    }
}
