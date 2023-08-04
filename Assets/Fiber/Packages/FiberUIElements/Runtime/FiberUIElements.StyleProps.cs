using UnityEngine.UIElements;
using Signals;
using UnityEngine;

namespace Fiber.UIElements
{
    // We need specific props for each style in order for implicit operators to work.
    // Implicit operators can't convert more than 1 step.
    public struct PositionProp
    {
        public SignalProp<StyleEnum<Position>> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public PositionProp(StyleEnum<Position> value)
        {
            SignalProp = value;
        }

        public PositionProp(BaseSignal<StyleEnum<Position>> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator PositionProp(StyleKeyword keyword)
        {
            StyleEnum<Position> styleEnum = keyword;
            return new PositionProp(styleEnum);
        }

        public static implicit operator PositionProp(Position position)
        {
            StyleEnum<Position> styleEnum = position;
            return new PositionProp(styleEnum);
        }

        public static implicit operator PositionProp(BaseSignal<StyleEnum<Position>> signal)
        {
            return new PositionProp(signal);
        }

        public StyleEnum<Position> Get() => SignalProp.Get();
    }

    public struct WorkLoopPositionProp
    {
        public WorkLoopSignalProp<StyleEnum<Position>> WorkLoopSignalProp { get; private set; }

        public WorkLoopPositionProp(PositionProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleEnum<Position> Get() => WorkLoopSignalProp.Get();
    }

    public struct StyleLengthProp
    {
        public SignalProp<StyleLength> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public StyleLengthProp(StyleLength value)
        {
            SignalProp = value;
        }

        public StyleLengthProp(BaseSignal<StyleLength> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator StyleLengthProp(StyleKeyword keyword)
        {
            StyleLength value = keyword;
            return new StyleLengthProp(value);
        }

        public static implicit operator StyleLengthProp(float v)
        {
            StyleLength value = v;
            return new StyleLengthProp(value);
        }

        public static implicit operator StyleLengthProp(Length v)
        {
            StyleLength value = v;
            return new StyleLengthProp(value);
        }

        public static implicit operator StyleLengthProp(BaseSignal<StyleLength> signal)
        {
            return new StyleLengthProp(signal);
        }

        public StyleLength Get() => SignalProp.Get();
    }

    public struct WorkLoopStyleLengthProp
    {
        public WorkLoopSignalProp<StyleLength> WorkLoopSignalProp { get; private set; }

        public WorkLoopStyleLengthProp(StyleLengthProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleLength Get() => WorkLoopSignalProp.Get();
    }

    public struct StyleFloatProp
    {
        public SignalProp<StyleFloat> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public StyleFloatProp(StyleFloat value)
        {
            SignalProp = value;
        }

        public StyleFloatProp(BaseSignal<StyleFloat> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator StyleFloatProp(StyleKeyword keyword)
        {
            StyleFloat value = keyword;
            return new StyleFloatProp(value);
        }

        public static implicit operator StyleFloatProp(float v)
        {
            StyleFloat value = v;
            return new StyleFloatProp(value);
        }

        public static implicit operator StyleFloatProp(BaseSignal<StyleFloat> signal)
        {
            return new StyleFloatProp(signal);
        }

        public StyleFloat Get() => SignalProp.Get();
    }

    public struct WorkLoopStyleFloatProp
    {
        public WorkLoopSignalProp<StyleFloat> WorkLoopSignalProp { get; private set; }

        public WorkLoopStyleFloatProp(StyleFloatProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleFloat Get() => WorkLoopSignalProp.Get();
    }

    public struct StyleColorProp
    {
        public SignalProp<StyleColor> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public StyleColorProp(StyleColor value)
        {
            SignalProp = value;
        }

        public StyleColorProp(BaseSignal<StyleColor> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator StyleColorProp(StyleKeyword keyword)
        {
            StyleColor value = keyword;
            return new StyleColorProp(value);
        }

        public static implicit operator StyleColorProp(Color v)
        {
            StyleColor value = v;
            return new StyleColorProp(value);
        }

        public static implicit operator StyleColorProp(BaseSignal<StyleColor> signal)
        {
            return new StyleColorProp(signal);
        }

        public StyleColor Get() => SignalProp.Get();
    }

    public struct WorkLoopStyleColorProp
    {
        public WorkLoopSignalProp<StyleColor> WorkLoopSignalProp { get; private set; }

        public WorkLoopStyleColorProp(StyleColorProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleColor Get() => WorkLoopSignalProp.Get();
    }

    public struct DisplayStyleProp
    {
        public SignalProp<StyleEnum<DisplayStyle>> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public DisplayStyleProp(StyleEnum<DisplayStyle> value)
        {
            SignalProp = value;
        }

        public DisplayStyleProp(BaseSignal<StyleEnum<DisplayStyle>> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator DisplayStyleProp(StyleKeyword keyword)
        {
            StyleEnum<DisplayStyle> styleEnum = keyword;
            return new DisplayStyleProp(styleEnum);
        }

        public static implicit operator DisplayStyleProp(DisplayStyle displayStyle)
        {
            StyleEnum<DisplayStyle> styleEnum = displayStyle;
            return new DisplayStyleProp(styleEnum);
        }

        public static implicit operator DisplayStyleProp(BaseSignal<StyleEnum<DisplayStyle>> signal)
        {
            return new DisplayStyleProp(signal);
        }

        public StyleEnum<DisplayStyle> Get() => SignalProp.Get();
    }

    public struct WorkLoopDisplayStyleProp
    {
        public WorkLoopSignalProp<StyleEnum<DisplayStyle>> WorkLoopSignalProp { get; private set; }

        public WorkLoopDisplayStyleProp(DisplayStyleProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleEnum<DisplayStyle> Get() => WorkLoopSignalProp.Get();
    }

    public struct FlexDirectionProp
    {
        public SignalProp<StyleEnum<FlexDirection>> SignalProp { get; private set; }
        public readonly bool IsEmpty => SignalProp.IsEmpty;
        public readonly bool IsValue => SignalProp.IsValue;
        public readonly bool IsSignal => SignalProp.IsSignal;

        public FlexDirectionProp(StyleEnum<FlexDirection> value)
        {
            SignalProp = value;
        }

        public FlexDirectionProp(BaseSignal<StyleEnum<FlexDirection>> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator FlexDirectionProp(StyleKeyword keyword)
        {
            StyleEnum<FlexDirection> styleEnum = keyword;
            return new FlexDirectionProp(styleEnum);
        }

        public static implicit operator FlexDirectionProp(FlexDirection flexDirection)
        {
            StyleEnum<FlexDirection> styleEnum = flexDirection;
            return new FlexDirectionProp(styleEnum);
        }

        public static implicit operator FlexDirectionProp(BaseSignal<StyleEnum<FlexDirection>> signal)
        {
            return new FlexDirectionProp(signal);
        }

        public readonly StyleEnum<FlexDirection> Get() => SignalProp.Get();
    }

    public struct WorkLoopFlexDirectionProp
    {
        public WorkLoopSignalProp<StyleEnum<FlexDirection>> WorkLoopSignalProp { get; private set; }

        public WorkLoopFlexDirectionProp(FlexDirectionProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public readonly bool Check() => WorkLoopSignalProp.Check();
        public readonly StyleEnum<FlexDirection> Get() => WorkLoopSignalProp.Get();
    }

    public struct JustifyProp
    {
        public SignalProp<StyleEnum<Justify>> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public JustifyProp(StyleEnum<Justify> value)
        {
            SignalProp = value;
        }

        public JustifyProp(BaseSignal<StyleEnum<Justify>> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator JustifyProp(StyleKeyword keyword)
        {
            StyleEnum<Justify> styleEnum = keyword;
            return new JustifyProp(styleEnum);
        }

        public static implicit operator JustifyProp(Justify justify)
        {
            StyleEnum<Justify> styleEnum = justify;
            return new JustifyProp(styleEnum);
        }

        public static implicit operator JustifyProp(BaseSignal<StyleEnum<Justify>> signal)
        {
            return new JustifyProp(signal);
        }

        public StyleEnum<Justify> Get() => SignalProp.Get();
    }

    public struct WorkLoopJustifyProp
    {
        public WorkLoopSignalProp<StyleEnum<Justify>> WorkLoopSignalProp { get; private set; }

        public WorkLoopJustifyProp(JustifyProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleEnum<Justify> Get() => WorkLoopSignalProp.Get();
    }

    public struct AlignProp
    {
        public SignalProp<StyleEnum<Align>> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public AlignProp(StyleEnum<Align> value)
        {
            SignalProp = value;
        }

        public AlignProp(BaseSignal<StyleEnum<Align>> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator AlignProp(StyleKeyword keyword)
        {
            StyleEnum<Align> styleEnum = keyword;
            return new AlignProp(styleEnum);
        }

        public static implicit operator AlignProp(Align align)
        {
            StyleEnum<Align> styleEnum = align;
            return new AlignProp(styleEnum);
        }

        public static implicit operator AlignProp(BaseSignal<StyleEnum<Align>> signal)
        {
            return new AlignProp(signal);
        }

        public StyleEnum<Align> Get() => SignalProp.Get();
    }

    public struct WorkLoopAlignProp
    {
        public WorkLoopSignalProp<StyleEnum<Align>> WorkLoopSignalProp { get; private set; }

        public WorkLoopAlignProp(AlignProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleEnum<Align> Get() => WorkLoopSignalProp.Get();
    }

    public struct StyleFontProp
    {
        public SignalProp<StyleFont> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public StyleFontProp(StyleFont value)
        {
            SignalProp = value;
        }

        public StyleFontProp(BaseSignal<StyleFont> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator StyleFontProp(Font value)
        {
            return new StyleFontProp(value);
        }

        public static implicit operator StyleFontProp(StyleFont value)
        {
            return new StyleFontProp(value);
        }

        public static implicit operator StyleFontProp(BaseSignal<StyleFont> signal)
        {
            return new StyleFontProp(signal);
        }

        public StyleFont Get() => SignalProp.Get();
    }

    public struct WorkLoopStyleFontProp
    {
        public WorkLoopSignalProp<StyleFont> WorkLoopSignalProp { get; private set; }

        public WorkLoopStyleFontProp(StyleFontProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleFont Get() => WorkLoopSignalProp.Get();
    }

    public struct StyleFontDefinitionProp
    {
        public SignalProp<StyleFontDefinition> SignalProp { get; private set; }
        public bool IsEmpty { get => SignalProp.IsEmpty; }
        public bool IsValue { get => SignalProp.IsValue; }
        public bool IsSignal { get => SignalProp.IsSignal; }

        public StyleFontDefinitionProp(StyleFontDefinition value)
        {
            SignalProp = value;
        }

        public StyleFontDefinitionProp(BaseSignal<StyleFontDefinition> signal)
        {
            SignalProp = signal;
        }

        public static implicit operator StyleFontDefinitionProp(StyleFontDefinition value)
        {
            return new StyleFontDefinitionProp(value);
        }

        public static implicit operator StyleFontDefinitionProp(StyleKeyword keyword)
        {
            return new StyleFontDefinitionProp(keyword);
        }

        public static implicit operator StyleFontDefinitionProp(BaseSignal<StyleFontDefinition> signal)
        {
            return new StyleFontDefinitionProp(signal);
        }

        public StyleFontDefinition Get() => SignalProp.Get();
    }

    public struct WorkLoopStyleFontDefinitionProp
    {
        public WorkLoopSignalProp<StyleFontDefinition> WorkLoopSignalProp { get; private set; }

        public WorkLoopStyleFontDefinitionProp(StyleFontDefinitionProp prop)
        {
            WorkLoopSignalProp = new(prop.SignalProp);
        }
        public bool Check() => WorkLoopSignalProp.Check();
        public StyleFontDefinition Get() => WorkLoopSignalProp.Get();
    }

    public struct Style
    {
        public PositionProp Position { get; private set; }
        public StyleLengthProp Right { get; private set; }
        public StyleLengthProp Bottom { get; private set; }
        public StyleLengthProp Left { get; private set; }
        public StyleLengthProp Top { get; private set; }
        public StyleLengthProp PaddingRight { get; private set; }
        public StyleLengthProp PaddingBottom { get; private set; }
        public StyleLengthProp PaddingLeft { get; private set; }
        public StyleLengthProp PaddingTop { get; private set; }
        public StyleLengthProp MarginRight { get; private set; }
        public StyleLengthProp MarginBottom { get; private set; }
        public StyleLengthProp MarginLeft { get; private set; }
        public StyleLengthProp MarginTop { get; private set; }
        public StyleLengthProp BorderTopRightRadius { get; private set; }
        public StyleLengthProp BorderTopLeftRadius { get; private set; }
        public StyleLengthProp BorderBottomRightRadius { get; private set; }
        public StyleLengthProp BorderBottomLeftRadius { get; private set; }
        public StyleFloatProp BorderRightWidth { get; private set; }
        public StyleFloatProp BorderBottomWidth { get; private set; }
        public StyleFloatProp BorderLeftWidth { get; private set; }
        public StyleFloatProp BorderTopWidth { get; private set; }
        public StyleColorProp BorderRightColor { get; private set; }
        public StyleColorProp BorderBottomColor { get; private set; }
        public StyleColorProp BorderLeftColor { get; private set; }
        public StyleColorProp BorderTopColor { get; private set; }
        public DisplayStyleProp Display { get; private set; }
        public StyleFloatProp FlexShrink { get; private set; }
        public StyleFloatProp FlexGrow { get; private set; }
        public FlexDirectionProp FlexDirection { get; private set; }
        public JustifyProp JustifyContent { get; private set; }
        public AlignProp AlignItems { get; private set; }
        public StyleLengthProp Width { get; private set; }
        public StyleLengthProp MaxWidth { get; private set; }
        public StyleLengthProp MinWidth { get; private set; }
        public StyleLengthProp Height { get; private set; }
        public StyleLengthProp MaxHeight { get; private set; }
        public StyleLengthProp MinHeight { get; private set; }
        public StyleColorProp BackgroundColor { get; private set; }
        public StyleColorProp Color { get; private set; }
        public StyleLengthProp FontSize { get; private set; }
        public StyleFontProp UnityFont { get; private set; }
        public StyleFontDefinitionProp UnityFontDefinition { get; private set; }

        public Style(
            PositionProp position = new(),
            StyleLengthProp right = new(),
            StyleLengthProp bottom = new(),
            StyleLengthProp left = new(),
            StyleLengthProp top = new(),
            StyleLengthProp paddingRight = new(),
            StyleLengthProp paddingBottom = new(),
            StyleLengthProp paddingLeft = new(),
            StyleLengthProp paddingTop = new(),
            StyleLengthProp marginRight = new(),
            StyleLengthProp marginBottom = new(),
            StyleLengthProp marginLeft = new(),
            StyleLengthProp marginTop = new(),
            StyleLengthProp borderTopRightRadius = new(),
            StyleLengthProp borderTopLeftRadius = new(),
            StyleLengthProp borderBottomRightRadius = new(),
            StyleLengthProp borderBottomLeftRadius = new(),
            StyleFloatProp borderRightWidth = new(),
            StyleFloatProp borderBottomWidth = new(),
            StyleFloatProp borderLeftWidth = new(),
            StyleFloatProp borderTopWidth = new(),
            StyleColorProp borderRightColor = new(),
            StyleColorProp borderBottomColor = new(),
            StyleColorProp borderLeftColor = new(),
            StyleColorProp borderTopColor = new(),
            DisplayStyleProp display = new(),
            StyleFloatProp flexShrink = new(),
            StyleFloatProp flexGrow = new(),
            FlexDirectionProp flexDirection = new(),
            JustifyProp justifyContent = new(),
            AlignProp alignItems = new(),
            StyleLengthProp width = new(),
            StyleLengthProp maxWidth = new(),
            StyleLengthProp minWidth = new(),
            StyleLengthProp height = new(),
            StyleLengthProp maxHeight = new(),
            StyleLengthProp minHeight = new(),
            StyleColorProp backgroundColor = new(),
            StyleColorProp color = new(),
            StyleLengthProp fontSize = new(),
            StyleFontProp unityFont = new(),
            StyleFontDefinitionProp unityFontDefinition = new()
        )
        {
            Position = position;
            Right = right;
            Bottom = bottom;
            Left = left;
            Top = top;
            PaddingRight = paddingRight;
            PaddingBottom = paddingBottom;
            PaddingLeft = paddingLeft;
            PaddingTop = paddingTop;
            MarginRight = marginRight;
            MarginBottom = marginBottom;
            MarginLeft = marginLeft;
            MarginTop = marginTop;
            BorderTopRightRadius = borderTopRightRadius;
            BorderTopLeftRadius = borderTopLeftRadius;
            BorderBottomRightRadius = borderBottomRightRadius;
            BorderBottomLeftRadius = borderBottomLeftRadius;
            BorderTopWidth = borderTopWidth;
            BorderRightWidth = borderRightWidth;
            BorderLeftWidth = borderLeftWidth;
            BorderBottomWidth = borderBottomWidth;
            BorderTopColor = borderTopColor;
            BorderRightColor = borderRightColor;
            BorderLeftColor = borderLeftColor;
            BorderBottomColor = borderBottomColor;
            Display = display;
            FlexShrink = flexShrink;
            FlexGrow = flexGrow;
            FlexDirection = flexDirection;
            JustifyContent = justifyContent;
            AlignItems = alignItems;
            Width = width;
            MaxWidth = maxWidth;
            MinWidth = minWidth;
            Height = height;
            MaxHeight = maxHeight;
            MinHeight = minHeight;
            BackgroundColor = backgroundColor;
            Color = color;
            FontSize = fontSize;
            UnityFont = unityFont;
            UnityFontDefinition = unityFontDefinition;
        }
    }
}