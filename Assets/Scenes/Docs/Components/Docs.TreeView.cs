using Fiber;
using Fiber.UI;
using Fiber.Router;

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

        var expandedItemIds = F.CreateComputedSignalList<Router, string>((router, list) =>
        {
            for (var i = 0; i < router.RouteStack.Count; ++i)
            {
                var route = router.RouteStack[i];
                list.Add(route.Id);
            }
            return list;
        }, router);

        return F.TreeViewContainer(
            role: DocsThemes.ROLES.DEEP_NEUTRAL,
            selectedItemId: selectedItemId,
            onItemIdSelected: (string id) =>
            {
                router.Navigate(id);
                drawerContext.IsOpen.Value = false;
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