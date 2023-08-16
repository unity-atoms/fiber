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
        var iconType = new Signal<string>("sun");

        return F.Header(children: F.Children(
            F.HeaderItemGroup(justifyContent: Justify.FlexStart, children: F.Children(
                F.Typography(
                    text: "fiber",
                    type: TypographyType.Heading3
                )
            )),
            F.HeaderItemGroup(justifyContent: Justify.FlexEnd, children: F.Children(
                F.IconButton(type: iconType, onPress: () =>
                {
                    if (themeStore.Value == DocsThemes.LIGHT_THEME)
                    {
                        themeStore.Value = DocsThemes.DARK_THEME;
                        iconType.Value = "sun";
                    }
                    else
                    {
                        themeStore.Value = DocsThemes.LIGHT_THEME;
                        iconType.Value = "moon";
                    }
                })
            ))
        ));
    }
}