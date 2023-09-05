using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Signals;
using Fiber.UIElements;

namespace Fiber.UI
{
    public class ThemeStore : Store<Theme>
    {
        private ScreenSizeSignal _screenSizeSignal;
        public ThemeStore(
            Theme theme,
            ScreenSizeSignal screenSizeSignal
        ) : base(theme)
        {
            _screenSizeSignal = screenSizeSignal;

            _spacingSignalsCache = new();
            _nonInteractiveColorSignalsCache = new();
            _fontSignalsCache = new();
            _fontSizeSignalsCache = new();
            _fontStyleSignalsCache = new();
            _iconSignalsCache = new();

            IsSmallScreen = new IsSmallScreenSignal(Value.Breakpoints, _screenSizeSignal);
            IsMediumScreen = new IsMediumScreenSignal(Value.Breakpoints, _screenSizeSignal);
            IsLargeScreen = new IsLargeScreenSignal(Value.Breakpoints, _screenSizeSignal);
            IsXLScreen = new IsXLScreenSignal(Value.Breakpoints, _screenSizeSignal);
            Breakpoint = new BreakpointSignal(Value.Breakpoints, _screenSizeSignal);
        }

        #region Spacing
        private readonly Dictionary<float, BaseSignal<StyleLength>> _spacingSignalsCache;
        public BaseSignal<StyleLength> Spacing(float multiplier)
        {
            if (_spacingSignalsCache.ContainsKey(multiplier))
            {
                return _spacingSignalsCache[multiplier];
            }

            var spacingSignal = new InlineComputedSignal<Theme, StyleLength>((theme) => theme.Spacing.Baseline.Value * multiplier, this);
            _spacingSignalsCache.Add(multiplier, spacingSignal);
            return spacingSignal;
        }
        #endregion
        #region Color
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
            ISignal<bool> isPressed,
            ISignal<bool> isHovered,
            ISignal<bool> isSelected = null,
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
        #endregion
        #region Typography
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
        #endregion
        #region Icon
        private readonly Dictionary<IconSize, BaseSignal<StyleLength>> _iconSignalsCache;
        public BaseSignal<StyleLength> Icon(IconSize size)
        {
            if (_iconSignalsCache.ContainsKey(size))
            {
                return _iconSignalsCache[size];
            }

            var iconSignal = new InlineComputedSignal<Theme, StyleLength>((theme) => theme.Spacing.Baseline.Value * theme.Icon.GetIconSignal(size).Value, this);
            _iconSignalsCache.Add(size, iconSignal);
            return iconSignal;
        }
        #endregion
        #region Breakpoints
        public IsSmallScreenSignal IsSmallScreen;
        public IsMediumScreenSignal IsMediumScreen;
        public IsLargeScreenSignal IsLargeScreen;
        public IsXLScreenSignal IsXLScreen;
        public BreakpointSignal Breakpoint;
        public ResponsiveSignal<T> Responsive<T>(ResponsiveProp<T> responsiveProp)
        {
            return new ResponsiveSignal<T>(Breakpoint, responsiveProp);
        }
        #endregion
    }
}