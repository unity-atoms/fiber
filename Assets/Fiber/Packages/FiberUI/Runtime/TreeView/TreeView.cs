using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

namespace Fiber.UI
{
    public static class TreeView
    {
        private class TreeViewContext
        {
            public Signal<string> SelectedItemId;
            public Action<string> OnItemSelected;

            public TreeViewContext(Signal<string> selectedItemId, Action<string> onItemSelected)
            {
                SelectedItemId = selectedItemId;
                OnItemSelected = onItemSelected;
            }
        }

        public class Container : BaseComponent
        {
            private Action<string> _onItemSelected;
            private string _role;

            public Container(
                List<VirtualNode> children,
                Action<string> onItemSelected = null,
                string role = Constants.INHERIT_ROLE
            ) : base(children)
            {
                _role = role;
                _onItemSelected = onItemSelected;
            }

            public override VirtualNode Render()
            {
                var theme = C<ThemeStore>().Get();
                var role = F.GetRole(_role);
                var selectedItemId = new Signal<string>(null);

                return F.ContextProvider(
                    value: new TreeViewContext(
                        selectedItemId: selectedItemId,
                        onItemSelected: (string id) =>
                        {
                            selectedItemId.Value = id;
                            _onItemSelected?.Invoke(id);
                        }
                    ),
                    children: F.Children(
                        F.RoleProvider(
                            role: _role,
                            children: F.Children(
                                F.View(
                                    style: new Style(
                                        flexShrink: 0,
                                        backgroundColor: theme.DesignTokens[role].Background.Default,
                                        borderRightColor: theme.DesignTokens[role].Border.Default,
                                        borderRightWidth: 1,
                                        minWidth: 100,
                                        maxWidth: 200,
                                        width: new Length(25, LengthUnit.Percent),
                                        minHeight: 20,
                                        height: StyleKeyword.Auto
                                    ),
                                    children: children
                                )
                            )
                        )
                    )
                );
            }
        }

        public class Item : BaseComponent
        {
            private SignalProp<string> _label;
            private readonly string _role;
            private readonly string _id;

            public Item(
                SignalProp<string> label,
                string id,
                List<VirtualNode> children = null,
                string role = Constants.INHERIT_ROLE
            ) : base(children)
            {
                _label = label;
                _id = id;
                _role = role;
            }

            public override VirtualNode Render()
            {
                var _ref = new Ref<VisualElement>();
                var theme = C<ThemeStore>().Get();
                var role = F.GetRole(_role);
                var isHovered = new Signal<bool>(false);
                var context = F.GetContext<TreeViewContext>();

                CreateEffect(() =>
                {
                    _ref.Current.RegisterCallback<MouseEnterEvent>(evt =>
                    {
                        isHovered.Value = true;
                    });
                    _ref.Current.RegisterCallback<MouseLeaveEvent>(evt =>
                    {
                        isHovered.Value = false;
                    });
                    _ref.Current.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        context.OnItemSelected(_id);
                    });
                    return null;
                });

                var color = CreateComputedSignal<bool, Theme, StyleColor>((isHovered, theme) =>
                {
                    return isHovered && theme.DesignTokens[role].Text.Hovered.Get().keyword != StyleKeyword.Null ?
                        theme.DesignTokens[role].Text.Hovered.Get() : theme.DesignTokens[role].Text.Default.Get();
                }, isHovered, theme);

                var backgroundColor = CreateComputedSignal<bool, string, Theme, StyleColor>((isHovered, selectedItemId, theme) =>
                {
                    if (isHovered && theme.DesignTokens[role].Background.Hovered.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.DesignTokens[role].Background.Hovered.Get();
                    }
                    else if (selectedItemId == _id && theme.DesignTokens[role].Background.Selected.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.DesignTokens[role].Background.Selected.Get();
                    }
                    return theme.DesignTokens[role].Background.Default.Get();
                }, isHovered, context.SelectedItemId, theme);


                var fontAwesome = Resources.Load<Font>("Fonts/FontAwesome/fontawesome-solid");

                return F.View(
                    _ref: _ref,
                    style: new Style(
                        backgroundColor: backgroundColor,
                        display: DisplayStyle.Flex,
                        flexDirection: FlexDirection.Row,
                        fontSize: 10,
                        alignItems: Align.Center,
                        paddingLeft: 4,
                        paddingTop: 4,
                        paddingRight: 4,
                        paddingBottom: 4,
                        width: new Length(100, LengthUnit.Percent)
                    ),
                    children: F.Children(
                        F.Text(
                            text: '\uf054'.ToString(),
                            style: new Style(color: color, unityFont: fontAwesome, unityFontDefinition: StyleKeyword.None)
                        ),
                        F.Text(
                            text: _label,
                            style: new Style(color: color)
                        )
                    )
                );
            }
        }
    }
}

