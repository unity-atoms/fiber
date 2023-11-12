using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static DividerComponent Divider(
            this BaseComponent component,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new()
        )
        {
            return new DividerComponent(
                role: role,
                subRole: subRole,
                variant: variant
            );
        }
    }


    public class DividerComponent : BaseComponent
    {
        private readonly string _role;
        private readonly string _subRole;
        private readonly SignalProp<string> _variant;

        public DividerComponent(
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new()
        ) : base()
        {
            _role = role;
            _variant = variant;
        }

        public override VirtualBody Render()
        {
            var themeStore = C<ThemeStore>();
            var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);
            var shine = themeStore.Color(role, elementType: ElementType.Gloom, subRole: subRole, variant: _variant);
            var gloom = themeStore.Color(role, elementType: ElementType.Shine, subRole: subRole, variant: _variant);

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