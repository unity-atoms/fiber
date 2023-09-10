using Fiber;
using Fiber.UIElements;
using Fiber.UI;
using Fiber.Router;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;

public class DocsLandingPageComponent : BaseComponent
{
    public class FeatureComponent : BaseComponent
    {
        private readonly Style _style;
        public FeatureComponent(List<VirtualNode> children, Style style = new()) : base(children)
        {
            _style = style;
        }
        public override VirtualNode Render()
        {
            var themeStore = C<ThemeStore>();

            return F.View(
                style: new Style(
                    mergedStyle: _style,
                    width: new Length(100, LengthUnit.Percent),
                    maxHeight: new Length(100, LengthUnit.Percent),
                    alignItems: Align.Center,
                    justifyContent: Justify.Center,
                    paddingLeft: themeStore.Spacing(7),
                    paddingRight: themeStore.Spacing(7),
                    marginTop: themeStore.Spacing(7),
                    marginBottom: themeStore.Spacing(7)
                ),
                children: children
            );
        }
    }

    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();
        var logoSize = F.CreateComputedSignal((isSmall) => !isSmall ? DocsLogoSize.Large : DocsLogoSize.XL, themeStore.IsSmallScreen);
        var typographyType = F.CreateComputedSignal((isSmall) => !isSmall ? TypographyType.Heading3 : TypographyType.Heading2, themeStore.IsSmallScreen);
        var router = C<Router>();

        return F.Fragment(
            children: F.Children(
                new FeatureComponent(
                    style: new Style(marginTop: themeStore.Spacing(24)),
                    children: F.Children(
                        F.View(
                            style: new Style(
                                display: DisplayStyle.Flex,
                                flexDirection: FlexDirection.Column,
                                justifyContent: Justify.Center,
                                alignItems: Align.Center,
                                marginBottom: themeStore.Spacing(6)
                            ),
                            children: F.Children(
                                new DocsLogoComponent(
                                    size: logoSize,
                                    style: new Style(
                                        marginBottom: themeStore.Spacing(-4)
                                    )
                                ),
                                F.Typography(
                                    text: "A declarative library for Unity",
                                    type: typographyType
                                )
                            )
                        ),
                        new FiberUIButtonComponent(
                            children: F.Children(F.Text(text: "Get Started")),
                            role: DocsThemes.ROLES.PRIMARY,
                            onPress: () => { router.Navigate(DocsRouting.ROUTES.INTRODUCTION); }
                        )
                    )
                )
            // Build robust and scaleable UIs faster
            // Component based, made to be rearranged like lego blocks 
            // Reactive at its core - powered by signals
            )
        );
    }
}
