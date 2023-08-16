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
                var context = F.GetContext<SelectedItemIdContext>();
                var isSelected = F.CreateComputedSignal((selectedItemId) => selectedItemId == _id, context.SelectedItemId);
                var isOpen = new Signal<bool>(false);
                var identationLevel = F.GetContext<IndentiationLevelContext>().IndentiationLeve;

                var interactiveElement = F.CreateInteractiveElement(isDisabled: null, onPress: () =>
                {
                    context.OnItemSelected(_id);
                    isOpen.Value = !isOpen.Value;
                });

                return F.Fragment(F.Children(
                    new VisualItem(
                        label: _label,
                        identationLevel: identationLevel,
                        role: _role,
                        hasSubItems: children != null && children.Count > 0,
                        interactiveElement: interactiveElement,
                        isSelected: isSelected,
                        isOpen: isOpen
                    ),
                    F.Active(
                        when: isOpen,
                        children: F.Children(
                            F.ContextProvider(
                                value: new IndentiationLevelContext(indentiationLeve: identationLevel + 1),
                                children: F.Children(new VisualItemGroup(
                                    children: children,
                                    identationLevel: identationLevel + 1,
                                    role: _role,
                                    isOpen: isOpen
                                ))
                            )
                        )
                    )
                ));
            }
        }

        private class VisualItemGroup : BaseComponent
        {
            private readonly int _identationLevel;
            private readonly string _role;
            private readonly BaseSignal<bool> _isOpen;

            public VisualItemGroup(
                List<VirtualNode> children,
                int identationLevel,
                string role,
                BaseSignal<bool> isOpen
            ) : base(children)
            {
                _identationLevel = identationLevel;
                _role = role;
                _isOpen = isOpen;
            }
            public override VirtualNode Render()
            {
                var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
                if (overrideVisualComponents?.CreateTreeViewItemGroup != null)
                {
                    return overrideVisualComponents.CreateTreeViewItemGroup(
                        children: children,
                        identationLevel: _identationLevel,
                        role: _role,
                        isOpen: _isOpen
                    );
                }

                return F.View(children: children);
            }

        }
        private class VisualItem : BaseComponent
        {
            private readonly SignalProp<string> _label;
            private readonly int _identationLevel;
            private readonly string _role;
            private readonly bool _hasSubItems;
            private readonly InteractiveElement _interactiveElement;
            private readonly BaseSignal<bool> _isSelected;
            private readonly BaseSignal<bool> _isOpen;

            public VisualItem(
                SignalProp<string> label,
                int identationLevel,
                string role,
                bool hasSubItems,
                InteractiveElement interactiveElement,
                BaseSignal<bool> isSelected,
                BaseSignal<bool> isOpen
            ) : base()
            {
                _label = label;
                _identationLevel = identationLevel;
                _role = role;
                _hasSubItems = hasSubItems;
                _interactiveElement = interactiveElement;
                _isSelected = isSelected;
                _isOpen = isOpen;
            }
            public override VirtualNode Render()
            {
                var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
                if (overrideVisualComponents?.CreateTreeViewItem != null)
                {
                    return overrideVisualComponents.CreateTreeViewItem(
                        label: _label,
                        identationLevel: _identationLevel,
                        role: _role,
                        hasSubItems: _hasSubItems,
                        interactiveElement: _interactiveElement,
                        isSelected: _isSelected,
                        isOpen: _isOpen
                    );
                }

                var themeStore = C<ThemeStore>();
                var role = F.ResolveRole(_role);

                var color = themeStore.Color(role, ElementType.Text, _interactiveElement.IsPressed, _interactiveElement.IsHovered, _isSelected);
                var backgroundColor = themeStore.Color(role, ElementType.Background, _interactiveElement.IsPressed, _interactiveElement.IsHovered, _isSelected);

                var iconType = CreateComputedSignal((isOpen) => isOpen ? "chevron-down" : "chevron-right", _isOpen);

                return F.View(
                    _ref: _interactiveElement.Ref,
                    style: new Style(
                        backgroundColor: backgroundColor,
                        display: DisplayStyle.Flex,
                        flexDirection: FlexDirection.Row,
                        alignItems: Align.Center,
                        justifyContent: Justify.SpaceBetween,
                        paddingLeft: themeStore.Spacing(3 + _identationLevel * 2),
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
                            when: new StaticSignal<bool>(_hasSubItems),
                            children: F.Children(F.Icon(type: iconType))
                        )
                    )
                );
            }
        }
    }
}

