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
        protected virtual bool ShouldSetDirty(RT newValue, RT previousValue) => true;

        protected override sealed void OnNotifySignalUpdate() { }
        public override bool IsDirty(byte otherDirtyBit)
        {
            Get(); // We need to run the signal in order to update the dirty bit
            return DirtyBit != otherDirtyBit;
        }
    }

    [Serializable]
    public class DynamicDependencies<T>
    {
        private readonly ISignal _dependent;
        public List<ISignal<T>> Signals { get => _signals; }
        private readonly List<ISignal<T>> _signals;
        private readonly List<byte> _dirtyBits;
        private int _count = 0;
        private int _previousCount = 0;

        public DynamicDependencies(ISignal dependent, IList<ISignal<T>> dependencies = null, bool initializeDirty = true)
        {
            _dependent = dependent;
            _signals = dependencies != null ? new(dependencies) : new();
            _dirtyBits = new(_signals.Count);
            for (var i = 0; i < _signals.Count; ++i)
            {
                var signal = _signals[i];
                signal.RegisterDependent(_dependent);
                _dirtyBits.Add(initializeDirty ? (byte)(signal.DirtyBit - 1) : signal.DirtyBit);
                _count++;
            }
        }
        ~DynamicDependencies()
        {
            for (int i = 0; i < _count; ++i)
            {
                _signals[i].UnregisterDependent(_dependent);
            }
        }

        public bool IsDirty()
        {
            bool isDirty = _count != _previousCount;
            _previousCount = _count;

            for (int i = 0; i < _count; ++i)
            {
                if (_signals[i].IsDirty(_dirtyBits[i]))
                {
                    isDirty = true;
                    _dirtyBits[i] = _signals[i].DirtyBit;
                }
            }

            return isDirty;
        }

        public void Add<ST>(ST signal) where ST : ISignal<T>
        {
            _signals.Add(signal);
            signal.RegisterDependent(_dependent);
            _dirtyBits.Add((byte)(signal.DirtyBit - 1));
            _count++;
        }

        public void Remove<ST>(ST signal) where ST : ISignal<T>
        {
            var index = _signals.IndexOf(signal);
            signal.UnregisterDependent(_dependent);
            _signals.RemoveAt(index);
            _dirtyBits.RemoveAt(index);
            _count--;
        }

        public void RemoveAt(int index)
        {
            var signal = _signals[index];
            signal.UnregisterDependent(_dependent);
            _signals.RemoveAt(index);
            _dirtyBits.RemoveAt(index);
            _count--;
        }

        public T GetValue(int index)
        {
            return _signals[index].Get();
        }

        public int Count => _count;
    }

    [Serializable]
    public abstract class DynamicComputedSignal<DT, RT> : BaseComputedSignal<RT>
    {
        private readonly DynamicDependencies<DT> _dynamicDependencies;

        public DynamicComputedSignal(IList<ISignal<DT>> dynamicDependencies = null) : base()
        {
            _dynamicDependencies = new(dependent: this, dynamicDependencies);
        }

        public override RT Get()
        {
            if (_dynamicDependencies.IsDirty())
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_dynamicDependencies);

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(DynamicDependencies<DT> dynamicDependencies);
    }


    [Serializable]
    public abstract class DynamicComputedSignal<T1, DT, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        private readonly DynamicDependencies<DT> _dynamicDependencies;

        public DynamicComputedSignal(ISignal<T1> signal1, IList<ISignal<DT>> dynamicDependencies = null) : base()
        {
            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
            _dynamicDependencies = new(dependent: this, dynamicDependencies);
        }

        ~DynamicComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
        }


        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _dynamicDependencies.IsDirty())
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _dynamicDependencies);

                _lastDirtyBit1 = _signal1.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, DynamicDependencies<DT> dynamicSignals);
    }

    [Serializable]
    public abstract class DynamicComputedSignal<T1, T2, DT, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        private readonly DynamicDependencies<DT> _dynamicDependencies;

        public DynamicComputedSignal(ISignal<T1> signal1, ISignal<T2> signal2, IList<ISignal<DT>> dynamicDependencies = null) : base()
        {
            _signal1 = signal1;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
            _signal1.RegisterDependent(this);
            _signal2 = signal2;
            _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
            _signal2.RegisterDependent(this);
            _dynamicDependencies = new(dependent: this, dynamicDependencies);
        }

        ~DynamicComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _dynamicDependencies.IsDirty())
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _dynamicDependencies);

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, DynamicDependencies<DT> dynamicSignals);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1
        ) : base()
        {
            UpdateDeps(signal1);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
        }

        public void UpdateDeps(
            ISignal<T1> signal1
        )
        {
            if (!ReferenceEquals(_signal1, signal1))
            {
                _signal1?.UnregisterDependent(this);
                _signal1 = signal1;
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2
        ) : base()
        {
            UpdateDeps(signal1, signal2);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
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
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
                _signal2?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit3;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        ) : base()
        {
            UpdateDeps(signal1, signal2, signal3);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
            _signal3?.UnregisterDependent(this);
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
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
                _signal2?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _lastDirtyBit3 = (byte)(signal3.DirtyBit - 1);
                _signal3?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _signal3.IsDirty(_lastDirtyBit3))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;
                _lastDirtyBit3 = _signal3.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit3;
        protected ISignal<T4> _signal4;
        protected byte _lastDirtyBit4;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        ) : base()
        {
            UpdateDeps(signal1, signal2, signal3, signal4);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
            _signal3?.UnregisterDependent(this);
            _signal4?.UnregisterDependent(this);
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
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
                _signal2?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _lastDirtyBit3 = (byte)(signal3.DirtyBit - 1);
                _signal3?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _lastDirtyBit4 = (byte)(signal4.DirtyBit - 1);
                _signal4?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _signal3.IsDirty(_lastDirtyBit3) || _signal4.IsDirty(_lastDirtyBit4))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;
                _lastDirtyBit3 = _signal3.DirtyBit;
                _lastDirtyBit4 = _signal4.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, T5, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit3;
        protected ISignal<T4> _signal4;
        protected byte _lastDirtyBit4;
        protected ISignal<T5> _signal5;
        protected byte _lastDirtyBit5;

        public ComputedSignal() : base() { }

        public ComputedSignal(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        ) : base()
        {
            UpdateDeps(signal1, signal2, signal3, signal4, signal5);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
            _signal3?.UnregisterDependent(this);
            _signal4?.UnregisterDependent(this);
            _signal5?.UnregisterDependent(this);
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
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
                _signal2?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _lastDirtyBit3 = (byte)(signal3.DirtyBit - 1);
                _signal3?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _lastDirtyBit4 = (byte)(signal4.DirtyBit - 1);
                _signal4?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal5, signal5))
            {
                _signal5?.UnregisterDependent(this);
                _signal5 = signal5;
                _lastDirtyBit5 = (byte)(signal5.DirtyBit - 1);
                _signal5?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _signal3.IsDirty(_lastDirtyBit3) || _signal4.IsDirty(_lastDirtyBit4) || _signal5.IsDirty(_lastDirtyBit5))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get(), _signal5.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;
                _lastDirtyBit3 = _signal3.DirtyBit;
                _lastDirtyBit4 = _signal4.DirtyBit;
                _lastDirtyBit5 = _signal5.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, T5, T6, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit3;
        protected ISignal<T4> _signal4;
        protected byte _lastDirtyBit4;
        protected ISignal<T5> _signal5;
        protected byte _lastDirtyBit5;
        protected ISignal<T6> _signal6;
        protected byte _lastDirtyBit6;

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
            UpdateDeps(signal1, signal2, signal3, signal4, signal5, signal6);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
            _signal3?.UnregisterDependent(this);
            _signal4?.UnregisterDependent(this);
            _signal5?.UnregisterDependent(this);
            _signal6?.UnregisterDependent(this);
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
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
                _signal2?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _lastDirtyBit3 = (byte)(signal3.DirtyBit - 1);
                _signal3?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _lastDirtyBit4 = (byte)(signal4.DirtyBit - 1);
                _signal4?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal5, signal5))
            {
                _signal5?.UnregisterDependent(this);
                _signal5 = signal5;
                _lastDirtyBit5 = (byte)(signal5.DirtyBit - 1);
                _signal5?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal6, signal6))
            {
                _signal6?.UnregisterDependent(this);
                _signal6 = signal6;
                _lastDirtyBit6 = (byte)(signal6.DirtyBit - 1);
                _signal6?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _signal3.IsDirty(_lastDirtyBit3) || _signal4.IsDirty(_lastDirtyBit4) || _signal5.IsDirty(_lastDirtyBit5) || _signal6.IsDirty(_lastDirtyBit6))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get(), _signal5.Get(), _signal6.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;
                _lastDirtyBit3 = _signal3.DirtyBit;
                _lastDirtyBit4 = _signal4.DirtyBit;
                _lastDirtyBit5 = _signal5.DirtyBit;
                _lastDirtyBit6 = _signal6.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
            }

            return _lastValue;
        }

        protected abstract RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6);
    }

    [Serializable]
    public abstract class ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> : BaseComputedSignal<RT>
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit3;
        protected ISignal<T4> _signal4;
        protected byte _lastDirtyBit4;
        protected ISignal<T5> _signal5;
        protected byte _lastDirtyBit5;
        protected ISignal<T6> _signal6;
        protected byte _lastDirtyBit6;
        protected ISignal<T7> _signal7;
        protected byte _lastDirtyBit7;

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
            UpdateDeps(signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        }

        ~ComputedSignal()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
            _signal3?.UnregisterDependent(this);
            _signal4?.UnregisterDependent(this);
            _signal5?.UnregisterDependent(this);
            _signal6?.UnregisterDependent(this);
            _signal7?.UnregisterDependent(this);
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
                _lastDirtyBit1 = (byte)(signal1.DirtyBit - 1);
                _signal1?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal2, signal2))
            {
                _signal2?.UnregisterDependent(this);
                _signal2 = signal2;
                _lastDirtyBit2 = (byte)(signal2.DirtyBit - 1);
                _signal2?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal3, signal3))
            {
                _signal3?.UnregisterDependent(this);
                _signal3 = signal3;
                _lastDirtyBit3 = (byte)(signal3.DirtyBit - 1);
                _signal3?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal4, signal4))
            {
                _signal4?.UnregisterDependent(this);
                _signal4 = signal4;
                _lastDirtyBit4 = (byte)(signal4.DirtyBit - 1);
                _signal4?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal5, signal5))
            {
                _signal5?.UnregisterDependent(this);
                _signal5 = signal5;
                _lastDirtyBit5 = (byte)(signal5.DirtyBit - 1);
                _signal5?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal6, signal6))
            {
                _signal6?.UnregisterDependent(this);
                _signal6 = signal6;
                _lastDirtyBit6 = (byte)(signal6.DirtyBit - 1);
                _signal6?.RegisterDependent(this);
            }
            if (!ReferenceEquals(_signal7, signal7))
            {
                _signal7?.UnregisterDependent(this);
                _signal7 = signal7;
                _lastDirtyBit7 = (byte)(signal7.DirtyBit - 1);
                _signal7?.RegisterDependent(this);
            }
        }

        public override RT Get()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _signal3.IsDirty(_lastDirtyBit3) || _signal4.IsDirty(_lastDirtyBit4) || _signal5.IsDirty(_lastDirtyBit5) || _signal6.IsDirty(_lastDirtyBit6) || _signal7.IsDirty(_lastDirtyBit7))
            {
                var previousValue = _lastValue;
                _lastValue = Compute(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get(), _signal5.Get(), _signal6.Get(), _signal7.Get());

                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;
                _lastDirtyBit3 = _signal3.DirtyBit;
                _lastDirtyBit4 = _signal4.DirtyBit;
                _lastDirtyBit5 = _signal5.DirtyBit;
                _lastDirtyBit6 = _signal6.DirtyBit;
                _lastDirtyBit7 = _signal7.DirtyBit;

                if (ShouldSetDirty(newValue: _lastValue, previousValue: previousValue))
                {
                    _dirtyBit++;
                }
                Cleanup(previousValue);
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
            Get(); // Ensure that the signals are up to date
            if (!_signalsByKey.ContainsKey(key))
            {
                return default;
            }
            return _signalsByKey[key].Get();
        }

        public ItemSignal GetSignal(Key key)
        {
            Get(); // Ensure that the signals are up to date
            if (!_signalsByKey.ContainsKey(key))
            {
                return default;
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
}