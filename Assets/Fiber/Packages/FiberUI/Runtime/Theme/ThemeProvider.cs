using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

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

    public class ThemeStore : Store<Theme>
    {
        public ThemeStore(
            Theme theme
        ) : base(theme)
        {
        }
    }

    public class ThemeProvider : BaseComponent
    {
        private ThemeStore _themeStore;
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