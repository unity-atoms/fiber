using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Fiber;
using FiberUtils;

namespace Fiber.GameObjects
{
    public static partial class BaseComponentExtensions
    {
        public static GameObjectPointerEventsManager GameObjectPointerEventsManager(
            this BaseComponent component,
            List<VirtualNode> children,
            Ref<Camera> currentCameraRef = null
        )
        {
            return new GameObjectPointerEventsManager(
                children: children,
                currentCameraRef: currentCameraRef
            );
        }

        public static GameObjectPointerEventsComponent GameObjectPointerEvents(
            this BaseComponent component,
            Action onPointerEnter = null,
            Action onPointerExit = null,
            Action onPointerStay = null,
            Action onClick = null
        )
        {
            return new GameObjectPointerEventsComponent(
                onPointerEnter: onPointerEnter,
                onPointerExit: onPointerExit,
                onPointerStay: onPointerStay,
                onClick: onClick
            );
        }
    }

    public class GameObjectPointerEventsComponent : BaseComponent
    {
        public Action OnPointerEnter { get; set; }
        public Action OnPointerExit { get; set; }
        public Action OnPointerStay { get; set; }
        public Action OnClick { get; set; }

        public GameObjectPointerEventsComponent(
            Action onPointerEnter = null,
            Action onPointerExit = null,
            Action onPointerStay = null,
            Action onClick = null
        ) : base(null)
        {
            OnPointerEnter = onPointerEnter;
            OnPointerExit = onPointerExit;
            OnPointerStay = onPointerStay;
            OnClick = onClick;
        }

        public override VirtualNode Render()
        {
            var parentGO = F.GetParentGameObject();

            F.CreateEffect(() =>
            {
                var subId = C<GameObjectPointerEventsContext>().Subscribe(
                    transform: parentGO.transform,
                    onPointerEnter: OnPointerEnter,
                    onPointerExit: OnPointerExit,
                    onPointerStay: OnPointerStay,
                    onClick: OnClick
                );

                return () =>
                {
                    C<GameObjectPointerEventsContext>().Unsubscribe(subId);
                };
            });

            return null;
        }
    }

    public class GameObjectPointerEventsContext
    {
        public class Subscriber
        {
            public Transform Transform;
            public Action OnPointerEnter;
            public Action OnPointerExit;
            public Action OnPointerStay;
            public Action OnClick;
            public bool IsCurrentlyHit;

            public Subscriber(
                Transform transform,
                Action onPointerEnter,
                Action onPointerExit,
                Action onPointerStay,
                Action onClick
            )
            {
                Transform = transform;
                OnPointerEnter = onPointerEnter;
                OnPointerExit = onPointerExit;
                OnPointerStay = onPointerStay;
                OnClick = onClick;
                IsCurrentlyHit = false;
            }
        }

        private IntIdGenerator _idGenerator;
        private IndexedDictionary<int, Subscriber> _subscribers = new();
        public int SubscriberCount => _subscribers.Count;

        public GameObjectPointerEventsContext()
        {
            _idGenerator = new IntIdGenerator();
        }

        public Subscriber GetSubscriber(int index)
        {
            return _subscribers.GetByIndex(index);
        }
        public int Subscribe(Transform transform, Action onPointerEnter, Action onPointerExit, Action onPointerStay, Action onClick)
        {
            var id = _idGenerator.NextId();
            _subscribers.Add(id, new Subscriber(transform, onPointerEnter, onPointerExit, onPointerStay, onClick));
            return id;
        }
        public void Unsubscribe(int id)
        {
            _subscribers.Remove(id);
        }
    }

    public class GameObjectPointerEventsManager : BaseComponent
    {
        private Ref<Camera> _currentCameraRef;
        private GameObjectPointerEventsContext _context;

        public GameObjectPointerEventsManager(List<VirtualNode> children, Ref<Camera> currentCameraRef = null) : base(children)
        {
            _currentCameraRef = currentCameraRef;
            _context = new GameObjectPointerEventsContext();
        }

        public override VirtualNode Render()
        {
            F.CreateUpdateEffect(Update);

            return ContextProvider<GameObjectPointerEventsContext>(
                value: _context,
                children: children
            );
        }

        private Camera GetCurrentCamera()
        {
            if (_currentCameraRef != null && _currentCameraRef.Current != null)
            {
                return _currentCameraRef.Current;
            }

            return Camera.main;
        }

        private void Update(float deltaTime)
        {
            var currentCamera = GetCurrentCamera();

            if (currentCamera == null)
            {
                return;
            }

            var canHit = EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject();
            // OPEN POINT: We need to replace this in the future using a more robust solution
            // instead of just getting the current mouse position. Look into how to make Input Actions
            // part of Fiber.
            var pointerPos = Mouse.current.position.ReadValue();
            var ray = currentCamera.ScreenPointToRay(pointerPos);

            Physics.Raycast(ray, out RaycastHit hit3D);
            var hit2D = Physics2D.Raycast(ray.origin, ray.direction);

            for (var i = 0; i < _context.SubscriberCount; ++i)
            {
                var subscriber = _context.GetSubscriber(i);
                if (canHit && (subscriber.Transform.IsAncestor(hit3D.transform) || subscriber.Transform.IsAncestor(hit2D.transform)))
                {
                    if (!subscriber.IsCurrentlyHit)
                    {
                        subscriber.IsCurrentlyHit = true;
                        subscriber.OnPointerEnter?.Invoke();
                    }
                    else
                    {
                        subscriber.OnPointerStay?.Invoke();
                    }

                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        subscriber.OnClick?.Invoke();
                    }
                }
                else if (subscriber.IsCurrentlyHit)
                {
                    subscriber.IsCurrentlyHit = false;
                    subscriber.OnPointerExit?.Invoke();
                }
            }
        }
    }
}