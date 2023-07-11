using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fiber.GameObjects
{
    public static class BaseComponentExtensions
    {
        public static GameObjectComponent GameObject(
            this BaseComponent component,
            string name = null,
            bool active = true,
            Ref<GameObject> _ref = null,
            Action<GameObject> onCreateRef = null,
            Action<GameObject> onMount = null,
            Func<GameObject, GameObject> getInstance = null,
            Action<GameObject> removeInstance = null,
            List<VirtualNode> children = null
        )
        {
            return new GameObjectComponent(
                name: name,
                active: active,
                _ref: _ref,
                onCreateRef: onCreateRef,
                onMount: onMount,
                getInstance: getInstance,
                removeInstance: removeInstance,
                children: children
            );
        }
    }
    public class GameObjectComponent : VirtualNode
    {
        public SignalProp<string> Name { get; private set; }
        public SignalProp<bool> Active { get; private set; }

        public Ref<GameObject> Ref { get; set; }
        public Action<GameObject> OnCreateRef { get; set; }
        // At a glance it might seem that this can be replaced with an effect. However, an effect in a component
        // that is defining a GameObjectComponent will run before we mount the GameObjectComponent, which means
        // that it is not enabled and not attached to its parent yet. 
        public Action<GameObject> OnMount { get; set; }

        // Function that is called when creating the instance. Can be used for using already existing game objects instead of creating new ones.
        public Func<GameObject, GameObject> GetInstance { get; set; }
        public Action<GameObject> RemoveInstance { get; set; }

        public GameObjectComponent() : base(null) { }
        public GameObjectComponent(List<VirtualNode> children) : base(children) { }

        public GameObjectComponent(
            SignalProp<string> name = new(),
            SignalProp<bool> active = new(),
            Ref<GameObject> _ref = null,
            Action<GameObject> onCreateRef = null,
            Action<GameObject> onMount = null,
            Func<GameObject, GameObject> getInstance = null,
            Action<GameObject> removeInstance = null,
            List<VirtualNode> children = null
        ) : base(children)
        {
            Name = name;
            Active = !active.IsEmpty ? active : true;

            Ref = _ref;
            OnCreateRef = onCreateRef;
            OnMount = onMount;

            GetInstance = getInstance;
            RemoveInstance = removeInstance;
        }
    }

    public class GameObjectNativeNode : NativeNode
    {
        public GameObject Instance { get; private set; }
        private GameObjectsRendererExtension _rendererExtension;
        private WorkLoopSignalProp<string> _nameWorkLoopItem;
        private WorkLoopSignalProp<bool> _activeWorkLoopItem;
        protected bool VisibilitySetFromFiber { get; set; }

        public GameObjectNativeNode(GameObjectComponent virtualNode, GameObject instance, GameObjectsRendererExtension rendererExtension)
        {
            Instance = instance;
            _rendererExtension = rendererExtension;

            if (!virtualNode.Name.IsEmpty)
            {
                Instance.name = virtualNode.Name.Get();
                _nameWorkLoopItem = new(virtualNode.Name);
            }
            if (!virtualNode.Active.IsEmpty)
            {
                _activeWorkLoopItem = new(virtualNode.Active);
            }
            if (virtualNode.Ref != null)
            {
                virtualNode.Ref.Current = instance;
            }
            if (virtualNode.OnCreateRef != null)
            {
                virtualNode.OnCreateRef(instance);
            }
        }

        public override void SetVisible(bool visible)
        {
            VisibilitySetFromFiber = visible;
            UpdateVisibility();
        }

        protected void UpdateVisibility()
        {
            Instance.SetActive(VisibilitySetFromFiber && _activeWorkLoopItem.Get());
        }

        public override void AddChild(FiberNode node, int index)
        {
            if (node.NativeNode is GameObjectNativeNode goChildNode)
            {
                var go = goChildNode.Instance;
                go.transform.SetParent(Instance.transform);
                if (go.transform.GetSiblingIndex() != index)
                {
                    go.transform.SetSiblingIndex(index);
                }
                var goChildVirtualNode = (GameObjectComponent)node.VirtualNode;
                if (goChildVirtualNode.OnMount != null)
                {
                    goChildVirtualNode.OnMount(go);
                }
                return;
            }

            throw new Exception($"Trying to add child of unknown type {node.VirtualNode.GetType()} at index {index}.");
        }

        public override void RemoveChild(FiberNode node)
        {
            if (node.NativeNode is GameObjectNativeNode goChildNode)
            {
                var goChildVirtualNode = (GameObjectComponent)node.VirtualNode;
                if (goChildVirtualNode.RemoveInstance != null)
                {
                    goChildVirtualNode.RemoveInstance(goChildNode.Instance);
                    return;
                }
                MonoBehaviour.Destroy(goChildNode.Instance);
                return;
            }

            throw new Exception($"Trying to remove child of unknown type {node.VirtualNode.GetType()}.");
        }

        public override void MoveChild(FiberNode node, int index)
        {
            if (node.NativeNode is GameObjectNativeNode goChildNode)
            {
                var currentIndex = Instance.transform.GetSiblingIndex();
                if (currentIndex != index)
                {
                    Instance.transform.SetSiblingIndex(index);
                }
            }
        }

        public override void WorkLoop()
        {
            if (_nameWorkLoopItem.Check())
            {
                Instance.name = _nameWorkLoopItem.Get();
            }
            if (_activeWorkLoopItem.Check())
            {
                UpdateVisibility();
            }
        }
    }

    public class GameObjectsRendererExtension : RendererExtension
    {
        public GameObjectsRendererExtension()
        {
        }

        protected GameObject GetOrCreateGameObject(FiberNode fiberNode, GameObjectComponent virtualNode)
        {
            GameObject gameObject = null;

            // Try get GameObject using GetInstance prop
            if (gameObject == null && virtualNode.GetInstance != null)
            {
                var parentNode = fiberNode.FindClosestAncestorWithNativeNode();
                gameObject = virtualNode.GetInstance(parentNode != null ? (parentNode.NativeNode as GameObjectNativeNode).Instance : null);
            }

            if (gameObject == null)
            {
                gameObject = new GameObject();
            }

            return gameObject;
        }

        public override NativeNode CreateNativeNode(FiberNode fiberNode)
        {
            var virtualNode = fiberNode.VirtualNode;

            if (virtualNode is GameObjectComponent gameObjectVirtualNode)
            {
                var gameObject = GetOrCreateGameObject(fiberNode, gameObjectVirtualNode);
                return new GameObjectNativeNode(gameObjectVirtualNode, gameObject, this);
            }

            return null;
        }
    }
}