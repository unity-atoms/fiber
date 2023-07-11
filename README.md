# Fiber 
Fiber is a declarative library for creating games in Unity. It is derived and inspired by web libraries such as [React](https://react.dev/) and [Solid](https://www.solidjs.com/).

- Declarative - Define what you want for particular state instead of defining how you want to create it.
- Component based - Create self contained components that can be reused in different contexts.
- Reactive - Signals are reactive primitives that makes it possible for Fiber to only update what needs to be updated.
- Extendable - Fiber is built to be extendable. Create your own renderer extension if there something that you natively are missing. 
- More than UI - Fiber is not only for UI. It can be used to declare anything in your game, eg. any game object in your scene.


## Example

```csharp
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.Suite;
using Fiber;
using Fiber.UIElements;
using Signals;

public class CounterExample : MonoBehaviour
{
    [SerializeField] private PanelSettings _defaultPanelSettings;
    public class CounterComponent : BaseComponent
    {
        public override VirtualNode Render()
        {
            var count = new Signal<int>(0);

            return F.UIDocument(
                children: F.Children(
                    F.Button(text: "Increment", onClick: (e) => { count.Value += 1; }),
                    F.Text(text: new IntToStringSignal(count))
                )
            );
        }
    }

    void Start()
    {
        var fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _defaultPanelSettings);
        fiber.Render(new CounterComponent());
    }
}
```

The above will render a classic counter that increments when clicking a button.

## Installation

Clone the repository into your project's Assets folder or make this repo a git submodule of your Unity project. 

___A more sophisticated installation process using Unity's package manager is on the roadmap.___

## Packages
- `FiberUtils`: Common utils and classes used by all other Fiber packages.
- `Signals`: Reactive primitives. Depends on FiberUtils.
- `Fiber`: The core declarative library. Depends on FiberUtils and Signals.
- `Fiber.GameObjects`: GameObjects renderer extension. Depends on FiberUtils, Signals and Fiber.
- `Fiber.UIElements`: UI Elements renderer extension. Depends on FiberUtils, Signals, Fiber and FiberGameObjects.
- [`Fiber.Router`](./Packages/FiberRouter/FiberRouter.md): A router for Fiber. Depends on Signals and Fiber.
- `Fiber.Suite`: A suite of all Fiber packages, exposing a convenient API for end users. Depends on all other Fiber packages.

## Reactivity 

Fiber is built upon reactivity and the ability to track changes to data.

### Signals

___It is possible to use Signals and Computed Signals in your game without using Fiber's renderer.___

Signals are reactive primitives that wraps a value. It is possible to both retrieve and imperatively set the value of a signal. When a signal is updated, Fiber will only update the parts of the UI that depends on that signal.

Useful built-in signals: 
- `Signal<T>` - A writeable signal.
- `ShallowSignalList<T>` - A list as a signal. Changes of items in the list are not tracked.
- `SignalList<T>` - A list as a signal where each item is a signal itself. Changes of items in the list are tracked.
- `ShallowSignalDictionary<T>` - A dictionary as a signal. Changes of items in the dictionary are not tracked.
- `SignalDictionary<T>` - A dictionary as a signal where each value is a signal itself. Changes of items in the dictionary are tracked.
- `IndexedSignalDictionary<T>` - Same as `SignalDictionary<T>`, but where each item also has an index. Useful for when you need to iterate over the dictionary witout allocating memory.
- `StaticSignal<T>` - A read only signal.

#### Computed signals

Computed signals are signals that are derived from other signals. When a signal that a computed signal depends on is updated, the computed signal will also be updated. Computed signals are read only.

Useful built-in computed signals: 
- `ComputedSignal<..., RT>` - A computed signal from 1 to many other signals.
- `DynamicComputedSignal<..., DT, RT>` - A computed signal where the exact signal dependencies are not known at the time of creation of the signal. In other words, signal dependencies can be added and removed after the computed signal has been created.
- `ComputedSignalsByKey<Key, KeysSignal, Keys, ItemSignal, ItemType>` - A computed signal dictionary where each value is in itself a computed signal. This is useful for more dynamic scenarios, eg. where we need a computed signal for each item in a `SignalList<T>`.
- `NegatedBoolSignal` - Computed signal that negates a bool signal.
- `IntToStringSignal` - Computed signal that converts an int signal to a string signal.

#### How signals work

A signal in itself can't be subscribed to directly. Instead, all signals have a dirty flag, called dirty bit. When a signal is updated, the dirty bit is incremented. Underlying primitives and systems (eg. effects or `SignalSubscribtionManager`) are polling and checking if the dirty bit has changed. For example when a computed signal's value is read, it will check if the dirty bit of any of its dependencies have changed, and if it has it will recompute its value.

### Effects

Effects takes one or more signals and calls a function each time a signal is updated. Effects are useful to perform side effects, eg. updating a game object's transform based on a signal. Note that effects are not called immediately when a signal is updated, but instead will be called by Fiber when there is time to do so, which most of the time is in the next frame.

Example of an effect that updates if a game object with a rigidbody is kinematic or not:
```csharp
public class PhysicsObjectComponent : BaseComponent
{
    BaseSignal<bool> IsKinematicSignal; // Created and set by a parent component
    public PhysicsObjectComponent(BaseSignal<bool> isKinematicSignal)
    {
        IsKinematicSignal = isKinematicSignal;
    }
    public override VirtualNode Render()
    {
        var _ref = new Ref<GameObject>();
        CreateEffect((isKinematic) =>
        {
            _ref.Current.GetComponent<Rigidbody>().isKinematic = isKinematic;
            return null;
        }, IsKinematicSignal, runOnMount: true);
        return F.GameObject(_ref: _ref, getInstance: () =>
        {
            var go = new GameObject();
            go.AddComponent<Rigidbody>();
            return go;
        });
    }
}
```

## Rendering

Rendering is the process of taking virtual nodes (user defined components of built-ins) and create native nodes. Native nodes are objects that wrap native Unity entities, eg. `GameObject` or `VisualElement`. 

### Entry

The entry point for rendering can easiest be defined using `Fiber.Suite`:     
```csharp
    var fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _myDefaultPanelSettings);
    fiber.Render(new MyComponent());
```

It is possible to define several entries in the same app in order to just Fiber in different smaller parts of your app. This can be useful if you for example want to gradually migrate an existing app to Fiber.

### Components

Components are self contained and re-useable pieces of code that defines one part of your app. 

___All built-in components can be added via the `F` property, eg. `F.GameObject`.___

#### User defined

A user defined component uses built in components and other user defined components to define a part of your app. The component can be re-used in other components and in multiple places in your app. 

##### Children

Components can be nested to create a tree and a hierarchy of components. The children of a component are defined by the `children` prop. The component itself should not care what children it renders, just where they are rendered. 

Simple example panel component using the `children` prop: 

```csharp
public class PanelComponent : BaseComponent
{
    public PanelComponent(List<VirtualNode> children) : base(children) { }

    public override VirtualNode Render()
    {
        return F.View(
            style: new Style(marginRight: 10, marginBottom: 10, marginLeft: 10, marginTop: 10, backgroundColor: Color.magenta),
            children: children
        );
    }
}
```

Example of using the above component adding different children to each instance of the panel:

```csharp
public class MyPageComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        return F.Fragment(
            F.Children(
                new PanelComponent(F.Children(F.Button(text: "Button 1", onClick: (e) => { Debug.Log("Button 1 clicked"); }))),
                new PanelComponent(F.Children(
                    F.Button(text: "Button 2", onClick: (e) => { Debug.Log("Button 2 clicked"); }),
                    F.Button(text: "Button 3", onClick: (e) => { Debug.Log("Button 3 clicked"); })
                )),
                new PanelComponent(F.Children(F.Button(text: "Button 4", onClick: (e) => { Debug.Log("Button 4 clicked"); })))
            )
        );
    }
}
```
##### Fragment

A Fragment is a component does not render anything itself, but instead renders its children directly. This is useful when you want to return multiple components from a component, eg. when you want to return a list of components from a component.

```csharp
    F.Fragment(children);
```

##### Context

Context is useful to pass values down the component tree without having to pass it down as props. A context can be defined like this: 

```csharp
    var intSignal = new Signal<int>(5);
    var myContext = new MyContext(intSignal);
    F.ContextProvider<MyContext>(value: myContext, children: children);
```

The above context can be accessed in any child component like this: 

```csharp
    var myContext = GetContext<MyContext>();
    // Alternatively the shorthand can be used:
    var myContext = C<MyContext>();
```

##### Globals

Globals are references that are injected from the outside and can be accessed from any component. Globals are useful to pass down references to services or other objects that are not part of the component tree. 

Globals are injected when creating a `FiberSuite` instance:

```csharp
    var myService = new MyService();
    new FiberSuite(
        rootGameObject: gameObject,
        globals: new()
        {
            {typeof(MyService), myService},
        }
    );
```
The above global can be accessed in any child component like this: 
```csharp
    var myService = GetGlobal<MyService>();
    // Alternatively the shorthand can be used:
    var myService = G<MyService>();
```

#### Built-ins - Fiber


##### `ContextProvider`

#### Built-ins - Fiber.GameObjects
##### `GameObjectComponent`

Component that renders a game object. 
```csharp
    F.GameObject(name: "MyGameObject", children: children);
```

#### Built-ins - Fiber.UIElements

##### `UIDocumentComponent`
Component that renders a game object with a `UIDocument` component. 
```csharp
    F.UIDocument(children: children);
```

##### `ViewComponent`
Component that renders a VirtualElement.
```csharp
    F.View(children: children);
```

##### `ButtonComponent`
Component that renders a Button.
```csharp
    F.Button(style: new Style(color: Color.black, fontSize: 20), text: "Click me", onClick: (e) => { Debug.Log("Clicked!"); });
```

##### `TextComponent`
Component that renders a TextElement.
```csharp
    F.Text(style: new Style(color: Color.black, fontSize: 20), text: "Hello world!");
```

##### `TextFieldComponent`

Component that renders a TextField.
```csharp
    var textFieldSignal = new Signal<string>("Foo");
    F.TextField(value: textFieldSignal, onChange: (e) => { textSignal.Value = e.newValue; });
```

##### `ScrollViewComponent`
Component that renders a ScrollView.
```csharp
    F.ScrollView(children: F.Children(
        F.View(className: F.ClassName("tall-container"))
    ));
```

#### Control flow

Control flow components are built-in components that will efficiently alter what is rendered based on state.

##### `Enable`
This component enables or disables underlying nodes and their effects to react to signal updates.

```csharp
    var enableSignal = new Signal<bool>(true);
    F.Enable(when: showSignal, children: F.Children(F.Text(text: "Hello world!")));
```

##### `Visible`
This component makes underlying native nodes visible or hidden.

```csharp
    var visibleSignal = new Signal<bool>(true);
    F.Visible(when: visibleSignal, children: F.Children(F.Text(text: "Hello world!")));
```

##### `Active`
This component is a composition of the `Enable` and `Visible` components above.

```csharp
    var activeSignal = new Signal<bool>(true);
    F.Active(when: activeSignal, children: F.Children(F.Text(text: "Hello world!")));
```

##### `Mount`
This component renders and mounts a component based on a signal value.

__NOTE:__ Compareable to solidjs's `Show` component.

```csharp
    var showSignal = new Signal<bool>(true);
    F.Mount(when: showSignal, children: F.Children(F.Text(text: "Hello world!")));
```

##### `For`
Renders a list of components based on a signal list. Each item in the list needs a key, which uniquely indentifies an item.

```csharp
var todoItemsSignal = new ShallowSignalList<TodoItem>(new ShallowSignalList<TodoItem>());
For<TodoItem, ShallowSignalList<TodoItem>, ShallowSignalList<TodoItem>, int>(
    each: todoItemsSignal,
    children: (item, i) =>
    {
        return (item.Id, F.Text(text: item.Text));
    }
);
```

## Architecture

The following sections describes how Fiber works under the hood.

### Virtual tree

In its essence, Fiber is building and maintaining a tree structure of nodes, which represents what currently is present in your scene. The tree is made up of so called Fiber nodes, which holds information about its parent, child and direct sibling. This info makes it easy to iterate the tree. The Fiber node can also hold a reference to a native node, which is a node wrapping a native object, such as a `GameObject` or a `VisualElement`. It also holds a reference to a virtual node, which is the underying component that was used to create the Fiber node.

### Work loop

Fiber has a work loop that runs every frame. The work loop prioritize and performs some units of work: 
- Rendering - The most prioritized work which executes the `Render` method of a component pending to be rendered. Note that rendering will create underlying native nodes, but nodes are not added to the tree yet and are set to not be visisble.
- Mount - Mounting is the process of adding a node to the tree and making it visible.
- Unmount - Unmounting is the process of removing a node from the tree and making it invisible.
- Move - Moving is the process of moving a node in the tree. 
- Node work loop - Runs update on the nodes in tree, which trigger effects if there are any pending and updates props tied to signals.

There is a time budget for the work loop (which is configureable). If the time budget is exceeded, the work loop will yield and continue the next frame.

### Node phases

A Fiber node is during its lifespan in different phases. Phases are chronlogical to the order of definitionm which means that Fiber nodes never can go back to a previous phase. The phases are:

- `AddedToVirtualTree` - Initial phase for when the node is created.
- `Rendered` - A node is set to `Rendered` after Fiber has rendered the node, eg created an underlyng game object.
- `RendereMountedd` - A node is set to `RenderedMountedd` after Fiber has mounted the node.
- `RemovedFromVirtualTree` - A node is set to `RemovedFromVirtualTree` when Fiber has decided to remove it. This action also sets the underlying native node to be not visible.
- `Unmounted` - A node is set to `Unmounted` when Fiber has unmounted the node.

