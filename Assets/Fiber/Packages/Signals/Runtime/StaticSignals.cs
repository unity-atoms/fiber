using System;
using UnityEngine;

namespace Signals
{
    [Serializable]
    public class StaticSignal<T> : BaseSignal<T>
    {
        [SerializeField]
        protected T _value;

        public StaticSignal() { }

        public StaticSignal(T value)
        {
            _value = value;
        }

        public override sealed T Get() => _value;
    }

    public static class StaticSignals
    {
        public readonly static StaticSignal<bool> TRUE = new(true);
        public readonly static StaticSignal<bool> FALSE = new(false);
        public readonly static StaticSignal<int> ZERO = new(0);
        public readonly static StaticSignal<string> NULL_STRING = new(null);
        public readonly static StaticSignal<string> EMPTY_STRING = new(string.Empty);
    }
}