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
        var iconName = new Signal<string>("sun");

        return F.Header(children: F.Children(
            F.HeaderItemGroup(justifyContent: Justify.FlexStart, children: F.Children(
                F.Typography(
                    text: "fiber",
                    type: TypographyType.Heading3
                )
            )),
            F.HeaderItemGroup(justifyContent: Justify.FlexEnd, children: F.Children(
                F.IconButton(variant: "github", iconName: "github", onPress: () =>
                {
                    Application.OpenURL("https://github.com/unity-atoms/fiber");
                }),
                F.IconButton(variant: "discord", iconName: "discord", onPress: () =>
                {
                    Application.OpenURL("https://discord.gg/Jw2hRhEB");
                }),
                F.IconButton(variant: "sun", iconName: iconName, onPress: () =>
                {
                    if (themeStore.Value == DocsThemes.LIGHT_THEME)
                    {
                        themeStore.Value = DocsThemes.DARK_THEME;
                        iconName.Value = "sun";
                    }
                    else
                    {
                        themeStore.Value = DocsThemes.LIGHT_THEME;
                        iconName.Value = "moon";
                    }
                })
            ))
        ));
    }
}