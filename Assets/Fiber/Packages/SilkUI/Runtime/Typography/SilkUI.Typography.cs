using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkTypographyComponent SilkTypography(
            this BaseComponent component,
            SignalProp<TypographyType> type,
            SignalProp<string> text = new(),
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            return new SilkTypographyComponent(
                type: type,
                text: text,
                role: role,
                variant: variant,
                style: style,
                forwardRef: forwardRef
            );
        }
    }
    public class SilkTypographyComponent : BaseComponent
    {
        private readonly SignalProp<TypographyType> _type;
        private SignalProp<string> _text;
        private readonly string _role;
        private readonly string _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;

        public SilkTypographyComponent(
            SignalProp<TypographyType> type,
            SignalProp<string> text = new(),
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        )
        {
            _type = type;
            _text = text;
            _role = role;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
        }

        public override VirtualNode Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateTypography != null)
            {
                return overrideVisualComponents.CreateTypography(
                    type: _type,
                    text: _text,
                    role: _role,
                    variant: _variant,
                    style: _style,
                    forwardRef: _forwardRef
                );
            }

            var themeStore = C<ThemeStore>();
            var role = F.ResolveRole(_role);

            return F.Text(
                _ref: _forwardRef,
                text: _text,
                style: new Style(
                    mergedStyle: _style,
                    unityFont: themeStore.Font(_type),
                    unityFontDefinition: StyleKeyword.None,
                    fontSize: themeStore.FontSize(_type),
                    unityFontStyle: themeStore.FontStyle(_type),
                    color: themeStore.Color(role, ElementType.Text, _variant)
                )
            );
        }
    }
}