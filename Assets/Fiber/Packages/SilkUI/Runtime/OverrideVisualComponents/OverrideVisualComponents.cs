using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber;
using Fiber.UIElements;
using Signals;

namespace SilkUI
{
    public static partial class BaseComponentExtensions
    {
        public static OverrideVisualComponentsProvider OverrideVisualComponentsProvider(
            this BaseComponent component,
            VirtualBody children,
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
            VirtualBody children,
            string role,
            string subRole,
            SignalProp<string> variant,
            EventCallback<ClickEvent> onClick
        );
        public CreateBackdropDelegate CreateBackdrop { get; private set; }

        // Drawer component
        public delegate BaseComponent CreateDrawerDelegate(
            VirtualBody children,
            BaseSignal<bool> isOpen,
            string role,
            string subRole,
            SignalProp<string> variant,
            DrawerPosition position,
            Style style,
            float outsideScreenPercentage = 100f
        );
        public CreateDrawerDelegate CreateDrawer { get; private set; }

        // Header component
        public delegate BaseComponent CreateHeaderContainerDelegate(
            VirtualBody children,
            string role,
            string subRole,
            SignalProp<string> variant,
            Style style
        );
        public CreateHeaderContainerDelegate CreateHeaderContainer { get; private set; }

        public delegate BaseComponent CreateHeaderItemGroupDelegate(
            VirtualBody children,
            Justify justifyContent,
            Style style
        );
        public CreateHeaderItemGroupDelegate CreateHeaderItemGroup { get; private set; }

        // Icon component
        public delegate BaseComponent CreateIconDelegate(
            SignalProp<string> iconName,
            string role,
            string subRole,
            SignalProp<string> variant,
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
            string subRole,
            SignalProp<string> variant,
            Style style,
            Ref<VisualElement> forwardRef
        );
        public CreateIconButtonDelegate CreateIconButton { get; private set; }

        // TreeView Container component
        public delegate BaseComponent CreateTreeViewContainerDelegate(
            VirtualBody children,
            string role,
            string subRole,
            Ref<VisualElement> forwardRef
        );
        public CreateTreeViewContainerDelegate CreateTreeViewContainer { get; private set; }


        // TreeView Item component
        public delegate BaseComponent CreateTreeViewItemDelegate(
            SignalProp<string> label,
            int identationLevel,
            string role,
            string subRole,
            bool hasSubItems,
            InteractiveElement interactiveElement,
            ISignal<bool> isSelected,
            ISignal<bool> isExpanded
        );
        public CreateTreeViewItemDelegate CreateTreeViewItem { get; private set; }

        // TreeView Item group component
        public delegate BaseComponent CreateTreeViewItemGroupDelegate(
            VirtualBody children,
            int identationLevel,
            string role,
            string subRole,
            ISignal<bool> isExpanded
        );
        public CreateTreeViewItemGroupDelegate CreateTreeViewItemGroup { get; private set; }

        // Typography component
        public delegate BaseComponent CreateTypographyDelegate(
            SignalProp<TypographyType> type,
            SignalProp<string> text,
            string role,
            string subRole,
            SignalProp<string> variant,
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
            CreateTreeViewContainer = createTreeViewContainer;
            CreateTreeViewItem = createTreeViewItem;
            CreateTypography = createTypography;
        }
    }

    public class OverrideVisualComponentsProvider : BaseComponent
    {
        private readonly OverrideVisualComponents _overrideVisualComponents;
        public OverrideVisualComponentsProvider(
            VirtualBody children,
            OverrideVisualComponents overrideVisualComponents
        ) : base(children)
        {
            _overrideVisualComponents = overrideVisualComponents;
        }

        public override VirtualBody Render()
        {
            return F.ContextProvider(
                value: _overrideVisualComponents,
                children: Children
            );
        }
    }
}