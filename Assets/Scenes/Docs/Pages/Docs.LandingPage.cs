using Fiber;
using Fiber.UIElements;
using SilkUI;
using Fiber.Router;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;

public class DocsLandingPageComponent : BaseComponent
{
    public class FeatureComponent : BaseComponent
    {
        private readonly Style _style;
        public FeatureComponent(VirtualBody children, Style style = new()) : base(children)
        {
            _style = style;
        }
        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();

            return F.ScrollView(
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
                children: Children
            );
        }
    }

    public override VirtualBody Render()
    {
        var themeStore = C<ThemeStore>();
        var logoSize = F.CreateComputedSignal((isSmall) => !isSmall ? DocsLogoSize.Large : DocsLogoSize.XL, themeStore.IsSmallScreen);
        var typographyType = F.CreateComputedSignal((isSmall) => !isSmall ? TypographyType.Heading3 : TypographyType.Heading2, themeStore.IsSmallScreen);
        var router = C<Router>();

        return F.Fragment(
            children: F.Nodes(
                new FeatureComponent(
                    style: new Style(marginTop: themeStore.Spacing(24)),
                    children: F.Nodes(
                        F.View(
                            style: new Style(
                                display: DisplayStyle.Flex,
                                flexDirection: FlexDirection.Column,
                                justifyContent: Justify.Center,
                                alignItems: Align.Center,
                                marginBottom: themeStore.Spacing(6)
                            ),
                            children: F.Nodes(
                                new DocsLogoComponent(
                                    size: logoSize,
                                    style: new Style(
                                        marginBottom: themeStore.Spacing(-4)
                                    )
                                ),
                                F.SilkTypography(
                                    text: "A declarative library for Unity",
                                    type: typographyType
                                )
                            )
                        ),
                        F.SilkButton(
                            children: F.Text(text: "Get Started"),
                            role: DocsThemes.ROLE.PRIMARY,
                            onPress: (evt) => { router.Navigate(DocsRouting.ROUTES.INTRODUCTION); }
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
