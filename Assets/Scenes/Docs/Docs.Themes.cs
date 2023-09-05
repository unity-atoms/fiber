using System.Collections.Generic;
using Fiber.UI;
using Fiber.UIElements;
using Fiber;
using UnityEngine;

public static class DocsThemes
{
    public static class ROLES
    {
        public const string NEUTRAL = "neutral";
        public const string DEEP_NEUTRAL = "deepNeutral";
        public const string DEBUG = "debug";
    }

    public class Provider : BaseComponent
    {
        public Provider(List<VirtualNode> children) : base(children) { }

        public override VirtualNode Render()
        {
            return F.ThemeProvider(
                themeStore: new ThemeStore(
                    theme: DARK_THEME,
                    screenSizeSignal: C<ScalingContext>().ScreenSizeSignal
                ),
                children: children
            );
        }
    }

    public static readonly TypographyTokens TYPOGRAPHY_TOKENS = new(
        heading1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 48,
            fontStyle: FontStyle.Bold
        ),
        heading3: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 24,
            fontStyle: FontStyle.Bold
        ),
        body1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 16,
            fontStyle: FontStyle.Normal
        ),
        subtitle2: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 16,
            fontStyle: FontStyle.Normal
        )
    );

    public static readonly Theme DARK_THEME = new Theme(
        fallbackRole: ROLES.NEUTRAL,
        color: new()
        {
            { ROLES.NEUTRAL, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_13,
                    selected: FIBER_COLOR_PALETTE.GRAY_20,
                    hovered: FIBER_COLOR_PALETTE.GRAY_20,
                    pressed: FIBER_COLOR_PALETTE.GRAY_7
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_7
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_100,
                    selected: FIBER_COLOR_PALETTE.LIGHT_BLUE_48,
                    variants: new ()
                    {
                        { "discord", new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: "#5865F2",
                            hovered: "#5865F2"
                        ) },
                        { "github", new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: "#FECEF1",
                            hovered: "#FECEF1"
                        ) },
                        { "sun", new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: "#F09D51",
                            hovered: "#F09D51"
                        ) },
                    }
                ),
                overlay: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                )
            ) },
            { ROLES.DEEP_NEUTRAL, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_7,
                    selected: FIBER_COLOR_PALETTE.GRAY_13,
                    hovered: FIBER_COLOR_PALETTE.GRAY_13,
                    pressed: FIBER_COLOR_PALETTE.GRAY_0
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_0
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_100,
                    selected: FIBER_COLOR_PALETTE.LIGHT_BLUE_48
                )
            ) },
            { ROLES.DEBUG, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.RED_41
                )
            ) },
        },
        typography: TYPOGRAPHY_TOKENS
    );

    public static readonly Theme LIGHT_THEME = new Theme(
        fallbackRole: ROLES.NEUTRAL,
        color: new()
        {
            { ROLES.NEUTRAL, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_100,
                    selected: FIBER_COLOR_PALETTE.GRAY_93,
                    hovered: FIBER_COLOR_PALETTE.GRAY_93,
                    pressed: FIBER_COLOR_PALETTE.GRAY_87
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_87
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_0,
                    selected: FIBER_COLOR_PALETTE.LIGHT_BLUE_37,
                    hovered: FIBER_COLOR_PALETTE.LIGHT_BLUE_37
                ),
                overlay: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                )
            ) },
            { ROLES.DEEP_NEUTRAL, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_93,
                    selected: FIBER_COLOR_PALETTE.GRAY_87,
                    hovered: FIBER_COLOR_PALETTE.GRAY_87,
                    pressed: FIBER_COLOR_PALETTE.GRAY_80
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_80
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_0,
                    selected: FIBER_COLOR_PALETTE.LIGHT_BLUE_37,
                    hovered: FIBER_COLOR_PALETTE.LIGHT_BLUE_37
                )
            ) },
        },
        typography: TYPOGRAPHY_TOKENS
    );
}