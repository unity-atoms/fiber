using System.Collections.Generic;
using UnityEngine;
using Fiber;

namespace SilkUI
{
    public static class THEME_CONSTANTS
    {
        public const string INHERIT_ROLE = "inherit";
    }

    public static class FIBER_COLOR_PALETTE
    {
        public static readonly Color BLACK_ALPHA_100 = new(0, 0, 0, 1);
        public static readonly Color BLACK_ALPHA_90 = new(0, 0, 0, 0.9f);
        public static readonly Color BLACK_ALPHA_80 = new(0, 0, 0, 0.8f);
        public static readonly Color BLACK_ALPHA_70 = new(0, 0, 0, 0.7f);
        public static readonly Color BLACK_ALPHA_60 = new(0, 0, 0, 0.6f);
        public static readonly Color BLACK_ALPHA_50 = new(0, 0, 0, 0.5f);
        public static readonly Color BLACK_ALPHA_40 = new(0, 0, 0, 0.4f);
        public static readonly Color BLACK_ALPHA_30 = new(0, 0, 0, 0.3f);
        public static readonly Color BLACK_ALPHA_20 = new(0, 0, 0, 0.2f);
        public static readonly Color BLACK_ALPHA_10 = new(0, 0, 0, 0.1f);
        public static readonly Color BLACK_ALPHA_0 = new(0, 0, 0, 0);


        public const string GRAY_0 = "#000000";
        public const string GRAY_7 = "#111111";
        public const string GRAY_13 = "#222222";
        public const string GRAY_20 = "#333333";
        public const string GRAY_27 = "#444444";
        public const string GRAY_33 = "#555555";
        public const string GRAY_40 = "#666666";
        public const string GRAY_47 = "#777777";
        public const string GRAY_53 = "#888888";
        public const string GRAY_60 = "#999999";
        public const string GRAY_67 = "#aaaaaa";
        public const string GRAY_73 = "#bbbbbb";
        public const string GRAY_80 = "#cccccc";
        public const string GRAY_87 = "#dddddd";
        public const string GRAY_93 = "#eeeeee";
        public const string GRAY_100 = "#ffffff";

        public const string SKY_15 = "#002B4C";
        public const string SKY_19 = "#013660";
        public const string SKY_23 = "#014175";
        public const string SKY_27 = "#014D89";
        public const string SKY_31 = "#01579B";
        public const string SKY_37 = "#0277BD";
        public const string SKY_41 = "#0288D1";
        public const string SKY_45 = "#039BE5";
        public const string SKY_48 = "#03A9F4";
        public const string SKY_56 = "#22B9FB";
        public const string SKY_64 = "#4FC3F7";
        public const string SKY_74 = "#81D4FA";
        public const string SKY_85 = "#B3E5FC";
        public const string SKY_94 = "#E1F5FE";

        public const string RED_31 = "#9B0104";
        public const string RED_37 = "#BD0205";
        public const string RED_41 = "#D10205";
        public const string RED_45 = "#E50307";
        public const string RED_48 = "#F40307";
        public const string RED_64 = "#F74F52";
        public const string RED_74 = "#FA8183";
        public const string RED_85 = "#FCB3B4";
        public const string RED_94 = "#FEE1E1";


        public const string ORANGE_31 = "#923C0C";
        public const string ORANGE_37 = "#AE480E";
        public const string ORANGE_41 = "#C15010";
        public const string ORANGE_45 = "#D45811";
        public const string ORANGE_48 = "#E25D13";
        public const string ORANGE_60 = "#F08244";
        public const string ORANGE_64 = "#F18D55";
        public const string ORANGE_74 = "#F5AD84";
        public const string ORANGE_85 = "#F9D0B8";
        public const string ORANGE_94 = "#FDECE3";

        public const string YELLOW_31 = "#958E09";
        public const string YELLOW_37 = "#B2A90A";
        public const string YELLOW_41 = "#C6BB0B";
        public const string YELLOW_45 = "#D9CE0C";
        public const string YELLOW_48 = "#E7DB0D";
        public const string YELLOW_60 = "#F4EA40";
        public const string YELLOW_64 = "#F5EC51";
        public const string YELLOW_74 = "#F8F182";
        public const string YELLOW_85 = "#FBF7B7";
        public const string YELLOW_94 = "#FDFCE2";
    }

    public static partial class BaseComponentExtensions
    {
        public static string ResolveRole(this BaseComponent component, string role)
        {
            var theme = component.C<ThemeStore>().Get();
            if (role == THEME_CONSTANTS.INHERIT_ROLE)
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