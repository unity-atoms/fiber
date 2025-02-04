using System.Collections.Generic;
using UnityEngine;

namespace FiberUtils
{
    public class ObjectPool<T> where T : new()
    {
        protected Queue<T> _pooledObjects;

        public ObjectPool(int initialCapacity = 10)
        {
            _pooledObjects = new Queue<T>(initialCapacity);
        }

        public virtual void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _pooledObjects.Enqueue(new T());
            }
        }

        public virtual T Get()
        {
            if (_pooledObjects.Count == 0)
            {
                return new T();
            }

            return _pooledObjects.Dequeue();
        }

        public virtual void Release(T pooledObject)
        {
            if (pooledObject == null)
            {
                Debug.LogWarning("Tried to release an object that was null.");
                return;
            }
            if (_pooledObjects.Contains(pooledObject))
            {
                Debug.LogWarning("Tried to release an object that already was pooled.");
                return;
            }
            _pooledObjects.Enqueue(pooledObject);
        }

        public virtual void Clear()
        {
            _pooledObjects.Clear();
        }
    }

    public class ListPool<T> : ObjectPool<List<T>>
    {
        public override void Release(List<T> pooledObject)
        {
            if (pooledObject == null || _pooledObjects.Contains(pooledObject)) return;
            pooledObject.Clear();
            _pooledObjects.Enqueue(pooledObject);
        }
    }
}