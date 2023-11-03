using Fiber;
using SilkUI;
using Fiber.Router;
using Signals;

public class DocsTreeViewComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        var router = C<Router>();
        var drawerContext = C<DocsDrawerContext>();
        var selectedItemId = F.CreateComputedSignal((router) =>
        {
            var route = router.PeekRoute();
            return route?.Id;
        }, router);

        var expandedItemIds = new ShallowSignalList<string>();
        for (var i = 0; i < router.RouteStack.Count; ++i)
        {
            var route = router.RouteStack[i];
            if (route.IsLayoutRoute)
            {
                expandedItemIds.Add(route.Id);
            }
        }

        return F.SilkTreeViewContainer(
            role: DocsThemes.ROLES.DEEP_NEUTRAL,
            selectedItemId: selectedItemId,
            onItemSelected: (string id) =>
            {
                var routeDefinition = router.GetRouteDefinition(id);
                if (routeDefinition.IsLayoutRoute)
                {
                    if (expandedItemIds.Contains(id))
                    {
                        expandedItemIds.Remove(id);
                    }
                    else
                    {
                        expandedItemIds.Add(id);
                    }
                }
                else
                {
                    router.Navigate(id);
                    drawerContext.IsOpen.Value = false;
                }
            },
            expandedItemIds: expandedItemIds,
            children: F.Children(
                F.SilkTreeViewItem(id: DocsRouting.ROUTES.INTRODUCTION, label: $"Introduction"),
                F.SilkTreeViewItem(id: DocsRouting.ROUTES.INSTALLATION, label: $"Installation"),
                F.SilkTreeViewItem(id: DocsRouting.ROUTES.PACKAGES, label: $"Packages", children: F.Children(
                    F.SilkTreeViewItem(id: DocsRouting.ROUTES.ROUTER, label: $"Router"),
                    F.SilkTreeViewItem(id: DocsRouting.ROUTES.UI, label: $"UI")
                ))
            )
        );
    }
}