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
            Ref<VisualElement> forwardRef = null
        )
        {
            return new SilkChipComponent(
                label: label,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style,
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
        private readonly Ref<VisualElement> _forwardRef;

        public SilkChipComponent(
            SignalProp<string> label,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new(),
            Ref<VisualElement> forwardRef = null
        ) : base()
        {
            _label = label;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
            _forwardRef = forwardRef;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

            var backgroundColor = themeStore.Color(role, ElementType.Background, subRole, _variant);
            var borderColor = themeStore.Color(role, ElementType.Border, subRole, _variant);

            return F.SilkTypography(
                type: TypographyType.Body2,
                text: _label,
                role: role,
                subRole: subRole,
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
                    paddingLeft: themeStore.Spacing(1.5f),
                    paddingRight: themeStore.Spacing(1.5f),
                    paddingTop: themeStore.Spacing(0.5f),
                    paddingBottom: themeStore.Spacing(0.5f)
                ),
                forwardRef: _forwardRef
            );
        }
    }
}