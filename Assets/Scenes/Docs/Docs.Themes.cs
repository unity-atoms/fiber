using System.Collections.Generic;
using Fiber.DesignTokens;
using Fiber.UI;
using Fiber;
using UnityEngine;

public static class DocsThemes
{
    public static class ROLES
    {
        public const string NEUTRAL = "neutral";
        public const string DEEP_NEUTRAL = "deepNeutral";
    }

    public static class COLOR_PALETTE
    {
        public const string GRAY_0 = "#000000";
        public const string GRAY_7 = "#111111";
        public const string GRAY_13 = "#222222";
        public const string GRAY_20 = "#333333";
        public const string GRAY_27 = "#444444";
        public const string GRAY_33 = "#555555";
        public const string GRAY_40 = "#666666";
        public const string GRAY_47 = "#777777";
        public const string GRAY_53 = "#888888";
        public const string GRAY_60 = "#999999";
        public const string GRAY_67 = "#aaaaaa";
        public const string GRAY_73 = "#bbbbbb";
        public const string GRAY_80 = "#cccccc";
        public const string GRAY_87 = "#dddddd";
        public const string GRAY_93 = "#eeeeee";
        public const string GRAY_100 = "#ffffff";

        public const string LIGHT_BLUE_31 = "#01579B";
        public const string LIGHT_BLUE_37 = "#0277BD";
        public const string LIGHT_BLUE_41 = "#0288D1";
        public const string LIGHT_BLUE_45 = "#039BE5";
        public const string LIGHT_BLUE_48 = "#03A9F4";
        public const string LIGHT_BLUE_64 = "#4FC3F7";
        public const string LIGHT_BLUE_74 = "#81D4FA";
        public const string LIGHT_BLUE_85 = "#B3E5FC";
        public const string LIGHT_BLUE_94 = "#E1F5FE";
    }

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

    public static readonly TypographyTokens TYPOGRAPHY_TOKENS = new(
        heading1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 40,
            fontStyle: FontStyle.Bold
        ),
        heading3: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 20,
            fontStyle: FontStyle.Bold
        ),
        body1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 12,
            fontStyle: FontStyle.Normal
        ),
        subtitle2: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 12,
            fontStyle: FontStyle.Normal
        )
    );

    public static readonly Theme DARK_THEME = new Theme(
        fallbackRole: ROLES.NEUTRAL,
        color: new()
        {
            { ROLES.NEUTRAL, new(
                background: new(
                    @default: COLOR_PALETTE.GRAY_13,
                    selected: COLOR_PALETTE.GRAY_20,
                    hovered: COLOR_PALETTE.GRAY_20,
                    pressed: COLOR_PALETTE.GRAY_7
                ),
                border: new(
                    @default: COLOR_PALETTE.GRAY_7
                ),
                text: new(
                    @default: COLOR_PALETTE.GRAY_100,
                    selected: COLOR_PALETTE.LIGHT_BLUE_48,
                    variants: new ()
                    {
                        { "discord", new(
                            @default: COLOR_PALETTE.GRAY_100,
                            selected: "#5865F2",
                            hovered: "#5865F2"
                        ) },
                        { "github", new(
                            @default: COLOR_PALETTE.GRAY_100,
                            selected: "#FECEF1",
                            hovered: "#FECEF1"
                        ) },
                        { "sun", new(
                            @default: COLOR_PALETTE.GRAY_100,
                            selected: "#F09D51",
                            hovered: "#F09D51"
                        ) },
                    }
                )
            ) },
            { ROLES.DEEP_NEUTRAL, new(
                background: new(
                    @default: COLOR_PALETTE.GRAY_7,
                    selected: COLOR_PALETTE.GRAY_13,
                    hovered: COLOR_PALETTE.GRAY_13,
                    pressed: COLOR_PALETTE.GRAY_0
                ),
                border: new(
                    @default: COLOR_PALETTE.GRAY_0
                ),
                text: new(
                    @default: COLOR_PALETTE.GRAY_100,
                    selected: COLOR_PALETTE.LIGHT_BLUE_48
                )
            ) },
        },
        typography: TYPOGRAPHY_TOKENS
    );

    public static readonly Theme LIGHT_THEME = new Theme(
        fallbackRole: ROLES.NEUTRAL,
        new()
        {
            { ROLES.NEUTRAL, new(
                background: new(
                    @default: COLOR_PALETTE.GRAY_100,
                    selected: COLOR_PALETTE.GRAY_93,
                    hovered: COLOR_PALETTE.GRAY_93,
                    pressed: COLOR_PALETTE.GRAY_87
                ),
                border: new(
                    @default: COLOR_PALETTE.GRAY_87
                ),
                text: new(
                    @default: COLOR_PALETTE.GRAY_0,
                    selected: COLOR_PALETTE.LIGHT_BLUE_37,
                    hovered: COLOR_PALETTE.LIGHT_BLUE_37
                )
            ) },
            { ROLES.DEEP_NEUTRAL, new(
                background: new(
                    @default: COLOR_PALETTE.GRAY_93,
                    selected: COLOR_PALETTE.GRAY_87,
                    hovered: COLOR_PALETTE.GRAY_87,
                    pressed: COLOR_PALETTE.GRAY_80
                ),
                border: new(
                    @default: COLOR_PALETTE.GRAY_80
                ),
                text: new(
                    @default: COLOR_PALETTE.GRAY_0,
                    selected: COLOR_PALETTE.LIGHT_BLUE_37,
                    hovered: COLOR_PALETTE.LIGHT_BLUE_37
                )
            ) },
        },
        typography: TYPOGRAPHY_TOKENS
    );
}