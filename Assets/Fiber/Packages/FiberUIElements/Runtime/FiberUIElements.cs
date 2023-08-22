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
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            Ref<ScrollView> scrollRef = null,
            Action<ScrollView> onCreateScrollRef = null,
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
                scrollRef: scrollRef,
                onCreateScrollRef: onCreateScrollRef,
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
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<Button> buttonRef = null,
            Action<Button> onCreateButtonRef = null,
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
                _ref: _ref,
                onCreateRef: onCreateRef,
                onKeyDown: onKeyDown,
                buttonRef: buttonRef,
                onCreateButtonRef: onCreateButtonRef,
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
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            Ref<TextElement> textRef = null,
            Action<TextElement> onCreateTextRef = null,
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
                textRef: textRef,
                onCreateTextRef: onCreateTextRef,
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
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<TextField> textFieldRef = null,
            Action<TextField> onCreateTextFieldRef = null,
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
                _ref: _ref,
                onCreateRef: onCreateRef,
                onKeyDown: onKeyDown,
                textFieldRef: textFieldRef,
                onCreateTextFieldRef: onCreateTextFieldRef,
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
        public Ref<ScrollView> ScrollRef { get; set; }
        public Action<ScrollView> OnCreateScrollRef { get; set; }

        public ScrollViewComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            Ref<ScrollView> scrollRef = null,
            Action<ScrollView> onCreateScrollRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            )
        {
            ScrollRef = scrollRef;
            OnCreateScrollRef = onCreateScrollRef;
        }
    }

    public class ButtonComponent : ViewComponent
    {
        public SignalProp<string> Text { get; private set; }
        public Ref<Button> ButtonRef { get; private set; }
        public Action<Button> OnCreateButtonRef { get; private set; }
        public EventCallback<KeyDownEvent> OnKeyDown { get; private set; }

        public ButtonComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> text = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<Button> buttonRef = null,
            Action<Button> onCreateButtonRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            )
        {
            Text = text;
            ButtonRef = buttonRef;
            OnCreateButtonRef = onCreateButtonRef;
            OnKeyDown = onKeyDown;
        }
    }

    public class TextComponent : ViewComponent
    {
        public SignalProp<string> Text { get; set; }
        public Ref<TextElement> TextRef { get; set; }
        public Action<TextElement> OnCreateTextRef { get; set; }

        public TextComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> text = new(),
            EventCallback<ClickEvent> onClick = null,
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            Ref<TextElement> textRef = null,
            Action<TextElement> onCreateTextRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            )
        {
            Text = text;
            TextRef = textRef;
            OnCreateTextRef = onCreateTextRef;
        }
    }

    public class TextFieldComponent : ViewComponent
    {
        public SignalProp<string> Value { get; set; }
        public EventCallback<ChangeEvent<string>> OnChange { get; set; }
        public EventCallback<KeyDownEvent> OnKeyDown { get; set; }
        public Ref<TextField> TextFieldRef { get; set; }
        public Action<TextField> OnCreateTextFieldRef { get; set; }

        public TextFieldComponent(
            SignalProp<Style> style = new(),
            SignalProp<string> name = new(),
            SignalProp<PickingMode> pickingMode = new(),
            SignalProp<string> value = new(),
            EventCallback<ChangeEvent<string>> onChange = null,
            EventCallback<ClickEvent> onClick = null,
            Ref<VisualElement> _ref = null,
            Action<VisualElement> onCreateRef = null,
            EventCallback<KeyDownEvent> onKeyDown = null,
            Ref<TextField> textFieldRef = null,
            Action<TextField> onCreateTextFieldRef = null,
            SignalProp<List<string>> className = new(),
            List<VirtualNode> children = null
        ) : base(
                style: style,
                name: name,
                pickingMode: !pickingMode.IsEmpty ? pickingMode : UnityEngine.UIElements.PickingMode.Position,
                onClick: onClick,
                _ref: _ref,
                onCreateRef: onCreateRef,
                className: className,
                children: children
            )
        {
            Value = value;
            OnChange = onChange;
            OnKeyDown = onKeyDown;
            TextFieldRef = textFieldRef;
            OnCreateTextFieldRef = onCreateTextFieldRef;
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

            if (virtualNode.TextFieldRef != null) virtualNode.TextFieldRef.Current = instance;
            if (virtualNode.OnCreateTextFieldRef != null) virtualNode.OnCreateTextFieldRef(instance);
            if (!virtualNode.Value.IsEmpty)
            {
                TextFieldElementInstance.value = virtualNode.Value.Get();
                if (virtualNode.Value.IsSignal)
                {
                    virtualNode.Value.Signal.RegisterParent(this);
                }
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

        public override void Cleanup()
        {
            base.Cleanup();

            if (_valueWorkLoopItem.IsSignal)
            {
                _valueWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
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

            if (virtualNode.TextRef != null) virtualNode.TextRef.Current = instance;
            virtualNode.OnCreateTextRef?.Invoke(instance);
            if (!virtualNode.Text.IsEmpty)
            {
                TextElementInstance.text = virtualNode.Text.Get();
                if (virtualNode.Text.IsSignal)
                {
                    virtualNode.Text.Signal.RegisterParent(this);
                }
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

        public override void Cleanup()
        {
            base.Cleanup();

            if (_textWorkLoopItem.IsSignal)
            {
                _textWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
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

            if (virtualNode.ButtonRef != null) virtualNode.ButtonRef.Current = instance;
            if (virtualNode.OnCreateButtonRef != null) virtualNode.OnCreateButtonRef(instance);
            if (!virtualNode.Text.IsEmpty)
            {
                ButtonInstance.text = virtualNode.Text.Get();
                _textWorkLoopItem = new(virtualNode.Text);
                if (virtualNode.Text.IsSignal)
                {
                    virtualNode.Text.Signal.RegisterParent(this);
                }
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

        public override void Cleanup()
        {
            base.Cleanup();

            if (_textWorkLoopItem.IsSignal)
            {
                _textWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
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
        private WorkLoopFontStyleProp _unityFontStyleWorkLoopItem;
        private WorkLoopStyleLengthProp _unityParagraphSpacingWorkLoopItem;
        private WorkLoopTextAnchorProp _unityTextAlignWorkLoopItem;
        private WorkLoopStylePropertyNamesProp _transitionPropertyWorkLoopItem;
        private WorkLoopTimeValuesProp _transitionDelayWorkLoopItem;
        private WorkLoopTimeValuesProp _transitionDurationWorkLoopItem;
        private WorkLoopEasingFunctionsProp _transitionTimingFunctionWorkLoopItem;
        private WorkLoopStyleTransformOriginProp _transformOriginWorkLoopItem;
        private WorkLoopStyleTranslateProp _translateWorkLoopItem;
        private WorkLoopStyleScaleProp _scaleWorkLoopItem;
        private WorkLoopStyleRotateProp _rotateWorkLoopItem;

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
                if (virtualNode.Style.IsSignal)
                {
                    virtualNode.Style.Signal.RegisterParent(this);
                }
                var style = virtualNode.Style.Get();
                _lastStyleFromSignal = style;

                if (!style.Position.IsEmpty)
                {
                    Instance.style.position = style.Position.Get();
                    if (style.Position.IsSignal)
                    {
                        style.Position.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _positionWorkLoopItem = new(style.Position);

                if (!style.Right.IsEmpty)
                {
                    Instance.style.right = style.Right.Get();
                    if (style.Right.IsSignal)
                    {
                        style.Right.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _rightWorkLoopItem = new(style.Right);

                if (!style.Bottom.IsEmpty)
                {
                    Instance.style.bottom = style.Bottom.Get();
                    if (style.Bottom.IsSignal)
                    {
                        style.Bottom.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _bottomWorkLoopItem = new(style.Bottom);

                if (!style.Left.IsEmpty)
                {
                    Instance.style.left = style.Left.Get();
                    if (style.Left.IsSignal)
                    {
                        style.Left.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _leftWorkLoopItem = new(style.Left);

                if (!style.Top.IsEmpty)
                {
                    Instance.style.top = style.Top.Get();
                    if (style.Top.IsSignal)
                    {
                        style.Top.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _topWorkLoopItem = new(style.Top);

                if (!style.PaddingRight.IsEmpty)
                {
                    Instance.style.paddingRight = style.PaddingRight.Get();
                    if (style.PaddingRight.IsSignal)
                    {
                        style.PaddingRight.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _paddingRightWorkLoopItem = new(style.PaddingRight);

                if (!style.PaddingBottom.IsEmpty)
                {
                    Instance.style.paddingBottom = style.PaddingBottom.Get();
                    if (style.PaddingBottom.IsSignal)
                    {
                        style.PaddingBottom.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _paddingBottomWorkLoopItem = new(style.PaddingBottom);

                if (!style.PaddingLeft.IsEmpty)
                {
                    Instance.style.paddingLeft = style.PaddingLeft.Get();
                    if (style.PaddingLeft.IsSignal)
                    {
                        style.PaddingLeft.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _paddingLeftWorkLoopItem = new(style.PaddingLeft);

                if (!style.PaddingTop.IsEmpty)
                {
                    Instance.style.paddingTop = style.PaddingTop.Get();
                    if (style.PaddingTop.IsSignal)
                    {
                        style.PaddingTop.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _paddingTopWorkLoopItem = new(style.PaddingTop);

                if (!style.MarginRight.IsEmpty)
                {
                    Instance.style.marginRight = style.MarginRight.Get();
                    if (style.MarginRight.IsSignal)
                    {
                        style.MarginRight.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _marginRightWorkLoopItem = new(style.MarginRight);

                if (!style.MarginBottom.IsEmpty)
                {
                    Instance.style.marginBottom = style.MarginBottom.Get();
                    if (style.MarginBottom.IsSignal)
                    {
                        style.MarginBottom.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _marginBottomWorkLoopItem = new(style.MarginBottom);

                if (!style.MarginLeft.IsEmpty)
                {
                    Instance.style.marginLeft = style.MarginLeft.Get();
                    if (style.MarginLeft.IsSignal)
                    {
                        style.MarginLeft.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _marginLeftWorkLoopItem = new(style.MarginLeft);

                if (!style.MarginTop.IsEmpty)
                {
                    Instance.style.marginTop = style.MarginTop.Get();
                    if (style.MarginTop.IsSignal)
                    {
                        style.MarginTop.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _marginTopWorkLoopItem = new(style.MarginTop);

                if (!style.BorderTopRightRadius.IsEmpty)
                {
                    Instance.style.borderTopRightRadius = style.BorderTopRightRadius.Get();
                    if (style.BorderTopRightRadius.IsSignal)
                    {
                        style.BorderTopRightRadius.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderTopRightRadiusWorkLoopItem = new(style.BorderTopRightRadius);

                if (!style.BorderTopLeftRadius.IsEmpty)
                {
                    Instance.style.borderTopLeftRadius = style.BorderTopLeftRadius.Get();
                    if (style.BorderTopLeftRadius.IsSignal)
                    {
                        style.BorderTopLeftRadius.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderTopLeftRadiusWorkLoopItem = new(style.BorderTopLeftRadius);

                if (!style.BorderBottomRightRadius.IsEmpty)
                {
                    Instance.style.borderBottomRightRadius = style.BorderBottomRightRadius.Get();
                    if (style.BorderBottomRightRadius.IsSignal)
                    {
                        style.BorderBottomRightRadius.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderBottomRightRadiusWorkLoopItem = new(style.BorderBottomRightRadius);

                if (!style.BorderBottomLeftRadius.IsEmpty)
                {
                    Instance.style.borderBottomLeftRadius = style.BorderBottomLeftRadius.Get();
                    if (style.BorderBottomLeftRadius.IsSignal)
                    {
                        style.BorderBottomLeftRadius.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderBottomLeftRadiusWorkLoopItem = new(style.BorderBottomLeftRadius);

                if (!style.BorderRightWidth.IsEmpty)
                {
                    Instance.style.borderRightWidth = style.BorderRightWidth.Get();
                    if (style.BorderRightWidth.IsSignal)
                    {
                        style.BorderRightWidth.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderRightWidthWorkLoopItem = new(style.BorderRightWidth);

                if (!style.BorderBottomWidth.IsEmpty)
                {
                    Instance.style.borderBottomWidth = style.BorderBottomWidth.Get();
                    if (style.BorderBottomWidth.IsSignal)
                    {
                        style.BorderBottomWidth.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderBottomWidthWorkLoopItem = new(style.BorderBottomWidth);

                if (!style.BorderLeftWidth.IsEmpty)
                {
                    Instance.style.borderLeftWidth = style.BorderLeftWidth.Get();
                    if (style.BorderLeftWidth.IsSignal)
                    {
                        style.BorderLeftWidth.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderLeftWidthWorkLoopItem = new(style.BorderLeftWidth);

                if (!style.BorderTopWidth.IsEmpty)
                {
                    Instance.style.borderTopWidth = style.BorderTopWidth.Get();
                    if (style.BorderTopWidth.IsSignal)
                    {
                        style.BorderTopWidth.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderTopWidthWorkLoopItem = new(style.BorderTopWidth);

                if (!style.BorderRightColor.IsEmpty)
                {
                    Instance.style.borderRightColor = style.BorderRightColor.Get();
                    if (style.BorderRightColor.IsSignal)
                    {
                        style.BorderRightColor.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderRightColorWorkLoopItem = new(style.BorderRightColor);

                if (!style.BorderBottomColor.IsEmpty)
                {
                    Instance.style.borderBottomColor = style.BorderBottomColor.Get();
                    if (style.BorderBottomColor.IsSignal)
                    {
                        style.BorderBottomColor.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderBottomColorWorkLoopItem = new(style.BorderBottomColor);

                if (!style.BorderLeftColor.IsEmpty)
                {
                    Instance.style.borderLeftColor = style.BorderLeftColor.Get();
                    if (style.BorderLeftColor.IsSignal)
                    {
                        style.BorderLeftColor.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderLeftColorWorkLoopItem = new(style.BorderLeftColor);

                if (!style.BorderTopColor.IsEmpty)
                {
                    Instance.style.borderTopColor = style.BorderTopColor.Get();
                    if (style.BorderTopColor.IsSignal)
                    {
                        style.BorderTopColor.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _borderTopColorWorkLoopItem = new(style.BorderTopColor);

                if (!style.Display.IsEmpty)
                {
                    Instance.style.display = style.Display.Get();
                    if (style.Display.IsSignal)
                    {
                        style.Display.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _displayWorkLoopItem = new(style.Display);

                if (!style.FlexShrink.IsEmpty)
                {
                    Instance.style.flexShrink = style.FlexShrink.Get();
                    if (style.FlexShrink.IsSignal)
                    {
                        style.FlexShrink.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _flexShrinkWorkLoopItem = new(style.FlexShrink);

                if (!style.FlexGrow.IsEmpty)
                {
                    Instance.style.flexGrow = style.FlexGrow.Get();
                    if (style.FlexGrow.IsSignal)
                    {
                        style.FlexGrow.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _flexGrowWorkLoopItem = new(style.FlexGrow);

                if (!style.FlexDirection.IsEmpty)
                {
                    Instance.style.flexDirection = style.FlexDirection.Get();
                    if (style.FlexDirection.IsSignal)
                    {
                        style.FlexDirection.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _flexDirectionWorkLoopItem = new(style.FlexDirection);

                if (!style.JustifyContent.IsEmpty)
                {
                    Instance.style.justifyContent = style.JustifyContent.Get();
                    if (style.JustifyContent.IsSignal)
                    {
                        style.JustifyContent.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _justifyContentWorkLoopItem = new(style.JustifyContent);

                if (!style.AlignItems.IsEmpty)
                {
                    Instance.style.alignItems = style.AlignItems.Get();
                    if (style.AlignItems.IsSignal)
                    {
                        style.AlignItems.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _alignItemsWorkLoopItem = new(style.AlignItems);

                if (!style.Width.IsEmpty)
                {
                    Instance.style.width = style.Width.Get();
                    if (style.Width.IsSignal)
                    {
                        style.Width.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _widthWorkLoopItem = new(style.Width);

                if (!style.MaxWidth.IsEmpty)
                {
                    Instance.style.maxWidth = style.MaxWidth.Get();
                    if (style.MaxWidth.IsSignal)
                    {
                        style.MaxWidth.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _maxWidthWorkLoopItem = new(style.MaxWidth);

                if (!style.MinWidth.IsEmpty)
                {
                    Instance.style.minWidth = style.MinWidth.Get();
                    if (style.MinWidth.IsSignal)
                    {
                        style.MinWidth.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _minWidthWorkLoopItem = new(style.MinWidth);

                if (!style.Height.IsEmpty)
                {
                    Instance.style.height = style.Height.Get();
                    if (style.Height.IsSignal)
                    {
                        style.Height.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _heightWorkLoopItem = new(style.Height);

                if (!style.MaxHeight.IsEmpty)
                {
                    Instance.style.maxHeight = style.MaxHeight.Get();
                    if (style.MaxHeight.IsSignal)
                    {
                        style.MaxHeight.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _maxHeightWorkLoopItem = new(style.MaxHeight);

                if (!style.MinHeight.IsEmpty)
                {
                    Instance.style.minHeight = style.MinHeight.Get();
                    if (style.MinHeight.IsSignal)
                    {
                        style.MinHeight.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _minHeightWorkLoopItem = new(style.MinHeight);

                if (!style.BackgroundColor.IsEmpty)
                {
                    Instance.style.backgroundColor = style.BackgroundColor.Get();
                    if (style.BackgroundColor.IsSignal)
                    {
                        style.BackgroundColor.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _backgroundColorWorkLoopItem = new(style.BackgroundColor);

                if (!style.Color.IsEmpty)
                {
                    Instance.style.color = style.Color.Get();
                    if (style.Color.IsSignal)
                    {
                        style.Color.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _colorWorkLoopItem = new(style.Color);

                if (!style.FontSize.IsEmpty)
                {
                    Instance.style.fontSize = style.FontSize.Get();
                    if (style.FontSize.IsSignal)
                    {
                        style.FontSize.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _fontSizeWorkLoopItem = new(style.FontSize);

                if (!style.UnityFont.IsEmpty)
                {
                    Instance.style.unityFont = style.UnityFont.Get();
                    if (style.UnityFont.IsSignal)
                    {
                        style.UnityFont.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _unityFontWorkLoopItem = new(style.UnityFont);

                if (!style.UnityFontDefinition.IsEmpty)
                {
                    Instance.style.unityFontDefinition = style.UnityFontDefinition.Get();
                    if (style.UnityFontDefinition.IsSignal)
                    {
                        style.UnityFontDefinition.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _unityFontDefinitionWorkLoopItem = new(style.UnityFontDefinition);

                if (!style.UnityFontStyle.IsEmpty)
                {
                    Instance.style.unityFontStyleAndWeight = style.UnityFontStyle.Get();
                    if (style.UnityFontStyle.IsSignal)
                    {
                        style.UnityFontStyle.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _unityFontStyleWorkLoopItem = new(style.UnityFontStyle);

                if (!style.UnityParagraphSpacing.IsEmpty)
                {
                    Instance.style.unityParagraphSpacing = style.UnityParagraphSpacing.Get();
                    if (style.UnityParagraphSpacing.IsSignal)
                    {
                        style.UnityParagraphSpacing.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _unityParagraphSpacingWorkLoopItem = new(style.UnityParagraphSpacing);

                if (!style.UnityTextAlign.IsEmpty)
                {
                    Instance.style.unityTextAlign = style.UnityTextAlign.Get();
                    if (style.UnityTextAlign.IsSignal)
                    {
                        style.UnityTextAlign.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _unityTextAlignWorkLoopItem = new(style.UnityTextAlign);

                if (!style.TransitionProperty.IsEmpty)
                {
                    Instance.style.transitionProperty = style.TransitionProperty.Get();
                    if (style.TransitionProperty.IsSignal)
                    {
                        style.TransitionProperty.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _transitionPropertyWorkLoopItem = new(style.TransitionProperty);

                if (!style.TransitionDelay.IsEmpty)
                {
                    Instance.style.transitionDelay = style.TransitionDelay.Get();
                    if (style.TransitionDelay.IsSignal)
                    {
                        style.TransitionDelay.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _transitionDelayWorkLoopItem = new(style.TransitionDelay);

                if (!style.TransitionDuration.IsEmpty)
                {
                    Instance.style.transitionDuration = style.TransitionDuration.Get();
                    if (style.TransitionDuration.IsSignal)
                    {
                        style.TransitionDuration.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _transitionDurationWorkLoopItem = new(style.TransitionDuration);

                if (!style.TransitionTimingFunction.IsEmpty)
                {
                    Instance.style.transitionTimingFunction = style.TransitionTimingFunction.Get();
                    if (style.TransitionTimingFunction.IsSignal)
                    {
                        style.TransitionTimingFunction.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _transitionTimingFunctionWorkLoopItem = new(style.TransitionTimingFunction);

                if (!style.TransformOrigin.IsEmpty)
                {
                    Instance.style.transformOrigin = style.TransformOrigin.Get();
                    if (style.TransformOrigin.IsSignal)
                    {
                        style.TransformOrigin.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _transformOriginWorkLoopItem = new(style.TransformOrigin);

                if (!style.Translate.IsEmpty)
                {
                    Instance.style.translate = style.Translate.Get();
                    if (style.Translate.IsSignal)
                    {
                        style.Translate.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _translateWorkLoopItem = new(style.Translate);

                if (!style.Scale.IsEmpty)
                {
                    Instance.style.scale = style.Scale.Get();
                    if (style.Scale.IsSignal)
                    {
                        style.Scale.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _scaleWorkLoopItem = new(style.Scale);

                if (!style.Rotate.IsEmpty)
                {
                    Instance.style.rotate = style.Rotate.Get();
                    if (style.Rotate.IsSignal)
                    {
                        style.Rotate.SignalProp.Signal.RegisterParent(this);
                    }
                }
                _rotateWorkLoopItem = new(style.Rotate);

            }
            if (!virtualNode.Name.IsEmpty)
            {
                instance.name = virtualNode.Name.Get();
                if (virtualNode.Name.IsSignal)
                {
                    virtualNode.Name.Signal.RegisterParent(this);
                }
                _nameWorkLoopItem = new(virtualNode.Name);
            }
            if (!virtualNode.PickingMode.IsEmpty)
            {
                instance.pickingMode = virtualNode.PickingMode.Get();
                if (virtualNode.PickingMode.IsSignal)
                {
                    virtualNode.PickingMode.Signal.RegisterParent(this);
                }
                _pickingModeWorkLoopItem = new(virtualNode.PickingMode);
            }
            if (virtualNode.OnClick != null)
            {
                instance.RegisterCallback<ClickEvent>(virtualNode.OnClick);
            }

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
                if (virtualNode.ClassName.IsSignal)
                {
                    virtualNode.ClassName.Signal.RegisterParent(this);
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

        private void UpdateStyle()
        {
            // Position
            // Right
            // Bottom
            // Left
            // Top
            // PaddingRight
            // PaddingBottom
            // PaddingLeft
            // PaddingTop
            // MarginRight
            // MarginBottom
            // MarginLeft
            // MarginTop
            // BorderTopRightRadius
            // BorderTopLeftRadius
            // BorderBottomRightRadius
            // BorderBottomLeftRadius
            // BorderRightWidth
            // BorderBottomWidth
            // BorderLeftWidth
            // BorderTopWidth
            // BorderRightColor
            // BorderBottomColor
            // BorderLeftColor
            // BorderTopColor
            // Display
            // FlexShrink
            // FlexGrow
            // FlexDirection
            // JustifyContent
            // AlignItems
            // Width
            // MaxWidth
            // MinWidth
            // Height
            // MaxHeight
            // MinHeight
            // BackgroundColor
            // Color
            // FontSize
            // UnityFont
            // UnityFontDefinition
            // UnityFontStyle
            // UnityParagraphSpacing
            // UnityTextAlign
            // TransitionProperty
            // TransitionDelay
            // TransitionDuration
            // TransitionTimingFunction
            // TransformOrigin
            // Translate
            // Scale
            // Rotate
            if (!_styleWorkLoopItem.IsEmpty)
            {
                if (_styleWorkLoopItem.IsSignal && !_styleWorkLoopItem.Check())
                {
                    return;
                }

                var style = _styleWorkLoopItem.Get();

                // OPEN POINT: We are currently not handling cleaning up signals that are no longer used when
                // the style itself is a signal and a prop goes from being a signal to a regular value

                // Position - Update instance value
                if (style.Position.IsSignal)
                {
                    if (_positionWorkLoopItem.Check())
                    {
                        Instance.style.position = _positionWorkLoopItem.Get();
                    }
                }
                else if (style.Position.IsValue)
                {
                    var value = style.Position.Get();
                    if (Instance.style.position != value)
                    {
                        Instance.style.position = value;
                    }
                }
                else if (style.Position.IsEmpty && !_lastStyleFromSignal.Position.IsEmpty)
                {
                    Instance.style.position = StyleKeyword.Initial;
                }
                // Position - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Position.SignalProp.Signal != style.Position.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Position.IsSignal)
                        {
                            _lastStyleFromSignal.Position.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Position.IsSignal)
                        {
                            style.Position.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Right - Update instance value
                if (style.Right.IsSignal)
                {
                    if (_rightWorkLoopItem.Check())
                    {
                        Instance.style.right = _rightWorkLoopItem.Get();
                    }
                }
                else if (style.Right.IsValue)
                {
                    var value = style.Right.Get();
                    if (Instance.style.right != value)
                    {
                        Instance.style.right = value;
                    }
                }
                else if (style.Right.IsEmpty && !_lastStyleFromSignal.Right.IsEmpty)
                {
                    Instance.style.right = StyleKeyword.Initial;
                }
                // Right - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Right.SignalProp.Signal != style.Right.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Right.IsSignal)
                        {
                            _lastStyleFromSignal.Right.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Right.IsSignal)
                        {
                            style.Right.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Bottom - Update instance value
                if (style.Bottom.IsSignal)
                {
                    if (_bottomWorkLoopItem.Check())
                    {
                        Instance.style.bottom = _bottomWorkLoopItem.Get();
                    }
                }
                else if (style.Bottom.IsValue)
                {
                    var value = style.Bottom.Get();
                    if (Instance.style.bottom != value)
                    {
                        Instance.style.bottom = value;
                    }
                }
                else if (style.Bottom.IsEmpty && !_lastStyleFromSignal.Bottom.IsEmpty)
                {
                    Instance.style.bottom = StyleKeyword.Initial;
                }
                // Bottom - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Bottom.SignalProp.Signal != style.Bottom.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Bottom.IsSignal)
                        {
                            _lastStyleFromSignal.Bottom.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Bottom.IsSignal)
                        {
                            style.Bottom.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Left - Update instance value
                if (style.Left.IsSignal)
                {
                    if (_leftWorkLoopItem.Check())
                    {
                        Instance.style.left = _leftWorkLoopItem.Get();
                    }
                }
                else if (style.Left.IsValue)
                {
                    var value = style.Left.Get();
                    if (Instance.style.left != value)
                    {
                        Instance.style.left = value;
                    }
                }
                else if (style.Left.IsEmpty && !_lastStyleFromSignal.Left.IsEmpty)
                {
                    Instance.style.left = StyleKeyword.Initial;
                }
                // Left - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Left.SignalProp.Signal != style.Left.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Left.IsSignal)
                        {
                            _lastStyleFromSignal.Left.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Left.IsSignal)
                        {
                            style.Left.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Top - Update instance value
                if (style.Top.IsSignal)
                {
                    if (_topWorkLoopItem.Check())
                    {
                        Instance.style.top = _topWorkLoopItem.Get();
                    }
                }
                else if (style.Top.IsValue)
                {
                    var value = style.Top.Get();
                    if (Instance.style.top != value)
                    {
                        Instance.style.top = value;
                    }
                }
                else if (style.Top.IsEmpty && !_lastStyleFromSignal.Top.IsEmpty)
                {
                    Instance.style.top = StyleKeyword.Initial;
                }
                // Top - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Top.SignalProp.Signal != style.Top.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Top.IsSignal)
                        {
                            _lastStyleFromSignal.Top.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Top.IsSignal)
                        {
                            style.Top.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // PaddingRight - Update instance value
                if (style.PaddingRight.IsSignal)
                {
                    if (_paddingRightWorkLoopItem.Check())
                    {
                        Instance.style.paddingRight = _paddingRightWorkLoopItem.Get();
                    }
                }
                else if (style.PaddingRight.IsValue)
                {
                    var value = style.PaddingRight.Get();
                    if (Instance.style.paddingRight != value)
                    {
                        Instance.style.paddingRight = value;
                    }
                }
                else if (style.PaddingRight.IsEmpty && !_lastStyleFromSignal.PaddingRight.IsEmpty)
                {
                    Instance.style.paddingRight = StyleKeyword.Initial;
                }
                // PaddingRight - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.PaddingRight.SignalProp.Signal != style.PaddingRight.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.PaddingRight.IsSignal)
                        {
                            _lastStyleFromSignal.PaddingRight.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.PaddingRight.IsSignal)
                        {
                            style.PaddingRight.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // PaddingBottom - Update instance value
                if (style.PaddingBottom.IsSignal)
                {
                    if (_paddingBottomWorkLoopItem.Check())
                    {
                        Instance.style.paddingBottom = _paddingBottomWorkLoopItem.Get();
                    }
                }
                else if (style.PaddingBottom.IsValue)
                {
                    var value = style.PaddingBottom.Get();
                    if (Instance.style.paddingBottom != value)
                    {
                        Instance.style.paddingBottom = value;
                    }
                }
                else if (style.PaddingBottom.IsEmpty && !_lastStyleFromSignal.PaddingBottom.IsEmpty)
                {
                    Instance.style.paddingBottom = StyleKeyword.Initial;
                }
                // PaddingBottom - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.PaddingBottom.SignalProp.Signal != style.PaddingBottom.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.PaddingBottom.IsSignal)
                        {
                            _lastStyleFromSignal.PaddingBottom.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.PaddingBottom.IsSignal)
                        {
                            style.PaddingBottom.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // PaddingLeft - Update instance value
                if (style.PaddingLeft.IsSignal)
                {
                    if (_paddingLeftWorkLoopItem.Check())
                    {
                        Instance.style.paddingLeft = _paddingLeftWorkLoopItem.Get();
                    }
                }
                else if (style.PaddingLeft.IsValue)
                {
                    var value = style.PaddingLeft.Get();
                    if (Instance.style.paddingLeft != value)
                    {
                        Instance.style.paddingLeft = value;
                    }
                }
                else if (style.PaddingLeft.IsEmpty && !_lastStyleFromSignal.PaddingLeft.IsEmpty)
                {
                    Instance.style.paddingLeft = StyleKeyword.Initial;
                }
                // PaddingLeft - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.PaddingLeft.SignalProp.Signal != style.PaddingLeft.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.PaddingLeft.IsSignal)
                        {
                            _lastStyleFromSignal.PaddingLeft.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.PaddingLeft.IsSignal)
                        {
                            style.PaddingLeft.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // PaddingTop - Update instance value
                if (style.PaddingTop.IsSignal)
                {
                    if (_paddingTopWorkLoopItem.Check())
                    {
                        Instance.style.paddingTop = _paddingTopWorkLoopItem.Get();
                    }
                }
                else if (style.PaddingTop.IsValue)
                {
                    var value = style.PaddingTop.Get();
                    if (Instance.style.paddingTop != value)
                    {
                        Instance.style.paddingTop = value;
                    }
                }
                else if (style.PaddingTop.IsEmpty && !_lastStyleFromSignal.PaddingTop.IsEmpty)
                {
                    Instance.style.paddingTop = StyleKeyword.Initial;
                }
                // PaddingTop - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.PaddingTop.SignalProp.Signal != style.PaddingTop.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.PaddingTop.IsSignal)
                        {
                            _lastStyleFromSignal.PaddingTop.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.PaddingTop.IsSignal)
                        {
                            style.PaddingTop.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MarginRight - Update instance value
                if (style.MarginRight.IsSignal)
                {
                    if (_marginRightWorkLoopItem.Check())
                    {
                        Instance.style.marginRight = _marginRightWorkLoopItem.Get();
                    }
                }
                else if (style.MarginRight.IsValue)
                {
                    var value = style.MarginRight.Get();
                    if (Instance.style.marginRight != value)
                    {
                        Instance.style.marginRight = value;
                    }
                }
                else if (style.MarginRight.IsEmpty && !_lastStyleFromSignal.MarginRight.IsEmpty)
                {
                    Instance.style.marginRight = StyleKeyword.Initial;
                }
                // MarginRight - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MarginRight.SignalProp.Signal != style.MarginRight.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MarginRight.IsSignal)
                        {
                            _lastStyleFromSignal.MarginRight.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MarginRight.IsSignal)
                        {
                            style.MarginRight.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MarginBottom - Update instance value
                if (style.MarginBottom.IsSignal)
                {
                    if (_marginBottomWorkLoopItem.Check())
                    {
                        Instance.style.marginBottom = _marginBottomWorkLoopItem.Get();
                    }
                }
                else if (style.MarginBottom.IsValue)
                {
                    var value = style.MarginBottom.Get();
                    if (Instance.style.marginBottom != value)
                    {
                        Instance.style.marginBottom = value;
                    }
                }
                else if (style.MarginBottom.IsEmpty && !_lastStyleFromSignal.MarginBottom.IsEmpty)
                {
                    Instance.style.marginBottom = StyleKeyword.Initial;
                }
                // MarginBottom - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MarginBottom.SignalProp.Signal != style.MarginBottom.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MarginBottom.IsSignal)
                        {
                            _lastStyleFromSignal.MarginBottom.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MarginBottom.IsSignal)
                        {
                            style.MarginBottom.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MarginLeft - Update instance value
                if (style.MarginLeft.IsSignal)
                {
                    if (_marginLeftWorkLoopItem.Check())
                    {
                        Instance.style.marginLeft = _marginLeftWorkLoopItem.Get();
                    }
                }
                else if (style.MarginLeft.IsValue)
                {
                    var value = style.MarginLeft.Get();
                    if (Instance.style.marginLeft != value)
                    {
                        Instance.style.marginLeft = value;
                    }
                }
                else if (style.MarginLeft.IsEmpty && !_lastStyleFromSignal.MarginLeft.IsEmpty)
                {
                    Instance.style.marginLeft = StyleKeyword.Initial;
                }
                // MarginLeft - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MarginLeft.SignalProp.Signal != style.MarginLeft.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MarginLeft.IsSignal)
                        {
                            _lastStyleFromSignal.MarginLeft.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MarginLeft.IsSignal)
                        {
                            style.MarginLeft.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MarginTop - Update instance value
                if (style.MarginTop.IsSignal)
                {
                    if (_marginTopWorkLoopItem.Check())
                    {
                        Instance.style.marginTop = _marginTopWorkLoopItem.Get();
                    }
                }
                else if (style.MarginTop.IsValue)
                {
                    var value = style.MarginTop.Get();
                    if (Instance.style.marginTop != value)
                    {
                        Instance.style.marginTop = value;
                    }
                }
                else if (style.MarginTop.IsEmpty && !_lastStyleFromSignal.MarginTop.IsEmpty)
                {
                    Instance.style.marginTop = StyleKeyword.Initial;
                }
                // MarginTop - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MarginTop.SignalProp.Signal != style.MarginTop.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MarginTop.IsSignal)
                        {
                            _lastStyleFromSignal.MarginTop.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MarginTop.IsSignal)
                        {
                            style.MarginTop.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderTopRightRadius - Update instance value
                if (style.BorderTopRightRadius.IsSignal)
                {
                    if (_borderTopRightRadiusWorkLoopItem.Check())
                    {
                        Instance.style.borderTopRightRadius = _borderTopRightRadiusWorkLoopItem.Get();
                    }
                }
                else if (style.BorderTopRightRadius.IsValue)
                {
                    var value = style.BorderTopRightRadius.Get();
                    if (Instance.style.borderTopRightRadius != value)
                    {
                        Instance.style.borderTopRightRadius = value;
                    }
                }
                else if (style.BorderTopRightRadius.IsEmpty && !_lastStyleFromSignal.BorderTopRightRadius.IsEmpty)
                {
                    Instance.style.borderTopRightRadius = StyleKeyword.Initial;
                }
                // BorderTopRightRadius - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderTopRightRadius.SignalProp.Signal != style.BorderTopRightRadius.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderTopRightRadius.IsSignal)
                        {
                            _lastStyleFromSignal.BorderTopRightRadius.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderTopRightRadius.IsSignal)
                        {
                            style.BorderTopRightRadius.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderTopLeftRadius - Update instance value
                if (style.BorderTopLeftRadius.IsSignal)
                {
                    if (_borderTopLeftRadiusWorkLoopItem.Check())
                    {
                        Instance.style.borderTopLeftRadius = _borderTopLeftRadiusWorkLoopItem.Get();
                    }
                }
                else if (style.BorderTopLeftRadius.IsValue)
                {
                    var value = style.BorderTopLeftRadius.Get();
                    if (Instance.style.borderTopLeftRadius != value)
                    {
                        Instance.style.borderTopLeftRadius = value;
                    }
                }
                else if (style.BorderTopLeftRadius.IsEmpty && !_lastStyleFromSignal.BorderTopLeftRadius.IsEmpty)
                {
                    Instance.style.borderTopLeftRadius = StyleKeyword.Initial;
                }
                // BorderTopLeftRadius - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderTopLeftRadius.SignalProp.Signal != style.BorderTopLeftRadius.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderTopLeftRadius.IsSignal)
                        {
                            _lastStyleFromSignal.BorderTopLeftRadius.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderTopLeftRadius.IsSignal)
                        {
                            style.BorderTopLeftRadius.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderBottomRightRadius - Update instance value
                if (style.BorderBottomRightRadius.IsSignal)
                {
                    if (_borderBottomRightRadiusWorkLoopItem.Check())
                    {
                        Instance.style.borderBottomRightRadius = _borderBottomRightRadiusWorkLoopItem.Get();
                    }
                }
                else if (style.BorderBottomRightRadius.IsValue)
                {
                    var value = style.BorderBottomRightRadius.Get();
                    if (Instance.style.borderBottomRightRadius != value)
                    {
                        Instance.style.borderBottomRightRadius = value;
                    }
                }
                else if (style.BorderBottomRightRadius.IsEmpty && !_lastStyleFromSignal.BorderBottomRightRadius.IsEmpty)
                {
                    Instance.style.borderBottomRightRadius = StyleKeyword.Initial;
                }
                // BorderBottomRightRadius - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderBottomRightRadius.SignalProp.Signal != style.BorderBottomRightRadius.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderBottomRightRadius.IsSignal)
                        {
                            _lastStyleFromSignal.BorderBottomRightRadius.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderBottomRightRadius.IsSignal)
                        {
                            style.BorderBottomRightRadius.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderBottomLeftRadius - Update instance value
                if (style.BorderBottomLeftRadius.IsSignal)
                {
                    if (_borderBottomLeftRadiusWorkLoopItem.Check())
                    {
                        Instance.style.borderBottomLeftRadius = _borderBottomLeftRadiusWorkLoopItem.Get();
                    }
                }
                else if (style.BorderBottomLeftRadius.IsValue)
                {
                    var value = style.BorderBottomLeftRadius.Get();
                    if (Instance.style.borderBottomLeftRadius != value)
                    {
                        Instance.style.borderBottomLeftRadius = value;
                    }
                }
                else if (style.BorderBottomLeftRadius.IsEmpty && !_lastStyleFromSignal.BorderBottomLeftRadius.IsEmpty)
                {
                    Instance.style.borderBottomLeftRadius = StyleKeyword.Initial;
                }
                // BorderBottomLeftRadius - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderBottomLeftRadius.SignalProp.Signal != style.BorderBottomLeftRadius.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderBottomLeftRadius.IsSignal)
                        {
                            _lastStyleFromSignal.BorderBottomLeftRadius.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderBottomLeftRadius.IsSignal)
                        {
                            style.BorderBottomLeftRadius.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderRightWidth - Update instance value
                if (style.BorderRightWidth.IsSignal)
                {
                    if (_borderRightWidthWorkLoopItem.Check())
                    {
                        Instance.style.borderRightWidth = _borderRightWidthWorkLoopItem.Get();
                    }
                }
                else if (style.BorderRightWidth.IsValue)
                {
                    var value = style.BorderRightWidth.Get();
                    if (Instance.style.borderRightWidth != value)
                    {
                        Instance.style.borderRightWidth = value;
                    }
                }
                else if (style.BorderRightWidth.IsEmpty && !_lastStyleFromSignal.BorderRightWidth.IsEmpty)
                {
                    Instance.style.borderRightWidth = StyleKeyword.Initial;
                }
                // BorderRightWidth - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderRightWidth.SignalProp.Signal != style.BorderRightWidth.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderRightWidth.IsSignal)
                        {
                            _lastStyleFromSignal.BorderRightWidth.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderRightWidth.IsSignal)
                        {
                            style.BorderRightWidth.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderBottomWidth - Update instance value
                if (style.BorderBottomWidth.IsSignal)
                {
                    if (_borderBottomWidthWorkLoopItem.Check())
                    {
                        Instance.style.borderBottomWidth = _borderBottomWidthWorkLoopItem.Get();
                    }
                }
                else if (style.BorderBottomWidth.IsValue)
                {
                    var value = style.BorderBottomWidth.Get();
                    if (Instance.style.borderBottomWidth != value)
                    {
                        Instance.style.borderBottomWidth = value;
                    }
                }
                else if (style.BorderBottomWidth.IsEmpty && !_lastStyleFromSignal.BorderBottomWidth.IsEmpty)
                {
                    Instance.style.borderBottomWidth = StyleKeyword.Initial;
                }
                // BorderBottomWidth - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderBottomWidth.SignalProp.Signal != style.BorderBottomWidth.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderBottomWidth.IsSignal)
                        {
                            _lastStyleFromSignal.BorderBottomWidth.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderBottomWidth.IsSignal)
                        {
                            style.BorderBottomWidth.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderLeftWidth - Update instance value
                if (style.BorderLeftWidth.IsSignal)
                {
                    if (_borderLeftWidthWorkLoopItem.Check())
                    {
                        Instance.style.borderLeftWidth = _borderLeftWidthWorkLoopItem.Get();
                    }
                }
                else if (style.BorderLeftWidth.IsValue)
                {
                    var value = style.BorderLeftWidth.Get();
                    if (Instance.style.borderLeftWidth != value)
                    {
                        Instance.style.borderLeftWidth = value;
                    }
                }
                else if (style.BorderLeftWidth.IsEmpty && !_lastStyleFromSignal.BorderLeftWidth.IsEmpty)
                {
                    Instance.style.borderLeftWidth = StyleKeyword.Initial;
                }
                // BorderLeftWidth - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderLeftWidth.SignalProp.Signal != style.BorderLeftWidth.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderLeftWidth.IsSignal)
                        {
                            _lastStyleFromSignal.BorderLeftWidth.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderLeftWidth.IsSignal)
                        {
                            style.BorderLeftWidth.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderTopWidth - Update instance value
                if (style.BorderTopWidth.IsSignal)
                {
                    if (_borderTopWidthWorkLoopItem.Check())
                    {
                        Instance.style.borderTopWidth = _borderTopWidthWorkLoopItem.Get();
                    }
                }
                else if (style.BorderTopWidth.IsValue)
                {
                    var value = style.BorderTopWidth.Get();
                    if (Instance.style.borderTopWidth != value)
                    {
                        Instance.style.borderTopWidth = value;
                    }
                }
                else if (style.BorderTopWidth.IsEmpty && !_lastStyleFromSignal.BorderTopWidth.IsEmpty)
                {
                    Instance.style.borderTopWidth = StyleKeyword.Initial;
                }
                // BorderTopWidth - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderTopWidth.SignalProp.Signal != style.BorderTopWidth.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderTopWidth.IsSignal)
                        {
                            _lastStyleFromSignal.BorderTopWidth.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderTopWidth.IsSignal)
                        {
                            style.BorderTopWidth.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderRightColor - Update instance value
                if (style.BorderRightColor.IsSignal)
                {
                    if (_borderRightColorWorkLoopItem.Check())
                    {
                        Instance.style.borderRightColor = _borderRightColorWorkLoopItem.Get();
                    }
                }
                else if (style.BorderRightColor.IsValue)
                {
                    var value = style.BorderRightColor.Get();
                    if (Instance.style.borderRightColor != value)
                    {
                        Instance.style.borderRightColor = value;
                    }
                }
                else if (style.BorderRightColor.IsEmpty && !_lastStyleFromSignal.BorderRightColor.IsEmpty)
                {
                    Instance.style.borderRightColor = StyleKeyword.Initial;
                }
                // BorderRightColor - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderRightColor.SignalProp.Signal != style.BorderRightColor.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderRightColor.IsSignal)
                        {
                            _lastStyleFromSignal.BorderRightColor.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderRightColor.IsSignal)
                        {
                            style.BorderRightColor.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderBottomColor - Update instance value
                if (style.BorderBottomColor.IsSignal)
                {
                    if (_borderBottomColorWorkLoopItem.Check())
                    {
                        Instance.style.borderBottomColor = _borderBottomColorWorkLoopItem.Get();
                    }
                }
                else if (style.BorderBottomColor.IsValue)
                {
                    var value = style.BorderBottomColor.Get();
                    if (Instance.style.borderBottomColor != value)
                    {
                        Instance.style.borderBottomColor = value;
                    }
                }
                else if (style.BorderBottomColor.IsEmpty && !_lastStyleFromSignal.BorderBottomColor.IsEmpty)
                {
                    Instance.style.borderBottomColor = StyleKeyword.Initial;
                }
                // BorderBottomColor - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderBottomColor.SignalProp.Signal != style.BorderBottomColor.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderBottomColor.IsSignal)
                        {
                            _lastStyleFromSignal.BorderBottomColor.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderBottomColor.IsSignal)
                        {
                            style.BorderBottomColor.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderLeftColor - Update instance value
                if (style.BorderLeftColor.IsSignal)
                {
                    if (_borderLeftColorWorkLoopItem.Check())
                    {
                        Instance.style.borderLeftColor = _borderLeftColorWorkLoopItem.Get();
                    }
                }
                else if (style.BorderLeftColor.IsValue)
                {
                    var value = style.BorderLeftColor.Get();
                    if (Instance.style.borderLeftColor != value)
                    {
                        Instance.style.borderLeftColor = value;
                    }
                }
                else if (style.BorderLeftColor.IsEmpty && !_lastStyleFromSignal.BorderLeftColor.IsEmpty)
                {
                    Instance.style.borderLeftColor = StyleKeyword.Initial;
                }
                // BorderLeftColor - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderLeftColor.SignalProp.Signal != style.BorderLeftColor.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderLeftColor.IsSignal)
                        {
                            _lastStyleFromSignal.BorderLeftColor.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderLeftColor.IsSignal)
                        {
                            style.BorderLeftColor.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BorderTopColor - Update instance value
                if (style.BorderTopColor.IsSignal)
                {
                    if (_borderTopColorWorkLoopItem.Check())
                    {
                        Instance.style.borderTopColor = _borderTopColorWorkLoopItem.Get();
                    }
                }
                else if (style.BorderTopColor.IsValue)
                {
                    var value = style.BorderTopColor.Get();
                    if (Instance.style.borderTopColor != value)
                    {
                        Instance.style.borderTopColor = value;
                    }
                }
                else if (style.BorderTopColor.IsEmpty && !_lastStyleFromSignal.BorderTopColor.IsEmpty)
                {
                    Instance.style.borderTopColor = StyleKeyword.Initial;
                }
                // BorderTopColor - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BorderTopColor.SignalProp.Signal != style.BorderTopColor.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BorderTopColor.IsSignal)
                        {
                            _lastStyleFromSignal.BorderTopColor.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BorderTopColor.IsSignal)
                        {
                            style.BorderTopColor.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Display - Update instance value
                UpdateDisplayStyle();
                // Display - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Display.SignalProp.Signal != style.Display.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Display.IsSignal)
                        {
                            _lastStyleFromSignal.Display.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Display.IsSignal)
                        {
                            style.Display.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // FlexShrink - Update instance value
                if (style.FlexShrink.IsSignal)
                {
                    if (_flexShrinkWorkLoopItem.Check())
                    {
                        Instance.style.flexShrink = _flexShrinkWorkLoopItem.Get();
                    }
                }
                else if (style.FlexShrink.IsValue)
                {
                    var value = style.FlexShrink.Get();
                    if (Instance.style.flexShrink != value)
                    {
                        Instance.style.flexShrink = value;
                    }
                }
                else if (style.FlexShrink.IsEmpty && !_lastStyleFromSignal.FlexShrink.IsEmpty)
                {
                    Instance.style.flexShrink = StyleKeyword.Initial;
                }
                // FlexShrink - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.FlexShrink.SignalProp.Signal != style.FlexShrink.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.FlexShrink.IsSignal)
                        {
                            _lastStyleFromSignal.FlexShrink.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.FlexShrink.IsSignal)
                        {
                            style.FlexShrink.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }
                // FlexGrow - Update instance value
                if (style.FlexGrow.IsSignal)
                {
                    if (_flexGrowWorkLoopItem.Check())
                    {
                        Instance.style.flexGrow = _flexGrowWorkLoopItem.Get();
                    }
                }
                else if (style.FlexGrow.IsValue)
                {
                    var value = style.FlexGrow.Get();
                    if (Instance.style.flexGrow != value)
                    {
                        Instance.style.flexGrow = value;
                    }
                }
                else if (style.FlexGrow.IsEmpty && !_lastStyleFromSignal.FlexGrow.IsEmpty)
                {
                    Instance.style.flexGrow = StyleKeyword.Initial;
                }
                // FlexGrow - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.FlexGrow.SignalProp.Signal != style.FlexGrow.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.FlexGrow.IsSignal)
                        {
                            _lastStyleFromSignal.FlexGrow.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.FlexGrow.IsSignal)
                        {
                            style.FlexGrow.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // FlexDirection - Update instance value
                if (style.FlexDirection.IsSignal)
                {
                    if (_flexDirectionWorkLoopItem.Check())
                    {
                        Instance.style.flexDirection = _flexDirectionWorkLoopItem.Get();
                    }
                }
                else if (style.FlexDirection.IsValue)
                {
                    var value = style.FlexDirection.Get();
                    if (Instance.style.flexDirection != value)
                    {
                        Instance.style.flexDirection = value;
                    }
                }
                else if (style.FlexDirection.IsEmpty && !_lastStyleFromSignal.FlexDirection.IsEmpty)
                {
                    Instance.style.flexDirection = StyleKeyword.Initial;
                }
                // FlexDirection - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.FlexDirection.SignalProp.Signal != style.FlexDirection.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.FlexDirection.IsSignal)
                        {
                            _lastStyleFromSignal.FlexDirection.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.FlexDirection.IsSignal)
                        {
                            style.FlexDirection.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // JustifyContent - Update instance value
                if (style.JustifyContent.IsSignal)
                {
                    if (_justifyContentWorkLoopItem.Check())
                    {
                        Instance.style.justifyContent = _justifyContentWorkLoopItem.Get();
                    }
                }
                else if (style.JustifyContent.IsValue)
                {
                    var value = style.JustifyContent.Get();
                    if (Instance.style.justifyContent != value)
                    {
                        Instance.style.justifyContent = value;
                    }
                }
                else if (style.JustifyContent.IsEmpty && !_lastStyleFromSignal.JustifyContent.IsEmpty)
                {
                    Instance.style.justifyContent = StyleKeyword.Initial;
                }
                // JustifyContent - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.JustifyContent.SignalProp.Signal != style.JustifyContent.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.JustifyContent.IsSignal)
                        {
                            _lastStyleFromSignal.JustifyContent.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.JustifyContent.IsSignal)
                        {
                            style.JustifyContent.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // AlignItems - Update instance value
                if (style.AlignItems.IsSignal)
                {
                    if (_alignItemsWorkLoopItem.Check())
                    {
                        Instance.style.alignItems = _alignItemsWorkLoopItem.Get();
                    }
                }
                else if (style.AlignItems.IsValue)
                {
                    var value = style.AlignItems.Get();
                    if (Instance.style.alignItems != value)
                    {
                        Instance.style.alignItems = value;
                    }
                }
                else if (style.AlignItems.IsEmpty && !_lastStyleFromSignal.AlignItems.IsEmpty)
                {
                    Instance.style.alignItems = StyleKeyword.Initial;
                }
                // AlignItems - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.AlignItems.SignalProp.Signal != style.AlignItems.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.AlignItems.IsSignal)
                        {
                            _lastStyleFromSignal.AlignItems.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.AlignItems.IsSignal)
                        {
                            style.AlignItems.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Width - Update instance value
                if (style.Width.IsSignal)
                {
                    if (_widthWorkLoopItem.Check())
                    {
                        Instance.style.width = _widthWorkLoopItem.Get();
                    }
                }
                else if (style.Width.IsValue)
                {
                    var value = style.Width.Get();
                    if (Instance.style.width != value)
                    {
                        Instance.style.width = value;
                    }
                }
                else if (style.Width.IsEmpty && !_lastStyleFromSignal.Width.IsEmpty)
                {
                    Instance.style.width = StyleKeyword.Initial;
                }
                // Width - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Width.SignalProp.Signal != style.Width.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Width.IsSignal)
                        {
                            _lastStyleFromSignal.Width.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Width.IsSignal)
                        {
                            style.Width.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MaxWidth - Update instance value
                if (style.MaxWidth.IsSignal)
                {
                    if (_maxWidthWorkLoopItem.Check())
                    {
                        Instance.style.maxWidth = _maxWidthWorkLoopItem.Get();
                    }
                }
                else if (style.MaxWidth.IsValue)
                {
                    var value = style.MaxWidth.Get();
                    if (Instance.style.maxWidth != value)
                    {
                        Instance.style.maxWidth = value;
                    }
                }
                else if (style.MaxWidth.IsEmpty && !_lastStyleFromSignal.MaxWidth.IsEmpty)
                {
                    Instance.style.maxWidth = StyleKeyword.Initial;
                }
                // MaxWidth - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MaxWidth.SignalProp.Signal != style.MaxWidth.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MaxWidth.IsSignal)
                        {
                            _lastStyleFromSignal.MaxWidth.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MaxWidth.IsSignal)
                        {
                            style.MaxWidth.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MinWidth - Update instance value
                if (style.MinWidth.IsSignal)
                {
                    if (_minWidthWorkLoopItem.Check())
                    {
                        Instance.style.minWidth = _minWidthWorkLoopItem.Get();
                    }
                }
                else if (style.MinWidth.IsValue)
                {
                    var value = style.MinWidth.Get();
                    if (Instance.style.minWidth != value)
                    {
                        Instance.style.minWidth = value;
                    }
                }
                else if (style.MinWidth.IsEmpty && !_lastStyleFromSignal.MinWidth.IsEmpty)
                {
                    Instance.style.minWidth = StyleKeyword.Initial;
                }
                // MinWidth - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MinWidth.SignalProp.Signal != style.MinWidth.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MinWidth.IsSignal)
                        {
                            _lastStyleFromSignal.MinWidth.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MinWidth.IsSignal)
                        {
                            style.MinWidth.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Height - Update instance value
                if (style.Height.IsSignal)
                {
                    if (_heightWorkLoopItem.Check())
                    {
                        Instance.style.height = _heightWorkLoopItem.Get();
                    }
                }
                else if (style.Height.IsValue)
                {
                    var value = style.Height.Get();
                    if (Instance.style.height != value)
                    {
                        Instance.style.height = value;
                    }
                }
                else if (style.Height.IsEmpty && !_lastStyleFromSignal.Height.IsEmpty)
                {
                    Instance.style.height = StyleKeyword.Initial;
                }
                // Height - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Height.SignalProp.Signal != style.Height.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Height.IsSignal)
                        {
                            _lastStyleFromSignal.Height.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Height.IsSignal)
                        {
                            style.Height.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MaxHeight - Update instance value
                if (style.MaxHeight.IsSignal)
                {
                    if (_maxHeightWorkLoopItem.Check())
                    {
                        Instance.style.maxHeight = _maxHeightWorkLoopItem.Get();
                    }
                }
                else if (style.MaxHeight.IsValue)
                {
                    var value = style.MaxHeight.Get();
                    if (Instance.style.maxHeight != value)
                    {
                        Instance.style.maxHeight = value;
                    }
                }
                else if (style.MaxHeight.IsEmpty && !_lastStyleFromSignal.MaxHeight.IsEmpty)
                {
                    Instance.style.maxHeight = StyleKeyword.Initial;
                }
                // MaxHeight - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MaxHeight.SignalProp.Signal != style.MaxHeight.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MaxHeight.IsSignal)
                        {
                            _lastStyleFromSignal.MaxHeight.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MaxHeight.IsSignal)
                        {
                            style.MaxHeight.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // MinHeight - Update instance value
                if (style.MinHeight.IsSignal)
                {
                    if (_minHeightWorkLoopItem.Check())
                    {
                        Instance.style.minHeight = _minHeightWorkLoopItem.Get();
                    }
                }
                else if (style.MinHeight.IsValue)
                {
                    var value = style.MinHeight.Get();
                    if (Instance.style.minHeight != value)
                    {
                        Instance.style.minHeight = value;
                    }
                }
                else if (style.MinHeight.IsEmpty && !_lastStyleFromSignal.MinHeight.IsEmpty)
                {
                    Instance.style.minHeight = StyleKeyword.Initial;
                }
                // MinHeight - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.MinHeight.SignalProp.Signal != style.MinHeight.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.MinHeight.IsSignal)
                        {
                            _lastStyleFromSignal.MinHeight.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.MinHeight.IsSignal)
                        {
                            style.MinHeight.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // BackgroundColor - Update instance value
                if (style.BackgroundColor.IsSignal)
                {
                    if (_backgroundColorWorkLoopItem.Check())
                    {
                        Instance.style.backgroundColor = _backgroundColorWorkLoopItem.Get();
                    }
                }
                else if (style.BackgroundColor.IsValue)
                {
                    var value = style.BackgroundColor.Get();
                    if (Instance.style.backgroundColor != value)
                    {
                        Instance.style.backgroundColor = value;
                    }
                }
                else if (style.BackgroundColor.IsEmpty && !_lastStyleFromSignal.BackgroundColor.IsEmpty)
                {
                    Instance.style.backgroundColor = StyleKeyword.Initial;
                }
                // BackgroundColor - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.BackgroundColor.SignalProp.Signal != style.BackgroundColor.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.BackgroundColor.IsSignal)
                        {
                            _lastStyleFromSignal.BackgroundColor.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.BackgroundColor.IsSignal)
                        {
                            style.BackgroundColor.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Color - Update instance value
                if (style.Color.IsSignal)
                {
                    if (_colorWorkLoopItem.Check())
                    {
                        Instance.style.color = _colorWorkLoopItem.Get();
                    }
                }
                else if (style.Color.IsValue)
                {
                    var value = style.Color.Get();
                    if (Instance.style.color != value)
                    {
                        Instance.style.color = value;
                    }
                }
                else if (style.Color.IsEmpty && !_lastStyleFromSignal.Color.IsEmpty)
                {
                    Instance.style.color = StyleKeyword.Initial;
                }
                // Color - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Color.SignalProp.Signal != style.Color.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Color.IsSignal)
                        {
                            _lastStyleFromSignal.Color.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Color.IsSignal)
                        {
                            style.Color.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // FontSize - Update instance value
                if (style.FontSize.IsSignal)
                {
                    if (_fontSizeWorkLoopItem.Check())
                    {
                        Instance.style.fontSize = _fontSizeWorkLoopItem.Get();
                    }
                }
                else if (style.FontSize.IsValue)
                {
                    var value = style.FontSize.Get();
                    if (Instance.style.fontSize != value)
                    {
                        Instance.style.fontSize = value;
                    }
                }
                else if (style.FontSize.IsEmpty && !_lastStyleFromSignal.FontSize.IsEmpty)
                {
                    Instance.style.fontSize = StyleKeyword.Initial;
                }
                // FontSize - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.FontSize.SignalProp.Signal != style.FontSize.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.FontSize.IsSignal)
                        {
                            _lastStyleFromSignal.FontSize.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.FontSize.IsSignal)
                        {
                            style.FontSize.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // UnityFont - Update instance value
                if (style.UnityFont.IsSignal)
                {
                    if (_unityFontWorkLoopItem.Check())
                    {
                        Instance.style.unityFont = _unityFontWorkLoopItem.Get();
                    }
                }
                else if (style.UnityFont.IsValue)
                {
                    var value = style.UnityFont.Get();
                    if (Instance.style.unityFont != value)
                    {
                        Instance.style.unityFont = value;
                    }
                }
                else if (style.UnityFont.IsEmpty && !_lastStyleFromSignal.UnityFont.IsEmpty)
                {
                    Instance.style.unityFont = StyleKeyword.Initial;
                }
                // UnityFont - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.UnityFont.SignalProp.Signal != style.UnityFont.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.UnityFont.IsSignal)
                        {
                            _lastStyleFromSignal.UnityFont.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.UnityFont.IsSignal)
                        {
                            style.UnityFont.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // UnityFontDefinition - Update instance value
                if (style.UnityFontDefinition.IsSignal)
                {
                    if (_unityFontDefinitionWorkLoopItem.Check())
                    {
                        Instance.style.unityFontDefinition = _unityFontDefinitionWorkLoopItem.Get();
                    }
                }
                else if (style.UnityFontDefinition.IsValue)
                {
                    var value = style.UnityFontDefinition.Get();
                    if (Instance.style.unityFontDefinition != value)
                    {
                        Instance.style.unityFontDefinition = value;
                    }
                }
                else if (style.UnityFontDefinition.IsEmpty && !_lastStyleFromSignal.UnityFontDefinition.IsEmpty)
                {
                    Instance.style.unityFontDefinition = StyleKeyword.Initial;
                }
                // UnityFontDefinition - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.UnityFontDefinition.SignalProp.Signal != style.UnityFontDefinition.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.UnityFontDefinition.IsSignal)
                        {
                            _lastStyleFromSignal.UnityFontDefinition.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.UnityFontDefinition.IsSignal)
                        {
                            style.UnityFontDefinition.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // UnityFontStyle - Update instance value
                if (style.UnityFontStyle.IsSignal)
                {
                    if (_unityFontStyleWorkLoopItem.Check())
                    {
                        Instance.style.unityFontStyleAndWeight = _unityFontStyleWorkLoopItem.Get();
                    }
                }
                else if (style.UnityFontStyle.IsValue)
                {
                    var value = style.UnityFontStyle.Get();
                    if (Instance.style.unityFontStyleAndWeight != value)
                    {
                        Instance.style.unityFontStyleAndWeight = value;
                    }
                }
                else if (style.UnityFontStyle.IsEmpty && !_lastStyleFromSignal.UnityFontStyle.IsEmpty)
                {
                    Instance.style.unityFontStyleAndWeight = StyleKeyword.Initial;
                }
                // UnityFontStyle - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.UnityFontStyle.SignalProp.Signal != style.UnityFontStyle.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.UnityFontStyle.IsSignal)
                        {
                            _lastStyleFromSignal.UnityFontStyle.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.UnityFontStyle.IsSignal)
                        {
                            style.UnityFontStyle.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // UnityParagraphSpacing - Update instance value
                if (style.UnityParagraphSpacing.IsSignal)
                {
                    if (_unityParagraphSpacingWorkLoopItem.Check())
                    {
                        Instance.style.unityParagraphSpacing = _unityParagraphSpacingWorkLoopItem.Get();
                    }
                }
                else if (style.UnityParagraphSpacing.IsValue)
                {
                    var value = style.UnityParagraphSpacing.Get();
                    if (Instance.style.unityParagraphSpacing != value)
                    {
                        Instance.style.unityParagraphSpacing = value;
                    }
                }
                else if (style.UnityParagraphSpacing.IsEmpty && !_lastStyleFromSignal.UnityParagraphSpacing.IsEmpty)
                {
                    Instance.style.unityParagraphSpacing = StyleKeyword.Initial;
                }
                // UnityParagraphSpacing - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.UnityParagraphSpacing.SignalProp.Signal != style.UnityParagraphSpacing.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.UnityParagraphSpacing.IsSignal)
                        {
                            _lastStyleFromSignal.UnityParagraphSpacing.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.UnityParagraphSpacing.IsSignal)
                        {
                            style.UnityParagraphSpacing.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // UnityTextAlign - Update instance value
                if (style.UnityTextAlign.IsSignal)
                {
                    if (_unityTextAlignWorkLoopItem.Check())
                    {
                        Instance.style.unityTextAlign = _unityTextAlignWorkLoopItem.Get();
                    }
                }
                else if (style.UnityTextAlign.IsValue)
                {
                    var value = style.UnityTextAlign.Get();
                    if (Instance.style.unityTextAlign != value)
                    {
                        Instance.style.unityTextAlign = value;
                    }
                }
                else if (style.UnityTextAlign.IsEmpty && !_lastStyleFromSignal.UnityTextAlign.IsEmpty)
                {
                    Instance.style.unityTextAlign = StyleKeyword.Initial;
                }
                // UnityTextAlign - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.UnityTextAlign.SignalProp.Signal != style.UnityTextAlign.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.UnityTextAlign.IsSignal)
                        {
                            _lastStyleFromSignal.UnityTextAlign.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.UnityTextAlign.IsSignal)
                        {
                            style.UnityTextAlign.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // TransitionProperty - Update instance value
                if (style.TransitionProperty.IsSignal)
                {
                    if (_transitionPropertyWorkLoopItem.Check())
                    {
                        Instance.style.transitionProperty = _transitionPropertyWorkLoopItem.Get();
                    }
                }
                else if (style.TransitionProperty.IsValue)
                {
                    var value = style.TransitionProperty.Get();
                    if (Instance.style.transitionProperty != value)
                    {
                        Instance.style.transitionProperty = value;
                    }
                }
                else if (style.TransitionProperty.IsEmpty && !_lastStyleFromSignal.TransitionProperty.IsEmpty)
                {
                    Instance.style.transitionProperty = StyleKeyword.Initial;
                }
                // TransitionProperty - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.TransitionProperty.SignalProp.Signal != style.TransitionProperty.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.TransitionProperty.IsSignal)
                        {
                            _lastStyleFromSignal.TransitionProperty.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.TransitionProperty.IsSignal)
                        {
                            style.TransitionProperty.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // TransitionDelay - Update instance value
                if (style.TransitionDelay.IsSignal)
                {
                    if (_transitionDelayWorkLoopItem.Check())
                    {
                        Instance.style.transitionDelay = _transitionDelayWorkLoopItem.Get();
                    }
                }
                else if (style.TransitionDelay.IsValue)
                {
                    var value = style.TransitionDelay.Get();
                    if (Instance.style.transitionDelay != value)
                    {
                        Instance.style.transitionDelay = value;
                    }
                }
                else if (style.TransitionDelay.IsEmpty && !_lastStyleFromSignal.TransitionDelay.IsEmpty)
                {
                    Instance.style.transitionDelay = StyleKeyword.Initial;
                }
                // TransitionDelay - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.TransitionDelay.SignalProp.Signal != style.TransitionDelay.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.TransitionDelay.IsSignal)
                        {
                            _lastStyleFromSignal.TransitionDelay.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.TransitionDelay.IsSignal)
                        {
                            style.TransitionDelay.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // TransitionDuration - Update instance value
                if (style.TransitionDuration.IsSignal)
                {
                    if (_transitionDurationWorkLoopItem.Check())
                    {
                        Instance.style.transitionDuration = _transitionDurationWorkLoopItem.Get();
                    }
                }
                else if (style.TransitionDuration.IsValue)
                {
                    var value = style.TransitionDuration.Get();
                    if (Instance.style.transitionDuration != value)
                    {
                        Instance.style.transitionDuration = value;
                    }
                }
                else if (style.TransitionDuration.IsEmpty && !_lastStyleFromSignal.TransitionDuration.IsEmpty)
                {
                    Instance.style.transitionDuration = StyleKeyword.Initial;
                }
                // TransitionDuration - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.TransitionDuration.SignalProp.Signal != style.TransitionDuration.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.TransitionDuration.IsSignal)
                        {
                            _lastStyleFromSignal.TransitionDuration.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.TransitionDuration.IsSignal)
                        {
                            style.TransitionDuration.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // TransitionTimingFunction - Update instance value
                if (style.TransitionTimingFunction.IsSignal)
                {
                    if (_transitionTimingFunctionWorkLoopItem.Check())
                    {
                        Instance.style.transitionTimingFunction = _transitionTimingFunctionWorkLoopItem.Get();
                    }
                }
                else if (style.TransitionTimingFunction.IsValue)
                {
                    var value = style.TransitionTimingFunction.Get();
                    if (Instance.style.transitionTimingFunction != value)
                    {
                        Instance.style.transitionTimingFunction = value;
                    }
                }
                else if (style.TransitionTimingFunction.IsEmpty && !_lastStyleFromSignal.TransitionTimingFunction.IsEmpty)
                {
                    Instance.style.transitionTimingFunction = StyleKeyword.Initial;
                }
                // TransitionTimingFunction - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.TransitionTimingFunction.SignalProp.Signal != style.TransitionTimingFunction.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.TransitionTimingFunction.IsSignal)
                        {
                            _lastStyleFromSignal.TransitionTimingFunction.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.TransitionTimingFunction.IsSignal)
                        {
                            style.TransitionTimingFunction.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // TransformOrigin - Update instance value
                if (style.TransformOrigin.IsSignal)
                {
                    if (_transformOriginWorkLoopItem.Check())
                    {
                        Instance.style.transformOrigin = _transformOriginWorkLoopItem.Get();
                    }
                }
                else if (style.TransformOrigin.IsValue)
                {
                    var value = style.TransformOrigin.Get();
                    if (Instance.style.transformOrigin != value)
                    {
                        Instance.style.transformOrigin = value;
                    }
                }
                else if (style.TransformOrigin.IsEmpty && !_lastStyleFromSignal.TransformOrigin.IsEmpty)
                {
                    Instance.style.transformOrigin = StyleKeyword.Initial;
                }
                // TransformOrigin - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.TransformOrigin.SignalProp.Signal != style.TransformOrigin.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.TransformOrigin.IsSignal)
                        {
                            _lastStyleFromSignal.TransformOrigin.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.TransformOrigin.IsSignal)
                        {
                            style.TransformOrigin.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Translate - Update instance value
                if (style.Translate.IsSignal)
                {
                    if (_translateWorkLoopItem.Check())
                    {
                        Instance.style.translate = _translateWorkLoopItem.Get();
                    }
                }
                else if (style.Translate.IsValue)
                {
                    var value = style.Translate.Get();
                    if (Instance.style.translate != value)
                    {
                        Instance.style.translate = value;
                    }
                }
                else if (style.Translate.IsEmpty && !_lastStyleFromSignal.Translate.IsEmpty)
                {
                    Instance.style.translate = StyleKeyword.Initial;
                }
                // Translate - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Translate.SignalProp.Signal != style.Translate.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Translate.IsSignal)
                        {
                            _lastStyleFromSignal.Translate.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Translate.IsSignal)
                        {
                            style.Translate.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Scale - Update instance value
                if (style.Scale.IsSignal)
                {
                    if (_scaleWorkLoopItem.Check())
                    {
                        Instance.style.scale = _scaleWorkLoopItem.Get();
                    }
                }
                else if (style.Scale.IsValue)
                {
                    var value = style.Scale.Get();
                    if (Instance.style.scale != value)
                    {
                        Instance.style.scale = value;
                    }
                }
                else if (style.Scale.IsEmpty && !_lastStyleFromSignal.Scale.IsEmpty)
                {
                    Instance.style.scale = StyleKeyword.Initial;
                }
                // Scale - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Scale.SignalProp.Signal != style.Scale.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Scale.IsSignal)
                        {
                            _lastStyleFromSignal.Scale.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Scale.IsSignal)
                        {
                            style.Scale.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }

                // Rotate - Update instance value
                if (style.Rotate.IsSignal)
                {
                    if (_rotateWorkLoopItem.Check())
                    {
                        Instance.style.rotate = _rotateWorkLoopItem.Get();
                    }
                }
                else if (style.Rotate.IsValue)
                {
                    var value = style.Rotate.Get();
                    if (Instance.style.rotate != value)
                    {
                        Instance.style.rotate = value;
                    }
                }
                else if (style.Rotate.IsEmpty && !_lastStyleFromSignal.Rotate.IsEmpty)
                {
                    Instance.style.rotate = StyleKeyword.Initial;
                }
                // Rotate - Register / unregister dependant signals
                if (_styleWorkLoopItem.IsSignal)
                {
                    if (_lastStyleFromSignal.Rotate.SignalProp.Signal != style.Rotate.SignalProp.Signal)
                    {
                        if (_lastStyleFromSignal.Rotate.IsSignal)
                        {
                            _lastStyleFromSignal.Rotate.SignalProp.Signal.UnregisterParent(this);
                        }
                        if (style.Rotate.IsSignal)
                        {
                            style.Rotate.SignalProp.Signal.RegisterParent(this);
                        }
                    }
                }


                _lastStyleFromSignal = style;
            }
        }

        public override void WorkLoop()
        {
            UpdateStyle();

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

        public override void Cleanup()
        {
            // start style
            if (_styleWorkLoopItem.IsSignal)
            {
                _styleWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
            }
            if (_positionWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _positionWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_rightWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _rightWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_bottomWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _bottomWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_leftWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _leftWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_topWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _topWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_paddingRightWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _paddingRightWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_paddingBottomWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _paddingBottomWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_paddingLeftWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _paddingLeftWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_paddingTopWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _paddingTopWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_marginRightWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _marginRightWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_marginBottomWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _marginBottomWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_marginLeftWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _marginLeftWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_marginTopWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _marginTopWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderTopRightRadiusWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderTopRightRadiusWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderTopLeftRadiusWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderTopLeftRadiusWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderBottomRightRadiusWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderBottomRightRadiusWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderBottomLeftRadiusWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderBottomLeftRadiusWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderRightWidthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderRightWidthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderBottomWidthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderBottomWidthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderLeftWidthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderLeftWidthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderTopWidthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderTopWidthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderRightColorWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderRightColorWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderBottomColorWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderBottomColorWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderLeftColorWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderLeftColorWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_borderTopColorWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _borderTopColorWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_displayWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _displayWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_flexShrinkWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _flexShrinkWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_flexGrowWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _flexGrowWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_flexDirectionWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _flexDirectionWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_justifyContentWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _justifyContentWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_alignItemsWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _alignItemsWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_widthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _widthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_maxWidthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _maxWidthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_minWidthWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _minWidthWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_heightWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _heightWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_maxHeightWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _maxHeightWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_minHeightWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _minHeightWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_backgroundColorWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _backgroundColorWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_colorWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _colorWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_fontSizeWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _fontSizeWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_unityFontWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _unityFontWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_unityFontDefinitionWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _unityFontDefinitionWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_unityFontStyleWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _unityFontStyleWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_unityParagraphSpacingWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _unityParagraphSpacingWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_unityTextAlignWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _unityTextAlignWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_transitionPropertyWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _transitionPropertyWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_transitionDelayWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _transitionDelayWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_transitionDurationWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _transitionDurationWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_transitionTimingFunctionWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _transitionTimingFunctionWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_transformOriginWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _transformOriginWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_translateWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _translateWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_scaleWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _scaleWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            if (_rotateWorkLoopItem.WorkLoopSignalProp.IsSignal)
            {
                _rotateWorkLoopItem.WorkLoopSignalProp.SignalProp.Signal.UnregisterParent(this);
            }
            // end style
            if (_nameWorkLoopItem.IsSignal)
            {
                _nameWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
            }
            if (_pickingModeWorkLoopItem.IsSignal)
            {
                _pickingModeWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
            }
            if (_classNameWorkLoopItem.IsSignal)
            {
                _classNameWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
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
                if (component.SortingOrder.IsSignal)
                {
                    component.SortingOrder.Signal.RegisterParent(this);
                }
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

        public override void Cleanup()
        {
            base.Cleanup();

            if (_sortingOrderWorkLoopItem.IsSignal)
            {
                _sortingOrderWorkLoopItem.SignalProp.Signal.UnregisterParent(this);
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