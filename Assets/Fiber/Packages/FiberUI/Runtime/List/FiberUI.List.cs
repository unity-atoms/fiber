using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber.UIElements;
using UnityEngine;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static ListItemComponent ListItem(
            this BaseComponent component,
            ListItemText text,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        )
        {
            return new ListItemComponent(
                text: text,
                role: role,
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

    public class ListItemComponent : BaseComponent
    {
        private readonly ListItemText _text;
        private readonly string _role;
        private readonly string _iconName;
        private readonly string _variant;
        private readonly Style _style;

        public ListItemComponent(
            ListItemText text,
            string iconName = "circle",
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        ) : base(new()) // Create new instance of children
        {
            _text = text;
            _iconName = iconName;
            _role = role;
            _variant = variant;
            _style = style;
        }

        public override VirtualNode Render()
        {
            children.Add(F.Icon(
                iconName: _iconName,
                role: _role,
                variant: _variant,
                style: new Style(
                    marginRight: C<ThemeStore>().Spacing(1),
                    marginLeft: C<ThemeStore>().Spacing(1),
                    unityTextAlign: TextAnchor.UpperCenter
                )
            ));

            if (!string.IsNullOrWhiteSpace(_text.Text))
            {
                children.Add(F.Typography(
                    type: TypographyType.Body1,
                    text: _text.Text,
                    role: _role,
                    variant: _variant
                ));
            }
            else if (_text.Children != null)
            {
                children.AddRange(_text.Children);
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
                children: children
            );
        }
    }
}