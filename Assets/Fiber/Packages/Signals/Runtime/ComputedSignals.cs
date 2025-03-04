using System;
using System.Collections.Generic;
using UnityEngine;
using FiberUtils;

namespace Signals
{
    [Serializable]
    public abstract class BaseComputedSignal<RT> : BaseSignal<RT>
    {
        [SerializeField]
        protected RT _lastValue = default;
        public RT LastValue { get => _lastValue; }

        protected virtual void Cleanup(RT previousValue) { }
    }

    [Serializable]
    public class DynamicDependencies<T>
    {
        private ISignal _dependent;
        public List<ISignal<T>> Signals { get => _signals; }
        private List<ISignal<T>> _signals;
        private int _count = 0;

        public DynamicDependencies() { }

        public DynamicDependencies(ISignal dependent, IList<ISignal<T>> dependencies = null)
        {
            Initialize(dependent, dependencies);
        }

        ~DynamicDependencies()
        {
            Dispose();
        }

        public void Dispose()
        {
            for (int i = 0; i < _count; ++i)
            {
                _signals[i].UnregisterDependent(_dependent);
            }
            _signals.Clear();
        }

        public void Initialize(ISignal dependent, IList<ISignal<T>> dependencies = null)
        {
            _dependent = dependent;
            _signals = dependencies != null ? new(dependencies) : new();
            for (var i = 0; i < _signals.Count; ++i)
            {
                var signal = _signals[i];
                signal.RegisterDependent(_dependent);
                _count++;
            }
        }

        public void Add<ST>(ST signal) where ST : ISignal<T>
        {
            _signals.Add(signal);
            signal.RegisterDependent(_dependent);
            _count++;
        }

        public void Remove<ST>(ST signal) where ST : ISignal<T>
        {
            var index = _signals.IndexOf(signal);
            signal.UnregisterDependent(_dependent);
            _signals.RemoveAt(index);
            _count--;
        }

        public void RemoveAt(int index)
        {
            var signal = _signals[index];
            signal.UnregisterDependent(_dependent);
            _signals.RemoveAt(index);
            _count--;
        }

        public T GetValue(int index)
        {
            return _signals[index].Get();
        }

        public ISignal<T> GetSignal(int index)
        {
            return _signals[index];
        }

        public int Count => _count;
    }

    [Serializable]
    public abstract class DynamicComputedSignal<DT, RT> : BaseComputedSignal<RT>
    {
        protected readonly DynamicDependencies<DT> _dynamicDependencies;
        private byte _lastDirtyBit;

        public DynamicComputedSignal(IList<ISignal<DT>> dynamicDependencies = null) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _dynamicDependencies = new(dependent: this, dynamicDependencies);
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;

                _lastValue = Compute(_dynamicDependencies);

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(DynamicDependencies<DT> dynamicDependencies);
    }


    [Serializable]
    public abstract class DynamicComputedSignal<T1, DT, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected readonly DynamicDependencies<DT> _dynamicDependencies;
        private byte _lastDirtyBit;

        public DynamicComputedSignal(ISignal<T1> signal1, IList<ISignal<DT>> dynamicDependencies = null) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _dynamicDependencies = new(dependent: this, dynamicDependencies);
        }

        public void UpdateDeps(
            ISignal<T1> signal1
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }

            SetDirty();
        }

        ~DynamicComputedSignal()
        {
            _signal1.UnregisterDependent(this);
        }


        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _dynamicDependencies);

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, DynamicDependencies<DT> dynamicSignals);
    }

    [Serializable]
    public abstract class DynamicComputedSignal<T1, T2, DT, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        protected readonly DynamicDependencies<DT> _dynamicDependencies;
        private byte _lastDirtyBit;

        public DynamicComputedSignal(ISignal<T1> signal1, ISignal<T2> signal2, IList<ISignal<DT>> dynamicDependencies = null) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _signal2 = signal2;
            _signal2.RegisterDependent(this);
            _dynamicDependencies = new(dependent: this, dynamicDependencies);
        }

        ~DynamicComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _dynamicDependencies);

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, DynamicDependencies<DT> dynamicSignals);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        private byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        private byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);

            _signal2 = signal2;
            _signal2.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);

            _signal2 = signal2;
            _signal2.RegisterDependent(this);

            _signal3 = signal3;
            _signal3.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _signal3.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        protected ISignal<T3> _signal3;
        protected ISignal<T4> _signal4;
        protected byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);

            _signal2 = signal2;
            _signal2.RegisterDependent(this);

            _signal3 = signal3;
            _signal3.RegisterDependent(this);

            _signal4 = signal4;
            _signal4.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
            _signal4.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _signal3.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _signal4.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, T5, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        protected ISignal<T3> _signal3;
        protected ISignal<T4> _signal4;
        protected ISignal<T5> _signal5;
        protected byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);

            _signal2 = signal2;
            _signal2.RegisterDependent(this);

            _signal3 = signal3;
            _signal3.RegisterDependent(this);

            _signal4 = signal4;
            _signal4.RegisterDependent(this);

            _signal5 = signal5;
            _signal5.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
            _signal4.UnregisterDependent(this);
            _signal5.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _signal3.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _signal4.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal5, signal5))
            {
                _signal5?.UnregisterDependent(this);
                _signal5 = signal5;
                _signal5.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get(), _signal5.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, T5, T6, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        protected ISignal<T3> _signal3;
        protected ISignal<T4> _signal4;
        protected ISignal<T5> _signal5;
        protected ISignal<T6> _signal6;
        protected byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);

            _signal2 = signal2;
            _signal2.RegisterDependent(this);

            _signal3 = signal3;
            _signal3.RegisterDependent(this);

            _signal4 = signal4;
            _signal4.RegisterDependent(this);

            _signal5 = signal5;
            _signal5.RegisterDependent(this);

            _signal6 = signal6;
            _signal6.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
            _signal4.UnregisterDependent(this);
            _signal5.UnregisterDependent(this);
            _signal6.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _signal3.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _signal4.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal5, signal5))
            {
                _signal5?.UnregisterDependent(this);
                _signal5 = signal5;
                _signal5.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal6, signal6))
            {
                _signal6?.UnregisterDependent(this);
                _signal6 = signal6;
                _signal6.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get(), _signal5.Get(), _signal6.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected ISignal<T2> _signal2;
        protected ISignal<T3> _signal3;
        protected ISignal<T4> _signal4;
        protected ISignal<T5> _signal5;
        protected ISignal<T6> _signal6;
        protected ISignal<T7> _signal7;
        protected byte _lastDirtyBit;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6,
            ISignal<T7> signal7
        ) : base()
        {
            _lastDirtyBit = (byte)(_dirtyBit - 1);

            _signal1 = signal1;
            _signal1.RegisterDependent(this);

            _signal2 = signal2;
            _signal2.RegisterDependent(this);

            _signal3 = signal3;
            _signal3.RegisterDependent(this);

            _signal4 = signal4;
            _signal4.RegisterDependent(this);

            _signal5 = signal5;
            _signal5.RegisterDependent(this);

            _signal6 = signal6;
            _signal6.RegisterDependent(this);

            _signal7 = signal7;
            _signal7.RegisterDependent(this);
        }

        ~ComputedSignal()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
            _signal4.UnregisterDependent(this);
            _signal5.UnregisterDependent(this);
            _signal6.UnregisterDependent(this);
            _signal7.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6,
            ISignal<T7> signal7
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _signal1.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _signal2.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _signal3.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _signal4.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal5, signal5))
            {
                _signal5?.UnregisterDependent(this);
                _signal5 = signal5;
                _signal5.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal6, signal6))
            {
                _signal6?.UnregisterDependent(this);
                _signal6 = signal6;
                _signal6.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal7, signal7))
            {
                _signal7?.UnregisterDependent(this);
                _signal7 = signal7;
                _signal7.RegisterDependent(this);
            }

            SetDirty();
        }

        public override RT Get()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get(), _signal5.Get(), _signal6.Get(), _signal7.Get());

                Cleanup(previousValue);
                _lastDirtyBit = _dirtyBit;
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7);
    }

    public abstract class ComputedSignalsByKey<
        Key,
        KeysSignal,
        Keys,
        ItemSignal,
        ItemType
    > : DynamicComputedSignal<
        Keys,
        ItemType,
        IndexedDictionary<Key, ItemSignal>
    >
        where Key : IEquatable<Key>
        where KeysSignal : ISignal<Keys>
        where Keys : IList<Key>
        where ItemSignal : ISignal<ItemType>
    {
        IndexedDictionary<Key, ItemSignal> _signalsByKey;

        public ComputedSignalsByKey(KeysSignal keysSignal) : base(keysSignal)
        {
            _signalsByKey = new();
        }

        public ItemType GetValue(Key key)
        {
            if (!_signalsByKey.ContainsKey(key))
            {
                Get(); // Test if the value gets created after a recomputation
                return !_signalsByKey.ContainsKey(key) ? default : _signalsByKey[key].Get();
            }
            return _signalsByKey[key].Get();
        }

        public ItemSignal GetSignal(Key key)
        {
            if (!_signalsByKey.ContainsKey(key))
            {
                Get(); // Test if the value gets created after a recomputation
                return !_signalsByKey.ContainsKey(key) ? default : _signalsByKey[key];
            }
            return _signalsByKey[key];
        }

        protected override IndexedDictionary<Key, ItemSignal> Compute(Keys keys, DynamicDependencies<ItemType> dynamicDependencies)
        {
            // Add new keys
            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (!_signalsByKey.ContainsKey(key))
                {
                    var itemSignal = CreateItemSignal(key);
                    dynamicDependencies.Add(itemSignal);
                    _signalsByKey.Add(key, itemSignal);
                }
                else
                {
                    UpdateItemSignal(key, _signalsByKey[key]);
                }
            }

            // Remove old keys
            for (var i = _signalsByKey.Count - 1; i >= 0; --i)
            {
                var kvp = _signalsByKey.GetKVPAt(i);
                if (!keys.Contains(kvp.Key))
                {
                    dynamicDependencies.RemoveAt(i);
                    _signalsByKey.Remove(kvp.Key);
                    CleanupItemSignal(kvp.Key, kvp.Value);
                }
            }

            return _signalsByKey;
        }

        protected abstract ItemSignal CreateItemSignal(Key key);
        protected virtual void CleanupItemSignal(Key key, ItemSignal itemSignal) { }
        protected virtual void UpdateItemSignal(Key key, ItemSignal itemSignal) { }
    }

    public class NegatedBoolSignal : ComputedSignal<bool, bool>
    {
        public NegatedBoolSignal(ISignal<bool> signal) : base(signal) { }
        protected override bool Compute(bool value)
        {
            return !value;
        }
    }

    public class IntToStringSignal : ComputedSignal<int, string>
    {
        public IntToStringSignal(ISignal<int> signal) : base(signal) { }
        protected override string Compute(int value)
        {
            return value.ToString();
        }
    }

    public class IsNullSignal<T> : ComputedSignal<T, bool> where T : class
    {
        public IsNullSignal(ISignal<T> signal) : base(signal) { }
        protected override bool Compute(T value)
        {
            return value == null;
        }
    }

    public class IsNotNullSignal<T> : ComputedSignal<T, bool> where T : class
    {
        public IsNotNullSignal(ISignal<T> signal) : base(signal) { }
        protected override bool Compute(T value)
        {
            return value != null;
        }
    }
}