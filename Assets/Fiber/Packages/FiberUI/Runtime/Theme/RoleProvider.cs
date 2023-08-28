using System.Collections.Generic;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static string ResolveRole(this BaseComponent component, string role)
        {
            var theme = component.C<ThemeStore>().Get();
            if (role == Constants.INHERIT_ROLE)
            {
                try
                {
                    return component.GetContext<RoleContext>().Value;
                }
                catch
                {
                    return theme.FallbackRole;
                }
            }

            return theme.Color.ContainsKey(role) ? role : theme.FallbackRole;
        }

        public static RoleProvider RoleProvider(
            this BaseComponent component,
            string role,
            List<VirtualNode> children
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
        public string Value;

        public RoleContext(string value)
        {
            Value = value;
        }
    }

    public class RoleProvider : BaseComponent
    {
        public RoleContext _roleContext;
        public RoleProvider(
            string role,
            List<VirtualNode> children
        ) : base(children)
        {
            _roleContext = new(role);
        }

        public override VirtualNode Render()
        {
            return F.ContextProvider(
                value: _roleContext,
                children: children
            );
        }
    }
}