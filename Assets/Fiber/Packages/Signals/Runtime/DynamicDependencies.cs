using System;
using System.Collections.Generic;
using FiberUtils;

namespace Signals
{
    [Serializable]
    public class DynamicDependencies<T>
    {
        public static readonly ObjectPool<DynamicDependencies<T>> Pool = new(InitialCapacityConstants.SMALL, (dynamicDeps) => { dynamicDeps.Dispose(); }, preload: false);

        private ISignal _dependent;
        public List<ISignal<T>> Signals { get => _signals; }
        private List<ISignal<T>> _signals;

        // Used when pooling
        public DynamicDependencies() { }

        public DynamicDependencies(ISignal dependent, IList<ISignal<T>> dependencies = null)
        {
            Initialize(dependent, dependencies);
        }

        public void Dispose()
        {
            for (int i = 0; i < _signals.Count; ++i)
            {
                _signals[i].UnregisterDependent(_dependent);
            }

            Pooling<T>.ISignalListPool.TryRelease(_signals);
        }

        public void Initialize(ISignal dependent, IList<ISignal<T>> dependencies = null)
        {
            _dependent = dependent;
            _signals = Pooling<T>.ISignalListPool.Get();
            if (dependencies != null)
            {
                _signals.AddRange(dependencies);
            }
            for (var i = 0; i < _signals.Count; ++i)
            {
                var signal = _signals[i];
                signal.RegisterDependent(_dependent);
            }
        }

        public void Add<ST>(ST signal) where ST : ISignal<T>
        {
            _signals.Add(signal);
            signal.RegisterDependent(_dependent);
        }

        public void Remove<ST>(ST signal) where ST : ISignal<T>
        {
            var index = _signals.IndexOf(signal);
            signal.UnregisterDependent(_dependent);
            _signals.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            var signal = _signals[index];
            signal.UnregisterDependent(_dependent);
            _signals.RemoveAt(index);
        }

        public T GetValue(int index)
        {
            return _signals[index].Get();
        }

        public ISignal<T> GetSignal(int index)
        {
            return _signals[index];
        }

        public int Count => _signals.Count;
    }
}