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
                    theme: LIGHT_THEME
                ),
                children: children
            );
        }
    }

    public static readonly Theme DARK_THEME = new Theme(
        new TokenCollection()
        {
            { "neutral", new Role(
                background: new (
                    regular: new (
                        @default: "#3F3F3F",
                        hovered: "#505050"
                    )
                ),
                border: new (
                    regular: new (
                        @default: "#2B2B2B"
                    )
                ),
                text: new (
                    regular: new (
                        @default: "#FFFFFF"
                    )
                )
            ) }
        }
    );

    public static readonly Theme LIGHT_THEME = new Theme(
        new TokenCollection()
        {
            { "neutral", new Role(
                background: new (
                    regular: new (
                        @default: "#FFFFFF",
                        hovered: "#F5F5F5"
                    )
                ),
                border: new (
                    regular: new (
                        @default: "#E0E0E0"
                    )
                ),
                text: new (
                    regular: new (
                        @default: "#000000"
                    )
                )
            ) }
        }
    );
}