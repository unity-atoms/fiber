using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.Router;
using Fiber.UI;
using Fiber.DesignTokens;
using Signals;

public class DocsHeaderComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        var themeStore = C<ThemeStore>();

        return F.Header(children: F.Children(
            F.View(
                style: new Style(
                    display: DisplayStyle.Flex,
                    flexDirection: FlexDirection.Row,
                    alignItems: Align.Center,
                    justifyContent: Justify.FlexStart,
                    paddingLeft: themeStore.Spacing(4),
                    paddingRight: themeStore.Spacing(4),
                    width: new Length(100, LengthUnit.Percent),
                    height: new Length(100, LengthUnit.Percent)
                ),
                children: F.Children(
                    F.Typography(
                        text: "fiber",
                        type: TypographyType.Heading3
                    )
                )
            )
        ));
    }
}