using Fiber;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static string ResolveRole(this BaseComponent component, string role)
        {
            var theme = component.C<ThemeStore>().Get();
            if (role == THEME_CONSTANTS.INHERIT)
            {
                var fromContext = component.GetContext<RoleContext>(throwIfNotFound: false);
                return fromContext?.InheritedRole ?? theme.FallbackRole;
            }

            return theme.Color.ContainsKey(role) ? role : theme.FallbackRole;
        }

        public static RoleProvider RoleProvider(
            this BaseComponent component,
            string role,
            VirtualBody children
        )
        {
            return new RoleProvider(
                role: role,
                children: children
            );
        }
    }

    public class RoleContext
    {
        public string InheritedRole;

        public RoleContext(string role)
        {
            InheritedRole = role;
        }
    }

    public class RoleProvider : BaseComponent
    {
        public RoleContext _roleContext;
        public RoleProvider(
            string role,
            VirtualBody children
        ) : base(children)
        {
            _roleContext = new(role);
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