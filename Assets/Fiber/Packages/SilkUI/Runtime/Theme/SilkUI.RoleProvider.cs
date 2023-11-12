using Fiber;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static string ResolveRole(this BaseComponent component, string role)
        {
            if (role == THEME_CONSTANTS.INHERIT)
            {
                var fromContext = component.GetContext<RoleContext>(throwIfNotFound: false);
                return fromContext?.InheritedRole ?? component.C<ThemeStore>().Get().FallbackRole;
            }

            return role;
        }

        public static string ResolveSubRole(this BaseComponent component, string subRole)
        {
            if (subRole == THEME_CONSTANTS.INHERIT)
            {
                var fromContext = component.GetContext<RoleContext>(throwIfNotFound: false);
                return fromContext?.InheritedSubRole ?? null;
            }

            return subRole;
        }

        public static (string, string) ResolveRoleAndSubRole(this BaseComponent component, string role, string subRole)
        {
            var resolvedRole = component.ResolveRole(role);
            var resolvedSubRole = component.ResolveSubRole(subRole);

            return (resolvedRole, resolvedSubRole);
        }

        public static RoleProvider RoleProvider(
            this BaseComponent component,
            string role,
            VirtualBody children,
            string subRole = null
        )
        {
            return new RoleProvider(
                role: role,
                subRole: subRole,
                children: children
            );
        }
    }

    public class RoleContext
    {
        public string InheritedRole;
        public string InheritedSubRole;

        public RoleContext(string role, string subRole)
        {
            InheritedRole = role == THEME_CONSTANTS.INHERIT ? null : role;
            InheritedSubRole = subRole == THEME_CONSTANTS.INHERIT ? null : subRole;
        }
    }

    public class RoleProvider : BaseComponent
    {
        public RoleContext _roleContext;
        public RoleProvider(
            string role,
            VirtualBody children,
            string subRole = null
        ) : base(children)
        {
            _roleContext = new(role, subRole);
        }

        public override VirtualBody Render()
        {
            return F.ContextProvider(
                value: _roleContext,
                children: Children
            );
        }
    }
}