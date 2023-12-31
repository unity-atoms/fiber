using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using UnityEngine;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkListItemComponent SilkListItem(
            this BaseComponent component,
            ListItemText text,
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new()
        )
        {
            return new SilkListItemComponent(
                text: text,
                role: role,
                subRole: subRole,
                variant: variant,
                style: style
            );
        }
    }

    public struct ListItemText
    {
        public string Text;
        public List<VirtualNode> Children;

        public ListItemText(string text)
        {
            Text = text;
            Children = null;
        }

        public ListItemText(List<VirtualNode> children)
        {
            Text = null;
            Children = children;
        }

        public static implicit operator ListItemText(string text) => new(text);
        public static implicit operator ListItemText(List<VirtualNode> children) => new(children);
    }

    public class SilkListItemComponent : BaseComponent
    {
        private readonly ListItemText _text;
        private readonly string _role;
        private readonly string _subRole;
        private readonly string _iconName;
        private readonly SignalProp<string> _variant;
        private readonly Style _style;

        public SilkListItemComponent(
            ListItemText text,
            string iconName = "circle",
            string role = THEME_CONSTANTS.INHERIT,
            string subRole = THEME_CONSTANTS.INHERIT,
            SignalProp<string> variant = new(),
            Style style = new()
        ) : base(new List<VirtualNode>()) // Create new instance of children
        {
            _text = text;
            _iconName = iconName;
            _role = role;
            _subRole = subRole;
            _variant = variant;
            _style = style;
        }

        public override VirtualBody Render()
        {
            Children.Add(F.SilkIcon(
                iconName: _iconName,
                size: IconSize.Tiny,
                role: _role,
                subRole: _subRole,
                variant: _variant,
                style: new Style(
                    height: C<ThemeStore>().Spacing(5.5f),
                    marginRight: C<ThemeStore>().Spacing(1),
                    marginLeft: C<ThemeStore>().Spacing(1),
                    unityTextAlign: TextAnchor.MiddleCenter
                )
            ));

            if (!string.IsNullOrWhiteSpace(_text.Text))
            {
                Children.Add(F.SilkTypography(
                    type: TypographyType.Body1,
                    text: _text.Text,
                    role: _role,
                    subRole: _subRole,
                    variant: _variant
                ));
            }
            else if (_text.Children != null)
            {
                Children.Add(_text.Children);
            }

            return F.View(
                style: new Style(
                    mergedStyle: _style,
                    display: DisplayStyle.Flex,
                    flexDirection: FlexDirection.Row,
                    alignItems: Align.FlexStart,
                    justifyContent: Justify.FlexStart,
                    marginBottom: C<ThemeStore>().Spacing(2)
                ),
                children: Children
            );
        }
    }
}