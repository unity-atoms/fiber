using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Fiber.DesignTokens;
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
                string typographyType,
                List<VirtualNode> children = null,
                string role = Constants.INHERIT_ROLE
        )
        {
            return new TreeViewComponent.Item(
                label: label,
                id: id,
                typographyType: typographyType,
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
            private readonly string _id;
            private readonly string _typographyType;
            private readonly string _role;

            public Item(
                SignalProp<string> label,
                string id,
                string typographyType,
                List<VirtualNode> children = null,
                string role = Constants.INHERIT_ROLE
            ) : base(children)
            {
                _label = label;
                _id = id;
                _typographyType = typographyType;
                _role = role;
            }

            public override VirtualNode Render()
            {
                var _ref = new Ref<VisualElement>();
                var themeStore = C<ThemeStore>();
                var role = F.GetRole(_role);
                var context = F.GetContext<SelectedItemIdContext>();
                var isHovered = new Signal<bool>(false);
                var isPressed = new Signal<bool>(false);
                var isSelected = F.CreateComputedSignal<string, bool>((selectedItemId) => selectedItemId == _id, context.SelectedItemId);
                var isOpen = new Signal<bool>(false);
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

                var color = themeStore.Color(role, ElementType.Text, isPressed, isHovered, isSelected);
                var backgroundColor = themeStore.Color(role, ElementType.Background, isPressed, isHovered, isSelected);

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
                            paddingLeft: themeStore.Spacing(3 + identationLevel * 2),
                            paddingTop: themeStore.Spacing(2),
                            paddingRight: themeStore.Spacing(3),
                            paddingBottom: themeStore.Spacing(2),
                            width: new Length(100, LengthUnit.Percent)
                        ),
                        pickingMode: PickingMode.Position,
                        children: F.Children(
                            F.Typography(
                                text: _label,
                                type: _typographyType,
                                style: new Style(color: color)
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

