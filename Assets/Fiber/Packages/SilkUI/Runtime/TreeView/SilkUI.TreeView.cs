using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;
using Fiber.InteractiveUI;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static SilkTreeViewComponent.Container SilkTreeViewContainer(
            this BaseComponent component,
                VirtualBody children,
                Action<string> onItemSelected,
                ISignal<string> selectedItemId,
                ISignalList<string> expandedItemIds,
                string role = THEME_CONSTANTS.INHERIT,
                string subRole = THEME_CONSTANTS.INHERIT,
                Ref<VisualElement> forwardRef = null
        )
        {
            return new SilkTreeViewComponent.Container(
                children: children,
                onItemSelected: onItemSelected,
                selectedItemId: selectedItemId,
                expandedItemIds: expandedItemIds,
                role: role,
                subRole: subRole,
                forwardRef: forwardRef
            );
        }

        public static SilkTreeViewComponent.Item SilkTreeViewItem(
            this BaseComponent component,
                SignalProp<string> label,
                string id,
                VirtualBody children = default,
                string role = THEME_CONSTANTS.INHERIT,
                string subRole = THEME_CONSTANTS.INHERIT
        )
        {
            return new SilkTreeViewComponent.Item(
                label: label,
                id: id,
                children: children,
                role: role,
                subRole: subRole
            );
        }
    }

    public static class SilkTreeViewComponent
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
            private readonly string _subRole;
            private readonly Ref<VisualElement> _forwardRef;

            public Container(
                VirtualBody children,
                Action<string> onItemSelected,
                ISignal<string> selectedItemId,
                ISignalList<string> expandedItemIds,
                string role = THEME_CONSTANTS.INHERIT,
                string subRole = THEME_CONSTANTS.INHERIT,
                Ref<VisualElement> forwardRef = null
            ) : base(children)
            {
                _role = role;
                _subRole = subRole;
                _onItemSelected = onItemSelected;
                _selectedItemId = selectedItemId;
                _expandedItemIds = expandedItemIds;
                _forwardRef = forwardRef;
            }

            public override VirtualBody Render()
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
                    children: F.RoleProvider(
                        role: _role,
                        subRole: _subRole,
                        children: F.ContextProvider(
                            value: new IndentiationLevelContext(indentiationLeve: 0),
                            children: new VisualContainer(children: Children, role: _role, forwardRef: _forwardRef)
                        )
                    )
                );
            }
        }

        private class VisualContainer : BaseComponent
        {
            private readonly string _role;
            private readonly string _subRole;
            private readonly Ref<VisualElement> _forwardRef;

            public VisualContainer(
                VirtualBody children,
                string role = THEME_CONSTANTS.INHERIT,
                string subRole = THEME_CONSTANTS.INHERIT,
                Ref<VisualElement> forwardRef = null
            ) : base(children)
            {
                _role = role;
                _subRole = subRole;
                _forwardRef = forwardRef;
            }
            public override VirtualBody Render()
            {
                var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
                if (overrideVisualComponents?.CreateTreeViewContainer != null)
                {
                    return overrideVisualComponents.CreateTreeViewContainer(
                        children: Children,
                        role: _role,
                        subRole: _subRole,
                        forwardRef: _forwardRef
                    );
                }

                return F.View(_ref: _forwardRef, children: Children);
            }
        }

        public class Item : BaseComponent
        {
            private SignalProp<string> _label;
            private readonly string _id;
            private readonly string _role;
            private readonly string _subRole;

            public Item(
                SignalProp<string> label,
                string id,
                VirtualBody children = default,
                string role = THEME_CONSTANTS.INHERIT,
                string subRole = THEME_CONSTANTS.INHERIT
            ) : base(children)
            {
                _label = label;
                _id = id;
                _role = role;
                _subRole = subRole;
            }

            public override VirtualBody Render()
            {
                var treeViewStateContext = F.GetContext<TreeViewStateContext>();
                var isSelected = F.CreateComputedSignal((selectedItemId) => selectedItemId == _id, treeViewStateContext.SelectedItemId);
                var isExpanded = F.CreateComputedSignal((expandedItemIds) => expandedItemIds.Contains(_id), treeViewStateContext.ExapndedItemIds);
                var identationLevel = F.GetContext<IndentiationLevelContext>().IndentiationLevel;

                var interactiveElement = F.CreateInteractiveElement(isDisabled: null, onPressUp: (evt) =>
                {
                    treeViewStateContext.OnItemSelected(_id);
                });

                return F.Nodes(
                    new VisualItem(
                        label: _label,
                        identationLevel: identationLevel,
                        role: _role,
                        subRole: _subRole,
                        hasSubItems: Children.Count > 0,
                        interactiveElement: interactiveElement,
                        isSelected: isSelected,
                        isExpanded: isExpanded
                    ),
                    F.Active(
                        when: isExpanded,
                        children: F.ContextProvider(
                            value: new IndentiationLevelContext(indentiationLeve: identationLevel + 1),
                            children: new VisualItemGroup(
                                children: Children,
                                identationLevel: identationLevel + 1,
                                role: _role,
                                subRole: _subRole,
                                isExpanded: isExpanded
                            )
                        )
                    )
                );
            }
        }

        private class VisualItemGroup : BaseComponent
        {
            private readonly int _identationLevel;
            private readonly string _role;
            private readonly string _subRole;
            private readonly ISignal<bool> _isExpanded;

            public VisualItemGroup(
                VirtualBody children,
                int identationLevel,
                string role,
                string subRole,
                ISignal<bool> isExpanded
            ) : base(children)
            {
                _identationLevel = identationLevel;
                _role = role;
                _subRole = subRole;
                _isExpanded = isExpanded;
            }
            public override VirtualBody Render()
            {
                var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
                if (overrideVisualComponents?.CreateTreeViewItemGroup != null)
                {
                    return overrideVisualComponents.CreateTreeViewItemGroup(
                        children: Children,
                        identationLevel: _identationLevel,
                        role: _role,
                        subRole: _subRole,
                        isExpanded: _isExpanded
                    );
                }

                return F.View(children: Children);
            }

        }
        private class VisualItem : BaseComponent
        {
            private readonly SignalProp<string> _label;
            private readonly int _identationLevel;
            private readonly string _role;
            private readonly string _subRole;
            private readonly bool _hasSubItems;
            private readonly InteractiveElement _interactiveElement;
            private readonly ISignal<bool> _isSelected;
            private readonly ISignal<bool> _isExpanded;

            public VisualItem(
                SignalProp<string> label,
                int identationLevel,
                string role,
                string subRole,
                bool hasSubItems,
                InteractiveElement interactiveElement,
                ISignal<bool> isSelected,
                ISignal<bool> isExpanded
            ) : base()
            {
                _label = label;
                _identationLevel = identationLevel;
                _role = role;
                _subRole = subRole;
                _hasSubItems = hasSubItems;
                _interactiveElement = interactiveElement;
                _isSelected = isSelected;
                _isExpanded = isExpanded;
            }
            public override VirtualBody Render()
            {
                var overrideVisualComponents = C<OverrideVisualComponents>(throwIfNotFound: false);
                if (overrideVisualComponents?.CreateTreeViewItem != null)
                {
                    return overrideVisualComponents.CreateTreeViewItem(
                        label: _label,
                        identationLevel: _identationLevel,
                        role: _role,
                        subRole: _subRole,
                        hasSubItems: _hasSubItems,
                        interactiveElement: _interactiveElement,
                        isSelected: _isSelected,
                        isExpanded: _isExpanded
                    );
                }

                var themeStore = C<ThemeStore>();
                var (role, subRole) = F.ResolveRoleAndSubRole(_role, _subRole);

                var color = themeStore.Color(role, ElementType.Text, _interactiveElement.IsPressed, _interactiveElement.IsHovered, _isSelected, subRole: subRole);
                var backgroundColor = themeStore.Color(role, ElementType.Background, _interactiveElement.IsPressed, _interactiveElement.IsHovered, _isSelected, subRole: subRole);

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
                    children: F.Nodes(
                        F.SilkTypography(
                            text: _label,
                            type: TypographyType.Subtitle2,
                            style: new Style(color: color)
                        ),
                        F.Visible(
                            when: new StaticSignal<bool>(_hasSubItems),
                            children: F.SilkIcon(iconName: iconName)
                        )
                    )
                );
            }
        }
    }
}

