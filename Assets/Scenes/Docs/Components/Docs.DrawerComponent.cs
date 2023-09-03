using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.UI;
using Signals;
using UnityEngine;

public class DocsDrawerComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();
        var drawerContext = C<DocsDrawerContext>();

        return F.Mount(
            when: new NegatedBoolSignal(themeStore.IsMediumScreen),
            children: F.Children(
                F.Visible(
                    when: drawerContext.IsOpen,
                    children: F.Children(new OverlayComponent())
                ),
                new DrawerComponent(
                    role: DocsThemes.ROLES.DEEP_NEUTRAL,
                    isOpen: drawerContext.IsOpen,
                    children: F.Children(
                        F.View(
                            style: new Style(
                                display: DisplayStyle.Flex,
                                flexDirection: FlexDirection.Column
                            ),
                            children: F.Children(
                                F.View(
                                    style: new Style(
                                        display: DisplayStyle.Flex,
                                        flexDirection: FlexDirection.Row,
                                        alignItems: Align.Center,
                                        justifyContent: Justify.SpaceBetween,
                                        height: themeStore.Spacing(14),
                                        paddingBottom: themeStore.Spacing(2),
                                        paddingTop: themeStore.Spacing(2),
                                        paddingLeft: themeStore.Spacing(4),
                                        paddingRight: themeStore.Spacing(4),
                                        backgroundColor: themeStore.Color(DocsThemes.ROLES.NEUTRAL, ElementType.Background),
                                        borderBottomWidth: 1,
                                        borderBottomColor: themeStore.Color(DocsThemes.ROLES.NEUTRAL, ElementType.Border)
                                    ),
                                    children: F.Children(
                                        F.Typography(
                                            text: "fiber",
                                            type: TypographyType.Heading3
                                        ),
                                        F.IconButton(
                                            iconName: "xmark",
                                            onPress: () =>
                                            {
                                                drawerContext.IsOpen.Value = false;
                                            }
                                        )
                                    )
                                ),
                                new DocsTreeViewComponent()
                            )
                        )
                    ),
                    position: DrawerPosition.Left
                )
            )
        );
    }
}

public class OverlayComponent : BaseComponent
{
    public OverlayComponent(List<VirtualNode> children = null) : base(children) { }
    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();

        return F.View(
            style: new Style(
                position: Position.Absolute,
                top: 0,
                left: 0,
                right: 0,
                bottom: 0,
                // TODO: Use color token
                backgroundColor: new Color(0, 0, 0, 0.5f)
            ),
            children: children
        );
    }
}

public class DocsDrawerContext
{
    public Signal<bool> IsOpen { get; private set; }

    public DocsDrawerContext(Signal<bool> isOpen)
    {
        IsOpen = isOpen;
    }
}

public class DocsDrawerContextProviderComponent : BaseComponent
{
    public DocsDrawerContextProviderComponent(List<VirtualNode> children = null) : base(children) { }

    public override VirtualNode Render()
    {
        var isOpen = new Signal<bool>(false);

        return F.ContextProvider(
            value: new DocsDrawerContext(isOpen),
            children: children
        );
    }
}

public enum DrawerPosition
{
    Left = 0,
    Right = 1
}

public class DrawerComponent : BaseComponent
{
    private readonly string _role;
    private readonly DrawerPosition _position;
    private readonly BaseSignal<bool> _isOpen;
    public DrawerComponent(
        List<VirtualNode> children,
        string role,
        BaseSignal<bool> isOpen,
        DrawerPosition position = DrawerPosition.Left
    ) : base(children)
    {
        _role = role;
        _isOpen = isOpen;
        _position = position;
    }

    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();
        var backgroundColor = themeStore.Color(_role, ElementType.Background);
        var borderColor = themeStore.Color(_role, ElementType.Border);
        var translate = F.CreateComputedSignal<bool, StyleTranslate>((isOpen) =>
        {
            return isOpen ?
                new Translate(new Length(0, LengthUnit.Percent), new Length(0, LengthUnit.Pixel)) :
                new Translate(new Length(-100, LengthUnit.Percent), new Length(0, LengthUnit.Pixel));
        }, _isOpen);

        return F.View(
            style: new Style(
                position: Position.Absolute,
                top: 0,
                left: _position == DrawerPosition.Left ? 0 : StyleKeyword.Initial,
                right: _position == DrawerPosition.Right ? 0 : StyleKeyword.Initial,
                bottom: 0,
                translate: translate,
                transitionProperty: new List<StylePropertyName>() { new("translate") },
                transitionDuration: new List<TimeValue>() { new(0.2f, TimeUnit.Second) },
                width: new Length(80, LengthUnit.Percent),
                backgroundColor: backgroundColor,
                borderRightColor: borderColor,
                borderLeftColor: borderColor,
                borderTopColor: borderColor,
                borderBottomColor: borderColor,
                borderRightWidth: _position == DrawerPosition.Left ? 1 : 0,
                borderLeftWidth: _position == DrawerPosition.Right ? 1 : 0,
                borderTopWidth: 1,
                borderBottomWidth: 1
            ),
            children: children
        );
    }
}