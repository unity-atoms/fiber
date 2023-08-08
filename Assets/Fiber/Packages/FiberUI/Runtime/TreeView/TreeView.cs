using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static TreeViewComponent.Container TreeViewContainer(
            this BaseComponent component,
                List<VirtualNode> children,
                Action<string> onItemIdSelected,
                BaseSignal<string> selectedItemId,
                string role = Constants.INHERIT_ROLE
        )
        {
            return new TreeViewComponent.Container(
                children: children,
                onItemIdSelected: onItemIdSelected,
                selectedItemId: selectedItemId,
                role: role
            );
        }

        public static TreeViewComponent.Item TreeViewItem(
            this BaseComponent component,
                SignalProp<string> label,
                string id,
                List<VirtualNode> children = null,
                string role = Constants.INHERIT_ROLE
        )
        {
            return new TreeViewComponent.Item(
                label: label,
                id: id,
                children: children,
                role: role
            );
        }
    }

    public static class TreeViewComponent
    {
        private class IndentiationLevelContext
        {
            public int IndentiationLeve;

            public IndentiationLevelContext(int indentiationLeve)
            {
                IndentiationLeve = indentiationLeve;
            }
        }

        private class SelectedItemIdContext
        {
            public BaseSignal<string> SelectedItemId;
            public Action<string> OnItemSelected;

            public SelectedItemIdContext(BaseSignal<string> selectedItemId, Action<string> onItemIdSelected)
            {
                SelectedItemId = selectedItemId;
                OnItemSelected = onItemIdSelected;
            }
        }

        public class Container : BaseComponent
        {
            private readonly Action<string> _onItemIdSelected;
            private readonly BaseSignal<string> _selectedItemId;
            private readonly string _role;

            public Container(
                List<VirtualNode> children,
                Action<string> onItemIdSelected,
                BaseSignal<string> selectedItemId,
                string role = Constants.INHERIT_ROLE
            ) : base(children)
            {
                _role = role;
                _onItemIdSelected = onItemIdSelected;
                _selectedItemId = selectedItemId;
            }

            public override VirtualNode Render()
            {
                var theme = C<ThemeStore>().Get();
                var role = F.GetRole(_role);

                return F.ContextProvider(
                    value: new SelectedItemIdContext(
                        selectedItemId: _selectedItemId,
                        onItemIdSelected: (string id) =>
                        {
                            _onItemIdSelected.Invoke(id);
                        }
                    ),
                    children: F.Children(
                        F.RoleProvider(
                            role: _role,
                            children: F.Children(
                                F.ContextProvider(
                                    value: new IndentiationLevelContext(indentiationLeve: 0),
                                    children: F.Children(
                                        F.View(children: children)
                                    )
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
                var isPressed = new Signal<bool>(false);
                var isOpen = new Signal<bool>(false);
                var context = F.GetContext<SelectedItemIdContext>();
                var identationLevel = F.GetContext<IndentiationLevelContext>().IndentiationLeve;

                CreateEffect(() =>
                {
                    _ref.Current.RegisterCallback<MouseEnterEvent>(evt =>
                    {
                        isHovered.Value = true;
                    });
                    _ref.Current.RegisterCallback<MouseLeaveEvent>(evt =>
                    {
                        isHovered.Value = false;
                        isPressed.Value = false;
                    });
                    _ref.Current.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        isPressed.Value = true;
                    });
                    _ref.Current.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        if (isPressed.Value)
                        {
                            context.OnItemSelected(_id);
                            isPressed.Value = false;
                            isOpen.Value = !isOpen.Value;
                        }
                    });
                    return null;
                });

                var color = CreateComputedSignal((isHovered, isPressed, selectedItemId, theme) =>
                {
                    if (isPressed && theme.Color[role].Text.Pressed.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.Color[role].Text.Pressed.Get();
                    }
                    else if (isHovered && theme.Color[role].Text.Hovered.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.Color[role].Text.Hovered.Get();
                    }
                    else if (selectedItemId == _id && theme.Color[role].Text.Selected.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.Color[role].Text.Selected.Get();
                    }
                    return theme.Color[role].Text.Default.Get();
                }, isHovered, isPressed, context.SelectedItemId, theme);

                var backgroundColor = CreateComputedSignal((isHovered, isPressed, selectedItemId, theme) =>
                {
                    if (isPressed && theme.Color[role].Background.Pressed.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.Color[role].Background.Pressed.Get();
                    }
                    else if (isHovered && theme.Color[role].Background.Hovered.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.Color[role].Background.Hovered.Get();
                    }
                    else if (selectedItemId == _id && theme.Color[role].Background.Selected.Get().keyword != StyleKeyword.Null)
                    {
                        return theme.Color[role].Background.Selected.Get();
                    }
                    return theme.Color[role].Background.Default.Get();
                }, isHovered, isPressed, context.SelectedItemId, theme);

                var iconUnicode = CreateComputedSignal((isOpen) =>
                {
                    return (isOpen ? '\uf078' : '\uf054').ToString();
                }, isOpen);


                var fontAwesome = Resources.Load<Font>("Fonts/FontAwesome/fontawesome-solid");

                return F.Fragment(F.Children(
                    F.View(
                        _ref: _ref,
                        style: new Style(
                            backgroundColor: backgroundColor,
                            display: DisplayStyle.Flex,
                            flexDirection: FlexDirection.Row,
                            alignItems: Align.Center,
                            justifyContent: Justify.SpaceBetween,
                            paddingLeft: theme.Spacing(3) + identationLevel * theme.Spacing(2),
                            paddingTop: theme.Spacing(2),
                            paddingRight: theme.Spacing(3),
                            paddingBottom: theme.Spacing(2),
                            width: new Length(100, LengthUnit.Percent)
                        ),
                        pickingMode: PickingMode.Position,
                        children: F.Children(
                            F.Text(
                                text: _label,
                                style: new Style(color: color, fontSize: 16)
                            ),
                            F.Visible(
                                when: new StaticSignal<bool>(children != null),
                                children: F.Children(
                                    F.Text(
                                        text: iconUnicode,
                                        style: new Style(color: color, unityFont: fontAwesome, unityFontDefinition: StyleKeyword.None)
                                    )
                            ))
                        )
                    ),
                    F.Active(
                        when: isOpen,
                        children: F.Children(
                            F.ContextProvider(
                                value: new IndentiationLevelContext(indentiationLeve: identationLevel + 1),
                                children: F.Children(
                                    F.View(children: children)
                                )
                            )
                        )
                    )
                ));
            }
        }
    }
}

