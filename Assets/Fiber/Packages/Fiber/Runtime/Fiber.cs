using System;
using System.Diagnostics;
using System.Collections.Generic;
using FiberUtils;
using Signals;

namespace Fiber
{
    public class Ref<T>
    {
        public T Current { get; set; }

        public Ref()
        {
            Current = default;
        }
        public Ref(T initialValue)
        {
            Current = initialValue;
        }
    }

    public abstract class NativeNode : BaseSignal
    {
        public abstract void AddChild(FiberNode node, int index);
        public abstract void RemoveChild(FiberNode node);
        public abstract void MoveChild(FiberNode node, int index);
        public abstract void Update();
        public abstract void Cleanup();
        public abstract void SetVisible(bool visible);
        protected override sealed void OnNotifySignalUpdate() { }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    public interface IComponentAPI
    {
        public VirtualNode ContextProvider<C>(C value, VirtualBody children);
        public VirtualNode ContextProvider<C>(VirtualBody children);
        public T GetGlobal<T>(bool throwIfNotFound = true);
        public C GetContext<C>(FiberNode node, bool throwIfNotFound = true);
        public NativeNode GetParentNativeNode();
        public void CreateEffect(BaseEffect effect);
        public void CreateEffect(Func<Action> effect);
        public void CreateEffect<T1>(Func<T1, Action> effect, ISignal<T1> signal1, bool runOnMount);
        public void CreateEffect<T1, T2>(
            Func<T1, T2, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            bool runOnMount
        );
        public void CreateEffect<T1, T2, T3>(
            Func<T1, T2, T3, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            bool runOnMount
        );
        public void CreateUpdateEffect(Action<float> onUpdate);
        public ISignal<T> WrapSignalProp<T>(SignalProp<T> signalProp);
        public ComputedSignal<T1, RT> CreateComputedSignal<T1, RT>(
            Func<T1, RT> compute, ISignal<T1> signal1
        );
        public ComputedSignal<T1, T2, RT> CreateComputedSignal<T1, T2, RT>(
            Func<T1, T2, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2
        );
        public ComputedSignal<T1, T2, T3, RT> CreateComputedSignal<T1, T2, T3, RT>(
            Func<T1, T2, T3, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        );
        public ComputedSignal<T1, T2, T3, T4, RT> CreateComputedSignal<T1, T2, T3, T4, RT>(
            Func<T1, T2, T3, T4, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        );
        public ComputedSignal<T1, T2, T3, T4, T5, RT> CreateComputedSignal<T1, T2, T3, T4, T5, RT>(
            Func<T1, T2, T3, T4, T5, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        );
        public ComputedSignal<T1, T2, T3, T4, T5, T6, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(
            Func<T1, T2, T3, T4, T5, T6, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        );
        public ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(
            Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, ItemType>(
            Func<T1, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, ItemType>(
            Func<T1, T2, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, ItemType>(
            Func<T1, T2, T3, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, ItemType>(
            Func<T1, T2, T3, T4, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(
            Func<T1, T2, T3, T4, T5, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Func<T1, T2, T3, T4, T5, T6, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Func<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        );
        public VirtualNode Fragment(VirtualBody children);
        public VirtualNode Enable(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode Visible(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode Active(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode Mount(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode For<ItemType, KeyType>(
            ISignalList<ItemType> each,
            Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children
        );
        public VirtualNode Switch(VirtualNode fallback, VirtualBody children);
        public VirtualNode Match(ISignal<bool> when, VirtualBody children);
    }

    public interface IEffectAPI
    {
        public C GetContext<C>(FiberNode node, bool throwIfNotFound = true);
        public T GetGlobal<T>(bool throwIfNotFound = true);
    }

    public abstract class BaseEffect : BaseSignal
    {
        public IEffectAPI Api { private get; set; }
        public FiberNode FiberNode { private get; set; }

        public C GetContext<C>(bool throwIfNotFound = true) => Api.GetContext<C>(FiberNode, throwIfNotFound: throwIfNotFound);
        public C C<C>(bool throwIfNotFound = true) => GetContext<C>(throwIfNotFound: throwIfNotFound);
        public T GetGlobal<T>(bool throwIfNotFound = true) => Api.GetGlobal<T>(throwIfNotFound);
        public T G<T>(bool throwIfNotFound = true) => GetGlobal<T>(throwIfNotFound);

        public abstract void RunIfDirty();
        public abstract void Cleanup();
        protected override sealed void OnNotifySignalUpdate() { }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    public abstract class Effect : BaseEffect
    {
        bool _hasRun = false;
        public Effect()
        {
            _hasRun = false;
        }

        public sealed override void RunIfDirty()
        {
            if (!_hasRun)
            {
                Run();
                _hasRun = true;
            }
        }

        protected abstract void Run();
    }

    public abstract class DynamicEffect<T> : BaseEffect
    {
        private DynamicDependencies<T> _dynamicSignals;
        bool _hasRun = false;

        public DynamicEffect(IList<ISignal<T>> signals, bool runOnMount = true)
        {
            _dynamicSignals = new DynamicDependencies<T>(this, signals, runOnMount);
        }

        public sealed override void RunIfDirty()
        {
            if (_dynamicSignals.IsDirty())
            {
                if (_hasRun)
                {
                    Cleanup();
                }
                Run(_dynamicSignals);
                _hasRun = true;
            }
        }

        protected abstract void Run(DynamicDependencies<T> signals);
    }

    public abstract class Effect<T1> : BaseEffect
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        bool _hasRun = false;

        public Effect(
            ISignal<T1> signal1,
            bool runOnMount = true
        )
        {
            _signal1 = signal1;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - (runOnMount ? 1 : 0));
            _signal1.RegisterDependent(this);
        }

        ~Effect()
        {
            _signal1?.UnregisterDependent(this);
        }

        public sealed override void RunIfDirty()
        {
            if (_signal1.IsDirty(_lastDirtyBit1))
            {
                if (_hasRun)
                {
                    Cleanup();
                }

                Run(_signal1.Get());
                _lastDirtyBit1 = _signal1.DirtyBit;

                _hasRun = true;
            }
        }

        protected abstract void Run(T1 value1);
    }

    public abstract class Effect<T1, T2> : BaseEffect
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        bool _hasRun = false;

        public Effect(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            bool runOnMount = true
        )
        {
            _signal1 = signal1;
            _signal2 = signal2;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - (runOnMount ? 1 : 0));
            _lastDirtyBit2 = (byte)(signal2.DirtyBit - (runOnMount ? 1 : 0));
            _signal1.RegisterDependent(this);
            _signal2.RegisterDependent(this);
        }

        ~Effect()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
        }

        public sealed override void RunIfDirty()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2))
            {
                if (_hasRun)
                {
                    Cleanup();
                }

                Run(_signal1.Get(), _signal2.Get());
                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;

                _hasRun = true;
            }
        }

        protected abstract void Run(T1 value1, T2 value2);
    }

    public abstract class Effect<T1, T2, T3> : BaseEffect
    {
        protected ISignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected ISignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected ISignal<T3> _signal3;
        protected byte _lastDirtyBit3;
        bool _hasRun = false;

        public Effect(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            bool runOnMount = true
        )
        {
            _signal1 = signal1;
            _signal2 = signal2;
            _signal3 = signal3;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - (runOnMount ? 1 : 0));
            _lastDirtyBit2 = (byte)(signal2.DirtyBit - (runOnMount ? 1 : 0));
            _lastDirtyBit3 = (byte)(signal3.DirtyBit - (runOnMount ? 1 : 0));
            _signal1.RegisterDependent(this);
            _signal2.RegisterDependent(this);
            _signal3.RegisterDependent(this);
        }

        ~Effect()
        {
            _signal1?.UnregisterDependent(this);
            _signal2?.UnregisterDependent(this);
            _signal3?.UnregisterDependent(this);
        }

        public sealed override void RunIfDirty()
        {
            if (_signal1.IsDirty(_lastDirtyBit1) || _signal2.IsDirty(_lastDirtyBit2) || _signal3.IsDirty(_lastDirtyBit3))
            {
                if (_hasRun)
                {
                    Cleanup();
                }

                Run(_signal1.Get(), _signal2.Get(), _signal3.Get());
                _lastDirtyBit1 = _signal1.DirtyBit;
                _lastDirtyBit2 = _signal2.DirtyBit;
                _lastDirtyBit3 = _signal3.DirtyBit;

                _hasRun = true;
            }
        }

        protected abstract void Run(T1 value1, T2 value2, T3 value3);
    }

    public class InlineEffect : Effect
    {
        Func<Action> _effect;
        Action _cleanup;

        public InlineEffect(Func<Action> effect)
        {
            _effect = effect;
        }

        protected override void Run()
        {
            _cleanup = _effect();
        }
        public override void Cleanup()
        {
            if (_cleanup != null)
            {
                _cleanup();
            }
        }
    }

    public class InlineEffect<T1> : Effect<T1>
    {
        Func<T1, Action> _effect;
        Action _cleanup;

        public InlineEffect(Func<T1, Action> effect, ISignal<T1> signal1, bool runOnMount)
            : base(signal1, runOnMount)
        {
            _effect = effect;
        }

        protected override void Run(T1 value1)
        {
            _cleanup = _effect(value1);
        }
        public override void Cleanup()
        {
            if (_cleanup != null)
            {
                _cleanup();
            }
        }
    }

    public class InlineEffect<T1, T2> : Effect<T1, T2>
    {
        Func<T1, T2, Action> _effect;
        Action _cleanup;

        public InlineEffect(
            Func<T1, T2, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            bool runOnMount
        )
            : base(signal1, signal2, runOnMount)
        {
            _effect = effect;
        }

        protected override void Run(T1 value1, T2 value2)
        {
            _cleanup = _effect(value1, value2);
        }
        public override void Cleanup()
        {
            if (_cleanup != null)
            {
                _cleanup();
            }
        }
    }

    public class InlineEffect<T1, T2, T3> : Effect<T1, T2, T3>
    {
        Func<T1, T2, T3, Action> _effect;
        Action _cleanup;

        public InlineEffect(
            Func<T1, T2, T3, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            bool runOnMount
        )
            : base(signal1, signal2, signal3, runOnMount)
        {
            _effect = effect;
        }

        protected override void Run(T1 value1, T2 value2, T3 value3)
        {
            _cleanup = _effect(value1, value2, value3);
        }
        public override void Cleanup()
        {
            if (_cleanup != null)
            {
                _cleanup();
            }
        }
    }

    public class InlineComputedSignal<T1, RT> : ComputedSignal<T1, RT>
    {
        Func<T1, RT> _compute;
        public InlineComputedSignal(Func<T1, RT> compute, ISignal<T1> signal1)
            : base(signal1)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1)
        {
            return _compute(value1);
        }
    }

    public class InlineComputedSignal<T1, T2, RT> : ComputedSignal<T1, T2, RT>
    {
        Func<T1, T2, RT> _compute;
        public InlineComputedSignal(Func<T1, T2, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2)
            : base(signal1, signal2)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2)
        {
            return _compute(value1, value2);
        }
    }

    public class InlineComputedSignal<T1, T2, T3, RT> : ComputedSignal<T1, T2, T3, RT>
    {
        Func<T1, T2, T3, RT> _compute;
        public InlineComputedSignal(Func<T1, T2, T3, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3)
            : base(signal1, signal2, signal3)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2, T3 value3)
        {
            return _compute(value1, value2, value3);
        }
    }

    public class InlineComputedSignal<T1, T2, T3, T4, RT> : ComputedSignal<T1, T2, T3, T4, RT>
    {
        Func<T1, T2, T3, T4, RT> _compute;
        public InlineComputedSignal(Func<T1, T2, T3, T4, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4)
            : base(signal1, signal2, signal3, signal4)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            return _compute(value1, value2, value3, value4);
        }
    }

    public class InlineComputedSignal<T1, T2, T3, T4, T5, RT> : ComputedSignal<T1, T2, T3, T4, T5, RT>
    {
        Func<T1, T2, T3, T4, T5, RT> _compute;
        public InlineComputedSignal(Func<T1, T2, T3, T4, T5, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5)
            : base(signal1, signal2, signal3, signal4, signal5)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            return _compute(value1, value2, value3, value4, value5);
        }
    }

    public class InlineComputedSignal<T1, T2, T3, T4, T5, T6, RT> : ComputedSignal<T1, T2, T3, T4, T5, T6, RT>
    {
        Func<T1, T2, T3, T4, T5, T6, RT> _compute;
        public InlineComputedSignal(Func<T1, T2, T3, T4, T5, T6, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6)
            : base(signal1, signal2, signal3, signal4, signal5, signal6)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            return _compute(value1, value2, value3, value4, value5, value6);
        }
    }

    public class InlineComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> : ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>
    {
        Func<T1, T2, T3, T4, T5, T6, T7, RT> _compute;
        public InlineComputedSignal(Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7)
            : base(signal1, signal2, signal3, signal4, signal5, signal6, signal7)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            return _compute(value1, value2, value3, value4, value5, value6, value7);
        }
    }

    public class InlineComputedSignalList<T1, ItemType> : ComputedSignal<T1, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1)
            : base(signal1)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1)
        {
            _list.Clear();
            return _compute(value1, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, ItemType> : ComputedSignal<T1, T2, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, T2, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, T2, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2)
            : base(signal1, signal2)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2)
        {
            _list.Clear();
            return _compute(value1, value2, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, ItemType> : ComputedSignal<T1, T2, T3, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, T2, T3, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, T2, T3, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3)
            : base(signal1, signal2, signal3)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3)
        {
            _list.Clear();
            return _compute(value1, value2, value3, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, ItemType> : ComputedSignal<T1, T2, T3, T4, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, T2, T3, T4, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, T2, T3, T4, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4)
            : base(signal1, signal2, signal3, signal4)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _list.Clear();
            return _compute(value1, value2, value3, value4, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, T5, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, T2, T3, T4, T5, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, T2, T3, T4, T5, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5)
            : base(signal1, signal2, signal3, signal4, signal5)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            _list.Clear();
            return _compute(value1, value2, value3, value4, value5, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, T2, T3, T4, T5, T6, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, T2, T3, T4, T5, T6, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6)
            : base(signal1, signal2, signal3, signal4, signal5, signal6)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            _list.Clear();
            return _compute(value1, value2, value3, value4, value5, value6, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>, IList<ItemType>> _compute;
        public InlineComputedSignalList(Func<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7)
            : base(signal1, signal2, signal3, signal4, signal5, signal6, signal7)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            _list.Clear();
            return _compute(value1, value2, value3, value4, value5, value6, value7, _list);
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    // A wrapper that is either a single VirtualNode or a list of VirtualNodes
    public struct VirtualBody
    {
        public List<VirtualNode> VirtualNodes { get; private set; }
        public VirtualNode VirtualNode { get; private set; }

        public VirtualBody(List<VirtualNode> virtualNodes)
        {
            VirtualNodes = virtualNodes;
            VirtualNode = null;
        }

        public VirtualBody(VirtualNode virtualNode)
        {
            VirtualNodes = null;
            VirtualNode = virtualNode;
        }

        public static readonly VirtualBody Empty = new();
        public readonly bool IsList => VirtualNodes != null;
        public readonly bool IsSingleNode => VirtualNode != null;
        public readonly bool IsEmpty => !IsList && !IsSingleNode;
        public readonly int Count => IsList ? VirtualNodes.Count : IsSingleNode ? 1 : 0;

        public readonly VirtualNode this[int i]
        {
            get { return VirtualNodes[i]; }
        }

        public readonly void Add(VirtualNode virtualNode)
        {
            VirtualNodes.Add(virtualNode);
        }

        public readonly void Add(VirtualBody virtualBody)
        {
            if (virtualBody.IsList)
            {
                VirtualNodes.AddRange(virtualBody.VirtualNodes);
            }
            else if (virtualBody.IsSingleNode)
            {
                VirtualNodes.Add(virtualBody.VirtualNode);
            }
        }

        public static implicit operator VirtualBody(List<VirtualNode> virtualNodes) => new(virtualNodes);
        public static implicit operator VirtualBody(VirtualNode virtualNode) => new(virtualNode);
    }

    public class VirtualNode
    {
        public VirtualBody children { get; private set; }

        public VirtualNode()
        {
            children = new();
        }

        public VirtualNode(VirtualBody children)
        {
            this.children = children;
        }
    }

    public interface IBuiltInComponent
    {
        VirtualBody Render(FiberNode fiberNode);
    }

    public abstract class BaseContextProvider : VirtualNode, IBuiltInComponent
    {
        public BaseContextProvider(VirtualBody children) : base(children) { }
        public VirtualBody Render(FiberNode fiberNode)
        {
            return children;
        }
    }

    public class ContextProvider<C> : BaseContextProvider
    {
        public C Value { get; private set; }
        public ContextProvider(C value, VirtualBody children) : base(children)
        {
            Value = value;
        }
    }

    public abstract class BaseContext { }

    public class Context<C> : BaseContext
    {
        C _initialValue;

        public Context(C initialValue)
        {
            _initialValue = initialValue;
        }

        public ContextProvider<C> ContextProvider(C value, VirtualBody children)
        {
            return new ContextProvider<C>(value, children);
        }

        public ContextProvider<C> ContextProvider(VirtualBody children)
        {
            return new ContextProvider<C>(_initialValue, children);
        }
    }

    public class ContextsAPI
    {
        private List<BaseContext> _contexts;

        public ContextsAPI(List<BaseContext> contexts = null)
        {
            _contexts = contexts == null ? new List<BaseContext>() : contexts;
        }

        public Context<C> GetContext<C>()
        {
            for (var i = 0; i < _contexts.Count; ++i)
            {
                var context = _contexts[i];
                if (context is Context<C> c)
                {
                    return c;
                }
            }

            var newContext = new Context<C>(default);
            _contexts.Add(newContext);
            return newContext;
        }
    }

    public abstract class BaseComponent : VirtualNode
    {
        public IComponentAPI Api { private get; set; }
        public FiberNode FiberNode { private get; set; }
        public BaseComponent F { get => this; }

        public BaseComponent() : base(new()) { }
        public BaseComponent(VirtualBody children) : base(children) { }
        public abstract VirtualBody Render();

        public VirtualNode ContextProvider<C>(C value, VirtualBody children) => Api.ContextProvider<C>(value, children);
        public VirtualNode ContextProvider<C>(VirtualBody children) => Api.ContextProvider<C>(children);
        public T GetGlobal<T>(bool throwIfNotFound = true) => Api.GetGlobal<T>(throwIfNotFound);
        public T G<T>(bool throwIfNotFound = true) => Api.GetGlobal<T>(throwIfNotFound);
        public C GetContext<C>(bool throwIfNotFound = true) => Api.GetContext<C>(FiberNode, throwIfNotFound: throwIfNotFound);
        public C C<C>(bool throwIfNotFound = true) => GetContext<C>(throwIfNotFound: throwIfNotFound);
        public NativeNode GetParentNativeNode() => Api.GetParentNativeNode();
        public void CreateEffect(BaseEffect effect) => Api.CreateEffect(effect);
        public void CreateEffect(Func<Action> effect) => Api.CreateEffect(effect);
        public void CreateEffect<T1>(Func<T1, Action> effect, ISignal<T1> signal1, bool runOnMount = true)
        {
            Api.CreateEffect(effect, signal1, runOnMount);
        }
        public void CreateEffect<T1, T2>(
            Func<T1, T2, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            bool runOnMount = true
        )
        {
            Api.CreateEffect(effect, signal1, signal2, runOnMount);
        }
        public void CreateEffect<T1, T2, T3>(
            Func<T1, T2, T3, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            bool runOnMount = true
        )
        {
            Api.CreateEffect(effect, signal1, signal2, signal3, runOnMount);
        }
        public void CreateUpdateEffect(Action<float> onUpdate) => Api.CreateUpdateEffect(onUpdate);
        public ISignal<T> WrapSignalProp<T>(SignalProp<T> signalProp) => Api.WrapSignalProp<T>(signalProp);
        public ComputedSignal<T1, RT> CreateComputedSignal<T1, RT>(
            Func<T1, RT> compute, ISignal<T1> signal1
        ) => Api.CreateComputedSignal<T1, RT>(compute, signal1);
        public ComputedSignal<T1, T2, RT> CreateComputedSignal<T1, T2, RT>(
            Func<T1, T2, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2
        ) => Api.CreateComputedSignal<T1, T2, RT>(compute, signal1, signal2);
        public ComputedSignal<T1, T2, T3, RT> CreateComputedSignal<T1, T2, T3, RT>(
            Func<T1, T2, T3, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        ) => Api.CreateComputedSignal<T1, T2, T3, RT>(compute, signal1, signal2, signal3);
        public ComputedSignal<T1, T2, T3, T4, RT> CreateComputedSignal<T1, T2, T3, T4, RT>(
            Func<T1, T2, T3, T4, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, RT>(compute, signal1, signal2, signal3, signal4);
        public ComputedSignal<T1, T2, T3, T4, T5, RT> CreateComputedSignal<T1, T2, T3, T4, T5, RT>(
            Func<T1, T2, T3, T4, T5, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, T5, RT>(compute, signal1, signal2, signal3, signal4, signal5);
        public ComputedSignal<T1, T2, T3, T4, T5, T6, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(
            Func<T1, T2, T3, T4, T5, T6, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        public ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(
            Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        public ISignalList<ItemType> CreateComputedSignalList<T1, ItemType>(
            Func<T1, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1
        ) => Api.CreateComputedSignalList<T1, ItemType>(compute, signal1);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, ItemType>(
            Func<T1, T2, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2
        ) => Api.CreateComputedSignalList<T1, T2, ItemType>(compute, signal1, signal2);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, ItemType>(
            Func<T1, T2, T3, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        ) => Api.CreateComputedSignalList<T1, T2, T3, ItemType>(compute, signal1, signal2, signal3);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, ItemType>(
            Func<T1, T2, T3, T4, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        ) => Api.CreateComputedSignalList<T1, T2, T3, T4, ItemType>(compute, signal1, signal2, signal3, signal4);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(
            Func<T1, T2, T3, T4, T5, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        ) => Api.CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(compute, signal1, signal2, signal3, signal4, signal5);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Func<T1, T2, T3, T4, T5, T6, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        ) => Api.CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Func<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        ) => Api.CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        public VirtualNode Fragment(VirtualBody children) => Api.Fragment(children);
        public VirtualNode Enable(ISignal<bool> when, VirtualBody children) => Api.Enable(when, children);
        public VirtualNode Visible(ISignal<bool> when, VirtualBody children) => Api.Visible(when, children);
        public VirtualNode Active(ISignal<bool> when, VirtualBody children) => Api.Active(when, children);
        public VirtualNode Mount(ISignal<bool> when, VirtualBody children) => Api.Mount(when, children);
        public VirtualNode For<ItemType, KeyType>(
            ISignalList<ItemType> each,
            Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children
        )
        {
            return Api.For<ItemType, KeyType>(each, children);
        }
        public VirtualNode Switch(VirtualNode fallback, VirtualBody children) => Api.Switch(fallback, children);
        public VirtualNode Match(ISignal<bool> when, VirtualBody children) => Api.Match(when, children);
        public VirtualBody Children()
        {
            var children = new List<VirtualNode>();
            return children;
        }
        public VirtualBody Children(VirtualNode c1)
        {
            var children = new List<VirtualNode>
            {
                c1
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4,
                c5
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4,
                c5,
                c6
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4,
                c5,
                c6,
                c7
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7, VirtualNode c8)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4,
                c5,
                c6,
                c7,
                c8
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7, VirtualNode c8, VirtualNode c9)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4,
                c5,
                c6,
                c7,
                c8,
                c9
            };
            return children;
        }
        public VirtualBody Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7, VirtualNode c8, VirtualNode c9, VirtualNode c10)
        {
            var children = new List<VirtualNode>
            {
                c1,
                c2,
                c3,
                c4,
                c5,
                c6,
                c7,
                c8,
                c9,
                c10
            };
            return children;
        }
    }

    public abstract class Component<P> : BaseComponent
    {
        public P Props { get; set; }

        public Component(P props, VirtualBody children = new()) : base(children)
        {
            Props = props;
        }
    }

    public enum FiberNodePhase
    {
        // The node is created and added to the virtual tree.
        AddedToVirtualTree = 0,
        // The node's virtual node has called Render(),
        // which sets up signals, effects, etc.
        // We also create the native node in this step, although
        // we don't add it to the native tree.
        Rendered = 1,
        // The node's associated native node (if any) has been
        // added to the native tree.
        Mounted = 2,
        // The node has been removed from the virtual tree.
        RemovedFromVirtualTree = 3,
        // The node's associated native node (if any) has been
        // removed from the native tree. Currently cleanup is part of this step as well.
        // This means that the node's virtual node has called Cleanup(),
        // which cleans up signals, effects, etc. We might want to split these two states up in the future.
        Unmounted = 4
    }

    public class FiberNode : BaseSignal
    {
        public int Id { get; set; } // Unique id for this node. Not used directly in Fiber, but used by other renderers, for example Fiber.GameObjects.
        private static IntIdGenerator _idGenerator = new IntIdGenerator();

        public NativeNode NativeNode { get; set; }
        public VirtualNode VirtualNode { get; private set; }
        public FiberNode Parent { get; private set; }
        public FiberNode Sibling { get; set; }
        public FiberNode Child { get; set; }
        public List<FiberNode> NativeNodeChildren { get; set; }
        public FiberNodePhase Phase { get; set; }
        // Enables and disables the node for updates in the WorkLoop.
        // If disabled underlying effects and signals will not update.
        // If disabled the full sub tree will also be excluded from
        // being processed in the WorkLoop.
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                var current = this;
                do
                {
                    if (!current._isEnabled)
                    {
                        return false;
                    }
                    current = current.Parent;
                } while (current != null);

                return true;
            }
            set
            {
                _isEnabled = value;
            }
        }

        private List<BaseEffect> _effects = new List<BaseEffect>();
        private Renderer _renderer;

        public FiberNode(
            Renderer renderer,
            NativeNode nativeNode,
            VirtualNode virtualNode,
            FiberNode parent,
            FiberNode sibling
        )
        {
            _renderer = renderer;
            Id = _idGenerator.NextId();
            NativeNode = nativeNode;
            VirtualNode = virtualNode;
            Parent = parent;
            Sibling = sibling;
            Phase = FiberNodePhase.AddedToVirtualTree;
            IsEnabled = true;
        }

        public void PushEffect(BaseEffect effect)
        {
            _effects.Add(effect);
            effect.RegisterDependent(this);
        }

        public void Update()
        {
            RunEffects();

            if (NativeNode != null)
            {
                NativeNode.Update();
            }
        }

        public void RunEffects()
        {
            var effectsCount = _effects.Count;
            if (effectsCount <= 0) return;

            for (var i = 0; i < effectsCount; ++i)
            {
                _effects[i].RunIfDirty();
            }
        }

        public void CleanupEffects()
        {
            var effectsCount = _effects.Count;
            if (effectsCount <= 0) return;

            for (var i = 0; i < effectsCount; ++i)
            {
                _effects[i].Cleanup();
                _effects[i].UnregisterDependent(this);
            }
        }

        public FiberNode FindClosestAncestorWithNativeNode(bool includeSelf = false)
        {
            var nativeNodeParentFiber = includeSelf ? this : Parent;
            while (nativeNodeParentFiber != null && nativeNodeParentFiber.NativeNode == null)
            {
                nativeNodeParentFiber = nativeNodeParentFiber.Parent;
            }

            return nativeNodeParentFiber;
        }

        public V FindClosesVirtualNode<V>(bool includeSelf = false) where V : VirtualNode
        {
            var current = includeSelf ? this : Parent;
            while (current != null && current.VirtualNode is not V)
            {
                current = current.Parent;
            }

            return current?.VirtualNode as V;
        }

        public bool IsAncestorOf(FiberNode node, FiberNode root = null, bool includeSelf = true)
        {
            var currentNode = includeSelf ? this : Parent;
            while (currentNode != root && currentNode != null)
            {
                if (currentNode == node)
                {
                    return true;
                }
                currentNode = currentNode.Parent;
            }

            return false;
        }

        public FiberNode FindClosestCommonAncestor(FiberNode other)
        {
            var currentNode = Parent;
            while (currentNode != null)
            {
                if (other.IsAncestorOf(currentNode, null, false))
                {
                    return currentNode;
                }
                currentNode = currentNode.Parent;
            }

            return null;
        }

        public FiberNode FindChild(int id)
        {
            for (var child = Child; child != null; child = child.Sibling)
            {
                if (child.Id == id)
                {
                    return child;
                }
            }

            return null;
        }

        // Get the index of the provided child in this node's current list of native node children by traversing the
        // tree depth first. Assumes that this node has a native node and that the child is a descendant of this node.
        public int FindNativeNodeIndex(FiberNode child)
        {
            if (NativeNode == null)
            {
                throw new Exception("FiberNode.FindPotentialNativeNodeIndex: parent.NativeNode is null");
            }

            if (NativeNodeChildren == null || NativeNodeChildren.Count == 0)
            {
                return 0;
            }

            var index = 0;
            var current = Child; // parent.Child can't be null here

            while (current != this && current != null)
            {
                if (current == child)
                {
                    return index;
                }
                var nativeNodeChildIndex = NativeNodeChildren.IndexOf(current);
                if (nativeNodeChildIndex != -1)
                {
                    index++;
                }

                current = current.NextNode();
            }

            throw new Exception($"Failed to find potential native node index. {child} is not a descendant of {this}.");
        }

        public void RemoveSelfFromTree()
        {
            if (Parent != null)
            {
                if (Parent.Child == this)
                {
                    Parent.Child = Sibling;
                }
                else
                {
                    var sibling = Parent.Child;
                    while (sibling.Sibling != this)
                    {
                        sibling = sibling.Sibling;
                    }
                    sibling.Sibling = Sibling;
                }
            }
            // We don't want to set RemovedFromVirtualTree for all decedents, since we want to know if 
            // something is added to this subtree, before we unmount, in order to account for that.
            Phase = FiberNodePhase.RemovedFromVirtualTree;
            // We are deliberatley not removing the reference to the Parent here,
            // since we need it when we unmount the node (when removing the native node).
        }

        public bool IsRemovedFromVirtualTree()
        {
            var current = this;
            while (current != null)
            {
                if (current.Phase == FiberNodePhase.RemovedFromVirtualTree)
                {
                    return true;
                }
                current = current.Parent;
            }
            return false;
        }

        // Get the next node in the virtual tree (traversing depth first)
        // In order to travers the tree you can do the following: 
        // for (var current = root; current != null; current = current.NextNode()) { .... }
        public FiberNode NextNode(FiberNode root = null, bool skipChildren = false)
        {
            if (Child != null && !skipChildren)
            {
                return Child;
            }
            else if (Sibling != null)
            {
                return Sibling;
            }
            else
            {
                var currentNode = this;
                while (currentNode.Parent != root && currentNode.Parent != null && currentNode.Sibling == null)
                {
                    currentNode = currentNode.Parent;
                }
                if (currentNode.Parent == null || currentNode.Parent == root)
                {
                    return null;
                }
                return currentNode.Sibling;
            }
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            if (Phase == FiberNodePhase.Mounted && IsEnabled)
            {
                _renderer.AddFiberNodeToUpdateQueue(this);
            }
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    public abstract class RendererExtension
    {
        public abstract NativeNode CreateNativeNode(FiberNode fiberNode);
        public abstract bool OwnsComponentType(VirtualNode node);
    }

    public class Renderer : IComponentAPI, IEffectAPI
    {
        public const long DEFAULT_WORK_LOOP_TIME_BUDGET_MS = 5;

        private Queue<FiberNode> _renderQueue;
        private MixedQueue _operationsQueue;
        private Queue<FiberNode> _fiberNodesToUpdate;
        protected List<RendererExtension> _rendererExtensions;
        private Dictionary<Type, object> _globals;
        private ContextsAPI _contextsAPI;
        private FiberNode _root;
        private readonly bool _autonomousWorkLoop;
        private int _workLoopSubId = -1;
        private readonly long _workLoopTimeBudgetMs;

        private bool _isUnmountingRoot = false;
        public bool IsMounted => _root != null;

        private struct MountOperation
        {
            public FiberNode Node;

            public MountOperation(FiberNode node)
            {
                Node = node;
            }
        }

        private struct UnmountOperation
        {
            public FiberNode Parent;
            public FiberNode Child;

            public UnmountOperation(FiberNode parent, FiberNode child)
            {
                Parent = parent;
                Child = child;
            }
        }

        private struct MoveOperation
        {
            public FiberNode Parent;
            public FiberNode Child;
            public int Index;

            public MoveOperation(FiberNode parent, FiberNode child, int index)
            {
                Parent = parent;
                Child = child;
                Index = index;
            }
        }

        public Renderer(
            List<RendererExtension> rendererExtensions,
            Dictionary<Type, object> globals = null,
            bool autonomousWorkLoop = true,
            long workLoopTimeBudgetMs = DEFAULT_WORK_LOOP_TIME_BUDGET_MS
        )
        {
            _renderQueue = new();
            _operationsQueue = new();
            _fiberNodesToUpdate = new();
            _globals = globals ?? new();
            _rendererExtensions = rendererExtensions;
            _contextsAPI = new();
            _autonomousWorkLoop = autonomousWorkLoop;
            _workLoopTimeBudgetMs = workLoopTimeBudgetMs;
        }

        ~Renderer()
        {
            StopAutoWorkLoop();
        }

        public void Render(VirtualNode virtualNode, NativeNode nativeNodeRoot)
        {
            if (_root != null || _isUnmountingRoot)
            {
                UnityEngine.Debug.LogWarning($"Trying to render when there is already a root node mounted. Aborting.");
                return;
            }

            _root = new FiberNode(
                renderer: this,
                nativeNode: nativeNodeRoot,
                virtualNode: virtualNode,
                parent: null,
                sibling: null
            );
            _renderQueue.Enqueue(_root);
            StartAutoWorkLoop();
        }

        public void Unmount(bool immediatelyExecuteRemainingWork = true)
        {
            if (_root == null || _isUnmountingRoot)
            {
                UnityEngine.Debug.LogWarning($"Trying to unmount when there is no root node mounted. Aborting.");
                return;
            }

            _isUnmountingRoot = true;
            _root.Phase = FiberNodePhase.RemovedFromVirtualTree;
            _operationsQueue.Enqueue(new UnmountOperation(null, _root));

            if (immediatelyExecuteRemainingWork)
            {
                WorkLoop(immediatelyExecuteRemainingWork: true);
            }
        }

        void StartAutoWorkLoop()
        {
            if (_autonomousWorkLoop)
            {
                _workLoopSubId = MonoBehaviourHelper.AddOnUpdateHandler(AutoWorkLoop);
            }
        }

        void StopAutoWorkLoop()
        {
            if (_workLoopSubId != -1)
            {
                MonoBehaviourHelper.RemoveOnUpdateHandler(_workLoopSubId);
            }

            _workLoopSubId = -1;
        }

        void AutoWorkLoop(float deltaTime)
        {
            WorkLoop();
        }

        private Stopwatch _stopWatch = new Stopwatch();
        public void WorkLoop(bool immediatelyExecuteRemainingWork = false)
        {
            _stopWatch.Restart();

            do
            {
                if (_renderQueue.Count > 0)
                {
                    // Renders the next node in the render queue.
                    // This takes the node from AddedToVirtualTree to Rendered.
                    // In some cases it might not render the node, if for example
                    // its parent have been removed from the virtual tree.
                    RenderNextInQueue();
                }
                else if (_operationsQueue.Count > 0)
                {
                    // Commit the next operation in the operations queue.
                    // An operation can be one of the following: 
                    // - Mount -> Adds the native node to the native tree and run initial effects.
                    // - Unmount -> Removes / cleans up the native node and runs cleanup of effects.
                    // - Move -> Moves the node in the virtual tree as well as in the native tree.
                    CommitNextOperation();
                }
                else if (_fiberNodesToUpdate.Count > 0)
                {
                    var fiberNode = _fiberNodesToUpdate.Dequeue();

                    if (fiberNode.Phase == FiberNodePhase.Mounted && fiberNode.IsEnabled)
                    {
                        fiberNode.Update();
                    }
                }
                else
                {
                    break;
                }
            } while (_stopWatch.ElapsedMilliseconds < _workLoopTimeBudgetMs || immediatelyExecuteRemainingWork);

            _stopWatch.Stop();
        }

        FiberNode _currentFiberNode;
        private void RenderNextInQueue()
        {
            var fiberNode = _renderQueue.Dequeue();
            if (fiberNode.IsRemovedFromVirtualTree())
            {
                return;
            }

            _currentFiberNode = fiberNode;

            if (fiberNode.VirtualNode is BaseComponent component)
            {
                component.Api = this;
                component.FiberNode = _currentFiberNode;
                var children = component.Render();
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode is IBuiltInComponent builtInComponent)
            {
                var children = builtInComponent.Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode is BaseForComponent forComponent)
            {
                // For component will handle the rendering of its children by itself.
                // The reason is that it need to keep track of its children and their
                // corresponding keys.
                forComponent.Render(fiberNode);
            }
            else
            {
                fiberNode.NativeNode = CreateNativeNode(fiberNode);
                if (fiberNode.NativeNode != null)
                {
                    fiberNode.NativeNode.RegisterDependent(fiberNode);
                    fiberNode.NativeNode.SetVisible(false);
                }
                RenderChildren(this, fiberNode, fiberNode.VirtualNode.children, _renderQueue);
            }

            fiberNode.Phase = FiberNodePhase.Rendered;
            _operationsQueue.Enqueue(new MountOperation(node: fiberNode));
        }

        public static void RenderChildren(Renderer renderer, FiberNode fiberNode, VirtualBody children, Queue<FiberNode> renderQueue)
        {
            if (children.IsEmpty)
            {
                return;
            }
            else if (children.IsSingleNode)
            {
                var childFiberNode = new FiberNode(
                    renderer: renderer,
                    nativeNode: null,
                    virtualNode: children.VirtualNode,
                    parent: fiberNode,
                    sibling: null
                );
                fiberNode.Child = childFiberNode;
                renderQueue.Enqueue(childFiberNode);
                return;
            }

            FiberNode previousChildFiberNode = null;
            for (var i = 0; i < children.Count; ++i)
            {
                var child = children[i];
                if (child != null)
                {
                    var childFiberNode = new FiberNode(
                        renderer: renderer,
                        nativeNode: null,
                        virtualNode: child,
                        parent: fiberNode,
                        sibling: null
                    );
                    if (i == 0)
                    {
                        fiberNode.Child = childFiberNode;
                    }
                    else if (previousChildFiberNode != null)
                    {
                        previousChildFiberNode.Sibling = childFiberNode;
                    }
                    previousChildFiberNode = childFiberNode;
                    renderQueue.Enqueue(childFiberNode);
                }
            }
        }

        public void AddFiberNodeToUpdateQueue(FiberNode fiberNode)
        {
            _fiberNodesToUpdate.Enqueue(fiberNode);
        }

        private bool IsVisible(FiberNode fiberNode)
        {
            var current = fiberNode;
            while (current != null)
            {
                if (current.VirtualNode is VisibleComponent visibleComponent)
                {
                    return visibleComponent.IsVisible;
                }
                current = current.Parent;
            }

            return true;
        }

        private void CommitNextOperation()
        {
            var operationType = _operationsQueue.PeekType();

            if (operationType == typeof(MountOperation))
            {
                var mountOperation = _operationsQueue.Dequeue<MountOperation>();
                var parent = mountOperation.Node.Parent;

                // Only the root should have a null parent
                if (parent != null && mountOperation.Node.NativeNode != null)
                {
                    var closestParentWithNativeNode = mountOperation.Node.FindClosestAncestorWithNativeNode();

                    if (closestParentWithNativeNode.NativeNodeChildren == null)
                    {
                        closestParentWithNativeNode.NativeNodeChildren = new();
                        closestParentWithNativeNode.NativeNodeChildren.Add(mountOperation.Node);
                        closestParentWithNativeNode.NativeNode.AddChild(mountOperation.Node, 0);
                    }
                    else
                    {
                        var index = closestParentWithNativeNode.FindNativeNodeIndex(mountOperation.Node);
                        closestParentWithNativeNode.NativeNodeChildren.Insert(index, mountOperation.Node);
                        closestParentWithNativeNode.NativeNode.AddChild(mountOperation.Node, index);
                    }

                    var isVisible = IsVisible(mountOperation.Node);
                    mountOperation.Node.NativeNode.SetVisible(isVisible);
                }

                mountOperation.Node.RunEffects();
                mountOperation.Node.Phase = FiberNodePhase.Mounted;
            }
            else if (operationType == typeof(UnmountOperation))
            {
                var unmountOperation = _operationsQueue.Dequeue<UnmountOperation>();
                // A node might have been deleted already if it for example was a child of a node that was deleted
                if (unmountOperation.Child.Phase != FiberNodePhase.RemovedFromVirtualTree)
                {
                    return;
                }

                // Run the effect clean ups first for the full sub tree
                // We don't delete the native nodes at this point since
                // there might be clean up effects that depends on the native nodes
                CleanupNode(unmountOperation.Child);

                // Then delete all the native nodes of the subtree
                if (!_isUnmountingRoot)
                {
                    DeleteNativeNode(fiberNode: unmountOperation.Child, parent: unmountOperation.Parent);
                }
                else
                {
                    // The root doesn't control the lifecycle of the native node, that is managed by the user.
                    DeleteNativeNode(fiberNode: unmountOperation.Child.Child, parent: unmountOperation.Child);
                }
                MarkAsUnmounted(unmountOperation.Child);

                if (_isUnmountingRoot)
                {
                    _root = null;
                    _isUnmountingRoot = false;
                    StopAutoWorkLoop();
                }
            }
            else if (operationType == typeof(MoveOperation))
            {
                var moveOperation = _operationsQueue.Dequeue<MoveOperation>();
                // A node might have been deleted already if it for example was a child of a node that was deleted
                if (moveOperation.Child.Phase != FiberNodePhase.Mounted)
                {
                    return;
                }

                if (moveOperation.Parent != null)
                {
                    // Delete references
                    if (moveOperation.Parent.Child == moveOperation.Child)
                    {
                        moveOperation.Parent.Child = moveOperation.Child.Sibling;
                    }
                    else
                    {
                        var sibling = moveOperation.Parent.Child;
                        while (sibling.Sibling != moveOperation.Child)
                        {
                            sibling = sibling.Sibling;
                        }
                        sibling.Sibling = moveOperation.Child.Sibling;
                    }

                    // Insert references
                    if (moveOperation.Index == 0)
                    {
                        var currentChild = moveOperation.Parent.Child;
                        moveOperation.Parent.Child = moveOperation.Child;
                        moveOperation.Child.Sibling = currentChild;
                    }
                    else
                    {
                        FiberNode siblingBefore = moveOperation.Parent.Child;
                        for (var i = 0; i < moveOperation.Index - 1; ++i)
                        {
                            siblingBefore = siblingBefore.Sibling;
                        }
                        var siblingAfter = siblingBefore.Sibling;
                        siblingBefore.Sibling = moveOperation.Child;
                        moveOperation.Child.Sibling = siblingAfter;
                    }

                    // Move all children with native nodes (including self) that is an ancestor of the moved node
                    var closestParentWithNativeNode = moveOperation.Child.FindClosestAncestorWithNativeNode();
                    var nativeNodeIndex = closestParentWithNativeNode.FindNativeNodeIndex(moveOperation.Child);

                    for (var i = 0; i < closestParentWithNativeNode.NativeNodeChildren.Count; ++i)
                    {
                        var nativeNodeChild = closestParentWithNativeNode.NativeNodeChildren[i];
                        if (moveOperation.Child.IsAncestorOf(nativeNodeChild))
                        {
                            closestParentWithNativeNode.NativeNode.MoveChild(moveOperation.Child, nativeNodeIndex);
                            nativeNodeIndex++;
                        }
                    }
                }

            }
        }

        void CleanupNode(FiberNode fiberNode)
        {
            fiberNode.CleanupEffects();
            fiberNode?.NativeNode?.Cleanup();

            var child = fiberNode.Child;
            while (child != null)
            {
                CleanupNode(child);
                child = child.Sibling;
            }
        }

        void DeleteNativeNode(FiberNode fiberNode, FiberNode parent)
        {
            if (fiberNode.NativeNode != null)
            {
                if (fiberNode.Phase == FiberNodePhase.RemovedFromVirtualTree || fiberNode.Phase == FiberNodePhase.Mounted)
                {
                    var closestAncestor = parent.FindClosestAncestorWithNativeNode(includeSelf: true);
                    closestAncestor.NativeNode.RemoveChild(fiberNode);
                    closestAncestor.NativeNodeChildren.Remove(fiberNode);
                }
                return;
            }

            var child = fiberNode.Child;
            while (child != null)
            {
                DeleteNativeNode(child, child.Parent);
                child = child.Sibling;
            }
        }

        void MarkAsUnmounted(FiberNode node)
        {
            node.Phase = FiberNodePhase.Unmounted;
            var child = node.Child;
            while (child != null)
            {
                MarkAsUnmounted(child);
                child = child.Sibling;
            }
        }

        private NativeNode CreateNativeNode(FiberNode fiberNode)
        {
            var virtualNode = fiberNode.VirtualNode;
            for (var i = 0; i < _rendererExtensions.Count; ++i)
            {
                var rendererExtension = _rendererExtensions[i];
                if (!rendererExtension.OwnsComponentType(virtualNode))
                {
                    continue;
                }
                var nativeNode = rendererExtension.CreateNativeNode(fiberNode);
                return nativeNode;
            }

            throw new Exception($"Unknown virtual node {virtualNode}.");
        }

        public T GetGlobal<T>(bool throwIfNotFound = true)
        {
            var type = typeof(T);
            if (_globals.ContainsKey(type))
            {
                return (T)_globals[type];
            }

            if (throwIfNotFound)
            {
                throw new Exception($"Global of type {type} not found. Did you forget to add it to the renderer?");
            }

            return default;
        }

        public VirtualNode ContextProvider<C>(C value, VirtualBody children)
        {
            return _contextsAPI.GetContext<C>().ContextProvider(value, children);
        }

        public VirtualNode ContextProvider<C>(VirtualBody children)
        {
            return _contextsAPI.GetContext<C>().ContextProvider(children);
        }

        public C GetContext<C>(FiberNode node, bool throwIfNotFound = true)
        {
            var fiberNode = node;

            while (fiberNode != null && !(fiberNode.VirtualNode is ContextProvider<C>))
            {
                fiberNode = fiberNode.Parent;
            }

            if (fiberNode == null)
            {
                if (!throwIfNotFound)
                {
                    return default;
                }
                throw new Exception($"No context provider of type {typeof(C)} found.");
            }
            return ((ContextProvider<C>)fiberNode.VirtualNode).Value;
        }

        public NativeNode GetParentNativeNode()
        {
            var closestAncesorWithNativeNode = _currentFiberNode.FindClosestAncestorWithNativeNode();
            if (closestAncesorWithNativeNode == null)
            {
                return null;
            }
            return closestAncesorWithNativeNode.NativeNode;
        }

        public void CreateEffect(BaseEffect effect)
        {
            effect.Api = this;
            effect.FiberNode = _currentFiberNode;
            _currentFiberNode.PushEffect(effect);
        }

        public void CreateEffect(Func<Action> effect)
        {
            var inlineEffect = new InlineEffect(effect)
            {
                Api = this,
                FiberNode = _currentFiberNode
            };
            _currentFiberNode.PushEffect(inlineEffect);
        }

        public void CreateEffect<T1>(Func<T1, Action> effect, ISignal<T1> signal1, bool runOnMount)
        {
            var inlineEffect = new InlineEffect<T1>(effect, signal1, runOnMount)
            {
                Api = this,
                FiberNode = _currentFiberNode
            };
            _currentFiberNode.PushEffect(inlineEffect);
        }

        public void CreateEffect<T1, T2>(
            Func<T1, T2, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            bool runOnMount
        )
        {
            var inlineEffect = new InlineEffect<T1, T2>(effect, signal1, signal2, runOnMount)
            {
                Api = this,
                FiberNode = _currentFiberNode
            };
            _currentFiberNode.PushEffect(inlineEffect);
        }

        public void CreateEffect<T1, T2, T3>(
            Func<T1, T2, T3, Action> effect,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            bool runOnMount
        )
        {
            var inlineEffect = new InlineEffect<T1, T2, T3>(effect, signal1, signal2, signal3, runOnMount)
            {
                Api = this,
                FiberNode = _currentFiberNode
            };
            _currentFiberNode.PushEffect(inlineEffect);
        }


        public void CreateUpdateEffect(Action<float> onUpdate)
        {
            CreateEffect(() =>
            {
                var subId = MonoBehaviourHelper.AddOnUpdateHandler(onUpdate);
                return () =>
                {
                    MonoBehaviourHelper.RemoveOnUpdateHandler(subId);
                };
            });
        }

        public ISignal<T> WrapSignalProp<T>(SignalProp<T> signalProp)
        {
            if (signalProp.IsValue)
            {
                return new StaticSignal<T>(signalProp.Value);
            }
            else if (signalProp.IsSignal)
            {
                return signalProp.Signal;
            }

            throw new Exception($"Trying to wrap empty signal prop");
        }

        public ComputedSignal<T1, RT> CreateComputedSignal<T1, RT>(
            Func<T1, RT> compute, ISignal<T1> signal1
        )
        {
            return new InlineComputedSignal<T1, RT>(compute, signal1);
        }

        public ComputedSignal<T1, T2, RT> CreateComputedSignal<T1, T2, RT>(
            Func<T1, T2, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2
        )
        {
            return new InlineComputedSignal<T1, T2, RT>(compute, signal1, signal2);
        }

        public ComputedSignal<T1, T2, T3, RT> CreateComputedSignal<T1, T2, T3, RT>(
            Func<T1, T2, T3, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        )
        {
            return new InlineComputedSignal<T1, T2, T3, RT>(compute, signal1, signal2, signal3);
        }

        public ComputedSignal<T1, T2, T3, T4, RT> CreateComputedSignal<T1, T2, T3, T4, RT>(
            Func<T1, T2, T3, T4, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, RT>(compute, signal1, signal2, signal3, signal4);
        }

        public ComputedSignal<T1, T2, T3, T4, T5, RT> CreateComputedSignal<T1, T2, T3, T4, T5, RT>(
            Func<T1, T2, T3, T4, T5, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, T5, RT>(compute, signal1, signal2, signal3, signal4, signal5);
        }

        public ComputedSignal<T1, T2, T3, T4, T5, T6, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(
            Func<T1, T2, T3, T4, T5, T6, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, T5, T6, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        }

        public ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(
            Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, ItemType>(
            Func<T1, IList<ItemType>, IList<ItemType>> compute, ISignal<T1> signal1
        )
        {
            return new InlineComputedSignalList<T1, ItemType>(compute, signal1);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, ItemType>(
            Func<T1, T2, IList<ItemType>, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2
        )
        {
            return new InlineComputedSignalList<T1, T2, ItemType>(compute, signal1, signal2);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, ItemType>(
            Func<T1, T2, T3, IList<ItemType>, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        )
        {
            return new InlineComputedSignalList<T1, T2, T3, ItemType>(compute, signal1, signal2, signal3);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, ItemType>(
            Func<T1, T2, T3, T4, IList<ItemType>, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        )
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, ItemType>(compute, signal1, signal2, signal3, signal4);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(
            Func<T1, T2, T3, T4, T5, IList<ItemType>, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        )
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, T5, ItemType>(compute, signal1, signal2, signal3, signal4, signal5);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Func<T1, T2, T3, T4, T5, T6, IList<ItemType>, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6
        )
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Func<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6,
            ISignal<T7> signal7
        )
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        }

        public T GetRendererExtension<T>() where T : RendererExtension
        {
            for (var i = 0; i < _rendererExtensions.Count; ++i)
            {
                if (_rendererExtensions[i] is T extension)
                {
                    return extension;
                }
            }

            return null;
        }


        public VirtualNode Fragment(VirtualBody children)
        {
            return new FragmentComponent(children);
        }

        private class FragmentComponent : VirtualNode, IBuiltInComponent
        {
            public FragmentComponent(VirtualBody children) : base(children) { }
            public VirtualBody Render(FiberNode fiberNode)
            {
                return children;
            }
        }

        public VirtualNode Enable(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new EnableComponent(whenSignal, children, this);
        }

        private class EnableComponent : VirtualNode, IBuiltInComponent
        {
            private readonly ISignal<bool> _whenSignal;
            private readonly Renderer _renderer;

            public EnableComponent(ISignal<bool> whenSignal, VirtualBody children, Renderer renderer) : base(children)
            {
                _whenSignal = whenSignal;
                _renderer = renderer;
            }

            private class EnabledEffect : Effect<bool>
            {
                private readonly FiberNode _fiberNode;
                private readonly Renderer _renderer;

                public EnabledEffect(
                    ISignal<bool> whenSignal,
                    FiberNode fiberNode,
                    Renderer renderer
                )
                    : base(whenSignal, runOnMount: true)
                {
                    _fiberNode = fiberNode;
                    _renderer = renderer;
                }

                private void RehydrateSubTree(FiberNode root)
                {
                    _renderer.AddFiberNodeToUpdateQueue(root);
                    for (var node = root.Child; node != null; node = node.NextNode(root: root, skipChildren: false))
                    {
                        _renderer.AddFiberNodeToUpdateQueue(node);
                    }
                }

                protected override void Run(bool enabled)
                {
                    // We need to enable / disable the children instead of this node since 
                    // this effect won't otherwise run (since its node is disabled...)
                    var child = _fiberNode.Child;
                    while (child != null)
                    {
                        var valueBeforeChange = child.IsEnabled;
                        child.IsEnabled = enabled;
                        if (!valueBeforeChange && enabled)
                        {
                            // Some props in the sub tree might have become dirty while the node was disabled.
                            // Just rehydrate the whole sub tree to be sure. We could optimize this in the future,
                            // but it is questionable if that will be worth it.
                            RehydrateSubTree(child);
                        }
                        child = child.Sibling;
                    }
                }
                public override void Cleanup() { }
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                fiberNode.PushEffect(new EnabledEffect(_whenSignal, fiberNode, _renderer));
                return children;
            }
        }

        public VirtualNode Visible(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new VisibleComponent(whenSignal, children);
        }

        private class VisibleComponent : VirtualNode, IBuiltInComponent
        {
            public bool IsVisible { get => _whenSignal.Get(); }
            private readonly ISignal<bool> _whenSignal;
            public VisibleComponent(ISignal<bool> whenSignal, VirtualBody children) : base(children)
            {
                _whenSignal = whenSignal;
            }

            private class VisibleEffect : Effect<bool>
            {
                private readonly FiberNode _fiberNode;
                public VisibleEffect(
                    ISignal<bool> whenSignal,
                    FiberNode fiberNode
                )
                    : base(whenSignal, runOnMount: true)
                {
                    _fiberNode = fiberNode;
                }

                protected override void Run(bool visible)
                {
                    // Iterate all decedents up to the point that a child is a VisibleComponent
                    var node = _fiberNode.Child;
                    while (node != null && node != _fiberNode)
                    {
                        node.NativeNode?.SetVisible(visible);

                        var isChildVisibleComponent = node.Child != null && node.Child.VirtualNode is VisibleComponent;
                        var isCurrentVisibleComponent = node.VirtualNode is VisibleComponent; // Could be true if a Sibling is a VisibleComponent
                        node = node.NextNode(root: _fiberNode, skipChildren: isChildVisibleComponent || isCurrentVisibleComponent);
                    }
                }
                public override void Cleanup() { }
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                fiberNode.PushEffect(new VisibleEffect(_whenSignal, fiberNode));
                return children;
            }
        }

        public VirtualNode Active(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new ActiveComponent(whenSignal, children, this);
        }

        private class ActiveComponent : VirtualNode, IBuiltInComponent
        {
            private readonly ISignal<bool> _whenSignal;
            private readonly Renderer _renderer;
            public ActiveComponent(ISignal<bool> whenSignal, VirtualBody children, Renderer renderer) : base(children)
            {
                _whenSignal = whenSignal;
                _renderer = renderer;
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                return new VisibleComponent(
                    _whenSignal,
                    new EnableComponent(
                        _whenSignal,
                        children,
                        _renderer
                    )
                );
            }
        }

        public VirtualNode Mount(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new MountComponent(whenSignal, children, _renderQueue, _operationsQueue, this);
        }

        private class MountComponent : VirtualNode, IBuiltInComponent
        {
            private readonly ISignal<bool> _whenSignal;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;
            private readonly Renderer _renderer;

            public MountComponent(
                ISignal<bool> whenSignal,
                VirtualBody children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue,
                Renderer renderer
            ) : base(children)
            {
                _whenSignal = whenSignal;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;
                _renderer = renderer;
            }

            private class MountEffect : Effect<bool>
            {
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly Renderer _renderer;
                private readonly FiberNode _mountFiberNode;
                private readonly VirtualBody _children;
                private bool _valueLastTime;

                public MountEffect(
                    ISignal<bool> whenSignal,
                    VirtualBody children,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    Renderer renderer,
                    FiberNode mountFiberNode
                )
                    : base(whenSignal, runOnMount: false)
                {
                    _children = children;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _renderer = renderer;
                    _mountFiberNode = mountFiberNode;
                    _valueLastTime = whenSignal.Get();
                }
                protected override void Run(bool mount)
                {
                    if (mount == _valueLastTime)
                    {
                        return;
                    }

                    if (mount)
                    {
                        RenderChildren(_renderer, _mountFiberNode, _children, _renderQueue);
                    }
                    else
                    {
                        for (var childFiberNode = _mountFiberNode.Child; childFiberNode != null; childFiberNode = childFiberNode.Sibling)
                        {
                            childFiberNode.RemoveSelfFromTree();
                            _operationsQueue.Enqueue(new UnmountOperation(parent: _mountFiberNode, child: childFiberNode));
                        }
                    }
                    _valueLastTime = mount;
                }
                public override void Cleanup() { }
            }

            public VirtualBody Render(FiberNode mountFiberNode)
            {
                mountFiberNode.PushEffect(new MountEffect(_whenSignal, children, _renderQueue, _operationsQueue, _renderer, mountFiberNode));
                return _whenSignal.Get() ? children : new();
            }
        }

        public VirtualNode For<ItemType, KeyType>(
            ISignalList<ItemType> each,
            Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children
        )
        {
            return new ForComponent<ItemType, KeyType>(each, children, _renderQueue, _operationsQueue, this);
        }

        // Class is only added in order to be able to type check when rendering (not possible with generic class)
        private abstract class BaseForComponent : VirtualNode
        {
            protected BaseForComponent() : base(new()) { }
            public abstract void Render(FiberNode fiberNode);
        }

        private class ForComponent<ItemType, KeyType> : BaseForComponent
        {
            private readonly ISignalList<ItemType> _eachSignal;
            private readonly Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> _children;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;
            private readonly Renderer _renderer;
            private readonly Dictionary<KeyType, int> _currentKeyToIdMap;
            private readonly Dictionary<int, KeyType> _currentIdToKeyMap;

            public ForComponent(
                ISignalList<ItemType> eachSignal,
                Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue,
                Renderer renderer
            ) : base()
            {
                _eachSignal = eachSignal;
                _children = children;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;
                _renderer = renderer;

                _currentKeyToIdMap = new();
                _currentIdToKeyMap = new();
            }

            private class ForEffect : Effect<IList<ItemType>>
            {
                private readonly Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> _children;
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly Renderer _renderer;
                private readonly FiberNode _fiberNode;
                private readonly Dictionary<KeyType, int> _currentKeyToIdMap;
                private readonly Dictionary<int, KeyType> _currentIdToKeyMap;
                private readonly HashSet<KeyType> _allKeys;

                public ForEffect(
                    ISignalList<ItemType> eachSignal,
                    Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    Renderer renderer,
                    FiberNode fiberNode,
                    Dictionary<KeyType, int> currentKeyToIdMap,
                    Dictionary<int, KeyType> currentIdToKeyMap
                )
                    : base(eachSignal, runOnMount: false)
                {
                    _children = children;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _renderer = renderer;
                    _fiberNode = fiberNode;
                    _currentKeyToIdMap = currentKeyToIdMap;
                    _currentIdToKeyMap = currentIdToKeyMap;
                    _allKeys = new();
                }

                protected override void Run(IList<ItemType> each)
                {
                    var currentChildAtIndex = _fiberNode.Child;
                    FiberNode previousChildFiberNode = null;
                    _allKeys.Clear();

                    for (var i = 0; i < each.Count; ++i)
                    {
                        var (key, child) = _children(each[i], i);
                        _allKeys.Add(key);

                        var currentChildFiberId = _currentKeyToIdMap.ContainsKey(key) ? _currentKeyToIdMap[key] : -1;
                        var currentChildOfKey = currentChildFiberId == -1 ? null : _fiberNode.FindChild(currentChildFiberId);

                        FiberNode createdChildNode = null;
                        if (currentChildAtIndex == null || currentChildOfKey == null || currentChildOfKey.Id != currentChildAtIndex.Id)
                        {
                            if (currentChildOfKey != null)
                            {
                                _operationsQueue.Enqueue(new MoveOperation(_fiberNode, currentChildOfKey, i));
                            }
                            else
                            {
                                createdChildNode = new FiberNode(
                                    renderer: _renderer,
                                    nativeNode: null,
                                    virtualNode: child,
                                    parent: _fiberNode,
                                    sibling: currentChildAtIndex
                                );
                                if (i == 0)
                                {
                                    _fiberNode.Child = createdChildNode;
                                }
                                else if (previousChildFiberNode != null)
                                {
                                    previousChildFiberNode.Sibling = createdChildNode;
                                }
                                _renderQueue.Enqueue(createdChildNode);
                                _currentKeyToIdMap.Add(key, createdChildNode.Id);
                                _currentIdToKeyMap.Add(createdChildNode.Id, key);
                            }
                        }

                        previousChildFiberNode = createdChildNode ?? currentChildAtIndex;
                        if (currentChildAtIndex != null)
                        {
                            currentChildAtIndex = currentChildAtIndex.Sibling;
                        }
                    }

                    for (var currentChild = _fiberNode.Child; currentChild != null; currentChild = currentChild.Sibling)
                    {

                        var key = _currentIdToKeyMap[currentChild.Id];
                        if (!_allKeys.Contains(key))
                        {
                            currentChild.RemoveSelfFromTree();
                            _operationsQueue.Enqueue(new UnmountOperation(parent: _fiberNode, child: currentChild));
                            _currentKeyToIdMap.Remove(key);
                            _currentIdToKeyMap.Remove(currentChild.Id);
                        }
                    }
                }
                public override void Cleanup() { }
            }

            public override void Render(FiberNode fiberNode)
            {
                fiberNode.PushEffect(new ForEffect(_eachSignal, _children, _renderQueue, _operationsQueue, _renderer, fiberNode, _currentKeyToIdMap, _currentIdToKeyMap));

                var each = _eachSignal.Get();
                FiberNode previousChildFiberNode = null;
                for (var i = 0; i < each.Count; ++i)
                {
                    var (key, child) = _children(each[i], i);

                    if (child != null)
                    {
                        var childFiberNode = new FiberNode(
                            renderer: _renderer,
                            nativeNode: null,
                            virtualNode: child,
                            parent: fiberNode,
                            sibling: null
                        );
                        if (i == 0)
                        {
                            fiberNode.Child = childFiberNode;
                        }
                        else if (previousChildFiberNode != null)
                        {
                            previousChildFiberNode.Sibling = childFiberNode;
                        }
                        previousChildFiberNode = childFiberNode;
                        _renderQueue.Enqueue(childFiberNode);
                        _currentKeyToIdMap.Add(key, childFiberNode.Id);
                        _currentIdToKeyMap.Add(childFiberNode.Id, key);
                    }
                }
            }
        }

        public VirtualNode Switch(VirtualNode fallback, VirtualBody children)
        {
            return new SwitchComponent(fallback, children, _renderQueue, _operationsQueue, this);
        }

        private class SwitchComponent : VirtualNode, IBuiltInComponent
        {
            private readonly VirtualNode _fallback;
            private readonly SignalList<ISignal<bool>> _matchSignals;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;
            private readonly Renderer _renderer;

            public SwitchComponent(
                VirtualNode fallback,
                VirtualBody children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue,
                Renderer renderer
            )
                : base(children)
            {
                _fallback = fallback;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;
                _renderer = renderer;

                _matchSignals = new(children.Count);
                for (var i = 0; i < children.Count; ++i)
                {
                    var child = children[i];
                    if (child.GetType() == typeof(MatchComponent))
                    {
                        _matchSignals.Add(((MatchComponent)child).When);
                    }
                }
            }

            private class SwitchEffect : DynamicEffect<bool>
            {
                private readonly VirtualBody _children;
                private readonly VirtualNode _fallback;
                private readonly FiberNode _fiberNode;
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly Renderer _renderer;
                private readonly Ref<int> _lastRenderedIndexRef;

                public SwitchEffect(
                    SignalList<ISignal<bool>> matchSignals,
                    VirtualBody children,
                    VirtualNode fallback,
                    FiberNode fiberNode,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    Renderer renderer,
                    Ref<int> lastRenderedIndexRef
                )
                : base(matchSignals, runOnMount: false)
                {
                    _children = children;
                    _fallback = fallback;
                    _fiberNode = fiberNode;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _renderer = renderer;
                    _lastRenderedIndexRef = lastRenderedIndexRef;
                }
                protected override void Run(DynamicDependencies<bool> matchSignals)
                {
                    for (var i = 0; i < matchSignals.Count; ++i)
                    {
                        if (matchSignals.GetValue(i))
                        {
                            if (_lastRenderedIndexRef.Current == i)
                            {
                                return;
                            }

                            if (_fiberNode.Child != null)
                            {
                                var childNode = _fiberNode.Child;
                                _fiberNode.Child.RemoveSelfFromTree();
                                _operationsQueue.Enqueue(new UnmountOperation(_fiberNode, childNode));
                            }

                            var child = _children[i];
                            var childFiberNode = new FiberNode(
                                renderer: _renderer,
                                nativeNode: null,
                                virtualNode: child,
                                parent: _fiberNode,
                                sibling: null
                            );
                            _fiberNode.Child = childFiberNode;
                            _renderQueue.Enqueue(childFiberNode);

                            _lastRenderedIndexRef.Current = i;
                            return;
                        }
                    }

                    if (_lastRenderedIndexRef.Current == -1)
                    {
                        return;
                    }
                    if (_fiberNode.Child != null)
                    {
                        var childNode = _fiberNode.Child;
                        _fiberNode.Child.RemoveSelfFromTree();
                        _operationsQueue.Enqueue(new UnmountOperation(_fiberNode, childNode));
                    }
                    if (_fallback != null)
                    {
                        var childFiberNode = new FiberNode(
                            renderer: _renderer,
                            nativeNode: null,
                            virtualNode: _fallback,
                            parent: _fiberNode,
                            sibling: null
                        );
                        _fiberNode.Child = childFiberNode;
                        _renderQueue.Enqueue(childFiberNode);
                    }
                    _lastRenderedIndexRef.Current = -1;
                }
                public override void Cleanup() { }
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                var _lastRenderedIndexRef = new Ref<int>(-1);
                fiberNode.PushEffect(new SwitchEffect(_matchSignals, children, _fallback, fiberNode, _renderQueue, _operationsQueue, _renderer, _lastRenderedIndexRef));

                for (var i = 0; i < children.Count; ++i)
                {
                    var child = children[i];
                    if (child.GetType() == typeof(MatchComponent))
                    {
                        var match = (MatchComponent)child;
                        if (match.When != null && match.When.Get())
                        {
                            _lastRenderedIndexRef.Current = i;
                            return child;
                        }
                    }
                }

                _lastRenderedIndexRef.Current = -1;
                return _fallback;
            }
        }

        public VirtualNode Match(ISignal<bool> when, VirtualBody children)
        {
            return new MatchComponent(when, children);
        }

        private class MatchComponent : VirtualNode, IBuiltInComponent
        {
            public ISignal<bool> When { get; private set; }

            public MatchComponent(ISignal<bool> when, VirtualBody children)
                : base(children)
            {
                When = when;
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                return children;
            }
        }
    }
}
