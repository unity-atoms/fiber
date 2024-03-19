using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.InteractiveUI;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkChipComponent SilkChip(
            this BaseComponent component,
            SignalProp<string> label,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            SignalProp<string> icon = new(),
            Ref<VisualElement> forwardRef = null,
            InteractiveElement interactiveElement = null
        )
        {
            return new SilkChipComponent(
                label: label,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
                icon: icon,
                forwardRef: forwardRef,
                interactiveElement: interactiveElement
            );
        }
    }


    public class SilkChipComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly SignalProp<string> _label;
        private readonly Style _style;
        private readonly SignalProp<string> _icon;
        private readonly Ref<VisualElement> _forwardRef;
        private readonly InteractiveElement _interactiveElement;

        public SilkChipComponent(
            SignalProp<string> label = new(),
            VirtualBody children = default,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            SignalProp<string> icon = new(),
            Ref<VisualElement> forwardRef = null,
            InteractiveElement interactiveElement = null
        ) : base(children)
        {
            _label = label;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _icon = icon;
            _forwardRef = forwardRef;
            _interactiveElement = interactiveElement;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            var backgroundColor = themeStore.Color(role: role, elementType: ElementType.Background, interactiveElement: _interactiveElement, subRole: subRole, variant: _variant);
            var borderColor = themeStore.Color(role: role, elementType: ElementType.Border, subRole: subRole, variant: _variant);

            return F.View(
                style: new Style(
                    mergedStyle: _style,
                    backgroundColor: backgroundColor,
                    borderTopColor: borderColor,
                    borderRightColor: borderColor,
                    borderBottomColor: borderColor,
                    borderLeftColor: borderColor,
                    borderTopWidth: themeStore.BorderWidth(),
                    borderRightWidth: themeStore.BorderWidth(),
                    borderBottomWidth: themeStore.BorderWidth(),
                    borderLeftWidth: themeStore.BorderWidth(),
                    borderTopRightRadius: themeStore.Spacing(2.5f),
                    borderTopLeftRadius: themeStore.Spacing(2.5f),
                    borderBottomRightRadius: themeStore.Spacing(2.5f),
                    borderBottomLeftRadius: themeStore.Spacing(2.5f),
                    paddingLeft: themeStore.Spacing(1f),
                    paddingRight: themeStore.Spacing(1f),
                    paddingTop: themeStore.Spacing(0f),
                    paddingBottom: themeStore.Spacing(0f),
                    display: DisplayStyle.Flex,
                    flexDirection: FlexDirection.Row
                ),
                _ref: _forwardRef,
                children: F.Nodes(
                    !_icon.IsEmpty ? F.SilkIcon(
                        iconName: _icon,
                        size: IconSize.Small,
                        role: role,
                        subRole: subRole,
                        variant: _variant,
                        style: new Style(
                            marginRight: themeStore.Spacing(0.5f)
                        ),
                        interactiveElement: _interactiveElement
                    ) : null,
                    !_label.IsEmpty ? F.SilkTypography(
                        type: TypographyType.Body2,
                        text: _label,
                        role: role,
                        subRole: subRole,
                        variant: _variant,
                        interactiveElement: _interactiveElement
                    ) : null,
                    F.Fragment(Children)
                )
            );
        }
    }
}