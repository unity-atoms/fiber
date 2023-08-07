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
                    @default: "#242424",
                    selected: "#333333",
                    hovered: "#333333",
                    pressed: "#222222"
                ),
                border: new(
                    @default: "#222222"
                ),
                text: new(
                    @default: "#FFFFFF",
                    selected: "#25C281"
                )
            ) },
            { "deepNeutral", new(
                background: new(
                    @default: "#1A1A1A",
                    selected: "#2C2C2C",
                    hovered: "#2C2C2C",
                    pressed: "#0F0F0F"
                ),
                border: new(
                    @default: "#0F0F0F"
                ),
                text: new(
                    @default: "#FFFFFF",
                    selected: "#25C281"
                )
            ) },
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
                    hovered: "#D2D2D2",
                    pressed: "#BEBEBE"
                ),
                border: new(
                    @default: "#E0E0E0"
                ),
                text: new(
                    @default: "#000000",
                    selected: "#25C281"
                )
            ) },
            { "deepNeutral", new(
                background: new(
                    @default: "#D2D2D2",
                    selected: "#E4E4E4",
                    hovered: "#E4E4E4",
                    pressed: "#BEBEBE"
                ),
                border: new(
                    @default: "#BEBEBE"
                ),
                text: new(
                    @default: "#000000",
                    selected: "#25C281"
                )
            ) },
        }
    );
}