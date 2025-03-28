using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Fiber.GameObjects;
using Fiber.UIElements;

namespace Fiber.Suite
{
    public class FiberSuite
    {
        private Renderer _renderer;
        private GameObjectNativeNode _rootGameObjectNativeNode;
        public bool IsMounted { get => _renderer.IsMounted; }

        public FiberSuite(
            GameObject rootGameObject,
            Dictionary<Type, object> globals = null,
            PanelSettings defaultPanelSettings = null,
            VisibilityStyleProp defaultVisibilityStyleProp = VisibilityStyleProp.Translate,
            bool autonomousWorkLoop = true
        )
        {
            var uiElementsRendererExtension = new UIElementsRendererExtension(defaultPanelSettings, defaultVisibilityStyleProp);
            var gameObjectRendererExtension = new GameObjectsRendererExtension();
            _renderer = new Fiber.Renderer(
                rendererExtensions: new List<RendererExtension>()
                {
                    uiElementsRendererExtension,
                    gameObjectRendererExtension,
                },
                globals: globals,
                autonomousWorkLoop: autonomousWorkLoop
            );
            _rootGameObjectNativeNode = new GameObjectNativeNode(new GameObjectComponent(), rootGameObject);
        }

        public void Render(VirtualNode component)
        {
            _renderer.Render(component, _rootGameObjectNativeNode);
        }

        public void Unmount(bool immediatelyExecuteRemainingWork = true)
        {
            _renderer.Unmount(immediatelyExecuteRemainingWork);
        }
    }
}

