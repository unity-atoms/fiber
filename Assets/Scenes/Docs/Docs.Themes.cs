using System.Collections.Generic;
using Fiber.DesignTokens;
using Fiber.UI;
using Fiber;

public static class DocsThemes
{
    public class Provider : BaseComponent
    {
        public Provider(List<VirtualNode> children) : base(children) { }

        public override VirtualNode Render()
        {
            return F.ThemeProvider(
                themeStore: new ThemeStore(
                    theme: DARK_THEME
                ),
                children: children
            );
        }
    }

    public static readonly Theme DARK_THEME = new Theme(
        fallbackRole: "neutral",
        new()
        {
            { "neutral", new(
                background: new(
                    @default: "#3F3F3F",
                    selected: "#505050",
                    hovered: "#626262"
                ),
                border: new(
                    @default: "#2B2B2B"
                ),
                text: new(
                    @default: "#FFFFFF"
                )
            ) }
        }
    );

    public static readonly Theme LIGHT_THEME = new Theme(
        fallbackRole: "neutral",
        new()
        {
            { "neutral", new(
                background: new(
                    @default: "#FFFFFF",
                    selected: "#D2D2D2",
                    hovered: "#D2D2D2"
                ),
                border: new(
                    @default: "#E0E0E0"
                ),
                text: new(
                    @default: "#000000"
                )
            ) }
        }
    );
}