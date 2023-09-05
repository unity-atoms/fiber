using System;
using System.Collections.Generic;
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
                Action<string> onItemSelected,
                ISignal<string> selectedItemId,
                ISignalList<string> expandedItemIds,
                string role = THEME_CONSTANTS.INHERIT_ROLE,
                Ref<VisualElement> forwardRef = null
        )
        {
            return new TreeViewComponent.Container(
                children: children,
                onItemSelected: onItemSelected,
                selectedItemId: selectedItemId,
                expandedItemIds: expandedItemIds,
                role: role,
                forwardRef: forwardRef
            );
        }

        public static TreeViewComponent.Item TreeViewItem(
            this BaseComponent component,
                SignalProp<string> label,
                string id,
                List<VirtualNode> children = null,
                string role = THEME_CONSTANTS.INHERIT_ROLE
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
            public int IndentiationLevel;

            public IndentiationLevelContext(int indentiationLeve)
            {
                IndentiationLevel = indentiationLeve;
            }
        }

        private class TreeViewStateContext
        {
            public ISignal<string> SelectedItemId;
            public Action<string> OnItemSelected;
            public ISignalList<string> ExapndedItemIds;

            public TreeViewStateContext(
                ISignal<string> selectedItemId,
                Action<string> onItemSelected,
                ISignalList<string> expandedItemIds
            )
            {
                SelectedItemId = selectedItemId;
                OnItemSelected = onItemSelected;
                ExapndedItemIds = expandedItemIds;
            }
        }

        public class Container : BaseComponent
        {
            private readonly Action<string> _onItemSelected;
            private readonly ISignal<string> _selectedItemId;
            private readonly ISignalList<string> _expandedItemIds;
            private readonly string _role;
            private readonly Ref<VisualElement> _forwardRef;

            public Container(
                List<VirtualNode> children,
                Action<string> onItemSelected,
                ISignal<string> selectedItemId,
                ISignalList<string> expandedItemIds,
                string role = THEME_CONSTANTS.INHERIT_ROLE,
                Ref<VisualElement> forwardRef = null
            ) : base(children)
            {
                _role = role;
                _onItemSelected = onItemSelected;
                _selectedItemId = selectedItemId;
                _expandedItemIds = expandedItemIds;
                _forwardRef = forwardRef;
            }

            public override VirtualNode Render()
            {
                return F.ContextProvider(
                    value: new TreeViewStateContext(
                        selectedItemId: _selectedItemId,
                        onItemSelected: (string id) =>
                        {
                            _onItemSelected.Invoke(id);
                        },
                        expandedItemIds: _expandedItemIds
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
                string role = THEME_CONSTANTS.INHERIT_ROLE,
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
                string role = THEME_CONSTANTS.INHERIT_ROLE
            ) : base(children)
            {
                _label = label;
                _id = id;
                _role = role;
            }

            public override VirtualNode Render()
            {
                var treeViewStateContext = F.GetContext<TreeViewStateContext>();
                var isSelected = F.CreateComputedSignal((selectedItemId) => selectedItemId == _id, treeViewStateContext.SelectedItemId);
                var isExpanded = F.CreateComputedSignal((expandedItemIds) => expandedItemIds.Contains(_id), treeViewStateContext.ExapndedItemIds);
                var identationLevel = F.GetContext<IndentiationLevelContext>().IndentiationLevel;

                var interactiveElement = F.CreateInteractiveElement(isDisabled: null, onPress: () =>
                {
                    treeViewStateContext.OnItemSelected(_id);
                });

                return F.Fragment(F.Children(
                    new VisualItem(
                        label: _label,
                        identationLevel: identationLevel,
                        role: _role,
                        hasSubItems: children != null && children.Count > 0,
                        interactiveElement: interactiveElement,
                        isSelected: isSelected,
                        isExpanded: isExpanded
                    ),
                    F.Active(
                        when: isExpanded,
                        children: F.Children(
                            F.ContextProvider(
                                value: new IndentiationLevelContext(indentiationLeve: identationLevel + 1),
                                children: F.Children(new VisualItemGroup(
                                    children: children,
                                    identationLevel: identationLevel + 1,
                                    role: _role,
                                    isExpanded: isExpanded
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
            private readonly ISignal<bool> _isExpanded;

            public VisualItemGroup(
                List<VirtualNode> children,
                int identationLevel,
                string role,
                ISignal<bool> isExpanded
            ) : base(children)
            {
                _identationLevel = identationLevel;
                _role = role;
                _isExpanded = isExpanded;
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
                        isExpanded: _isExpanded
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
            private readonly ISignal<bool> _isSelected;
            private readonly ISignal<bool> _isExpanded;

            public VisualItem(
                SignalProp<string> label,
                int identationLevel,
                string role,
                bool hasSubItems,
                InteractiveElement interactiveElement,
                ISignal<bool> isSelected,
                ISignal<bool> isExpanded
            ) : base()
            {
                _label = label;
                _identationLevel = identationLevel;
                _role = role;
                _hasSubItems = hasSubItems;
                _interactiveElement = interactiveElement;
                _isSelected = isSelected;
                _isExpanded = isExpanded;
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
                        isExpanded: _isExpanded
                    );
                }

                var themeStore = C<ThemeStore>();
                var role = F.ResolveRole(_role);

                var color = themeStore.Color(role, ElementType.Text, _interactiveElement.IsPressed, _interactiveElement.IsHovered, _isSelected);
                var backgroundColor = themeStore.Color(role, ElementType.Background, _interactiveElement.IsPressed, _interactiveElement.IsHovered, _isSelected);

                var iconName = CreateComputedSignal((isExpanded) => isExpanded ? "chevron-down" : "chevron-right", _isExpanded);

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
                            children: F.Children(F.Icon(iconName: iconName))
                        )
                    )
                );
            }
        }
    }
}

