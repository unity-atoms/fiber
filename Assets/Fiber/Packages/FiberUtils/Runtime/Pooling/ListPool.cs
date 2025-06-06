using System;
using System.Collections.Generic;

namespace FiberUtils
{
    public class ListPool<T> : BaseObjectPool<List<T>>, IPreload
    {
        private static void OnRelease(List<T> list)
        {
            list.Clear();
        }

        public ListPool(int initialCapacity = 10, bool preload = false) : base(initialCapacity, OnRelease)
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
                var list = new List<T>();
                _currentPool.Enqueue(list);
                _allObjectsCreated.Add(list);
            }
        }


        public List<T> Get()
        {
            if (_currentPool.Count == 0)
            {
                var list = new List<T>();
                _allObjectsCreated.Add(list);
                return list;
            }

            return _currentPool.Dequeue();
        }
    }
}