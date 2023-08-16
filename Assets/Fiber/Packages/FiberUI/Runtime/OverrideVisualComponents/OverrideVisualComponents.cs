using System.Collections.Generic;
using UnityEngine.UIElements;
using Fiber.UIElements;

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

        // Icon button component

        // TreeView component

        // Typography component

        public OverrideVisualComponents(
            CreateHeaderContainerDelegate createHeaderContainer = null,
            CreateHeaderItemGroupDelegate createHeaderItemGroup = null
        )
        {
            CreateHeaderContainer = createHeaderContainer;
            CreateHeaderItemGroup = createHeaderItemGroup;
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