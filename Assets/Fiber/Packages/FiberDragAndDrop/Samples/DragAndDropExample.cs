using System.Collections.Generic;
using UnityEngine;
using Fiber.Suite;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using SilkUI;
using Signals;
using Fiber.DragAndDrop;

public class DragAndDropExample : MonoBehaviour
{
    [SerializeField] private PanelSettings _defaultPanelSettings;

    private const string DEBUG_ROLE = "debug";

    public class ItemComponent : BaseComponent
    {
        private readonly int _id;
        public ItemComponent(int id) : base()
        {
            _id = id;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var borderColor = themeStore.Color(DEBUG_ROLE, ElementType.Border);
            var backgroundColor = themeStore.Color(DEBUG_ROLE, ElementType.Background);


            return F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    flexDirection: FlexDirection.Row,
                    justifyContent: Justify.Center,
                    alignItems: Align.Center
                ),
                children: F.Nodes(
                    F.DragHandle<int>(
                        children: F.SilkIcon(
                            iconName: "grip-vertical",
                            size: IconSize.Small,
                            style: new Style()
                        )
                    ),
                    F.View(
                        pickingMode: PickingMode.Position,
                        style: new Style(
                            width: 32,
                            height: 32,
                            paddingBottom: themeStore.Spacing(1),
                            paddingLeft: themeStore.Spacing(1),
                            paddingRight: themeStore.Spacing(1),
                            paddingTop: themeStore.Spacing(1),
                            backgroundColor: backgroundColor,
                            borderRightColor: borderColor,
                            borderLeftColor: borderColor,
                            borderTopColor: borderColor,
                            borderBottomColor: borderColor,
                            borderRightWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            borderLeftWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            borderTopWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            borderBottomWidth: themeStore.BorderWidth(BorderWidthType.Thick),
                            display: DisplayStyle.Flex,
                            justifyContent: Justify.Center,
                            alignItems: Align.Center,
                            marginRight: themeStore.Spacing(1),
                            marginBottom: themeStore.Spacing(1),
                            marginTop: themeStore.Spacing(1),
                            marginLeft: themeStore.Spacing(1)
                        ),
                        children: F.SilkTypography(
                            role: DEBUG_ROLE,
                            type: TypographyType.Body1,
                            text: $"{_id}"
                        )
                    )
                )
            );
        }
    }

    public class ExampleContainer : BaseComponent
    {
        public override VirtualBody Render()
        {
            var items = new ShallowSignalList<int>(new List<int> { 1, 2, 3, 4, 5 });
            var themeStore = C<ThemeStore>();

            return F.SilkUIProvider(
                children: F.UIDocument(
                    children: F.View(
                        style: new Style(
                            flexDirection: FlexDirection.Row,
                            justifyContent: Justify.Center,
                            alignItems: Align.Center,
                            width: 400,
                            height: 400,
                            backgroundColor: themeStore.Color(DEBUG_ROLE, ElementType.Background)
                        ),
                        children: F.DragAndDropList(
                            items: items,
                            children: (item, index) =>
                            {
                                return (item, new ItemComponent(item));
                            },
                            animationType: DragAndDropListAnimationType.Linear,
                            isItemDragHandle: false
                        )
                    )
                )
            );
        }
    }

    private class ExampleRoot : BaseComponent
    {
        public ExampleRoot() : base() { }

        public override VirtualBody Render()
        {
            return F.SilkUIProvider(
                children: new ExampleContainer()
            );
        }
    }

    void Start()
    {
        var fiber = new FiberSuite(rootGameObject: gameObject, defaultPanelSettings: _defaultPanelSettings);
        fiber.Render(new ExampleRoot());
    }
}
