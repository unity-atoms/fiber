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
            bool autonomousWorkLoop = true,
            long workLoopTimeBudgetMs = Renderer.DEFAULT_WORK_LOOP_TIME_BUDGET_MS
        )
        {
            var uiElementsRendererExtension = new UIElementsRendererExtension(defaultPanelSettings);
            var gameObjectRendererExtension = new GameObjectsRendererExtension();
            _renderer = new Fiber.Renderer(
                rendererExtensions: new List<RendererExtension>()
                {
                    uiElementsRendererExtension,
                    gameObjectRendererExtension,
                },
                globals: globals
            );
            _rootGameObjectNativeNode = new GameObjectNativeNode(new GameObjectComponent(), rootGameObject, gameObjectRendererExtension);
        }

        public void Render(VirtualNode component)
        {
            _renderer.Render(component, _rootGameObjectNativeNode);
        }

        public void Unmount()
        {
            _renderer.Unmount();
        }
    }
}

