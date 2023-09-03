using Fiber;
using Fiber.UI;
using Fiber.Router;
using Signals;

public class DocsTreeViewComponent : BaseComponent
{
    public override VirtualNode Render()
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

        return F.TreeViewContainer(
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
                F.TreeViewItem(id: DocsRouting.ROUTES.INTRODUCTION, label: $"Introduction"),
                F.TreeViewItem(id: DocsRouting.ROUTES.INSTALLATION, label: $"Installation"),
                F.TreeViewItem(id: DocsRouting.ROUTES.PACKAGES, label: $"Packages", children: F.Children(
                    F.TreeViewItem(id: DocsRouting.ROUTES.ROUTER, label: $"Router"),
                    F.TreeViewItem(id: DocsRouting.ROUTES.UI, label: $"UI")
                ))
            )
        );
    }
}