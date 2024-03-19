using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Fiber.InteractiveUI;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkDividerComponent SilkDivider(
            this BaseComponent component,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            InteractiveElement interactiveElement = null
        )
        {
            return new SilkDividerComponent(
                role: role,
                subRole: subRole,
                variant: variant,
                interactiveElement: interactiveElement
            );
        }
    }


    public class SilkDividerComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;
        private readonly InteractiveElement _interactiveElement;

        public SilkDividerComponent(
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            InteractiveElement interactiveElement = null
        ) : base()
        {
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _interactiveElement = interactiveElement;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);
            var shine = themeStore.Color(role: role, elementType: ElementType.Gloom, interactiveElement: _interactiveElement, subRole: subRole, variant: _variant);
            var gloom = themeStore.Color(role: role, elementType: ElementType.Shine, interactiveElement: _interactiveElement, subRole: subRole, variant: _variant);

            return F.Nodes(
                F.View(
                    style: new Style(
                        width: new Length(100, LengthUnit.Percent),
                        height: new Length(1, LengthUnit.Pixel),
                        backgroundColor: shine
                    )
                ),
                F.View(
                    style: new Style(
                        width: new Length(100, LengthUnit.Percent),
                        height: new Length(1, LengthUnit.Pixel),
                        backgroundColor: gloom
                    )
                )
            );
        }
    }
}