using Fiber;
using Fiber.UIElements;
using Signals;
using Fiber.Cursed;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkUIProviderComponent SilkUIProvider(
            this BaseComponent component,
            VirtualBody children,
            OverrideVisualComponents overrideVisualComponents = null,
            Theme theme = null,
            Store<ShallowSignalList<CursorDefinition>> cursorDefinitionsStore = null
        )
        {
            return new SilkUIProviderComponent(
                children: children,
                overrideVisualComponents: overrideVisualComponents,
                theme: theme,
                cursorDefinitionsStore: cursorDefinitionsStore
            );
        }
    }

    public class SilkUIProviderComponent : BaseComponent
    {
        private readonly OverrideVisualComponents _overrideVisualComponents;
        private readonly Theme _theme;
        private readonly Store<ShallowSignalList<CursorDefinition>> _cursorDefinitionsStore;
        public SilkUIProviderComponent(
            VirtualBody children,
            OverrideVisualComponents overrideVisualComponents = null,
            Theme theme = null,
            Store<ShallowSignalList<CursorDefinition>> cursorDefinitionsStore = null
        ) : base(children)
        {
            _overrideVisualComponents = overrideVisualComponents;
            _theme = theme;
            _cursorDefinitionsStore = cursorDefinitionsStore;
        }

        // Needed since we are reaching for scaling context in the render function
        private class InlineThemeProvider : BaseComponent
        {
            private readonly Theme _theme;
            public InlineThemeProvider(VirtualBody children, Theme theme) : base(children) { _theme = theme; }
            private readonly Theme DEFAULT_MINIMAL_THEME = new(
                name: "default-minimal",
                fallbackRole: "debug",
                spacing: new(),
                color: new()
                {
                    { "debug", new(
                        background: new(FIBER_COLOR_PALETTE.GRAY_100),
                        border: new(FIBER_COLOR_PALETTE.GRAY_0),
                        text: new(FIBER_COLOR_PALETTE.GRAY_0),
                        overlay: new(FIBER_COLOR_PALETTE.BLACK_ALPHA_40),
                        shine: new(FIBER_COLOR_PALETTE.WHITE_ALPHA_40),
                        gloom: new(FIBER_COLOR_PALETTE.BLACK_ALPHA_40)
                    ) }
                },
                typography: new()
            );

            public override VirtualBody Render()
            {
                return F.ThemeProvider(
                    themeStore: new ThemeStore(
                        theme: _theme ?? DEFAULT_MINIMAL_THEME,
                        screenSizeSignal: C<ScalingContext>().ScreenSizeSignal
                    ),
                    children: Children
                );
            }
        }


        public override VirtualBody Render()
        {
            return F.ScalingProvider(
                children: F.OverrideVisualComponentsProvider(
                    overrideVisualComponents: _overrideVisualComponents ?? new OverrideVisualComponents(),
                    children: new InlineThemeProvider(
                        theme: _theme,
                        children: F.CursorManager(
                            cursorDefinitionsStore: _cursorDefinitionsStore ?? new Store<ShallowSignalList<CursorDefinition>>(),
                            children: Children
                        )
                    )
                )
            );
        }
    }

}

