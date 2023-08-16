using System;
using System.Collections.Generic;
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
                string role = Constants.INHERIT_ROLE,
                Ref<VisualElement> forwardRef = null
        )
        {
            return new TreeViewComponent.Container(
                children: children,
                onItemIdSelected: onItemIdSelected,
                selectedItemId: selectedItemId,
                role: role,
                forwardRef: forwardRef
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
            private readonly Ref<VisualElement> _forwardRef;

            public Container(
                List<VirtualNode> children,
                Action<string> onItemIdSelected,
                BaseSignal<string> selectedItemId,
                string role = Constants.INHERIT_ROLE,
                Ref<VisualElement> forwardRef = null
            ) : base(children)
            {
                _role = role;
                _onItemIdSelected = onItemIdSelected;
                _selectedItemId = selectedItemId;
                _forwardRef = forwardRef;
            }

            public override VirtualNode Render()
            {
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
                                        new VisualContainer(children: children, role: _role, forwardRef: _forwardRef)
                                    )
                                )
                            )
                        )
                    )
                );
            }
        }

        private class VisualContainer : BaseComponent
        {
            private readonly string _role;
            private readonly Ref<VisualElement> _forwardRef;

            public VisualContainer(
                List<VirtualNode> children,
                string role = Constants.INHERIT_ROLE,
                Ref<VisualElement> forwardRef = null
            ) : base(children)
            {
                _role = role;
                _forwardRef = forwardRef;
            }
            public override VirtualNode Render()
            {
                var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
                if (overrideVisualComponents?.CreateTreeViewContainer != null)
                {
                    return overrideVisualComponents.CreateTreeViewContainer(
                        children: children,
                        role: _role,
                        forwardRef: _forwardRef
                    );
                }

                return F.View(_ref: _forwardRef, children: children);
            }
        }

        public class Item : BaseComponent
        {
            private SignalProp<string> _label;
            private readonly string _id;
            private readonly string _role;

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
                var themeStore = C<ThemeStore>();
                var role = F.ResolveRole(_role);
                var context = F.GetContext<SelectedItemIdContext>();
                var isSelected = F.CreateComputedSignal<string, bool>((selectedItemId) => selectedItemId == _id, context.SelectedItemId);
                var isOpen = new Signal<bool>(false);
                var identationLevel = F.GetContext<IndentiationLevelContext>().IndentiationLeve;

                var interactiveElement = F.CreateInteractiveElement(isDisabled: null, onPress: () =>
                {
                    context.OnItemSelected(_id);
                    isOpen.Value = !isOpen.Value;
                });

                var color = themeStore.Color(role, ElementType.Text, interactiveElement.IsPressed, interactiveElement.IsHovered, isSelected);
                var backgroundColor = themeStore.Color(role, ElementType.Background, interactiveElement.IsPressed, interactiveElement.IsHovered, isSelected);

                var iconType = CreateComputedSignal((isOpen) => isOpen ? "chevron-down" : "chevron-right", isOpen);

                return F.Fragment(F.Children(
                    F.View(
                        _ref: interactiveElement.Ref,
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
                                type: TypographyType.Subtitle2,
                                style: new Style(color: color)
                            ),
                            F.Visible(
                                when: new StaticSignal<bool>(children != null),
                                children: F.Children(
                                    F.Icon(type: iconType)
                                )
                            )
                        )
                    ),
                    F.Active(
                        when: isOpen,
                        children: F.Children(
                            F.ContextProvider(
                                value: new IndentiationLevelContext(indentiationLeve: identationLevel + 1),
                                children: children
                            )
                        )
                    )
                ));
            }
        }
    }
}

