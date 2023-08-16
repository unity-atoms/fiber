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
        // Header component
        public delegate BaseComponent CreateHeaderContainerDelegate(
            List<VirtualNode> children,
            string role,
            string variant = null,
            Style style = new()
        );
        public CreateHeaderContainerDelegate CreateHeaderContainer { get; private set; }

        public delegate BaseComponent CreateHeaderItemGroupDelegate(
            List<VirtualNode> children,
            Justify justifyContent = Justify.FlexStart,
            Style style = new()
        );
        public CreateHeaderItemGroupDelegate CreateHeaderItemGroup { get; private set; }

        // Icon component
        public delegate BaseComponent CreateIconDelegate(
            SignalProp<string> type,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new(),
            Ref<TextElement> forwardRef = null
        );
        public CreateIconDelegate CreateIcon { get; private set; }

        // Icon button component
        public delegate BaseComponent CreateIconButtonDelegate(
            SignalProp<string> type,
            Action onPress,
            InteractiveRef<TextElement> interactiveRef,
            string role = Constants.INHERIT_ROLE,
            string variant = null,
            Style style = new()
        );
        public CreateIconButtonDelegate CreateIconButton { get; private set; }

        // TreeView component

        // Typography component

        public OverrideVisualComponents(
            CreateHeaderContainerDelegate createHeaderContainer = null,
            CreateHeaderItemGroupDelegate createHeaderItemGroup = null,
            CreateIconDelegate createIcon = null,
            CreateIconButtonDelegate createIconButton = null
        )
        {
            CreateHeaderContainer = createHeaderContainer;
            CreateHeaderItemGroup = createHeaderItemGroup;
            CreateIcon = createIcon;
            CreateIconButton = createIconButton;
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