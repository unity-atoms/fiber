using System.Collections.Generic;
using Fiber;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static ThemeProvider ThemeProvider(
            this BaseComponent component,
            ThemeStore themeStore,
            VirtualBody children
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
            VirtualBody children
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