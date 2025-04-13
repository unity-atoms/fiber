using System;

namespace FiberUtils
{
    public class ObjectPool<T> : BaseObjectPool<T>, IPreload
        where T : new()
    {
        public ObjectPool(int initialCapacity = 10, Action<T> onRelease = null, bool preload = false) : base(initialCapacity, onRelease)
        {
            if (preload)
            {
                PoolingPreloadScheduler.Instance.SchedulePreload(this, initialCapacity);
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