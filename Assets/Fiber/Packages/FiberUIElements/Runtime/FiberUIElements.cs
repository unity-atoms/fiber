using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Signals;
using Fiber.GameObjects;

namespace Fiber.UIElements
{
    public static class BaseComponentExtensions
    {
        public static List<string> ClassName(
            this BaseComponent component,
            string c1,
            string c2 = null,
            string c3 = null,
            string c4 = null,
            string c5 = null,
            string c6 = null,
            string c7 = null,
            string c8 = null,
            string c9 = null,
            string c10 = null
        )
        {
            var result = new List<string>
            {
                c1
            };
            if (c2 != null) result.Add(c2);
            if (c3 != null) result.Add(c3);
            if (c4 != null) result.Add(c4);
            if (c5 != null) result.Add(c5);
            if (c6 != null) result.Add(c6);
            if (c7 != null) result.Add(c7);
            if (c8 != null) result.Add(c8);
            if (c9 != null) result.Add(c9);
            if (c10 != null) result.Add(c10);

            return result;
        }

        public static ViewComponent View(
            this BaseComponent component,
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        )
        {
            return new ViewComponent(
                style: style,
                name: name,
                pickingMode: pickingMode,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            );
        }

        public static ScrollViewComponent ScrollView(
            this BaseComponent component,
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<ScrollView> _ref = null,
            Action<ScrollView> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        )
        {
            return new ScrollViewComponent(
                style: style,
                name: name,
                pickingMode: pickingMode,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            );
        }

        public static ButtonComponent Button(
            this BaseComponent component,
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> text = new(),
            EventCallback<ClickEvent> onClick = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<Button> _ref = null,
            Action<Button> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        )
        {
            return new ButtonComponent(
                style: style,
                name: name,
                pickingMode: pickingMode,
                text: text,
                onClick: onClick,
                onKeyDown: onKeyDown,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            );
        }

        public static TextComponent Text(
            this BaseComponent component,
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> text = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<TextElement> _ref = null,
            Action<TextElement> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        )
        {
            return new TextComponent(
                style: style,
                name: name,
                pickingMode: pickingMode,
                text: text,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            );
        }

        public static TextFieldComponent TextField(
            this BaseComponent component,
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> value = new(),
            EventCallback<ChangeEvent<string>> onChange = null,
            EventCallback<ClickEvent> onClick = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<TextField> _ref = null,
            Action<TextField> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        )
        {
            return new TextFieldComponent(
                style: style,
                name: name,
                pickingMode: pickingMode,
                value: value,
                onChange: onChange,
                onClick: onClick,
                onKeyDown: onKeyDown,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            );
        }

        public static UIDocumentComponent UIDocument(
            this BaseComponent component,
            SignalProp<string> name = new(),
            SignalProp<bool> active = new(),
            Ref<GameObject> _ref = null,
            Action<GameObject> onCreateRef = null,
            Func<GameObject, GameObject> getInstance = null,
            Action<GameObject> removeInstance = null,
            PanelSettings panelSettings = null,
            float sortingOrder = 0f,
            List<VirtualNode> children = null
        )
        {
            return new UIDocumentComponent(
                name: name,
                active: active,
                _ref: _ref,
                onCreateRef: onCreateRef,
                getInstance: getInstance,
                removeInstance: removeInstance,
                panelSettings: panelSettings,
                sortingOrder: sortingOrder,
                children: children
            );
        }
    }

    public class ViewComponent : VirtualNode
    {
        public SignalProp<Style> Style { get; private set; }
        public SignalProp<string> Name { get; private set; }
        public SignalProp<PickingMode> PickingMode { get; private set; }
        public EventCallback<ClickEvent> OnClick { get; private set; }
        public Ref<VisualElement> Ref { get; private set; }
        public Action<VisualElement> OnCreateRef { get; private set; }
        public SignalProp<List<string>> ClassName { get; private set; }

        public ViewComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                children: children
            )
        {
            Style = style;
            Name = name;
            PickingMode = !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Ignore;
            OnClick = onClick;
            Ref = _ref;
            OnCreateRef = onCreateRef;
            ClassName = className;
        }
    }

    public class ScrollViewComponent : ViewComponent
    {
        public new Ref<ScrollView> Ref { get; set; }
        public new Action<ScrollView> OnCreateRef { get; set; }

        public ScrollViewComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<ScrollView> _ref = null,
            Action<ScrollView> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                className: className,
                children: children
            )
        {
            Ref = _ref;
            OnCreateRef = onCreateRef;
        }
    }

    public class ButtonComponent : ViewComponent
    {
        public SignalProp<string> Text { get; private set; }
        public new Ref<Button> Ref { get; private set; }
        public new Action<Button> OnCreateRef { get; private set; }
        public EventCallback<KeyDownEvent> OnKeyDown { get; private set; }

        public ButtonComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> text = new(),
            EventCallback<ClickEvent> onClick = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<Button> _ref = null,
            Action<Button> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                className: className,
                children: children
            )
        {
            Text = text;
            Ref = _ref;
            OnCreateRef = onCreateRef;
            OnKeyDown = onKeyDown;
        }
    }

    public class TextComponent : ViewComponent
    {
        public SignalProp<string> Text { get; set; }
        public new Ref<TextElement> Ref { get; set; }
        public new Action<TextElement> OnCreateRef { get; set; }

        public TextComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> text = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<TextElement> _ref = null,
            Action<TextElement> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                className: className,
                children: children
            )
        {
            Text = text;
            Ref = _ref;
            OnCreateRef = onCreateRef;
        }
    }

    public class TextFieldComponent : ViewComponent
    {
        public SignalProp<string> Value { get; set; }
        public EventCallback<ChangeEvent<string>> OnChange { get; set; }
        public EventCallback<KeyDownEvent> OnKeyDown { get; set; }
        public new Ref<TextField> Ref { get; set; }
        public new Action<TextField> OnCreateRef { get; set; }

        public TextFieldComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> value = new(),
            EventCallback<ChangeEvent<string>> onChange = null,
            EventCallback<ClickEvent> onClick = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<TextField> _ref = null,
            Action<TextField> onCreateRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                className: className,
                children: children
            )
        {
            Value = value;
            OnChange = onChange;
            OnKeyDown = onKeyDown;
            Ref = _ref;
            OnCreateRef = onCreateRef;
        }
    }

    public class UIDocumentComponent : GameObjectComponent
    {
        public PanelSettings PanelSettings { get; set; }
        public SignalProp<float> SortingOrder { get; set; }

        public UIDocumentComponent(
            SignalProp<string> name = new(),
            SignalProp<bool> active = new(),
            Ref<GameObject> _ref = null,
            Action<GameObject> onCreateRef = null,
            Func<GameObject, GameObject> getInstance = null,
            Action<GameObject> removeInstance = null,
            PanelSettings panelSettings = null,
            SignalProp<float> sortingOrder = new(),
            List<VirtualNode> children = null
        ) : base(
                name: name,
                active: active,
                _ref: _ref,
                onCreateRef: onCreateRef,
                getInstance: getInstance,
                removeInstance: removeInstance,
                children: children
            )
        {
            PanelSettings = panelSettings;
            SortingOrder = !sortingOrder.IsEmpty ? sortingOrder : 0f;
        }
    }

    public class TextFieldNativeNode : VisualElementNativeNode
    {
        public virtual TextField TextFieldElementInstance { get; private set; }
        private WorkLoopSignalProp<string> _valueWorkLoopItem;

        public TextFieldNativeNode(TextFieldComponent virtualNode, TextField instance) : base(virtualNode, instance)
        {
            TextFieldElementInstance = instance;

            if (virtualNode.Ref != null) virtualNode.Ref.Current = instance;
            if (virtualNode.OnCreateRef != null) virtualNode.OnCreateRef(instance);
            if (!virtualNode.Value.IsEmpty)
            {
                TextFieldElementInstance.value = virtualNode.Value.Get();
                _valueWorkLoopItem = new(virtualNode.Value);
            }
            if (virtualNode.OnChange != null)
            {
                TextFieldElementInstance.RegisterCallback<ChangeEvent<string>>(virtualNode.OnChange);
            }
            if (virtualNode.OnKeyDown != null)
            {
                TextFieldElementInstance.RegisterCallback<KeyDownEvent>(virtualNode.OnKeyDown);
            }
        }

        public override void WorkLoop()
        {
            base.WorkLoop();
            if (_valueWorkLoopItem.Check())
            {
                TextFieldElementInstance.value = _valueWorkLoopItem.Get();
            }
        }
    }

    public class TextElementNativeNode : VisualElementNativeNode
    {
        public virtual TextElement TextElementInstance { get; private set; }
        private WorkLoopSignalProp<string> _textWorkLoopItem;

        public TextElementNativeNode(TextComponent virtualNode, TextElement instance) : base(virtualNode, instance)
        {
            TextElementInstance = instance;

            if (virtualNode.Ref != null) virtualNode.Ref.Current = instance;
            virtualNode.OnCreateRef?.Invoke(instance);
            if (!virtualNode.Text.IsEmpty)
            {
                TextElementInstance.text = virtualNode.Text.Get();
                _textWorkLoopItem = new(virtualNode.Text);
            }
        }

        public override void WorkLoop()
        {
            base.WorkLoop();
            if (_textWorkLoopItem.Check())
            {
                TextElementInstance.text = _textWorkLoopItem.Get();
            }
        }
    }

    public class ScrollViewNativeNode : VisualElementNativeNode
    {
        public virtual ScrollView ScrollViewInstance { get; private set; }

        public ScrollViewNativeNode(ScrollViewComponent virtualNode, ScrollView instance) : base(virtualNode, instance)
        {
            ScrollViewInstance = instance;
            if (virtualNode.Ref != null) virtualNode.Ref.Current = instance;
            virtualNode.OnCreateRef?.Invoke(instance);
        }
    }

    public class ButtonNativeNode : VisualElementNativeNode
    {
        public virtual Button ButtonInstance { get; private set; }
        private WorkLoopSignalProp<string> _textWorkLoopItem;

        public ButtonNativeNode(ButtonComponent virtualNode, Button instance) : base(virtualNode, instance)
        {
            ButtonInstance = instance;

            if (virtualNode.Ref != null) virtualNode.Ref.Current = instance;
            if (virtualNode.OnCreateRef != null) virtualNode.OnCreateRef(instance);
            if (!virtualNode.Text.IsEmpty)
            {
                ButtonInstance.text = virtualNode.Text.Get();
                _textWorkLoopItem = new(virtualNode.Text);
            }
            if (virtualNode.OnKeyDown != null)
            {
                ButtonInstance.RegisterCallback<KeyDownEvent>(virtualNode.OnKeyDown);
            }
        }

        public override void WorkLoop()
        {
            base.WorkLoop();
            if (_textWorkLoopItem.Check())
            {
                ButtonInstance.text = _textWorkLoopItem.Get();
            }
        }
    }

    public class VisualElementNativeNode : NativeNode
    {
        public virtual VisualElement Instance { get; private set; }
        private bool IsVisible_SetByFiber { get; set; }
        #region styles
        private WorkLoopSignalProp<Style> _styleWorkLoopItem;
        private Style _lastStyleFromSignal;
        private WorkLoopPositionProp _positionWorkLoopItem;
        private WorkLoopStyleLengthProp _rightWorkLoopItem;
        private WorkLoopStyleLengthProp _bottomWorkLoopItem;
        private WorkLoopStyleLengthProp _leftWorkLoopItem;
        private WorkLoopStyleLengthProp _topWorkLoopItem;
        private WorkLoopStyleLengthProp _paddingRightWorkLoopItem;
        private WorkLoopStyleLengthProp _paddingBottomWorkLoopItem;
        private WorkLoopStyleLengthProp _paddingLeftWorkLoopItem;
        private WorkLoopStyleLengthProp _paddingTopWorkLoopItem;
        private WorkLoopStyleLengthProp _marginRightWorkLoopItem;
        private WorkLoopStyleLengthProp _marginBottomWorkLoopItem;
        private WorkLoopStyleLengthProp _marginLeftWorkLoopItem;
        private WorkLoopStyleLengthProp _marginTopWorkLoopItem;
        private WorkLoopStyleLengthProp _borderTopRightRadiusWorkLoopItem;
        private WorkLoopStyleLengthProp _borderTopLeftRadiusWorkLoopItem;
        private WorkLoopStyleLengthProp _borderBottomRightRadiusWorkLoopItem;
        private WorkLoopStyleLengthProp _borderBottomLeftRadiusWorkLoopItem;
        private WorkLoopStyleFloatProp _borderRightWidthWorkLoopItem;
        private WorkLoopStyleFloatProp _borderBottomWidthWorkLoopItem;
        private WorkLoopStyleFloatProp _borderLeftWidthWorkLoopItem;
        private WorkLoopStyleFloatProp _borderTopWidthWorkLoopItem;
        private WorkLoopStyleColorProp _borderRightColorWorkLoopItem;
        private WorkLoopStyleColorProp _borderBottomColorWorkLoopItem;
        private WorkLoopStyleColorProp _borderLeftColorWorkLoopItem;
        private WorkLoopStyleColorProp _borderTopColorWorkLoopItem;
        private WorkLoopDisplayStyleProp _displayWorkLoopItem;
        private WorkLoopStyleFloatProp _flexShrinkWorkLoopItem;
        private WorkLoopStyleFloatProp _flexGrowWorkLoopItem;
        private WorkLoopFlexDirectionProp _flexDirectionWorkLoopItem;
        private WorkLoopJustifyProp _justifyContentWorkLoopItem;
        private WorkLoopAlignProp _alignItemsWorkLoopItem;
        private WorkLoopStyleLengthProp _widthWorkLoopItem;
        private WorkLoopStyleLengthProp _maxWidthWorkLoopItem;
        private WorkLoopStyleLengthProp _minWidthWorkLoopItem;
        private WorkLoopStyleLengthProp _heightWorkLoopItem;
        private WorkLoopStyleLengthProp _maxHeightWorkLoopItem;
        private WorkLoopStyleLengthProp _minHeightWorkLoopItem;
        private WorkLoopStyleColorProp _backgroundColorWorkLoopItem;
        private WorkLoopStyleColorProp _colorWorkLoopItem;
        private WorkLoopStyleLengthProp _fontSizeWorkLoopItem;
        private WorkLoopStyleFontProp _unityFontWorkLoopItem;
        private WorkLoopStyleFontDefinitionProp _unityFontDefinitionWorkLoopItem;
        private WorkLoopStylePropertyNamesProp _transitionPropertyWorkLoopItem;
        private WorkLoopTimeValuesProp _transitionDelayWorkLoopItem;
        private WorkLoopTimeValuesProp _transitionDurationWorkLoopItem;
        private WorkLoopEasingFunctionsProp _transitionTimingFunctionWorkLoopItem;

        #endregion
        private WorkLoopSignalProp<string> _nameWorkLoopItem;
        private WorkLoopSignalProp<PickingMode> _pickingModeWorkLoopItem;
        private WorkLoopSignalProp<List<string>> _classNameWorkLoopItem;
        private readonly List<string> _previousClassNameList;

        public VisualElementNativeNode(ViewComponent virtualNode, VisualElement instance)
        {
            Instance = instance;

            if (virtualNode.Ref != null) virtualNode.Ref.Current = instance;
            virtualNode.OnCreateRef?.Invoke(instance);
            if (!virtualNode.Style.IsEmpty)
            {
                _styleWorkLoopItem = new(virtualNode.Style);
                var style = virtualNode.Style.Get();
                _lastStyleFromSignal = style;
                var isStyleValue = virtualNode.Style.IsValue;

                if (!style.Position.IsEmpty)
                {
                    Instance.style.position = style.Position.Get();
                    if (isStyleValue)
                    {
                        _positionWorkLoopItem = new(style.Position);
                    }
                }
                if (!style.Right.IsEmpty)
                {
                    Instance.style.right = style.Right.Get();
                    if (isStyleValue)
                    {
                        _rightWorkLoopItem = new(style.Right);
                    }
                }
                if (!style.Bottom.IsEmpty)
                {
                    Instance.style.bottom = style.Bottom.Get();
                    if (isStyleValue)
                    {
                        _bottomWorkLoopItem = new(style.Bottom);
                    }
                }
                if (!style.Left.IsEmpty)
                {
                    Instance.style.left = style.Left.Get();
                    if (isStyleValue)
                    {
                        _leftWorkLoopItem = new(style.Left);
                    }
                }
                if (!style.Top.IsEmpty)
                {
                    Instance.style.top = style.Top.Get();
                    if (isStyleValue)
                    {
                        _topWorkLoopItem = new(style.Top);
                    }
                }
                if (!style.PaddingRight.IsEmpty)
                {
                    Instance.style.paddingRight = style.PaddingRight.Get();
                    if (isStyleValue)
                    {
                        _paddingRightWorkLoopItem = new(style.PaddingRight);
                    }
                }
                if (!style.PaddingBottom.IsEmpty)
                {
                    Instance.style.paddingBottom = style.PaddingBottom.Get();
                    if (isStyleValue)
                    {
                        _paddingBottomWorkLoopItem = new(style.PaddingBottom);
                    }
                }
                if (!style.PaddingLeft.IsEmpty)
                {
                    Instance.style.paddingLeft = style.PaddingLeft.Get();
                    if (isStyleValue)
                    {
                        _paddingLeftWorkLoopItem = new(style.PaddingLeft);
                    }
                }
                if (!style.PaddingTop.IsEmpty)
                {
                    Instance.style.paddingTop = style.PaddingTop.Get();
                    if (isStyleValue)
                    {
                        _paddingTopWorkLoopItem = new(style.PaddingTop);
                    }
                }
                if (!style.MarginRight.IsEmpty)
                {
                    Instance.style.marginRight = style.MarginRight.Get();
                    if (isStyleValue)
                    {
                        _marginRightWorkLoopItem = new(style.MarginRight);
                    }
                }
                if (!style.MarginBottom.IsEmpty)
                {
                    Instance.style.marginBottom = style.MarginBottom.Get();
                    if (isStyleValue)
                    {
                        _marginBottomWorkLoopItem = new(style.MarginBottom);
                    }
                }
                if (!style.MarginLeft.IsEmpty)
                {
                    Instance.style.marginLeft = style.MarginLeft.Get();
                    if (isStyleValue)
                    {
                        _marginLeftWorkLoopItem = new(style.MarginLeft);
                    }
                }
                if (!style.MarginTop.IsEmpty)
                {
                    Instance.style.marginTop = style.MarginTop.Get();
                    if (isStyleValue)
                    {
                        _marginTopWorkLoopItem = new(style.MarginTop);
                    }
                }
                if (!style.BorderTopRightRadius.IsEmpty)
                {
                    Instance.style.borderTopRightRadius = style.BorderTopRightRadius.Get();
                    if (isStyleValue)
                    {
                        _borderTopRightRadiusWorkLoopItem = new(style.BorderTopRightRadius);
                    }
                }
                if (!style.BorderTopLeftRadius.IsEmpty)
                {
                    Instance.style.borderTopLeftRadius = style.BorderTopLeftRadius.Get();
                    if (isStyleValue)
                    {
                        _borderTopLeftRadiusWorkLoopItem = new(style.BorderTopLeftRadius);
                    }
                }
                if (!style.BorderBottomRightRadius.IsEmpty)
                {
                    Instance.style.borderBottomRightRadius = style.BorderBottomRightRadius.Get();
                    if (isStyleValue)
                    {
                        _borderBottomRightRadiusWorkLoopItem = new(style.BorderBottomRightRadius);
                    }
                }
                if (!style.BorderBottomLeftRadius.IsEmpty)
                {
                    Instance.style.borderBottomLeftRadius = style.BorderBottomLeftRadius.Get();
                    if (isStyleValue)
                    {
                        _borderBottomLeftRadiusWorkLoopItem = new(style.BorderBottomLeftRadius);
                    }
                }
                if (!style.BorderRightWidth.IsEmpty)
                {
                    Instance.style.borderRightWidth = style.BorderRightWidth.Get();
                    if (isStyleValue)
                    {
                        _borderRightWidthWorkLoopItem = new(style.BorderRightWidth);
                    }
                }
                if (!style.BorderBottomWidth.IsEmpty)
                {
                    Instance.style.borderBottomWidth = style.BorderBottomWidth.Get();
                    if (isStyleValue)
                    {
                        _borderBottomWidthWorkLoopItem = new(style.BorderBottomWidth);
                    }
                }
                if (!style.BorderLeftWidth.IsEmpty)
                {
                    Instance.style.borderLeftWidth = style.BorderLeftWidth.Get();
                    if (isStyleValue)
                    {
                        _borderLeftWidthWorkLoopItem = new(style.BorderLeftWidth);
                    }
                }
                if (!style.BorderTopWidth.IsEmpty)
                {
                    Instance.style.borderTopWidth = style.BorderTopWidth.Get();
                    if (isStyleValue)
                    {
                        _borderTopWidthWorkLoopItem = new(style.BorderTopWidth);
                    }
                }
                if (!style.BorderRightColor.IsEmpty)
                {
                    Instance.style.borderRightColor = style.BorderRightColor.Get();
                    if (isStyleValue)
                    {
                        _borderRightColorWorkLoopItem = new(style.BorderRightColor);
                    }
                }
                if (!style.BorderBottomColor.IsEmpty)
                {
                    Instance.style.borderBottomColor = style.BorderBottomColor.Get();
                    if (isStyleValue)
                    {
                        _borderBottomColorWorkLoopItem = new(style.BorderBottomColor);
                    }
                }
                if (!style.BorderLeftColor.IsEmpty)
                {
                    Instance.style.borderLeftColor = style.BorderLeftColor.Get();
                    if (isStyleValue)
                    {
                        _borderLeftColorWorkLoopItem = new(style.BorderLeftColor);
                    }
                }
                if (!style.BorderTopColor.IsEmpty)
                {
                    Instance.style.borderTopColor = style.BorderTopColor.Get();
                    if (isStyleValue)
                    {
                        _borderTopColorWorkLoopItem = new(style.BorderTopColor);
                    }
                }
                if (!style.Display.IsEmpty)
                {
                    Instance.style.display = style.Display.Get();
                    if (isStyleValue)
                    {
                        _displayWorkLoopItem = new(style.Display);
                    }
                }
                if (!style.FlexShrink.IsEmpty)
                {
                    Instance.style.flexShrink = style.FlexShrink.Get();
                    if (isStyleValue)
                    {
                        _flexShrinkWorkLoopItem = new(style.FlexShrink);
                    }
                }
                if (!style.FlexGrow.IsEmpty)
                {
                    Instance.style.flexGrow = style.FlexGrow.Get();
                    if (isStyleValue)
                    {
                        _flexGrowWorkLoopItem = new(style.FlexGrow);
                    }
                }
                if (!style.FlexDirection.IsEmpty)
                {
                    Instance.style.flexDirection = style.FlexDirection.Get();
                    if (isStyleValue)
                    {
                        _flexDirectionWorkLoopItem = new(style.FlexDirection);
                    }
                }
                if (!style.JustifyContent.IsEmpty)
                {
                    Instance.style.justifyContent = style.JustifyContent.Get();
                    if (isStyleValue)
                    {
                        _justifyContentWorkLoopItem = new(style.JustifyContent);
                    }
                }
                if (!style.AlignItems.IsEmpty)
                {
                    Instance.style.alignItems = style.AlignItems.Get();
                    if (isStyleValue)
                    {
                        _alignItemsWorkLoopItem = new(style.AlignItems);
                    }
                }
                if (!style.Width.IsEmpty)
                {
                    Instance.style.width = style.Width.Get();
                    if (isStyleValue)
                    {
                        _widthWorkLoopItem = new(style.Width);
                    }
                }
                if (!style.MaxWidth.IsEmpty)
                {
                    Instance.style.maxWidth = style.MaxWidth.Get();
                    if (isStyleValue)
                    {
                        _maxWidthWorkLoopItem = new(style.MaxWidth);
                    }
                }
                if (!style.MinWidth.IsEmpty)
                {
                    Instance.style.minWidth = style.MinWidth.Get();
                    if (isStyleValue)
                    {
                        _minWidthWorkLoopItem = new(style.MinWidth);
                    }
                }
                if (!style.Height.IsEmpty)
                {
                    Instance.style.height = style.Height.Get();
                    if (isStyleValue)
                    {
                        _heightWorkLoopItem = new(style.Height);
                    }
                }
                if (!style.MaxHeight.IsEmpty)
                {
                    Instance.style.maxHeight = style.MaxHeight.Get();
                    if (isStyleValue)
                    {
                        _maxHeightWorkLoopItem = new(style.MaxHeight);
                    }
                }
                if (!style.MinHeight.IsEmpty)
                {
                    Instance.style.minHeight = style.MinHeight.Get();
                    if (isStyleValue)
                    {
                        _minHeightWorkLoopItem = new(style.MinHeight);
                    }
                }
                if (!style.BackgroundColor.IsEmpty)
                {
                    Instance.style.backgroundColor = style.BackgroundColor.Get();
                    if (isStyleValue)
                    {
                        _backgroundColorWorkLoopItem = new(style.BackgroundColor);
                    }
                }
                if (!style.Color.IsEmpty)
                {
                    Instance.style.color = style.Color.Get();
                    if (isStyleValue)
                    {
                        _colorWorkLoopItem = new(style.Color);
                    }
                }
                if (!style.FontSize.IsEmpty)
                {
                    Instance.style.fontSize = style.FontSize.Get();
                    if (isStyleValue)
                    {
                        _fontSizeWorkLoopItem = new(style.FontSize);
                    }
                }
                if (!style.UnityFont.IsEmpty)
                {
                    Instance.style.unityFont = style.UnityFont.Get();
                    if (isStyleValue)
                    {
                        _unityFontWorkLoopItem = new(style.UnityFont);
                    }
                }
                if (!style.UnityFontDefinition.IsEmpty)
                {
                    Instance.style.unityFontDefinition = style.UnityFontDefinition.Get();
                    if (isStyleValue)
                    {
                        _unityFontDefinitionWorkLoopItem = new(style.UnityFontDefinition);
                    }
                }
                if (!style.TransitionProperty.IsEmpty)
                {
                    Instance.style.transitionProperty = style.TransitionProperty.Get();
                    if (isStyleValue)
                    {
                        _transitionPropertyWorkLoopItem = new(style.TransitionProperty);
                    }
                }
                if (!style.TransitionDelay.IsEmpty)
                {
                    Instance.style.transitionDelay = style.TransitionDelay.Get();
                    if (isStyleValue)
                    {
                        _transitionDelayWorkLoopItem = new(style.TransitionDelay);
                    }
                }
                if (!style.TransitionDuration.IsEmpty)
                {
                    Instance.style.transitionDuration = style.TransitionDuration.Get();
                    if (isStyleValue)
                    {
                        _transitionDurationWorkLoopItem = new(style.TransitionDuration);
                    }
                }
                if (!style.TransitionTimingFunction.IsEmpty)
                {
                    Instance.style.transitionTimingFunction = style.TransitionTimingFunction.Get();
                    if (isStyleValue)
                    {
                        _transitionTimingFunctionWorkLoopItem = new(style.TransitionTimingFunction);
                    }
                }
            }
            if (!virtualNode.Name.IsEmpty)
            {
                instance.name = virtualNode.Name.Get();
                _nameWorkLoopItem = new(virtualNode.Name);
            }
            if (!virtualNode.PickingMode.IsEmpty)
            {
                instance.pickingMode = virtualNode.PickingMode.Get();
                _pickingModeWorkLoopItem = new(virtualNode.PickingMode);
            }
            if (virtualNode.OnClick != null) instance.RegisterCallback<ClickEvent>(virtualNode.OnClick);

            if (!virtualNode.ClassName.IsEmpty)
            {
                var names = virtualNode.ClassName.Get();
                _previousClassNameList = names != null ? new(names.Count) : new();
                for (var i = 0; names != null && i < names.Count; i++)
                {
                    var name = names[i];
                    _previousClassNameList.Add(name);
                    Instance.AddToClassList(name);
                }
                _classNameWorkLoopItem = new(virtualNode.ClassName);
            }
        }

        public override void SetVisible(bool visible)
        {
            IsVisible_SetByFiber = visible;
            UpdateDisplayStyle();
        }

        private void UpdateDisplayStyle()
        {
            var currentStyle = GetCurrentDisplayStyle();
            if (Instance.style.display != currentStyle)
            {
                Instance.style.display = currentStyle;
            }
        }

        private StyleEnum<DisplayStyle> GetCurrentDisplayStyle()
        {
            // Visibility set from Fiber always take precedence
            if (!IsVisible_SetByFiber)
            {
                return DisplayStyle.None;
            }

            // Handle if the style prop is a signal
            if (_styleWorkLoopItem.IsSignal)
            {
                var style = _styleWorkLoopItem.Get();
                if (!style.Display.IsEmpty)
                {
                    return style.Display.Get();
                }
                else
                {
                    return StyleKeyword.Initial;
                }
            }
            // Handle if the style prop is a value (with individual values / signals as its props)
            else if (_styleWorkLoopItem.IsValue)
            {
                if (_displayWorkLoopItem.WorkLoopSignalProp.IsEmpty)
                {
                    return StyleKeyword.Initial;
                }
                return _displayWorkLoopItem.Get();
            }

            // The style prop is empty
            return StyleKeyword.Initial;
        }

        public override void AddChild(FiberNode node, int index)
        {
            if (node.NativeNode is VisualElementNativeNode visualElementChildNode)
            {
                Instance.Insert(index, visualElementChildNode.Instance);
                return;
            }

            throw new Exception($"Trying to add child of unknown type {node.VirtualNode.GetType()} at index {index}.");
        }

        public override void RemoveChild(FiberNode node)
        {
            if (node.NativeNode is VisualElementNativeNode visualElementChildNode)
            {
                Instance.Remove(visualElementChildNode.Instance);
                return;
            }
            throw new Exception($"Trying to remove child of unknown type {node.VirtualNode.GetType()}.");
        }

        public override void MoveChild(FiberNode node, int index)
        {
            if (node.NativeNode is VisualElementNativeNode visualElementChildNode)
            {
                var currentIndex = Instance.IndexOf(visualElementChildNode.Instance);
                if (currentIndex > index)
                {
                    var currentElementAtIndex = Instance.ElementAt(index);
                    visualElementChildNode.Instance.PlaceBehind(currentElementAtIndex);
                }
                else if (currentIndex < index)
                {
                    var currentElementBeforeIndex = Instance.ElementAt(index - 1);
                    visualElementChildNode.Instance.PlaceInFront(currentElementBeforeIndex);
                }
                return;
            }

            throw new Exception($"Trying to move child of unknown type {node.VirtualNode.GetType()}.");
        }

        public override void WorkLoop()
        {
            // If style is a signal itself we listen to changes to it and then just re set all values that are not empty.
            // If style is a value we instead check if the value has changed and only set the values that have changed.
            // TODO: Removing of properties
            if (_styleWorkLoopItem.IsSignal && _styleWorkLoopItem.Check())
            {
                var style = _styleWorkLoopItem.Get();
                // OPEN POINT: Should we compare current value with new value before applying?
                if (!style.Position.IsEmpty)
                {
                    Instance.style.position = style.Position.Get();
                }
                else if (!_lastStyleFromSignal.Position.IsEmpty)
                {
                    Instance.style.position = StyleKeyword.Initial;
                }

                if (!style.Right.IsEmpty)
                {
                    Instance.style.right = style.Right.Get();
                }
                else if (!_lastStyleFromSignal.Right.IsEmpty)
                {
                    Instance.style.right = StyleKeyword.Initial;
                }

                if (!style.Bottom.IsEmpty)
                {
                    Instance.style.bottom = style.Bottom.Get();
                }
                else if (!_lastStyleFromSignal.Bottom.IsEmpty)
                {
                    Instance.style.bottom = StyleKeyword.Initial;
                }

                if (!style.Left.IsEmpty)
                {
                    Instance.style.left = style.Left.Get();
                }
                else if (!_lastStyleFromSignal.Left.IsEmpty)
                {
                    Instance.style.left = StyleKeyword.Initial;
                }

                if (!style.Top.IsEmpty)
                {
                    Instance.style.top = style.Top.Get();
                }
                else if (!_lastStyleFromSignal.Top.IsEmpty)
                {
                    Instance.style.top = StyleKeyword.Initial;
                }

                if (!style.PaddingRight.IsEmpty)
                {
                    Instance.style.paddingRight = style.PaddingRight.Get();
                }
                else if (!_lastStyleFromSignal.PaddingRight.IsEmpty)
                {
                    Instance.style.paddingRight = StyleKeyword.Initial;
                }

                if (!style.PaddingBottom.IsEmpty)
                {
                    Instance.style.paddingBottom = style.PaddingBottom.Get();
                }
                else if (!_lastStyleFromSignal.PaddingBottom.IsEmpty)
                {
                    Instance.style.paddingBottom = StyleKeyword.Initial;
                }

                if (!style.PaddingLeft.IsEmpty)
                {
                    Instance.style.paddingLeft = style.PaddingLeft.Get();
                }
                else if (!_lastStyleFromSignal.PaddingLeft.IsEmpty)
                {
                    Instance.style.paddingLeft = StyleKeyword.Initial;
                }

                if (!style.PaddingTop.IsEmpty)
                {
                    Instance.style.paddingTop = style.PaddingTop.Get();
                }
                else if (!_lastStyleFromSignal.PaddingTop.IsEmpty)
                {
                    Instance.style.paddingTop = StyleKeyword.Initial;
                }

                if (!style.MarginRight.IsEmpty)
                {
                    Instance.style.marginRight = style.MarginRight.Get();
                }
                else if (!_lastStyleFromSignal.MarginRight.IsEmpty)
                {
                    Instance.style.marginRight = StyleKeyword.Initial;
                }

                if (!style.MarginBottom.IsEmpty)
                {
                    Instance.style.marginBottom = style.MarginBottom.Get();
                }
                else if (!_lastStyleFromSignal.MarginBottom.IsEmpty)
                {
                    Instance.style.marginBottom = StyleKeyword.Initial;
                }

                if (!style.MarginLeft.IsEmpty)
                {
                    Instance.style.marginLeft = style.MarginLeft.Get();
                }
                else if (!_lastStyleFromSignal.MarginLeft.IsEmpty)
                {
                    Instance.style.marginLeft = StyleKeyword.Initial;
                }

                if (!style.MarginTop.IsEmpty)
                {
                    Instance.style.marginTop = style.MarginTop.Get();
                }
                else if (!_lastStyleFromSignal.MarginTop.IsEmpty)
                {
                    Instance.style.marginTop = StyleKeyword.Initial;
                }

                if (!style.BorderTopRightRadius.IsEmpty)
                {
                    Instance.style.borderTopRightRadius = style.BorderTopRightRadius.Get();
                }
                else if (!_lastStyleFromSignal.BorderTopRightRadius.IsEmpty)
                {
                    Instance.style.borderTopRightRadius = StyleKeyword.Initial;
                }

                if (!style.BorderTopLeftRadius.IsEmpty)
                {
                    Instance.style.borderTopLeftRadius = style.BorderTopLeftRadius.Get();
                }
                else if (!_lastStyleFromSignal.BorderTopLeftRadius.IsEmpty)
                {
                    Instance.style.borderTopLeftRadius = StyleKeyword.Initial;
                }

                if (!style.BorderBottomRightRadius.IsEmpty)
                {
                    Instance.style.borderBottomRightRadius = style.BorderBottomRightRadius.Get();
                }
                else if (!_lastStyleFromSignal.BorderBottomRightRadius.IsEmpty)
                {
                    Instance.style.borderBottomRightRadius = StyleKeyword.Initial;
                }

                if (!style.BorderBottomLeftRadius.IsEmpty)
                {
                    Instance.style.borderBottomLeftRadius = style.BorderBottomLeftRadius.Get();
                }
                else if (!_lastStyleFromSignal.BorderBottomLeftRadius.IsEmpty)
                {
                    Instance.style.borderBottomLeftRadius = StyleKeyword.Initial;
                }

                if (!style.BorderRightWidth.IsEmpty)
                {
                    Instance.style.borderRightWidth = style.BorderRightWidth.Get();
                }
                else if (!_lastStyleFromSignal.BorderRightWidth.IsEmpty)
                {
                    Instance.style.borderRightWidth = StyleKeyword.Initial;
                }

                if (!style.BorderBottomWidth.IsEmpty)
                {
                    Instance.style.borderBottomWidth = style.BorderBottomWidth.Get();
                }
                else if (!_lastStyleFromSignal.BorderBottomWidth.IsEmpty)
                {
                    Instance.style.borderBottomWidth = StyleKeyword.Initial;
                }

                if (!style.BorderLeftWidth.IsEmpty)
                {
                    Instance.style.borderLeftWidth = style.BorderLeftWidth.Get();
                }
                else if (!_lastStyleFromSignal.BorderLeftWidth.IsEmpty)
                {
                    Instance.style.borderLeftWidth = StyleKeyword.Initial;
                }

                if (!style.BorderTopWidth.IsEmpty)
                {
                    Instance.style.borderTopWidth = style.BorderTopWidth.Get();
                }
                else if (!_lastStyleFromSignal.BorderTopWidth.IsEmpty)
                {
                    Instance.style.borderTopWidth = StyleKeyword.Initial;
                }

                if (!style.BorderRightColor.IsEmpty)
                {
                    Instance.style.borderRightColor = style.BorderRightColor.Get();
                }
                else if (!_lastStyleFromSignal.BorderRightColor.IsEmpty)
                {
                    Instance.style.borderRightColor = StyleKeyword.Initial;
                }

                if (!style.BorderBottomColor.IsEmpty)
                {
                    Instance.style.borderBottomColor = style.BorderBottomColor.Get();
                }
                else if (!_lastStyleFromSignal.BorderBottomColor.IsEmpty)
                {
                    Instance.style.borderBottomColor = StyleKeyword.Initial;
                }

                if (!style.BorderLeftColor.IsEmpty)
                {
                    Instance.style.borderLeftColor = style.BorderLeftColor.Get();
                }
                else if (!_lastStyleFromSignal.BorderLeftColor.IsEmpty)
                {
                    Instance.style.borderLeftColor = StyleKeyword.Initial;
                }

                if (!style.BorderTopColor.IsEmpty)
                {
                    Instance.style.borderTopColor = style.BorderTopColor.Get();
                }
                else if (!_lastStyleFromSignal.BorderTopColor.IsEmpty)
                {
                    Instance.style.borderTopColor = StyleKeyword.Initial;
                }

                if (!style.Display.IsEmpty || !_lastStyleFromSignal.Display.IsEmpty)
                {
                    UpdateDisplayStyle();
                }

                if (!style.FlexShrink.IsEmpty)
                {
                    Instance.style.flexShrink = style.FlexShrink.Get();
                }
                else if (!_lastStyleFromSignal.FlexShrink.IsEmpty)
                {
                    Instance.style.flexShrink = StyleKeyword.Initial;
                }

                if (!style.FlexGrow.IsEmpty)
                {
                    Instance.style.flexGrow = style.FlexGrow.Get();
                }
                else if (!_lastStyleFromSignal.FlexGrow.IsEmpty)
                {
                    Instance.style.flexGrow = StyleKeyword.Initial;
                }

                if (!style.FlexDirection.IsEmpty)
                {
                    Instance.style.flexDirection = style.FlexDirection.Get();
                }
                else if (!_lastStyleFromSignal.FlexDirection.IsEmpty)
                {
                    Instance.style.flexDirection = StyleKeyword.Initial;
                }

                if (!style.JustifyContent.IsEmpty)
                {
                    Instance.style.justifyContent = style.JustifyContent.Get();
                }
                else if (!_lastStyleFromSignal.JustifyContent.IsEmpty)
                {
                    Instance.style.justifyContent = StyleKeyword.Initial;
                }

                if (!style.AlignItems.IsEmpty)
                {
                    Instance.style.alignItems = style.AlignItems.Get();
                }
                else if (!_lastStyleFromSignal.AlignItems.IsEmpty)
                {
                    Instance.style.alignItems = StyleKeyword.Initial;
                }

                if (!style.Width.IsEmpty)
                {
                    Instance.style.width = style.Width.Get();
                }
                else if (!_lastStyleFromSignal.Width.IsEmpty)
                {
                    Instance.style.width = StyleKeyword.Initial;
                }

                if (!style.MaxWidth.IsEmpty)
                {
                    Instance.style.maxWidth = style.MaxWidth.Get();
                }
                else if (!_lastStyleFromSignal.MaxWidth.IsEmpty)
                {
                    Instance.style.maxWidth = StyleKeyword.Initial;
                }

                if (!style.MinWidth.IsEmpty)
                {
                    Instance.style.minWidth = style.MinWidth.Get();
                }
                else if (!_lastStyleFromSignal.MinWidth.IsEmpty)
                {
                    Instance.style.minWidth = StyleKeyword.Initial;
                }

                if (!style.Height.IsEmpty)
                {
                    Instance.style.height = style.Height.Get();
                }
                else if (!_lastStyleFromSignal.Height.IsEmpty)
                {
                    Instance.style.height = StyleKeyword.Initial;
                }

                if (!style.MaxHeight.IsEmpty)
                {
                    Instance.style.maxHeight = style.MaxHeight.Get();
                }
                else if (!_lastStyleFromSignal.MaxHeight.IsEmpty)
                {
                    Instance.style.maxHeight = StyleKeyword.Initial;
                }

                if (!style.MinHeight.IsEmpty)
                {
                    Instance.style.minHeight = style.MinHeight.Get();
                }
                else if (!_lastStyleFromSignal.MinHeight.IsEmpty)
                {
                    Instance.style.minHeight = StyleKeyword.Initial;
                }

                if (!style.BackgroundColor.IsEmpty)
                {
                    Instance.style.backgroundColor = style.BackgroundColor.Get();
                }
                else if (!_lastStyleFromSignal.BackgroundColor.IsEmpty)
                {
                    Instance.style.backgroundColor = StyleKeyword.Initial;
                }

                if (!style.Color.IsEmpty)
                {
                    Instance.style.color = style.Color.Get();
                }
                else if (!_lastStyleFromSignal.Color.IsEmpty)
                {
                    Instance.style.color = StyleKeyword.Initial;
                }

                if (!style.FontSize.IsEmpty)
                {
                    Instance.style.fontSize = style.FontSize.Get();
                }
                else if (!_lastStyleFromSignal.FontSize.IsEmpty)
                {
                    Instance.style.fontSize = StyleKeyword.Initial;
                }

                if (!style.UnityFont.IsEmpty)
                {
                    Instance.style.unityFont = style.UnityFont.Get();
                }
                else if (!_lastStyleFromSignal.UnityFont.IsEmpty)
                {
                    Instance.style.unityFont = StyleKeyword.Initial;
                }

                if (!style.UnityFontDefinition.IsEmpty)
                {
                    Instance.style.unityFontDefinition = style.UnityFontDefinition.Get();
                }
                else if (!_lastStyleFromSignal.UnityFontDefinition.IsEmpty)
                {
                    Instance.style.unityFontDefinition = StyleKeyword.Initial;
                }

                if (!style.TransitionProperty.IsEmpty)
                {
                    Instance.style.transitionProperty = style.TransitionProperty.Get();
                }
                else if (!_lastStyleFromSignal.TransitionProperty.IsEmpty)
                {
                    Instance.style.transitionProperty = StyleKeyword.Initial;
                }

                if (!style.TransitionDelay.IsEmpty)
                {
                    Instance.style.transitionDelay = style.TransitionDelay.Get();
                }
                else if (!_lastStyleFromSignal.TransitionDelay.IsEmpty)
                {
                    Instance.style.transitionDelay = StyleKeyword.Initial;
                }

                if (!style.TransitionDuration.IsEmpty)
                {
                    Instance.style.transitionDuration = style.TransitionDuration.Get();
                }
                else if (!_lastStyleFromSignal.TransitionDuration.IsEmpty)
                {
                    Instance.style.transitionDuration = StyleKeyword.Initial;
                }

                if (!style.TransitionTimingFunction.IsEmpty)
                {
                    Instance.style.transitionTimingFunction = style.TransitionTimingFunction.Get();
                }
                else if (!_lastStyleFromSignal.TransitionTimingFunction.IsEmpty)
                {
                    Instance.style.transitionTimingFunction = StyleKeyword.Initial;
                }

                _lastStyleFromSignal = style;
            }
            else if (_styleWorkLoopItem.IsValue)
            {
                if (_positionWorkLoopItem.Check())
                {
                    Instance.style.position = _positionWorkLoopItem.Get();
                }
                if (_rightWorkLoopItem.Check())
                {
                    Instance.style.right = _rightWorkLoopItem.Get();
                }
                if (_bottomWorkLoopItem.Check())
                {
                    Instance.style.bottom = _bottomWorkLoopItem.Get();
                }
                if (_leftWorkLoopItem.Check())
                {
                    Instance.style.left = _leftWorkLoopItem.Get();
                }
                if (_topWorkLoopItem.Check())
                {
                    Instance.style.top = _topWorkLoopItem.Get();
                }
                if (_paddingRightWorkLoopItem.Check())
                {
                    Instance.style.paddingRight = _paddingRightWorkLoopItem.Get();
                }
                if (_paddingBottomWorkLoopItem.Check())
                {
                    Instance.style.paddingBottom = _paddingBottomWorkLoopItem.Get();
                }
                if (_paddingLeftWorkLoopItem.Check())
                {
                    Instance.style.paddingLeft = _paddingLeftWorkLoopItem.Get();
                }
                if (_paddingTopWorkLoopItem.Check())
                {
                    Instance.style.paddingTop = _paddingTopWorkLoopItem.Get();
                }
                if (_marginRightWorkLoopItem.Check())
                {
                    Instance.style.marginRight = _marginRightWorkLoopItem.Get();
                }
                if (_marginBottomWorkLoopItem.Check())
                {
                    Instance.style.marginBottom = _marginBottomWorkLoopItem.Get();
                }
                if (_marginLeftWorkLoopItem.Check())
                {
                    Instance.style.marginLeft = _marginLeftWorkLoopItem.Get();
                }
                if (_marginTopWorkLoopItem.Check())
                {
                    Instance.style.marginTop = _marginTopWorkLoopItem.Get();
                }
                if (_borderTopRightRadiusWorkLoopItem.Check())
                {
                    Instance.style.borderTopRightRadius = _borderTopRightRadiusWorkLoopItem.Get();
                }
                if (_borderTopLeftRadiusWorkLoopItem.Check())
                {
                    Instance.style.borderTopLeftRadius = _borderTopLeftRadiusWorkLoopItem.Get();
                }
                if (_borderBottomRightRadiusWorkLoopItem.Check())
                {
                    Instance.style.borderBottomRightRadius = _borderBottomRightRadiusWorkLoopItem.Get();
                }
                if (_borderBottomLeftRadiusWorkLoopItem.Check())
                {
                    Instance.style.borderBottomLeftRadius = _borderBottomLeftRadiusWorkLoopItem.Get();
                }
                if (_borderRightWidthWorkLoopItem.Check())
                {
                    Instance.style.borderRightWidth = _borderRightWidthWorkLoopItem.Get();
                }
                if (_borderBottomWidthWorkLoopItem.Check())
                {
                    Instance.style.borderBottomWidth = _borderBottomWidthWorkLoopItem.Get();
                }
                if (_borderLeftWidthWorkLoopItem.Check())
                {
                    Instance.style.borderLeftWidth = _borderLeftWidthWorkLoopItem.Get();
                }
                if (_borderTopWidthWorkLoopItem.Check())
                {
                    Instance.style.borderTopWidth = _borderTopWidthWorkLoopItem.Get();
                }
                if (_borderRightColorWorkLoopItem.Check())
                {
                    Instance.style.borderRightColor = _borderRightColorWorkLoopItem.Get();
                }
                if (_borderBottomColorWorkLoopItem.Check())
                {
                    Instance.style.borderBottomColor = _borderBottomColorWorkLoopItem.Get();
                }
                if (_borderLeftColorWorkLoopItem.Check())
                {
                    Instance.style.borderLeftColor = _borderLeftColorWorkLoopItem.Get();
                }
                if (_borderTopColorWorkLoopItem.Check())
                {
                    Instance.style.borderTopColor = _borderTopColorWorkLoopItem.Get();
                }
                if (_displayWorkLoopItem.Check())
                {
                    UpdateDisplayStyle();
                }
                if (_flexShrinkWorkLoopItem.Check())
                {
                    Instance.style.flexShrink = _flexShrinkWorkLoopItem.Get();
                }
                if (_flexGrowWorkLoopItem.Check())
                {
                    Instance.style.flexGrow = _flexGrowWorkLoopItem.Get();
                }
                if (_flexDirectionWorkLoopItem.Check())
                {
                    Instance.style.flexDirection = _flexDirectionWorkLoopItem.Get();
                }
                if (_justifyContentWorkLoopItem.Check())
                {
                    Instance.style.justifyContent = _justifyContentWorkLoopItem.Get();
                }
                if (_alignItemsWorkLoopItem.Check())
                {
                    Instance.style.alignItems = _alignItemsWorkLoopItem.Get();
                }
                if (_widthWorkLoopItem.Check())
                {
                    Instance.style.width = _widthWorkLoopItem.Get();
                }
                if (_maxWidthWorkLoopItem.Check())
                {
                    Instance.style.maxWidth = _maxWidthWorkLoopItem.Get();
                }
                if (_minWidthWorkLoopItem.Check())
                {
                    Instance.style.minWidth = _minWidthWorkLoopItem.Get();
                }
                if (_heightWorkLoopItem.Check())
                {
                    Instance.style.height = _heightWorkLoopItem.Get();
                }
                if (_maxHeightWorkLoopItem.Check())
                {
                    Instance.style.maxHeight = _maxHeightWorkLoopItem.Get();
                }
                if (_minHeightWorkLoopItem.Check())
                {
                    Instance.style.minHeight = _minHeightWorkLoopItem.Get();
                }
                if (_backgroundColorWorkLoopItem.Check())
                {
                    Instance.style.backgroundColor = _backgroundColorWorkLoopItem.Get();
                }
                if (_colorWorkLoopItem.Check())
                {
                    Instance.style.color = _colorWorkLoopItem.Get();
                }
                if (_fontSizeWorkLoopItem.Check())
                {
                    Instance.style.fontSize = _fontSizeWorkLoopItem.Get();
                }
                if (_unityFontWorkLoopItem.Check())
                {
                    Instance.style.unityFont = _unityFontWorkLoopItem.Get();
                }
                if (_unityFontDefinitionWorkLoopItem.Check())
                {
                    Instance.style.unityFontDefinition = _unityFontDefinitionWorkLoopItem.Get();
                }
                if (_transitionPropertyWorkLoopItem.Check())
                {
                    Instance.style.transitionProperty = _transitionPropertyWorkLoopItem.Get();
                }
                if (_transitionDelayWorkLoopItem.Check())
                {
                    Instance.style.transitionDelay = _transitionDelayWorkLoopItem.Get();
                }
                if (_transitionDurationWorkLoopItem.Check())
                {
                    Instance.style.transitionDuration = _transitionDurationWorkLoopItem.Get();
                }
                if (_transitionTimingFunctionWorkLoopItem.Check())
                {
                    Instance.style.transitionTimingFunction = _transitionTimingFunctionWorkLoopItem.Get();
                }
            }
            if (_nameWorkLoopItem.Check())
            {
                Instance.name = _nameWorkLoopItem.Get();
            }
            if (_pickingModeWorkLoopItem.Check())
            {
                Instance.pickingMode = _pickingModeWorkLoopItem.Get();
            }
            if (_classNameWorkLoopItem.Check())
            {
                var names = _classNameWorkLoopItem.Get();

                // Remove previous class names
                for (var i = _previousClassNameList.Count - 1; i >= 0; --i)
                {
                    var name = _previousClassNameList[i];
                    if (!names.Contains(name))
                    {
                        Instance.RemoveFromClassList(name);
                        _previousClassNameList.RemoveAt(i);
                    }
                }

                // Add new ones
                for (var i = 0; names != null && i < names.Count; i++)
                {
                    var name = names[i];

                    if (!_previousClassNameList.Contains(name))
                    {
                        _previousClassNameList.Add(name);
                        Instance.AddToClassList(name);
                    }
                }
            }
        }
    }

    public class UIDocumentNativeNode : GameObjectNativeNode
    {
        private readonly UIDocument _uiDocument;
        private WorkLoopSignalProp<float> _sortingOrderWorkLoopItem;

        public UIDocumentNativeNode(
            UIDocumentComponent component,
            UIDocument uiDocument,
            GameObjectsRendererExtension rendererExtension,
            PanelSettings defaultPanelSettings
        ) : base(component, uiDocument.gameObject, rendererExtension)
        {
            _uiDocument = uiDocument;

            uiDocument.panelSettings = component.PanelSettings ?? defaultPanelSettings;
            if (!component.SortingOrder.IsEmpty)
            {
                uiDocument.sortingOrder = component.SortingOrder.Get();
                _sortingOrderWorkLoopItem = new(component.SortingOrder);
            }
        }

        public override void SetVisible(bool visible)
        {
            // Don't set UIDocument to inactive, since that will destroy the root visual element.
            // See this thread for more info: https://forum.unity.com/threads/does-uidocument-clear-contents-when-disabled.1097659/
            IsVisible_SetByFiber = true;
            UpdateVisibility();
        }

        public override void AddChild(FiberNode node, int index)
        {
            if (node.NativeNode is VisualElementNativeNode visualElementChildNode)
            {
                _uiDocument.rootVisualElement.Insert(index, visualElementChildNode.Instance);
                return;
            }

            base.AddChild(node, index);
        }

        public override void RemoveChild(FiberNode node)
        {
            if (node.NativeNode is VisualElementNativeNode visualElementChildNode)
            {
                _uiDocument.rootVisualElement.Remove(visualElementChildNode.Instance);
                return;
            }

            base.RemoveChild(node);
        }

        public override void MoveChild(FiberNode node, int index)
        {
            if (node.NativeNode is VisualElementNativeNode visualElementChildNode)
            {
                var currentIndex = _uiDocument.rootVisualElement.IndexOf(visualElementChildNode.Instance);
                if (currentIndex > index)
                {
                    var currentElementAtIndex = _uiDocument.rootVisualElement.ElementAt(index);
                    visualElementChildNode.Instance.PlaceBehind(currentElementAtIndex);
                }
                else if (currentIndex < index)
                {
                    var currentElementBeforeIndex = _uiDocument.rootVisualElement.ElementAt(index - 1);
                    visualElementChildNode.Instance.PlaceInFront(currentElementBeforeIndex);
                }
                return;
            }

            base.MoveChild(node, index);
        }

        public override void WorkLoop()
        {
            base.WorkLoop();
            if (_sortingOrderWorkLoopItem.Check())
            {
                _uiDocument.sortingOrder = _sortingOrderWorkLoopItem.Get();
            }
        }
    }

    public class UIElementsRendererExtension : GameObjectsRendererExtension
    {
        public PanelSettings DefaultPanelSettings { get; private set; }

        public UIElementsRendererExtension(
            PanelSettings defaultPanelSettings = null
        )
        : base()
        {
            DefaultPanelSettings = defaultPanelSettings;
        }

        public override NativeNode CreateNativeNode(FiberNode fiberNode)
        {
            var virtualNode = fiberNode.VirtualNode;

            if (virtualNode is UIDocumentComponent uiDocumentComponent)
            {
                var gameObject = GetOrCreateGameObject(fiberNode, uiDocumentComponent);
                if (!gameObject.TryGetComponent<UIDocument>(out var uiDocument))
                {
                    uiDocument = gameObject.AddComponent<UIDocument>();
                }
                return new UIDocumentNativeNode(uiDocumentComponent, uiDocument, this, DefaultPanelSettings);
            }
            else if (virtualNode is ScrollViewComponent scrollViewComponent)
            {
                var scrollView = new ScrollView();
                return new ScrollViewNativeNode(scrollViewComponent, scrollView);
            }
            else if (virtualNode is TextFieldComponent textFieldComponent)
            {
                var textField = new TextField();
                return new TextFieldNativeNode(textFieldComponent, textField);
            }
            else if (virtualNode is TextComponent textComponent)
            {
                var textElement = new TextElement();
                return new TextElementNativeNode(textComponent, textElement);
            }
            else if (virtualNode is ButtonComponent buttonComponent)
            {
                var button = new Button();
                return new ButtonNativeNode(buttonComponent, button);
            }
            else if (virtualNode is ViewComponent viewComponent)
            {
                var visualElement = new VisualElement();
                return new VisualElementNativeNode(viewComponent, visualElement);
            }

            return null;
        }

        public override bool OwnsComponentType(VirtualNode virtualNode)
        {
            var type = virtualNode.GetType();

            return type == typeof(UIDocumentComponent)
                || type == typeof(ScrollViewComponent)
                || type == typeof(TextFieldComponent)
                || type == typeof(TextComponent)
                || type == typeof(ButtonComponent)
                || type == typeof(ViewComponent);
        }
    }
}