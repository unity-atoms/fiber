using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
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
            Ref<VisualElement> forwardRef = null
        )
        {
            return new SilkChipComponent(
                label: label,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
                icon: icon,
                forwardRef: forwardRef
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

        public SilkChipComponent(
            SignalProp<string> label = new(),
            VirtualBody children = default,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            SignalProp<string> icon = new(),
            Ref<VisualElement> forwardRef = null
        ) : base(children)
        {
            _label = label;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _icon = icon;
            _forwardRef = forwardRef;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            var backgroundColor = themeStore.Color(role, ElementType.Background, subRole, _variant);
            var borderColor = themeStore.Color(role, ElementType.Border, subRole, _variant);

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
                    borderTopRightRadius: themeStore.Spacing(3),
                    borderTopLeftRadius: themeStore.Spacing(3),
                    borderBottomRightRadius: themeStore.Spacing(3),
                    borderBottomLeftRadius: themeStore.Spacing(3),
                    paddingLeft: themeStore.Spacing(0.5f),
                    paddingRight: themeStore.Spacing(0.5f),
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
                        )
                    ) : null,
                    !_label.IsEmpty ? F.SilkTypography(
                        type: TypographyType.Body2,
                        text: _label,
                        role: role,
                        subRole: subRole,
                        variant: _variant
                    ) : null,
                    F.Fragment(Children)
                )
            );
        }
    }
}