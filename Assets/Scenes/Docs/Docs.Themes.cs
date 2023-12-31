using System.Collections.Generic;
using SilkUI;
using Fiber.UIElements;
using Fiber;
using UnityEngine;

public static class DocsThemes
{
    public static class ROLE
    {
        public const string PRIMARY = "primary";
        public const string SECONDARY = "secondary";
        public const string NEUTRAL = "neutral";
        public const string DEBUG = "debug";
    }

    public static class SUBROLE
    {
        public const string DEEP = "deep";
    }

    public class Provider : BaseComponent
    {
        public Provider(VirtualBody children) : base(children) { }

        public override VirtualBody Render()
        {
            return F.ThemeProvider(
                themeStore: new ThemeStore(
                    theme: DARK_THEME,
                    screenSizeSignal: C<ScalingContext>().ScreenSizeSignal
                ),
                children: Children
            );
        }
    }

    public static readonly TypographyTokens TYPOGRAPHY_TOKENS = new(
        heading1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 48,
            fontStyle: FontStyle.Bold
        ),
        heading2: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 32,
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
        ),
        button: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/DM_Sans/DMSans-VariableFont"),
            fontSize: 16,
            fontStyle: FontStyle.Bold
        )
    );

    public static readonly TypographyTokens PIXEL_TYPOGRAPHY_TOKENS = new(
        heading1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/8_bit_hud/8-bit-hud"),
            fontSize: 24,
            fontStyle: FontStyle.Bold
        ),
        heading3: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/8_bit_hud/8-bit-hud"),
            fontSize: 12,
            fontStyle: FontStyle.Bold
        ),
        body1: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/8_bit_hud/8-bit-hud"),
            fontSize: 8,
            fontStyle: FontStyle.Normal
        ),
        subtitle2: new TypographyTypeTokens(
            font: Resources.Load<Font>("Fonts/8_bit_hud/8-bit-hud"),
            fontSize: 8,
            fontStyle: FontStyle.Normal
        )
    );

    public static readonly Theme DARK_THEME = new Theme(
        name: "dark",
        fallbackRole: ROLE.NEUTRAL,
        spacing: new(),
        color: new()
        {
            { ROLE.NEUTRAL, new(
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
                    selected: FIBER_COLOR_PALETTE.SKY_56,
                    variants: new ()
                    {
                        { "discord", new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: "#5865F2", // Discord's official blue
                            hovered: "#5865F2"
                        ) },
                        { "github", new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: FIBER_COLOR_PALETTE.ORANGE_60,
                            hovered: FIBER_COLOR_PALETTE.ORANGE_60
                        ) },
                        { "sun", new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: FIBER_COLOR_PALETTE.YELLOW_60,
                            hovered: FIBER_COLOR_PALETTE.YELLOW_60
                        ) },
                    }
                ),
                overlay: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                ),
                subRoles: new()
                {
                    { SUBROLE.DEEP, new(
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
                            selected: FIBER_COLOR_PALETTE.SKY_56
                        )
                    ) }
                }
            ) },
            { ROLE.DEBUG, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.RED_41
                )
            ) },
            { ROLE.PRIMARY, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.SKY_56,
                    selected: FIBER_COLOR_PALETTE.SKY_74,
                    hovered: FIBER_COLOR_PALETTE.SKY_74,
                    pressed: FIBER_COLOR_PALETTE.SKY_31
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.SKY_56,
                    selected: FIBER_COLOR_PALETTE.SKY_74,
                    hovered: FIBER_COLOR_PALETTE.SKY_74,
                    pressed: FIBER_COLOR_PALETTE.SKY_31
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_100
                )
            ) },
        },
        typography: TYPOGRAPHY_TOKENS
    );

    public static readonly Theme LIGHT_THEME = new Theme(
        name: "light",
        fallbackRole: ROLE.NEUTRAL,
        spacing: new(),
        color: new()
        {
            { ROLE.NEUTRAL, new(
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
                    selected: FIBER_COLOR_PALETTE.SKY_37,
                    hovered: FIBER_COLOR_PALETTE.SKY_37
                ),
                overlay: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                ),
                subRoles: new()
                {
                    { SUBROLE.DEEP, new(
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
                            selected: FIBER_COLOR_PALETTE.SKY_37,
                            hovered: FIBER_COLOR_PALETTE.SKY_37
                        )
                    ) }
                }
            ) },
            { ROLE.PRIMARY, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.SKY_56,
                    selected: FIBER_COLOR_PALETTE.SKY_74,
                    hovered: FIBER_COLOR_PALETTE.SKY_74,
                    pressed: FIBER_COLOR_PALETTE.SKY_31
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.SKY_56,
                    selected: FIBER_COLOR_PALETTE.SKY_74,
                    hovered: FIBER_COLOR_PALETTE.SKY_74,
                    pressed: FIBER_COLOR_PALETTE.SKY_31
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_100
                )
            ) },
        },
        typography: TYPOGRAPHY_TOKENS
    );

    public static readonly Theme ICE_PIXEL = new Theme(
        name: "icePixel",
        fallbackRole: ROLE.NEUTRAL,
        spacing: new(),
        color: new()
        {
            { ROLE.NEUTRAL, new(
                background: new(
                    @default: FIBER_COLOR_PALETTE.SKY_37,
                    selected: FIBER_COLOR_PALETTE.SKY_45,
                    hovered: FIBER_COLOR_PALETTE.SKY_45,
                    pressed: FIBER_COLOR_PALETTE.SKY_31
                ),
                border: new(
                    @default: FIBER_COLOR_PALETTE.SKY_31
                ),
                text: new(
                    @default: FIBER_COLOR_PALETTE.GRAY_100,
                    selected: FIBER_COLOR_PALETTE.SKY_74,
                    hovered: FIBER_COLOR_PALETTE.SKY_74
                ),
                overlay: new(
                    @default: FIBER_COLOR_PALETTE.BLACK_ALPHA_40
                ),
                subRoles: new()
                {
                    { SUBROLE.DEEP, new(
                        background: new(
                            @default: FIBER_COLOR_PALETTE.SKY_27,
                            selected: FIBER_COLOR_PALETTE.SKY_37,
                            hovered: FIBER_COLOR_PALETTE.SKY_37,
                            pressed: FIBER_COLOR_PALETTE.SKY_19
                        ),
                        border: new(
                            @default: FIBER_COLOR_PALETTE.SKY_23
                        ),
                        text: new(
                            @default: FIBER_COLOR_PALETTE.GRAY_100,
                            selected: FIBER_COLOR_PALETTE.SKY_85,
                            hovered: FIBER_COLOR_PALETTE.SKY_85
                        )
                    ) }
                }
            ) },
        },
        typography: PIXEL_TYPOGRAPHY_TOKENS
    );
}