using System;
using System.Collections.Generic;
using UnityEngine;

namespace FiberUtils
{
    public abstract class BaseObjectPool<T>
    {
        protected Queue<T> _currentPool;
        protected HashSet<T> _allObjectsCreated;
        private readonly Action<T> _onRelease;

        public BaseObjectPool(int initialCapacity = 10, Action<T> onRelease = null)
        {
            _currentPool = new Queue<T>(initialCapacity);
            _allObjectsCreated = new HashSet<T>(initialCapacity);
            _onRelease = onRelease;
        }

        public virtual void Release(T pooledObject)
        {
            if (pooledObject == null)
            {
                Debug.LogWarning("Tried to release an object that was null.");
                return;
            }
            if (_currentPool.Contains(pooledObject))
            {
                Debug.LogWarning("Tried to release an object that already was pooled.");
                return;
            }
            if (_allObjectsCreated.Contains(pooledObject))
            {
                // Tried to release an object that was never owned by the pool.
                // Not logging a warning here because this is a valid use case.
                return;
            }
            _onRelease?.Invoke(pooledObject);
            _currentPool.Enqueue(pooledObject);
        }

        public void Clear()
        {
            _currentPool.Clear();
            _allObjectsCreated.Clear();
        }

        public int Count => _currentPool.Count;
    }

    public class ObjectPool<T> : BaseObjectPool<T>, IPreload
        where T : new()
    {
        public ObjectPool(int initialCapacity = 10, Action<T> onRelease = null, bool preload = false) : base(initialCapacity, onRelease)
        {
            if (preload)
            {
                Preload(initialCapacity);
            }
        }

        public void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = new T();
                _currentPool.Enqueue(obj);
                _allObjectsCreated.Add(obj);
            }
        }

        public virtual T Get()
        {
            if (_currentPool.Count == 0)
            {
                var obj = new T();
                _allObjectsCreated.Add(obj);
                return obj;
            }

            return _currentPool.Dequeue();
        }
    }
}