using System.Collections.Generic;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static ThemeProvider ThemeProvider(
            this BaseComponent component,
            ThemeStore themeStore,
            List<VirtualNode> children
        )
        {
            return new ThemeProvider(
                themeStore: themeStore,
                children: children
            );
        }
    }

    public class ThemeProvider : BaseComponent
    {
        private readonly ThemeStore _themeStore;
        public ThemeProvider(
            ThemeStore themeStore,
            List<VirtualNode> children
        ) : base(children)
        {
            _themeStore = themeStore;
        }

        public override VirtualNode Render()
        {
            return F.ContextProvider(
                value: _themeStore,
                children: children
            );
        }
    }
}