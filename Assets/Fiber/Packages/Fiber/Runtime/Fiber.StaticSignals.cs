using Signals;
using System.Collections.Generic;

namespace Fiber
{
    public static class FiberStaticSignals<T>
    {
        private static readonly Dictionary<T, StaticSignal<T>> _signals = new();
        private static readonly StaticSignal<T> _defaultSignal = new(default);
        public static StaticSignal<T> Default => _defaultSignal;

        public static StaticSignal<T> Get(T item)
        {
            if (_signals.ContainsKey(item))
            {
                return _signals[item];
            }

            var signal = new StaticSignal<T>(item);
            _signals.Add(item, signal);
            return signal;
        }
    }
}