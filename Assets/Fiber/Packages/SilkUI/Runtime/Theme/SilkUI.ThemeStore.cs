using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber;
using Signals;
using Fiber.UIElements;
using Fiber.InteractiveUI;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static BaseSignal<StyleColor> Color(
            this BaseComponent component,
            SignalProp<string> role,
            SignalProp<ElementType> elementType,
            SignalProp<string> subRole = default,
            SignalProp<string> variant = default,
            ThemeStore themeStore = null
        )
        {
            themeStore ??= component.C<ThemeStore>();
            return component.CreateComputedSignal((theme, role, elementType, subRole, variant) =>
                {
                    var colorModifiers = theme.GetColorModifiers(role, elementType, subRole, variant);
                    return colorModifiers.Default.Get();
                },
                themeStore,
                component.WrapSignalProp(role),
                component.WrapSignalProp(elementType),
                component.WrapSignalProp(subRole),
                component.WrapSignalProp(variant)
            );
        }
    }

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
            _borderWidthSignalsCache = new();
            _textOutlineSignalsCache = new();
            _nonInteractiveColorSignalsCache = new();
            _fontSignalsCache = new();
            _fontSizeSignalsCache = new();
            _fontStyleSignalsCache = new();
            _outlineWidthSignalsCache = new();
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
        #region BorderWidth
        private readonly Dictionary<BorderWidthType, BaseSignal<StyleFloat>> _borderWidthSignalsCache;
        public BaseSignal<StyleFloat> BorderWidth(BorderWidthType borderWidthType = BorderWidthType.Default)
        {
            if (_borderWidthSignalsCache.ContainsKey(borderWidthType))
            {
                return _borderWidthSignalsCache[borderWidthType];
            }

            var signal = new InlineComputedSignal<Theme, StyleFloat>((theme) =>
            {
                return theme.Spacing.BorderWidth.GetBorderWidthSignal(borderWidthType).Get();
            }, this);
            return signal;
        }
        #endregion
        #region OutlineWidth
        private readonly Dictionary<TextOutlineType, BaseSignal<StyleFloat>> _textOutlineSignalsCache;
        public BaseSignal<StyleFloat> OutlineWidth(TextOutlineType textOutlineType = TextOutlineType.Default)
        {
            if (_textOutlineSignalsCache.ContainsKey(textOutlineType))
            {
                return _textOutlineSignalsCache[textOutlineType];
            }

            var signal = new InlineComputedSignal<Theme, StyleFloat>((theme) =>
            {
                return theme.Spacing.TextOutline.GetTextOutlineSignal(textOutlineType).Get();
            }, this);
            return signal;
        }
        #endregion
        #region Color
        private readonly Dictionary<ValueTuple<string, ElementType, string, SignalProp<string>>, BaseSignal<StyleColor>> _nonInteractiveColorSignalsCache;
        public BaseSignal<StyleColor> Color(string role, ElementType elementType, string subRole = null, SignalProp<string> variant = new())
        {
            if (_nonInteractiveColorSignalsCache.ContainsKey((role, elementType, subRole, variant)))
            {
                return _nonInteractiveColorSignalsCache[(role, elementType, subRole, variant)];
            }

            var signal = new InlineComputedSignal<Theme, string, StyleColor>((theme, variant) =>
            {
                var colorModifiers = theme.GetColorModifiers(role, elementType, subRole, variant);
                return colorModifiers.Default.Get();
            }, this, variant.IsSignal ? variant.Signal : new StaticSignal<string>(variant.Value));
            _nonInteractiveColorSignalsCache.Add((role, elementType, subRole, variant), signal);

            return signal;
        }

        public BaseSignal<StyleColor> Color(
            string role,
            ElementType elementType,
            InteractiveElement interactiveElement,
            string subRole = null,
            SignalProp<string> variant = new()
        )
        {
            if (interactiveElement == null)
            {
                return Color(role: role, elementType: elementType, subRole: subRole, variant: variant);
            }

            var signal = new InlineComputedSignal<Theme, bool, bool, bool, bool, string, StyleColor>((theme, isPressed, isHovered, isSelected, isDisabled, variant) =>
                {
                    var colorModifiers = theme.GetColorModifiers(role, elementType, subRole, variant);
                    if (isDisabled && colorModifiers.Disabled.Get().keyword != StyleKeyword.Null)
                    {
                        return colorModifiers.Disabled.Get();
                    }
                    else if (isPressed && colorModifiers.Pressed.Get().keyword != StyleKeyword.Null)
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
                },
                this,
                interactiveElement.IsPressed,
                interactiveElement.IsHovered,
                interactiveElement.IsSelected ?? StaticSignals.FALSE,
                interactiveElement.IsDisabled ?? StaticSignals.FALSE,
                variant.IsSignal ? variant.Signal : new StaticSignal<string>(variant.Value)
            );

            return signal;
        }
        #endregion
        #region Typography
        private readonly Dictionary<TypographyType, BaseSignal<StyleFont>> _fontSignalsCache;
        public BaseSignal<StyleFont> Font(SignalProp<TypographyType> typographyTypeProp)
        {
            if (typographyTypeProp.IsValue)
            {
                var typographyType = typographyTypeProp.Value;
                if (_fontSignalsCache.ContainsKey(typographyType))
                {
                    return _fontSignalsCache[typographyType];
                }

                var signal = new InlineComputedSignal<Theme, StyleFont>((theme) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return typographyTokens?.Font?.Get() ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Default Unity font
                }, this);
                _fontSignalsCache.Add(typographyType, signal);

                return signal;
            }
            else if (typographyTypeProp.IsSignal)
            {
                var signal = new InlineComputedSignal<Theme, TypographyType, StyleFont>((theme, typographyType) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return typographyTokens?.Font?.Get() ?? new StyleFont();
                }, this, typographyTypeProp.Signal);
                return signal;
            }
            throw new ArgumentException($"TypographyTypeProp can't be empty");

        }

        private readonly Dictionary<TypographyType, BaseSignal<StyleLength>> _fontSizeSignalsCache;
        public BaseSignal<StyleLength> FontSize(SignalProp<TypographyType> typographyTypeProp)
        {
            if (typographyTypeProp.IsValue)
            {
                var typographyType = typographyTypeProp.Value;
                if (_fontSizeSignalsCache.ContainsKey(typographyType))
                {
                    return _fontSizeSignalsCache[typographyType];
                }

                var signal = new InlineComputedSignal<Theme, StyleLength>((theme) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return typographyTokens?.FontSize?.Get() ?? 16;
                }, this);
                _fontSizeSignalsCache.Add(typographyType, signal);

                return signal;
            }
            else if (typographyTypeProp.IsSignal)
            {
                var signal = new InlineComputedSignal<Theme, TypographyType, StyleLength>((theme, typographyType) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return typographyTokens?.FontSize?.Get() ?? 16;
                }, this, typographyTypeProp.Signal);
                return signal;
            }
            throw new ArgumentException($"TypographyTypeProp can't be empty");
        }

        private readonly Dictionary<TypographyType, BaseSignal<StyleEnum<FontStyle>>> _fontStyleSignalsCache;
        public BaseSignal<StyleEnum<FontStyle>> FontStyle(SignalProp<TypographyType> typographyTypeProp)
        {
            if (typographyTypeProp.IsValue)
            {
                var typographyType = typographyTypeProp.Value;
                if (_fontStyleSignalsCache.ContainsKey(typographyType))
                {
                    return _fontStyleSignalsCache[typographyType];
                }

                var signal = new InlineComputedSignal<Theme, StyleEnum<FontStyle>>((theme) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return typographyTokens?.FontStyle?.Get() ?? UnityEngine.FontStyle.Normal;
                }, this);
                _fontStyleSignalsCache.Add(typographyType, signal);

                return signal;
            }
            else if (typographyTypeProp.IsSignal)
            {
                var signal = new InlineComputedSignal<Theme, TypographyType, StyleEnum<FontStyle>>((theme, typographyType) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return typographyTokens?.FontStyle?.Get() ?? UnityEngine.FontStyle.Normal;
                }, this, typographyTypeProp.Signal);
                return signal;
            }
            throw new ArgumentException($"TypographyTypeProp can't be empty");
        }

        private readonly Dictionary<TypographyType, BaseSignal<StyleFloat>> _outlineWidthSignalsCache;
        public BaseSignal<StyleFloat> OutlineWidth(SignalProp<TypographyType> typographyTypeProp)
        {
            if (typographyTypeProp.IsValue)
            {
                var typographyType = typographyTypeProp.Value;
                if (_outlineWidthSignalsCache.ContainsKey(typographyType))
                {
                    return _outlineWidthSignalsCache[typographyType];
                }

                var signal = new InlineComputedSignal<Theme, StyleFloat>((theme) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return theme.Spacing.TextOutline.GetTextOutlineSignal(typographyTokens?.OutlineWidth?.Get() ?? TextOutlineType.Default).Get();
                }, this);
                _outlineWidthSignalsCache.Add(typographyType, signal);

                return signal;
            }
            else if (typographyTypeProp.IsSignal)
            {
                var signal = new InlineComputedSignal<Theme, TypographyType, StyleFloat>((theme, typographyType) =>
                {
                    var typographyTokens = theme.Typography.GetTypographyTypeTokens(typographyType);
                    return theme.Spacing.TextOutline.GetTextOutlineSignal(typographyTokens.OutlineWidth.Get()).Get();
                }, this, typographyTypeProp.Signal);
                return signal;
            }
            throw new ArgumentException($"TypographyTypeProp can't be empty");
        }
        #endregion
        #region Icon
        private readonly Dictionary<IconSize, BaseSignal<StyleLength>> _iconSignalsCache;
        public BaseSignal<StyleLength> IconSize(IconSize size)
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