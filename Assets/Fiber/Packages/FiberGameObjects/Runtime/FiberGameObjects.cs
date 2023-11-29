using System;
using System.Collections.Generic;
using UnityEngine;
using Signals;

namespace Fiber.GameObjects
{
    public static partial class BaseComponentExtensions
    {
        public static GameObjectPortalDestinationComponent GameObjectPortalDestination(
            this BaseComponent component,
            string id
        )
        {
            return new GameObjectPortalDestinationComponent(id: id);
        }

        public static GameObjectComponent GameObject(
            this BaseComponent component,
            string name = null,
            bool active = true,
            SignalProp<Vector3> position = new(),
            SignalProp<Vector3> localScale = new(),
            PrimitiveType? primitiveType = null,
            Ref<GameObject> _ref = null,
            Action<GameObject> onCreateRef = null,
            Action<GameObject> onMount = null,
            Func<GameObject, GameObject> getInstance = null,
            Action<GameObject> removeInstance = null,
            VirtualBody children = default
        )
        {
            return new GameObjectComponent(
                name: name,
                active: active,
                _ref: _ref,
                position: position,
                localScale: localScale,
                primitiveType: primitiveType,
                onCreateRef: onCreateRef,
                onMount: onMount,
                getInstance: getInstance,
                removeInstance: removeInstance,
                children: children
            );
        }

        public static GameObject GetParentGameObject(
            this BaseComponent component
        )
        {
            var parentNativeNode = component.GetParentNativeNode();
            if (parentNativeNode != null && parentNativeNode is GameObjectNativeNode gameObjectNativeNode)
            {
                return gameObjectNativeNode.Instance;
            }

            return null;
        }
    }

    public class GameObjectPortalDestinationComponent : PortalDestinationBaseComponent, IBuiltInComponent
    {
        public GameObjectPortalDestinationComponent(string id) : base(id) { }
        public VirtualBody Render(FiberNode fiberNode)
        {
            BaseImplementation(fiberNode);
            return new GameObjectComponent();
        }
    }

    public class GameObjectComponent : VirtualNode
    {
        public SignalProp<string> Name { get; private set; }
        public SignalProp<bool> Active { get; private set; }

        public SignalProp<Vector3> Position { get; private set; }
        public SignalProp<Vector3> LocalScale { get; private set; }

        public PrimitiveType? PrimitiveType { get; private set; }

        public Ref<GameObject> Ref { get; set; }
        public Action<GameObject> OnCreateRef { get; set; }
        // At a glance it might seem that this can be replaced with an effect. However, an effect in a component
        // that is defining a GameObjectComponent will run before we mount the GameObjectComponent, which means
        // that it is not enabled and not attached to its parent yet. 
        public Action<GameObject> OnMount { get; set; }

        // Function that is called when creating the instance. Can be used for using already existing game objects instead of creating new ones.
        public Func<GameObject, GameObject> GetInstance { get; set; }
        public Action<GameObject> RemoveInstance { get; set; }

        public GameObjectComponent() : base() { }
        public GameObjectComponent(VirtualBody children) : base(children) { }

        public GameObjectComponent(
            SignalProp<string> name = new(),
            SignalProp<bool> active = new(),
            SignalProp<Vector3> position = new(),
            SignalProp<Vector3> localScale = new(),
            PrimitiveType? primitiveType = null,
            Ref<GameObject> _ref = null,
            Action<GameObject> onCreateRef = null,
            Action<GameObject> onMount = null,
            Func<GameObject, GameObject> getInstance = null,
            Action<GameObject> removeInstance = null,
            VirtualBody children = new()
        ) : base(children)
        {
            Name = name;
            Active = !active.IsEmpty ? active : true;

            Position = position;
            LocalScale = localScale;

            PrimitiveType = primitiveType;

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
        private WorkLoopSignalProp<string> _nameWorkLoopItem;
        private WorkLoopSignalProp<bool> _activeWorkLoopItem;
        private WorkLoopSignalProp<Vector3> _positionWorkLoopItem;
        private WorkLoopSignalProp<Vector3> _localScaleWorkLoopItem;
        protected bool IsVisible_SetByFiber { get; set; }

        public GameObjectNativeNode(GameObjectComponent virtualNode, GameObject instance)
        {
            Instance = instance;

            if (!virtualNode.Name.IsEmpty)
            {
                Instance.name = virtualNode.Name.Get();
                if (virtualNode.Name.IsSignal)
                {
                    virtualNode.Name.Signal.RegisterDependent(this);
                }
                _nameWorkLoopItem = new(virtualNode.Name);
            }
            if (!virtualNode.Active.IsEmpty)
            {
                // Don't set active here since it will be handled by Fiber when calling SetVisible()
                if (virtualNode.Active.IsSignal)
                {
                    virtualNode.Active.Signal.RegisterDependent(this);
                }
                _activeWorkLoopItem = new(virtualNode.Active);
            }
            if (!virtualNode.Position.IsEmpty)
            {
                Instance.transform.position = virtualNode.Position.Get();
                if (virtualNode.Position.IsSignal)
                {
                    virtualNode.Position.Signal.RegisterDependent(this);
                }
                _positionWorkLoopItem = new(virtualNode.Position);
            }
            if (!virtualNode.LocalScale.IsEmpty)
            {
                Instance.transform.localScale = virtualNode.LocalScale.Get();
                if (virtualNode.LocalScale.IsSignal)
                {
                    virtualNode.LocalScale.Signal.RegisterDependent(this);
                }
                _localScaleWorkLoopItem = new(virtualNode.LocalScale);
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
            IsVisible_SetByFiber = visible;
            UpdateVisibility();
        }

        protected void UpdateVisibility()
        {
            Instance.SetActive(IsVisible_SetByFiber && _activeWorkLoopItem.Get());
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

        public override void Update()
        {
            if (_nameWorkLoopItem.Check())
            {
                Instance.name = _nameWorkLoopItem.Get();
            }
            if (_activeWorkLoopItem.Check())
            {
                UpdateVisibility();
            }
            if (_positionWorkLoopItem.Check())
            {
                Instance.transform.position = _positionWorkLoopItem.Get();
            }
            if (_localScaleWorkLoopItem.Check())
            {
                Instance.transform.localScale = _localScaleWorkLoopItem.Get();
            }
        }

        public override void Cleanup()
        {
            if (_nameWorkLoopItem.IsSignal)
            {
                _nameWorkLoopItem.SignalProp.Signal.UnregisterDependent(this);
            }
            if (_activeWorkLoopItem.IsSignal)
            {
                _activeWorkLoopItem.SignalProp.Signal.UnregisterDependent(this);
            }
            if (_positionWorkLoopItem.IsSignal)
            {
                _positionWorkLoopItem.SignalProp.Signal.UnregisterDependent(this);
            }
            if (_localScaleWorkLoopItem.IsSignal)
            {
                _localScaleWorkLoopItem.SignalProp.Signal.UnregisterDependent(this);
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
                var parentNode = fiberNode.FindClosestAncestorWithNativeNodeOrPortalDestination();
                gameObject = virtualNode.GetInstance(parentNode != null ? (parentNode.NativeNode as GameObjectNativeNode).Instance : null);
            }

            if (gameObject == null)
            {
                if (virtualNode.PrimitiveType != null)
                {
                    gameObject = GameObject.CreatePrimitive(virtualNode.PrimitiveType.Value);
                }
                else
                {
                    gameObject = new GameObject();
                }
            }

            return gameObject;
        }

        public override NativeNode CreateNativeNode(FiberNode fiberNode)
        {
            var virtualNode = fiberNode.VirtualNode;

            if (virtualNode is GameObjectComponent gameObjectVirtualNode)
            {
                var gameObject = GetOrCreateGameObject(fiberNode, gameObjectVirtualNode);
                return new GameObjectNativeNode(gameObjectVirtualNode, gameObject);
            }
            if (virtualNode is GameObjectPointerEventsComponent)
            {
                return null;
            }

            return null;
        }

        public override bool OwnsComponentType(VirtualNode virtualNode)
        {
            var type = virtualNode.GetType();

            return type == typeof(GameObjectComponent)
                || type == typeof(GameObjectPointerEventsManager)
                || type == typeof(GameObjectPointerEventsComponent)
                || type == typeof(MeshRendererComponent);
        }
    }
}