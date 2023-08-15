using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.DesignTokens;
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
            _spacingSignalsCache = new();
            _nonInteractiveColorSignalsCache = new();
            _fontSignalsCache = new();
            _fontSizeSignalsCache = new();
            _fontStyleSignalsCache = new();
        }


        private readonly Dictionary<int, BaseSignal<StyleLength>> _spacingSignalsCache;
        public BaseSignal<StyleLength> Spacing(int multiplier)
        {
            if (_spacingSignalsCache.ContainsKey(multiplier))
            {
                return _spacingSignalsCache[multiplier];
            }

            var spacingSignal = new InlineComputedSignal<Theme, StyleLength>((theme) => theme.Spacing.Baseline.Value * multiplier, this);
            _spacingSignalsCache.Add(multiplier, spacingSignal);
            return spacingSignal;
        }

        private readonly Dictionary<ValueTuple<string, ElementType, string>, BaseSignal<StyleColor>> _nonInteractiveColorSignalsCache;
        public BaseSignal<StyleColor> Color(string role, ElementType elementType, string variant = null)
        {
            if (_nonInteractiveColorSignalsCache.ContainsKey((role, elementType, variant)))
            {
                return _nonInteractiveColorSignalsCache[(role, elementType, variant)];
            }

            var signal = new InlineComputedSignal<Theme, StyleColor>((theme) =>
            {
                var colorModifiers = theme.GetColorModifiers(role, elementType, variant);
                return colorModifiers.Default.Get();
            }, this);
            _nonInteractiveColorSignalsCache.Add((role, elementType, variant), signal);

            return signal;
        }

        public BaseSignal<StyleColor> Color(
            string role,
            ElementType elementType,
            BaseSignal<bool> isPressed,
            BaseSignal<bool> isHovered,
            BaseSignal<bool> isSelected = null,
            string variant = null
        )
        {
            var signal = new InlineComputedSignal<Theme, bool, bool, bool, StyleColor>((theme, isPressed, isHovered, isSelected) =>
            {
                var colorModifiers = theme.GetColorModifiers(role, elementType, variant);
                if (isPressed && colorModifiers.Pressed.Get().keyword != StyleKeyword.Null)
                {
                    return colorModifiers.Pressed.Get();
                }
                else if (isHovered && colorModifiers.Hovered.Get().keyword != StyleKeyword.Null)
                {
                    return colorModifiers.Hovered.Get();
                }
                else if (isSelected && colorModifiers.Selected.Get().keyword != StyleKeyword.Null)
                {
                    return colorModifiers.Selected.Get();
                }
                return colorModifiers.Default.Get();
            }, this, isPressed, isHovered, isSelected ?? new StaticSignal<bool>(false));

            return signal;
        }

        private readonly Dictionary<TypographyType, BaseSignal<StyleFont>> _fontSignalsCache;
        public BaseSignal<StyleFont> Font(TypographyType typographyType)
        {
            if (_fontSignalsCache.ContainsKey(typographyType))
            {
                return _fontSignalsCache[typographyType];
            }

            var signal = new InlineComputedSignal<Theme, StyleFont>((theme) =>
            {
                var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                return typographyTokens.Font.Get();
            }, this);
            _fontSignalsCache.Add(typographyType, signal);

            return signal;
        }

        private readonly Dictionary<TypographyType, BaseSignal<StyleLength>> _fontSizeSignalsCache;
        public BaseSignal<StyleLength> FontSize(TypographyType typographyType)
        {
            if (_fontSizeSignalsCache.ContainsKey(typographyType))
            {
                return _fontSizeSignalsCache[typographyType];
            }

            var signal = new InlineComputedSignal<Theme, StyleLength>((theme) =>
            {
                var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                return typographyTokens.FontSize.Get();
            }, this);
            _fontSizeSignalsCache.Add(typographyType, signal);

            return signal;
        }

        private readonly Dictionary<TypographyType, BaseSignal<StyleEnum<FontStyle>>> _fontStyleSignalsCache;
        public BaseSignal<StyleEnum<FontStyle>> FontStyle(TypographyType typographyType)
        {
            if (_fontStyleSignalsCache.ContainsKey(typographyType))
            {
                return _fontStyleSignalsCache[typographyType];
            }

            var signal = new InlineComputedSignal<Theme, StyleEnum<FontStyle>>((theme) =>
            {
                var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                return typographyTokens.FontStyle.Get();
            }, this);
            _fontStyleSignalsCache.Add(typographyType, signal);

            return signal;
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