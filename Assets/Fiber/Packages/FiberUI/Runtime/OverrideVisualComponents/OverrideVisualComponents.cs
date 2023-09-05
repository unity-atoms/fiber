using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber.UIElements;
using Signals;

namespace Fiber.UI
{
    public static partial class BaseComponentExtensions
    {
        public static OverrideVisualComponentsProvider OverrideVisualComponentsProvider(
            this BaseComponent component,
            List<VirtualNode> children,
            OverrideVisualComponents overrideVisualComponents
        )
        {
            return new OverrideVisualComponentsProvider(
                children: children,
                overrideVisualComponents: overrideVisualComponents
            );
        }
    }

    public class OverrideVisualComponents
    {
        // Backdrop component
        public delegate BaseComponent CreateBackdropDelegate(
            List<VirtualNode> children = null,
            string role = THEME_CONSTANTS.INHERIT_ROLE,
            string variant = null,
            EventCallback<ClickEvent> onClick = null
        );
        public CreateBackdropDelegate CreateBackdrop { get; private set; }

        // Drawer component
        public delegate BaseComponent CreateDrawerDelegate(
            List<VirtualNode> children,
            string role,
            BaseSignal<bool> isOpen,
            DrawerPosition position,
            Style style
        );
        public CreateDrawerDelegate CreateDrawer { get; private set; }

        // Header component
        public delegate BaseComponent CreateHeaderContainerDelegate(
            List<VirtualNode> children,
            string role,
            string variant,
            Style style
        );
        public CreateHeaderContainerDelegate CreateHeaderContainer { get; private set; }

        public delegate BaseComponent CreateHeaderItemGroupDelegate(
            List<VirtualNode> children,
            Justify justifyContent,
            Style style
        );
        public CreateHeaderItemGroupDelegate CreateHeaderItemGroup { get; private set; }

        // Icon component
        public delegate BaseComponent CreateIconDelegate(
            SignalProp<string> iconName,
            string role,
            string variant,
            Style style,
            Ref<VisualElement> forwardRef
        );
        public CreateIconDelegate CreateIcon { get; private set; }

        // Icon button component
        public delegate BaseComponent CreateIconButtonDelegate(
            SignalProp<string> iconName,
            Action onPress,
            InteractiveElement interactiveRef,
            string role,
            string variant,
            Style style,
            Ref<VisualElement> forwardRef
        );
        public CreateIconButtonDelegate CreateIconButton { get; private set; }

        // List item component
        public delegate BaseComponent CreateListItemDelegate(
            ListItemText text,
            string iconName,
            string role,
            string variant,
            Style style
        );
        public CreateListItemDelegate CreateListItem { get; private set; }

        // TreeView Container component
        public delegate BaseComponent CreateTreeViewContainerDelegate(
            List<VirtualNode> children,
            string role,
            Ref<VisualElement> forwardRef
        );
        public CreateTreeViewContainerDelegate CreateTreeViewContainer { get; private set; }


        // TreeView Item component
        public delegate BaseComponent CreateTreeViewItemDelegate(
            SignalProp<string> label,
            int identationLevel,
            string role,
            bool hasSubItems,
            InteractiveElement interactiveElement,
            ISignal<bool> isSelected,
            ISignal<bool> isExpanded
        );
        public CreateTreeViewItemDelegate CreateTreeViewItem { get; private set; }

        // TreeView Item group component
        public delegate BaseComponent CreateTreeViewItemGroupDelegate(
            List<VirtualNode> children,
            int identationLevel,
            string role,
            ISignal<bool> isExpanded
        );
        public CreateTreeViewItemGroupDelegate CreateTreeViewItemGroup { get; private set; }

        // Typography component
        public delegate BaseComponent CreateTypographyDelegate(
            TypographyType type,
            SignalProp<string> text,
            string role,
            string variant,
            Style style,
            Ref<VisualElement> forwardRef
        );
        public CreateTypographyDelegate CreateTypography { get; private set; }

        public OverrideVisualComponents(
            CreateBackdropDelegate createBackdrop = null,
            CreateDrawerDelegate createDrawer = null,
            CreateHeaderContainerDelegate createHeaderContainer = null,
            CreateHeaderItemGroupDelegate createHeaderItemGroup = null,
            CreateIconDelegate createIcon = null,
            CreateIconButtonDelegate createIconButton = null,
            CreateListItemDelegate createListItem = null,
            CreateTreeViewContainerDelegate createTreeViewContainer = null,
            CreateTreeViewItemDelegate createTreeViewItem = null,
            CreateTypographyDelegate createTypography = null
        )
        {
            CreateBackdrop = createBackdrop;
            CreateDrawer = createDrawer;
            CreateHeaderContainer = createHeaderContainer;
            CreateHeaderItemGroup = createHeaderItemGroup;
            CreateIcon = createIcon;
            CreateIconButton = createIconButton;
            CreateListItem = createListItem;
            CreateTreeViewContainer = createTreeViewContainer;
            CreateTreeViewItem = createTreeViewItem;
            CreateTypography = createTypography;
        }
    }

    public class OverrideVisualComponentsProvider : BaseComponent
    {
        private readonly OverrideVisualComponents _overrideVisualComponents;
        public OverrideVisualComponentsProvider(
            List<VirtualNode> children,
            OverrideVisualComponents overrideVisualComponents
        ) : base(children)
        {
            _overrideVisualComponents = overrideVisualComponents;
        }

        public override VirtualNode Render()
        {
            return F.ContextProvider(
                value: _overrideVisualComponents,
                children: children
            );
        }
    }
}