using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;
using Fiber.InteractiveUI;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkTypographyComponent SilkTypography(
            this BaseComponent component,
            SignalProp<TypographyType> type,
            SignalProp<string> text = new(),
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null,
            InteractiveElement interactiveElement = null
        )
        {
            return new SilkTypographyComponent(
                type: type,
                text: text,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
                forwardRef: forwardRef,
                interactiveElement: interactiveElement
            );
        }
    }
    public class SilkTypographyComponent : BaseComponent
    {
        private readonly SignalProp<TypographyType> _type;
        private readonly SignalProp<string> _text;
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly Style _style;
        private readonly Ref<VisualElement> _forwardRef;
        private readonly InteractiveElement _interactiveElement;

        public SilkTypographyComponent(
            SignalProp<TypographyType> type,
            SignalProp<string> text = new(),
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null,
            InteractiveElement interactiveElement = null
        )
        {
            _type = type;
            _text = text;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
            _interactiveElement = interactiveElement;
        }

        public override VirtualBody Render()
        {
            var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
            if (overrideVisualComponents?.CreateTypography != null)
            {
                return overrideVisualComponents.CreateTypography(
                    type: _type,
                    text: _text,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant,
                    style: _style,
                    forwardRef: _forwardRef,
                    interactiveElement: _interactiveElement
                );
            }

            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            return F.Text(
                _ref: _forwardRef,
                text: _text,
                style: new Style(
                    mergedStyle: _style,
                    unityFont: themeStore.Font(_type),
                    unityFontDefinition: StyleKeyword.None,
                    fontSize: themeStore.FontSize(_type),
                    unityFontStyle: themeStore.FontStyle(_type),
                    color: themeStore.Color(role, ElementType.Text, subRole: subRole, _variant),
                    unityTextOutlineWidth: themeStore.OutlineWidth(_type),
                    unityTextOutlineColor: themeStore.Color(role: role, elementType: ElementType.TextOutline, interactiveElement: _interactiveElement, subRole: subRole, variant: _variant)
                )
            );
        }
    }
}