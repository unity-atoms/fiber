# Fiber Router

Fiber router is a declarative router solution. It is inspired by [react-router](https://reactrouter.com/en/main).

## Example
```csharp
public class GameComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        return F.RouterProvider(GameRouting.ROUTER_TREE);
    }
}
```

## Router tree
 
A router tree declares how routing works in the game. It is a tree structure where each node is a route. 

### Navigateable routes vs layout routes

There are 2 types of routes, navigateable routes and layout routes. The difference is that navigateable routes can be directly navigated to, while layout routes are used to provide the same component hierarchy for several navigateable routes. An example of a layout route is a "root route" that renders a header and a footer. It is not possible to navigate directly to the header and footer, but they are part of both the "world selection" route and the "in world" route.


### Modals

Both type of routes can define modal routes. Modal routes are routes that are displayed on top of the current route. The typical use case is a pause menu that is displayed on top of the current route. Modals stack in the order that they are declared in the route.

### Example
```csharp
    public static RouteDefinition ROUTER_TREE = new RouteDefinition(
        id: "root",
        isLayoutRoute: true,
        component: new SimpleRouteComponent(new MyRootComponent()),
        children: new List<RouteDefinition>() {
            new RouteDefinition(
                id: "mainMenu",
                isLayoutRoute: false,
                component: new SimpleRouteComponent(component: new MainMenuComponent())
            ),
            new RouteDefinition(
                id: "inGame",
                isLayoutRoute: true,
                component: new SimpleRouteComponent(component: new InGameComponent()),
                children: new List<RouteDefinition>()
                {
                    new RouteDefinition(
                        id: "worldSelection",
                        isLayoutRoute: false,
                        component: new SimpleRouteComponent(component: new WorldSelectionComponent())
                    ),
                    new RouteDefinition(
                        id: "inWorld",
                        isLayoutRoute: false,
                        component: new SimpleRouteComponent(component: new InWorldComponent())
                    ),
                },
                modals: new List<ModalRouteDefinition>()
                {
                    new ModalRouteDefinition(
                        id: "pauseMenu",
                        component: new PauseMenuComponent()
                    )
                }
            ),
        },
        modals: new List<ModalRouteDefinition>()
        {
            new ModalRouteDefinition(
                id: "terminal",
                component: new TerminalComponent()
            )
        }
    );
```

- The root route defines a common component, `MyRootComponent`, that wraps the entire game. It also defines a terminal modal that can be displayed on top of any route. The root route is of course a layout route that is not navigateable.
-  The first navigateable route is "mainMenu". It defines `MainMenuComponent` which will be mounted when calling `Navigate("mainMenu")`. 
- Next comes "inGame", which is a layout route. It defines 2 sub routes, "worldSelection" and "inWorld". It also defines a pause menu modal that can be displayed on top of any of the sub routes. Note that the terminal defined in the root will be displayed on top of the pause menu if both are mounted.
- "worldSelection" and "inWorld" are two navigateable routes that both define a component that will be mounted when navigating to them.

## Router API
- `Navigate(string path)` - navigate to the route.
- `Navigate<T>(string path, T context)` - navigate to the route and pass a context to that route.
- `PushModal(string id)` - show a modal.
- `PushModal<T>(string id, T context)` - show a modal and pass a context to that modal.
- `PopModal(string id)` - hide a modal by id.
- `IsModalShowing(string id)` - returns `true` if modal by id is showing.
- `PeekRoute()` - returns the current route.
- `PeekModal()` - returns the current modal.

## Components

### `RouterProvider`
Wrap your application in this provider to enable routing. It takes a router tree as input.
```csharp
public class GameComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        return F.RouterProvider(GameRouting.ROUTER_TREE);
    }
}
```

### `SimpleRouteComponent`
Every component defined in the router tree needs to be a route component. `SimpleRouteComponent` is the most simple route component, which just mounts the component if visible, otherwise it unmounts it. 

### `KeepMountedRouteComponent`
Anoher route component, which is similar to `SimpleRouteComponent`. However, after the first mount of this component, it will stay mounted and instead become active / inactive (using `ActiveComponent`) depending on the route state. 

### `OutletComponent`

This component is used in parent routes to render the child routes. For example, `MyRootComponent` needs to render `OutletComponent` in order to render the child routes.

```csharp
public class MyRootComponent : BaseComponent
{
    public override VirtualBody Render()
    {
        return new MyProvider(F.Children(
            F.Outlet()
        ));
    }
}
```