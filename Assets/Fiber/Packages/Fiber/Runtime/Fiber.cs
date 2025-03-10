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
        public abstract void RemoveChild(FiberNode node, bool destroyInstance);
        public abstract void MoveChild(FiberNode node, int index);
        public abstract void Update();
        public abstract void Cleanup();
        public abstract void SetVisible(bool visible);
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
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, ItemType>(
            Action<T1, IList<ItemType>> compute, ISignal<T1> signal1
        );
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, ItemType>(
            Action<T1, T2, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2
        );
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, ItemType>(
            Action<T1, T2, T3, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        );
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, ItemType>(
            Action<T1, T2, T3, T4, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        );
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType>(
            Action<T1, T2, T3, T4, T5, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        );
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        );
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        );
        public ISignalList<ItemType> CreateComputedSignalList<T1, ItemType>(
            Action<T1, IList<ItemType>> compute, ISignal<T1> signal1
        ) where ItemType : ISignal;
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, ItemType>(
            Action<T1, T2, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2
        ) where ItemType : ISignal;
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, ItemType>(
            Action<T1, T2, T3, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        ) where ItemType : ISignal;
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, ItemType>(
            Action<T1, T2, T3, T4, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        ) where ItemType : ISignal;
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(
            Action<T1, T2, T3, T4, T5, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        ) where ItemType : ISignal;
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        ) where ItemType : ISignal;
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        ) where ItemType : ISignal;
        public SignalList<ItemType> CreateSignalList<ItemType>(ISignal dependent = null) where ItemType : ISignal;
        public ShallowSignalList<ItemType> CreateShallowSignalList<ItemType>(ISignal dependent = null);
        public Signal<T> CreateSignal<T>(T value = default, BaseSignal dependent = null);
        public StaticSignal<T> CreateStaticSignal<T>(T value);
        public Ref<T> CreateRef<T>(T initialValue = default);
        public BaseSignal<T> ToSignal<T>(SignalProp<T> signalProp);
        public VirtualNode Fragment(VirtualBody children);
        public VirtualNode Enable(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode Visible(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode Active(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode Mount(ISignal<bool> whenSignal, VirtualBody children);
        public VirtualNode For<ItemType, KeyType>(
            ISignalList<ItemType> each,
            Func<ItemType, int, VirtualNode> renderItem,
            Func<ItemType, int, KeyType> createItemKey
        );
        public VirtualNode Switch(VirtualBody fallback, VirtualBody children);
        public VirtualNode Match(ISignal<bool> when, VirtualBody children);
        public VirtualNode Portal(SignalProp<string> destinationId, VirtualBody children);
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
        public virtual void Dispose() { }
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
        private byte _lastDirtyBit;

        public DynamicEffect(IList<ISignal<T>> signals, bool runOnMount = true)
        {
            Initialize(signals, runOnMount);
        }

        public void Initialize(IList<ISignal<T>> signals, bool runOnMount = true)
        {
            _dynamicSignals = DynamicDependencies<T>.Pool.Get();
            _dynamicSignals.Initialize(this, signals);
            _lastDirtyBit = (byte)(_dirtyBit - (runOnMount ? 1 : 0));
        }

        public override void Dispose()
        {
            DynamicDependencies<T>.Pool.Release(_dynamicSignals);
        }

        public sealed override void RunIfDirty()
        {
            if (_lastDirtyBit != _dirtyBit)
            {
                if (_hasRun)
                {
                    Cleanup();
                }
                Run(_dynamicSignals);
                _hasRun = true;

                _lastDirtyBit = _dirtyBit;
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

    public class InlineComputedShallowSignalList<T1, ItemType> : ComputedSignal<T1, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, IList<ItemType>> compute, ISignal<T1> signal1)
            : base(signal1)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1)
        {
            _list.Clear();
            _compute(value1, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedShallowSignalList<T1, T2, ItemType> : ComputedSignal<T1, T2, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, T2, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, T2, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2)
            : base(signal1, signal2)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2)
        {
            _list.Clear();
            _compute(value1, value2, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedShallowSignalList<T1, T2, T3, ItemType> : ComputedSignal<T1, T2, T3, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, T2, T3, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3)
            : base(signal1, signal2, signal3)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3)
        {
            _list.Clear();
            _compute(value1, value2, value3, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedShallowSignalList<T1, T2, T3, T4, ItemType> : ComputedSignal<T1, T2, T3, T4, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, T2, T3, T4, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4)
            : base(signal1, signal2, signal3, signal4)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, T5, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, T2, T3, T4, T5, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5)
            : base(signal1, signal2, signal3, signal4, signal5)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, value5, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6)
            : base(signal1, signal2, signal3, signal4, signal5, signal6)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, value5, value6, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> _compute;
        public InlineComputedShallowSignalList(Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7)
            : base(signal1, signal2, signal3, signal4, signal5, signal6, signal7)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, value5, value6, value7, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, ItemType> : ComputedSignal<T1, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, IList<ItemType>> compute, ISignal<T1> signal1)
            : base(signal1)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1)
        {
            _list.Clear();
            _compute(value1, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, ItemType> : ComputedSignal<T1, T2, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, T2, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, T2, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2)
            : base(signal1, signal2)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2)
        {
            _list.Clear();
            _compute(value1, value2, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, ItemType> : ComputedSignal<T1, T2, T3, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, T2, T3, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3)
            : base(signal1, signal2, signal3)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3)
        {
            _list.Clear();
            _compute(value1, value2, value3, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, ItemType> : ComputedSignal<T1, T2, T3, T4, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, T2, T3, T4, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4)
            : base(signal1, signal2, signal3, signal4)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, T5, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, T5, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, T2, T3, T4, T5, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5)
            : base(signal1, signal2, signal3, signal4, signal5)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, value5, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6)
            : base(signal1, signal2, signal3, signal4, signal5, signal6)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, value5, value6, _list);
            return _list;
        }
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;
    }

    public class InlineComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>>, ISignalList<ItemType>
        where ItemType : ISignal
    {
        private readonly SignalList<ItemType> _list;
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> _compute;
        public InlineComputedSignalList(Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7)
            : base(signal1, signal2, signal3, signal4, signal5, signal6, signal7)
        {
            _compute = compute;
            _list = new();
        }
        protected override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            _list.Clear();
            _compute(value1, value2, value3, value4, value5, value6, value7, _list);
            return _list;
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
            get
            {
                if (IsList)
                {
                    return VirtualNodes[i];
                }
                if (i == 0 && IsSingleNode)
                {
                    return VirtualNode;
                }
                throw new IndexOutOfRangeException($"Can't get index {i} from VirtualBody");
            }
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

    public abstract class VirtualNode
    {
        public VirtualBody Children { get; private set; }
        public VirtualNodeType Type { get; private set; }

        public VirtualNode(VirtualBody children, VirtualNodeType type)
        {
            Children = children;
            Type = type;
        }

        public virtual void Dispose() { }
    }

    public class ContextProvider<C> : VirtualNode
    {
        public C Value { get; private set; }

        public ContextProvider(C value, VirtualBody children) : base(children, VirtualNodeType.Context)
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

        public BaseComponent() : base(new(), VirtualNodeType.CustomComponent) { }
        public BaseComponent(VirtualBody children) : base(children, VirtualNodeType.CustomComponent) { }
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
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, ItemType>(
            Action<T1, IList<ItemType>> compute, ISignal<T1> signal1
        ) => Api.CreateComputedShallowSignalList<T1, ItemType>(compute, signal1);
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, ItemType>(
            Action<T1, T2, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2
        ) => Api.CreateComputedShallowSignalList<T1, T2, ItemType>(compute, signal1, signal2);
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, ItemType>(
            Action<T1, T2, T3, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        ) => Api.CreateComputedShallowSignalList<T1, T2, T3, ItemType>(compute, signal1, signal2, signal3);
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, ItemType>(
            Action<T1, T2, T3, T4, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        ) => Api.CreateComputedShallowSignalList<T1, T2, T3, T4, ItemType>(compute, signal1, signal2, signal3, signal4);
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType>(
            Action<T1, T2, T3, T4, T5, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        ) => Api.CreateComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType>(compute, signal1, signal2, signal3, signal4, signal5);
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        ) => Api.CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        ) => Api.CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        public ISignalList<ItemType> CreateComputedSignalList<T1, ItemType>(
            Action<T1, IList<ItemType>> compute, ISignal<T1> signal1
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, ItemType>(compute, signal1);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, ItemType>(
            Action<T1, T2, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, T2, ItemType>(compute, signal1, signal2);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, ItemType>(
            Action<T1, T2, T3, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, T2, T3, ItemType>(compute, signal1, signal2, signal3);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, ItemType>(
            Action<T1, T2, T3, T4, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, T2, T3, T4, ItemType>(compute, signal1, signal2, signal3, signal4);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(
            Action<T1, T2, T3, T4, T5, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(compute, signal1, signal2, signal3, signal4, signal5);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute, ISignal<T1> signal1, ISignal<T2> signal2, ISignal<T3> signal3, ISignal<T4> signal4, ISignal<T5> signal5, ISignal<T6> signal6, ISignal<T7> signal7
        ) where ItemType : ISignal => Api.CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        public SignalList<ItemType> CreateSignalList<ItemType>(ISignal dependent = null) where ItemType : ISignal => Api.CreateSignalList<ItemType>(dependent);
        public ShallowSignalList<ItemType> CreateShallowSignalList<ItemType>(ISignal dependent = null) => Api.CreateShallowSignalList<ItemType>(dependent);
        public Signal<T> CreateSignal<T>(T value = default, BaseSignal dependent = null) => Api.CreateSignal<T>(value, dependent);
        public StaticSignal<T> CreateStaticSignal<T>(T value) => Api.CreateStaticSignal<T>(value);
        public Ref<T> CreateRef<T>(T initialValue = default) => Api.CreateRef<T>(initialValue);
        public BaseSignal<T> ToSignal<T>(SignalProp<T> signalProp) => Api.ToSignal<T>(signalProp);
        public VirtualNode Fragment(VirtualBody children) => Api.Fragment(children);
        public VirtualNode Enable(ISignal<bool> when, VirtualBody children) => Api.Enable(when, children);
        public VirtualNode Visible(ISignal<bool> when, VirtualBody children) => Api.Visible(when, children);
        public VirtualNode Active(ISignal<bool> when, VirtualBody children) => Api.Active(when, children);
        public VirtualNode Mount(ISignal<bool> when, VirtualBody children) => Api.Mount(when, children);
        public VirtualNode For<ItemType, KeyType>(
            ISignalList<ItemType> each,
            Func<ItemType, int, VirtualNode> renderItem,
            Func<ItemType, int, KeyType> createItemKey
        )
        {
            return Api.For<ItemType, KeyType>(each, renderItem, createItemKey);
        }
        public VirtualNode Switch(VirtualBody fallback, VirtualBody children) => Api.Switch(fallback, children);
        public VirtualNode Match(ISignal<bool> when, VirtualBody children) => Api.Match(when, children);
        public VirtualNode Portal(SignalProp<string> destinationId, VirtualBody children) => Api.Portal(destinationId, children);
        public VirtualBody Nodes()
        {
            var nodes = new List<VirtualNode>();
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1)
        {
            var nodes = new List<VirtualNode>
            {
                n1
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4, VirtualNode n5)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4,
                n5
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4, VirtualNode n5, VirtualNode n6)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4,
                n5,
                n6
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4, VirtualNode n5, VirtualNode n6, VirtualNode n7)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4,
                n5,
                n6,
                n7
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4, VirtualNode n5, VirtualNode n6, VirtualNode n7, VirtualNode n8)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4,
                n5,
                n6,
                n7,
                n8
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4, VirtualNode n5, VirtualNode n6, VirtualNode n7, VirtualNode n8, VirtualNode n9)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4,
                n5,
                n6,
                n7,
                n8,
                n9
            };
            return nodes;
        }
        public VirtualBody Nodes(VirtualNode n1, VirtualNode n2, VirtualNode n3, VirtualNode n4, VirtualNode n5, VirtualNode n6, VirtualNode n7, VirtualNode n8, VirtualNode n9, VirtualNode n10)
        {
            var nodes = new List<VirtualNode>
            {
                n1,
                n2,
                n3,
                n4,
                n5,
                n6,
                n7,
                n8,
                n9,
                n10
            };
            return nodes;
        }
        public void CreateOnEnableEffect(Action<bool> onEnable)
        {
            var enableContext = GetContext<Renderer.EnableContext>(throwIfNotFound: false);
            if (enableContext == null)
            {
                CreateEffect(() =>
                {
                    onEnable(true);
                    return () => onEnable(false);
                });
            }
            else
            {
                CreateEffect(() =>
                {
                    var id = enableContext.Subscribe(onEnable);
                    return () => enableContext.Unsubscribe(id);
                });
            }
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
        public FiberNode PortalDestination { get; set; }
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
        public ShallowSignalDictionary<string, FiberNode> PortalDestinations { get => _renderer.PortalDestinations; }
        private VirtualNodeType _virtualNodeType;

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
            _virtualNodeType = VirtualNode?.Type ?? VirtualNodeType.Null;
            _signalType = SignalType.FiberNode;
        }

        public void PushEffect(BaseEffect effect)
        {
            _effects.Add(effect);
            effect.RegisterDependent(this);
        }

        public void Update()
        {
            // Node needs to be mounted and enabled in order to run effects.
            // The exception is built in components, which are always updated.
            if (Phase != FiberNodePhase.Mounted || (!IsEnabled && !_virtualNodeType.IsBuiltInComponent()))
            {
                return;
            }

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
                _effects[i].Dispose();
            }
        }

        public void RegisterPortalDestination(string id, FiberNode fiberNode)
        {
            _renderer.PortalDestinations.Add(id, fiberNode);
        }

        public void UnregisterPortalDestination(string id)
        {
            _renderer.PortalDestinations.Remove(id);
        }

        public FiberNode FindClosestAncestorWithNativeNodeOrPortalDestination(bool includeSelf = false)
        {
            var nativeNodeParentFiber = includeSelf ? this : Parent;
            while (nativeNodeParentFiber != null && nativeNodeParentFiber.NativeNode == null && nativeNodeParentFiber.PortalDestination == null)
            {
                nativeNodeParentFiber = nativeNodeParentFiber.Parent;
            }

            return nativeNodeParentFiber;
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

        public bool IsDescendentOf(FiberNode node, FiberNode root = null, bool includeSelf = true)
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

        protected override void OnNotifySignalUpdate()
        {
            // OPEN POINT: We could optimize this to see if already in queue.
            // Keeping it like this for now for simplicity reasons.
            _renderer.AddFiberNodeToUpdateQueue(this);
        }

        public void TryDisposeVirtualNode()
        {
            if (!_virtualNodeType.IsUsedByFiberAfterMount())
            {
                VirtualNode.Dispose();
                VirtualNode = null;
            }
        }
    }

    public abstract class RendererExtension
    {
        public abstract NativeNode CreateNativeNode(FiberNode fiberNode);
        public abstract bool OwnsComponentType(VirtualNode node);
    }

    public class Renderer : IComponentAPI, IEffectAPI
    {
        public const long DEFAULT_WORK_LOOP_TIME_BUDGET_MS = 4;

        private Queue<FiberNode> _renderQueue;
        private MixedQueue _operationsQueue;
        private Queue<FiberNode> _fiberNodesToUpdate;
        protected List<RendererExtension> _rendererExtensions;
        private Dictionary<Type, object> _globals;
        private ContextsAPI _contextsAPI;
        private FiberNode _root;
        private readonly bool _autonomousWorkLoop;
        private int _workLoopSubId = -1;

        private bool _isUnmountingRoot = false;
        public bool IsMounted => _root != null;

        public ShallowSignalDictionary<string, FiberNode> PortalDestinations { get; private set; } = new();

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

        private struct SetPortalDestinationOperation
        {
            public FiberNode PortalDestination;
            public FiberNode Node;

            public SetPortalDestinationOperation(FiberNode node, FiberNode portalDestination)
            {
                Node = node;
                PortalDestination = portalDestination;
            }
        }

        public Renderer(
            List<RendererExtension> rendererExtensions,
            Dictionary<Type, object> globals = null,
            bool autonomousWorkLoop = true
        )
        {
            _renderQueue = new();
            _operationsQueue = new();
            _fiberNodesToUpdate = new();
            _globals = globals ?? new();
            _rendererExtensions = rendererExtensions;
            _contextsAPI = new();
            _autonomousWorkLoop = autonomousWorkLoop;
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

        public void WorkLoop(bool immediatelyExecuteRemainingWork = false)
        {
            TimeBudgetManager.Instance.StartTimer();

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
                    fiberNode.Update();
                }
                else
                {
                    break;
                }
            } while (TimeBudgetManager.Instance.HasBudgetLeft() || immediatelyExecuteRemainingWork);

            TimeBudgetManager.Instance.StopTimer();
        }

        FiberNode _currentFiberNode;
        private void RenderNextInQueue()
        {
            var fiberNode = _renderQueue.Dequeue();
            // Checking if virtual node is null here allows us to render null in components
            if (fiberNode.IsRemovedFromVirtualTree() || fiberNode.VirtualNode == null)
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

            else if (fiberNode.VirtualNode.Type == VirtualNodeType.Context || fiberNode.VirtualNode.Type == VirtualNodeType.Fragment)
            {
                // These components doesn't render anything besides its children...
                RenderChildren(this, fiberNode, fiberNode.VirtualNode.Children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.EnableComponent)
            {
                var children = ((EnableComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.VisibleComponent)
            {
                var children = ((VisibleComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.ActiveComponent)
            {
                var children = ((ActiveComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.MountComponent)
            {
                var children = ((MountComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.SwitchComponent)
            {
                var children = ((SwitchComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.MatchComponent)
            {
                var children = ((MatchComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode.Type == VirtualNodeType.PortalComponent)
            {
                var children = ((PortalComponent)fiberNode.VirtualNode).Render(fiberNode);
                RenderChildren(this, fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode is IPortalDestination portalDestination)
            {
                var children = portalDestination.Render(fiberNode);
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
                RenderChildren(this, fiberNode, fiberNode.VirtualNode.Children, _renderQueue);
            }

            fiberNode.Phase = FiberNodePhase.Rendered;
            _operationsQueue.Enqueue(new MountOperation(node: fiberNode));

            fiberNode.TryDisposeVirtualNode();
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
            var childIndex = 0;
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
                    if (childIndex == 0)
                    {
                        fiberNode.Child = childFiberNode;
                    }
                    else if (previousChildFiberNode != null)
                    {
                        previousChildFiberNode.Sibling = childFiberNode;
                    }
                    previousChildFiberNode = childFiberNode;
                    renderQueue.Enqueue(childFiberNode);
                    ++childIndex;
                }
            }
        }

        public void AddFiberNodeToUpdateQueue(FiberNode fiberNode)
        {
            _fiberNodesToUpdate.Enqueue(fiberNode);
        }

        private bool GetIsInitiallyVisible(FiberNode fiberNode)
        {
            var current = fiberNode;
            do
            {
                if (current.VirtualNode is VisibleComponent visibleComponent)
                {
                    return visibleComponent.IsVisible.Get();
                }
                current = current.Parent;
                // We return when we reach the first native node, since all 
                // renderers will work the same way when it comes to visibility, 
                // eg. when a parent is not visible, then all children are also not visible.
            } while (current != null && current.NativeNode == null);

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
                    var closestParentWithNativeNode = mountOperation.Node.FindClosestAncestorWithNativeNodeOrPortalDestination();

                    if (closestParentWithNativeNode.NativeNodeChildren == null)
                    {
                        closestParentWithNativeNode.NativeNodeChildren = new() { mountOperation.Node };
                        closestParentWithNativeNode.NativeNode.AddChild(mountOperation.Node, 0);
                    }
                    else
                    {
                        var index = closestParentWithNativeNode.FindNativeNodeIndex(mountOperation.Node);
                        closestParentWithNativeNode.NativeNodeChildren.Insert(index, mountOperation.Node);
                        closestParentWithNativeNode.NativeNode.AddChild(mountOperation.Node, index);
                    }

                    var isVisible = GetIsInitiallyVisible(mountOperation.Node);
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
                if (moveOperation.Child.Phase != FiberNodePhase.Mounted || moveOperation.Parent == null)
                {
                    return;
                }

                // Figure out what native nodes need to be moved.
                // There can be more than one native node being a descendent of the node being moved,
                // so a consecutive chunk of native nodes (0-n) might need to be moved. 
                var closestParentWithNativeNode = moveOperation.Child.FindClosestAncestorWithNativeNodeOrPortalDestination();

                var moveCount = 0;
                var moveFromIndex = -1;
                var moveToFirstChunkIndex = -1;

                for (var i = 0; i < closestParentWithNativeNode.NativeNodeChildren.Count; ++i)
                {
                    var nativeNodeChild = closestParentWithNativeNode.NativeNodeChildren[i];
                    if (nativeNodeChild.IsDescendentOf(moveOperation.Child))
                    {
                        if (moveCount == 0)
                        {
                            moveFromIndex = i;
                            // Calling FindNativeNodeIndex to get the first index here works 
                            //since we have already updated the virtual tree above.
                            moveToFirstChunkIndex = closestParentWithNativeNode.FindNativeNodeIndex(moveOperation.Child);
                        }

                        moveCount++;
                    }
                    else if (moveCount != 0)
                    {
                        // If we have found at least one native node to move, but this sibling native node
                        // isn't a descendent, then no other native nodes should be moved.
                        break;
                    }
                }

                // Do the actual move and update the order of the list of native node children in the closest parent with native node.
                var isMovingForward = moveToFirstChunkIndex > moveFromIndex;

                for (var i = 0; i < moveCount; ++i)
                {
                    if (isMovingForward)
                    {
                        // Since we are moving items further down in the list, then that means that the next item to
                        // move will be pushed back, meaning that the move from index never changes.
                        var nativeNodeChild = closestParentWithNativeNode.NativeNodeChildren[moveFromIndex];
                        closestParentWithNativeNode.NativeNode.MoveChild(nativeNodeChild, moveToFirstChunkIndex + moveCount - 1);
                        closestParentWithNativeNode.NativeNodeChildren.Move(moveFromIndex, moveToFirstChunkIndex + moveCount);
                    }
                    else
                    {
                        var moveTo = moveToFirstChunkIndex + i;
                        var moveFrom = moveFromIndex + i;
                        var nativeNodeChild = closestParentWithNativeNode.NativeNodeChildren[moveFrom];
                        closestParentWithNativeNode.NativeNode.MoveChild(nativeNodeChild, moveTo);
                        closestParentWithNativeNode.NativeNodeChildren.Move(moveFrom, moveTo);
                    }
                }
            }
            else if (operationType == typeof(SetPortalDestinationOperation))
            {
                var setPortalDestinationOperation = _operationsQueue.Dequeue<SetPortalDestinationOperation>();

                // A node might have been deleted already if it for example was a child of a node that was deleted
                if (setPortalDestinationOperation.Node.Phase != FiberNodePhase.Mounted)
                {
                    return;
                }

                var node = setPortalDestinationOperation.Node;
                if (setPortalDestinationOperation.PortalDestination == node.PortalDestination)
                {
                    return;
                }

                var portalDestinationBefore = node.PortalDestination;
                node.PortalDestination = setPortalDestinationOperation.PortalDestination;

                if (portalDestinationBefore == null)
                {
                    // Remove old native node references from parent and remove from parent native node 
                    var closestParentWithNativeNode = node.FindClosestAncestorWithNativeNodeOrPortalDestination(includeSelf: false);
                    for (var i = closestParentWithNativeNode.NativeNodeChildren.Count - 1; i >= 0; --i)
                    {
                        var nativeNodeChild = closestParentWithNativeNode.NativeNodeChildren[i];
                        if (nativeNodeChild.IsDescendentOf(node))
                        {
                            closestParentWithNativeNode.NativeNodeChildren.RemoveAt(i);
                            closestParentWithNativeNode.NativeNode.RemoveChild(nativeNodeChild, destroyInstance: false);
                        }
                    }

                    // Add to new destination
                    node.PortalDestination.NativeNode.AddChild(node, 0);
                    // We currently don't care about the ordering of the native node children in the portal destination
                    if (node.PortalDestination.NativeNodeChildren == null)
                    {
                        node.PortalDestination.NativeNodeChildren = new() { node };
                    }
                    else
                    {
                        node.PortalDestination.NativeNodeChildren.Add(node);
                    }
                }
                else
                {
                    portalDestinationBefore.NativeNode.RemoveChild(node, destroyInstance: false);
                    portalDestinationBefore.NativeNodeChildren.Remove(node);

                    if (node.PortalDestination == null)
                    {
                        var closestParentWithNativeNode = node.FindClosestAncestorWithNativeNodeOrPortalDestination();
                        var index = closestParentWithNativeNode.FindNativeNodeIndex(node);
                        closestParentWithNativeNode.NativeNodeChildren.Insert(index, node);
                        closestParentWithNativeNode.NativeNode.AddChild(node, index);
                    }
                    else
                    {
                        node.PortalDestination.NativeNode.AddChild(node, 0);
                        if (node.PortalDestination.NativeNodeChildren == null)
                        {
                            node.PortalDestination.NativeNodeChildren = new() { node };
                        }
                        else
                        {
                            node.PortalDestination.NativeNodeChildren.Add(node);
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
                    if (fiberNode.VirtualNode is PortalDestinationBaseComponent && fiberNode.NativeNodeChildren != null)
                    {
                        // Give back all the native nodes to its original parents
                        for (var i = fiberNode.NativeNodeChildren.Count - 1; i >= 0; --i)
                        {
                            var nativeNodeChild = fiberNode.NativeNodeChildren[i];
                            nativeNodeChild.PortalDestination = null;

                            var closestParentWithNativeNode = nativeNodeChild.FindClosestAncestorWithNativeNodeOrPortalDestination();
                            var index = closestParentWithNativeNode.FindNativeNodeIndex(nativeNodeChild);
                            closestParentWithNativeNode.NativeNodeChildren.Insert(index, nativeNodeChild);
                            closestParentWithNativeNode.NativeNode.AddChild(nativeNodeChild, index);
                        }
                    }

                    var closestAncestor = parent.FindClosestAncestorWithNativeNode(includeSelf: true);
                    closestAncestor.NativeNode.RemoveChild(fiberNode, destroyInstance: true);
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
            var closestAncesorWithNativeNode = _currentFiberNode.FindClosestAncestorWithNativeNodeOrPortalDestination();
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

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, ItemType>(
            Action<T1, IList<ItemType>> compute, ISignal<T1> signal1
        )
        {
            return new InlineComputedShallowSignalList<T1, ItemType>(compute, signal1);
        }

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, ItemType>(
            Action<T1, T2, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2
        )
        {
            return new InlineComputedShallowSignalList<T1, T2, ItemType>(compute, signal1, signal2);
        }

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, ItemType>(
            Action<T1, T2, T3, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        )
        {
            return new InlineComputedShallowSignalList<T1, T2, T3, ItemType>(compute, signal1, signal2, signal3);
        }

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, ItemType>(
            Action<T1, T2, T3, T4, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        )
        {
            return new InlineComputedShallowSignalList<T1, T2, T3, T4, ItemType>(compute, signal1, signal2, signal3, signal4);
        }

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType>(
            Action<T1, T2, T3, T4, T5, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        )
        {
            return new InlineComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType>(compute, signal1, signal2, signal3, signal4, signal5);
        }

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6
        )
        {
            return new InlineComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        }

        public ISignalList<ItemType> CreateComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6,
            ISignal<T7> signal7
        )
        {
            return new InlineComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, ItemType>(
            Action<T1, IList<ItemType>> compute, ISignal<T1> signal1
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, ItemType>(compute, signal1);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, ItemType>(
            Action<T1, T2, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, T2, ItemType>(compute, signal1, signal2);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, ItemType>(
            Action<T1, T2, T3, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, T2, T3, ItemType>(compute, signal1, signal2, signal3);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, ItemType>(
            Action<T1, T2, T3, T4, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, ItemType>(compute, signal1, signal2, signal3, signal4);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, ItemType>(
            Action<T1, T2, T3, T4, T5, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, T5, ItemType>(compute, signal1, signal2, signal3, signal4, signal5);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, T5, T6, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        }

        public ISignalList<ItemType> CreateComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(
            Action<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>> compute,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6,
            ISignal<T7> signal7
        ) where ItemType : ISignal
        {
            return new InlineComputedSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        }

        public SignalList<ItemType> CreateSignalList<ItemType>(ISignal dependent = null) where ItemType : ISignal
        {
            return new SignalList<ItemType>(dependent);
        }

        public ShallowSignalList<ItemType> CreateShallowSignalList<ItemType>(ISignal dependent = null)
        {
            return new ShallowSignalList<ItemType>(dependent);
        }

        public Signal<T> CreateSignal<T>(T value = default, BaseSignal dependent = null)
        {
            return new Signal<T>(value, dependent);
        }

        public StaticSignal<T> CreateStaticSignal<T>(T value)
        {
            return new StaticSignal<T>(value);
        }

        public Ref<T> CreateRef<T>(T initialValue = default)
        {
            return new Ref<T>(initialValue);
        }

        public BaseSignal<T> ToSignal<T>(SignalProp<T> signalProp)
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

        private class FragmentComponent : VirtualNode
        {
            public FragmentComponent(VirtualBody children) : base(children, VirtualNodeType.Fragment) { }
        }

        public VirtualNode Enable(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new EnableComponent(whenSignal, children, this);
        }


        public class EnableContext
        {
            public class Subscription
            {
                public Action<bool> Handler;
                public bool LastValue;
                public Subscription(Action<bool> handler, bool lastValue)
                {
                    Handler = handler;
                    LastValue = lastValue;
                }
            }

            public IndexedDictionary<int, Subscription> Subscriptions;
            private readonly ISignal<bool> _whenSignal;
            private IntIdGenerator _idGenerator;

            public EnableContext(ISignal<bool> whenSignal)
            {
                _whenSignal = whenSignal;
                _idGenerator = new IntIdGenerator();
            }

            public int Subscribe(Action<bool> handler)
            {
                if (Subscriptions == null)
                {
                    Subscriptions = new();
                }
                var id = _idGenerator.NextId();
                var enabled = _whenSignal.Get();
                Subscriptions.Add(id, new Subscription(handler, enabled));
                handler(enabled);
                return id;
            }

            public void Unsubscribe(int subscriptionId)
            {
                var subscription = Subscriptions.Remove(subscriptionId);
                subscription.Handler(false);
            }
        }

        private class EnableComponent : VirtualNode
        {
            private readonly ISignal<bool> _whenSignal;
            private readonly Renderer _renderer;
            private readonly EnableContext _enableContext;
            private EnabledEffect _enabledEffect;

            public EnableComponent(ISignal<bool> whenSignal, VirtualBody children, Renderer renderer) : base(children, VirtualNodeType.EnableComponent)
            {
                _whenSignal = whenSignal;
                _renderer = renderer;
                _enableContext = new(_whenSignal);
            }

            private class EnabledEffect : Effect<bool>
            {
                private readonly FiberNode _fiberNode;
                private readonly Renderer _renderer;
                private readonly EnableContext _enableContext;

                public EnabledEffect(
                    ISignal<bool> whenSignal,
                    FiberNode fiberNode,
                    Renderer renderer,
                    EnableContext enableContext
                )
                    : base(whenSignal, runOnMount: true)
                {
                    _fiberNode = fiberNode;
                    _renderer = renderer;
                    _enableContext = enableContext;
                }

                protected override void Run(bool enabled)
                {
                    // We need to enable / disable the children instead of this node since 
                    // this effect won't otherwise run (since its node is disabled...)
                    _fiberNode.IsEnabled = enabled;
                    _renderer.AddFiberNodeToUpdateQueue(_fiberNode);
                    for (var node = _fiberNode.Child; node != null; node = node.NextNode(root: _fiberNode, skipChildren: false))
                    {
                        _renderer.AddFiberNodeToUpdateQueue(node);
                        if (node.VirtualNode is EnableComponent enableComponent)
                        {
                            // We need to run subscriptions here since decendent subscriptions might need to re-run based on
                            // the new enabled state of this node (a great ancestor). In other words, decendent enable components
                            // might not run since their where condition isn't tied to the enabled state of this node.
                            //
                            // An alternative solution would be for enable components to listen to the enabled state of their parent.
                            // The most natural would in that case be to create an "is enabled signal". However, that signal won't trigger
                            // effects in custom components since we treat custom components differently and thus it might be confusing.
                            enableComponent._enabledEffect.RunSubscriptions();
                        }
                    }
                    RunSubscriptions();
                }

                private void RunSubscriptions()
                {
                    if (_enableContext.Subscriptions != null)
                    {
                        var isFullTreeEnabled = _fiberNode.IsEnabled;
                        for (var i = 0; i < _enableContext.Subscriptions.Count; ++i)
                        {
                            var subscription = _enableContext.Subscriptions[i];
                            if (subscription.LastValue != isFullTreeEnabled)
                            {
                                subscription.Handler(isFullTreeEnabled);
                                subscription.LastValue = isFullTreeEnabled;
                            }
                        }
                    }
                }
                public override void Cleanup() { }
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                _enabledEffect = new EnabledEffect(_whenSignal, fiberNode, _renderer, _enableContext);
                fiberNode.PushEffect(_enabledEffect);

                return new ContextProvider<EnableContext>(
                    value: _enableContext,
                    children: Children
                );
            }
        }

        public VirtualNode Visible(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new VisibleComponent(whenSignal, children);
        }

        public class VisibleComponent : VirtualNode
        {
            public ISignal<bool> IsVisible { get => _isVisibleSignal; }
            private readonly ISignal<bool> _whenSignal;
            private ISignal<bool> _isVisibleSignal;

            public VisibleComponent(ISignal<bool> whenSignal, VirtualBody children) : base(children, VirtualNodeType.VisibleComponent)
            {
                _whenSignal = whenSignal;
            }

            private class VisibleEffect : Effect<bool>
            {
                private readonly FiberNode _fiberNode;
                public VisibleEffect(
                    ISignal<bool> isVisibleSignal,
                    FiberNode fiberNode
                )
                    : base(isVisibleSignal, runOnMount: true)
                {
                    _fiberNode = fiberNode;
                }

                protected override void Run(bool visible)
                {
                    // Iterate all decedents up to the point that a child is a VisibleComponent or a NativeNode
                    var node = _fiberNode.Child;
                    while (node != null && node != _fiberNode)
                    {
                        var hasNativeNode = node.NativeNode != null;

                        if (hasNativeNode)
                        {
                            node.NativeNode.SetVisible(visible);
                        }

                        var isChildVisibleComponent = node.Child != null && node.Child.VirtualNode is VisibleComponent;
                        var isCurrentVisibleComponent = node.VirtualNode is VisibleComponent; // Could be true if a Sibling is a VisibleComponent
                        node = node.NextNode(root: _fiberNode, skipChildren: isChildVisibleComponent || isCurrentVisibleComponent || hasNativeNode);
                    }
                }
                public override void Cleanup() { }
            }

            private VisibleComponent FindVisibleComponentAncestor(FiberNode fiberNode)
            {
                var current = fiberNode.Parent;
                while (current != null)
                {
                    if (current.VirtualNode != null && current.VirtualNode is VisibleComponent visibleComponent)
                    {
                        return visibleComponent;
                    }
                    current = current.Parent;
                }
                return null;
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                var ancestorVisibleComponent = FindVisibleComponentAncestor(fiberNode);
                var isAncestorVisible = ancestorVisibleComponent == null ? StaticSignals.TRUE : ancestorVisibleComponent.IsVisible;
                _isVisibleSignal = new InlineComputedSignal<bool, bool, bool>((when, isParentVisible) => when && isParentVisible, _whenSignal, isAncestorVisible);
                fiberNode.PushEffect(new VisibleEffect(_isVisibleSignal, fiberNode));
                return Children;
            }
        }

        public VirtualNode Active(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new ActiveComponent(whenSignal, children, this);
        }

        private class ActiveComponent : VirtualNode
        {
            private readonly ISignal<bool> _whenSignal;
            private readonly Renderer _renderer;

            public ActiveComponent(ISignal<bool> whenSignal, VirtualBody children, Renderer renderer) : base(children, VirtualNodeType.ActiveComponent)
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
                        Children,
                        _renderer
                    )
                );
            }
        }

        public VirtualNode Mount(ISignal<bool> whenSignal, VirtualBody children)
        {
            return new MountComponent(whenSignal, children, _renderQueue, _operationsQueue, this);
        }

        private class MountComponent : VirtualNode
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
            ) : base(children, VirtualNodeType.MountComponent)
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
                mountFiberNode.PushEffect(new MountEffect(_whenSignal, Children, _renderQueue, _operationsQueue, _renderer, mountFiberNode));
                return _whenSignal.Get() ? Children : VirtualBody.Empty;
            }
        }

        public VirtualNode For<ItemType, KeyType>(
            ISignalList<ItemType> each,
            Func<ItemType, int, VirtualNode> renderItem,
            Func<ItemType, int, KeyType> createItemKey
        )
        {
            return new ForComponent<ItemType, KeyType>(each, renderItem, createItemKey, _renderQueue, _operationsQueue, this);
        }

        // Class is only added in order to be able to type check when rendering (not possible with generic class)
        private abstract class BaseForComponent : VirtualNode
        {
            protected BaseForComponent() : base(VirtualBody.Empty, VirtualNodeType.ForComponent) { }
            public abstract void Render(FiberNode fiberNode);
        }

        private class ForComponent<ItemType, KeyType> : BaseForComponent
        {
            private readonly ISignalList<ItemType> _eachSignal;
            private readonly Func<ItemType, int, VirtualNode> _renderItem;
            private readonly Func<ItemType, int, KeyType> _createItemKey;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;
            private readonly Renderer _renderer;
            private readonly Dictionary<KeyType, int> _currentKeyToIdMap;
            private readonly Dictionary<int, KeyType> _currentIdToKeyMap;

            public ForComponent(
                ISignalList<ItemType> eachSignal,
                Func<ItemType, int, VirtualNode> renderItem,
                Func<ItemType, int, KeyType> createItemKey,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue,
                Renderer renderer
            ) : base()
            {
                _eachSignal = eachSignal;
                _renderItem = renderItem;
                _createItemKey = createItemKey;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;
                _renderer = renderer;

                _currentKeyToIdMap = new();
                _currentIdToKeyMap = new();
            }

            private class ForEffect : Effect<IList<ItemType>>
            {
                private readonly Func<ItemType, int, VirtualNode> _renderItem;
                private readonly Func<ItemType, int, KeyType> _createItemKey;
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly Renderer _renderer;
                private readonly FiberNode _fiberNode;
                private readonly Dictionary<KeyType, int> _currentKeyToIdMap;
                private readonly Dictionary<int, KeyType> _currentIdToKeyMap;
                private readonly List<KeyType> _allKeys;
                private readonly List<FiberNode> _previousFiberNodes;

                public ForEffect(
                    ISignalList<ItemType> eachSignal,
                    Func<ItemType, int, VirtualNode> renderItem,
                    Func<ItemType, int, KeyType> createItemKey,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    Renderer renderer,
                    FiberNode fiberNode,
                    Dictionary<KeyType, int> currentKeyToIdMap,
                    Dictionary<int, KeyType> currentIdToKeyMap
                )
                    : base(eachSignal, runOnMount: false)
                {
                    _renderItem = renderItem;
                    _createItemKey = createItemKey;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _renderer = renderer;
                    _fiberNode = fiberNode;
                    _currentKeyToIdMap = currentKeyToIdMap;
                    _currentIdToKeyMap = currentIdToKeyMap;
                    _allKeys = new(10);
                    _previousFiberNodes = new(10);
                }

                protected override void Run(IList<ItemType> each)
                {
                    _allKeys.Clear();

                    // Create all keys
                    for (var i = 0; i < each.Count; ++i)
                    {
                        var key = _createItemKey(each[i], i);
                        _allKeys.Add(key);
                    }

                    // Remove children not in the updated list
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

                    // Save previous fiber nodes
                    _previousFiberNodes.Clear();
                    for (var currentChild = _fiberNode.Child; currentChild != null; currentChild = currentChild.Sibling)
                    {
                        _previousFiberNodes.Add(currentChild);
                    }

                    // Move nodes to correct position and create new nodes
                    FiberNode previousChildFiberNode = null;
                    for (var i = 0; i < _allKeys.Count; ++i)
                    {
                        var key = _allKeys[i];

                        var currentChildFiberId = _currentKeyToIdMap.ContainsKey(key) ? _currentKeyToIdMap[key] : -1;

                        if (currentChildFiberId != -1)
                        {
                            // Find current child and index
                            FiberNode currentChildOfKey = null;
                            int currentChildOfKeyIndex = -1;
                            for (var j = 0; j < _previousFiberNodes.Count; ++j)
                            {
                                var previousChild = _previousFiberNodes[j];
                                if (_currentIdToKeyMap[previousChild.Id].Equals(key))
                                {
                                    currentChildOfKey = previousChild;
                                    currentChildOfKeyIndex = j;
                                    break;
                                }
                            }

                            // Update sibling / child references
                            if (i == 0)
                            {
                                _fiberNode.Child = currentChildOfKey;
                            }
                            else if (i == _allKeys.Count - 1)
                            {
                                currentChildOfKey.Sibling = null;
                            }

                            if (previousChildFiberNode != null)
                            {
                                previousChildFiberNode.Sibling = currentChildOfKey;
                            }
                            previousChildFiberNode = currentChildOfKey;


                            // Decide if we should move the child
                            if (currentChildOfKey != null && currentChildOfKeyIndex != i)
                            {
                                _operationsQueue.Enqueue(new MoveOperation(_fiberNode, currentChildOfKey, i));
                            }
                        }
                        else
                        {
                            // Create new child
                            var child = _renderItem(each[i], i);
                            var createdChildNode = new FiberNode(
                                renderer: _renderer,
                                nativeNode: null,
                                virtualNode: child,
                                parent: _fiberNode,
                                sibling: null
                            );

                            // Update sibling / child references
                            if (i == 0)
                            {
                                _fiberNode.Child = createdChildNode;
                            }
                            else if (previousChildFiberNode != null)
                            {
                                previousChildFiberNode.Sibling = createdChildNode;
                            }
                            previousChildFiberNode = createdChildNode;

                            // Add to render queue and update caches
                            _renderQueue.Enqueue(createdChildNode);
                            _currentKeyToIdMap.Add(key, createdChildNode.Id);
                            _currentIdToKeyMap.Add(createdChildNode.Id, key);
                        }
                    }
                }

                public override void Cleanup() { }
            }

            public override void Render(FiberNode fiberNode)
            {
                // We need to clear cache in case this instance has already been rendered before (mounted -> unmounted -> mounted again in for example a MatchComponent)
                _currentKeyToIdMap.Clear();
                _currentIdToKeyMap.Clear();


                fiberNode.PushEffect(new ForEffect(_eachSignal, _renderItem, _createItemKey, _renderQueue, _operationsQueue, _renderer, fiberNode, _currentKeyToIdMap, _currentIdToKeyMap));

                var each = _eachSignal.Get();
                FiberNode previousChildFiberNode = null;
                for (var i = 0; i < each.Count; ++i)
                {
                    var key = _createItemKey(each[i], i);
                    var child = _renderItem(each[i], i);

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

        public VirtualNode Switch(VirtualBody fallback, VirtualBody children)
        {
            return new SwitchComponent(fallback, children, _renderQueue, _operationsQueue, this);
        }

        private class SwitchComponent : VirtualNode
        {
            private readonly VirtualBody _fallback;
            private readonly SignalList<ISignal<bool>> _matchSignals;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;
            private readonly Renderer _renderer;

            public SwitchComponent(
                VirtualBody fallback,
                VirtualBody children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue,
                Renderer renderer
            )
                : base(children, VirtualNodeType.SwitchComponent)
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
                public static readonly ObjectPool<SwitchEffect> Pool = new(InitialCapacityConstants.SMALL, null, preload: false);

                private VirtualBody _children;
                private VirtualBody _fallback;
                private FiberNode _fiberNode;
                private Queue<FiberNode> _renderQueue;
                private MixedQueue _operationsQueue;
                private Renderer _renderer;
                private Ref<int> _lastRenderedIndexRef;

                public SwitchEffect() : base(null) { }

                public SwitchEffect(
                    SignalList<ISignal<bool>> matchSignals,
                    VirtualBody children,
                    VirtualBody fallback,
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

                public SwitchEffect Initialize(
                    SignalList<ISignal<bool>> matchSignals,
                    VirtualBody children,
                    VirtualBody fallback,
                    FiberNode fiberNode,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    Renderer renderer,
                    Ref<int> lastRenderedIndexRef
                )
                {
                    base.Initialize(matchSignals, runOnMount: false);

                    _children = children;
                    _fallback = fallback;
                    _fiberNode = fiberNode;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _renderer = renderer;
                    _lastRenderedIndexRef = lastRenderedIndexRef;

                    return this;
                }

                public override void Dispose()
                {
                    base.Dispose();
                    Pool.Release(this);
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
                    if (!_fallback.IsEmpty)
                    {
                        for (var i = 0; i < _fallback.Count; ++i)
                        {
                            var child = _fallback[i];
                            var childFiberNode = new FiberNode(
                                renderer: _renderer,
                                nativeNode: null,
                                virtualNode: child,
                                parent: _fiberNode,
                                sibling: null
                            );
                            if (i == 0)
                            {
                                _fiberNode.Child = childFiberNode;
                            }
                            else
                            {
                                var previousChild = _fiberNode.Child;
                                while (previousChild.Sibling != null)
                                {
                                    previousChild = previousChild.Sibling;
                                }
                                previousChild.Sibling = childFiberNode;
                            }
                            _renderQueue.Enqueue(childFiberNode);
                        }
                    }
                    _lastRenderedIndexRef.Current = -1;
                }
                public override void Cleanup() { }
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                var _lastRenderedIndexRef = new Ref<int>(-1);
                fiberNode.PushEffect(SwitchEffect.Pool.Get().Initialize(_matchSignals, Children, _fallback, fiberNode, _renderQueue, _operationsQueue, _renderer, _lastRenderedIndexRef));

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];
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

        private class MatchComponent : VirtualNode
        {
            public ISignal<bool> When { get; private set; }

            public MatchComponent(ISignal<bool> when, VirtualBody children)
                : base(children, VirtualNodeType.MatchComponent)
            {
                When = when;
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                return Children;
            }
        }

        public VirtualNode Portal(SignalProp<string> destinationId, VirtualBody children)
        {
            return new PortalComponent(children: children, destinationId: destinationId, _operationsQueue);
        }

        private class PortalComponent : VirtualNode
        {
            private readonly SignalProp<string> _destinationId;
            private readonly MixedQueue _operationsQueue;
            public PortalComponent(
                VirtualBody children,
                SignalProp<string> destinationId,
                MixedQueue operationsQueue
            ) : base(children, VirtualNodeType.PortalComponent)
            {
                _destinationId = destinationId;
                _operationsQueue = operationsQueue;
            }

            private class PortalEffect : Effect<string, ShallowSignalDictionary<string, FiberNode>>
            {
                private readonly FiberNode _fiberNode;
                private readonly MixedQueue _operationsQueue;
                private string _lastId;
                public PortalEffect(
                    BaseSignal<string> id,
                    ShallowSignalDictionary<string, FiberNode> portalDestinations,
                    FiberNode fiberNode,
                    MixedQueue operationsQueue
                ) : base(id, portalDestinations)
                {
                    _fiberNode = fiberNode;
                    _operationsQueue = operationsQueue;
                }

                protected override void Run(string id, ShallowSignalDictionary<string, FiberNode> portalDestinations)
                {
                    _lastId = id;

                    FiberNode portalDestination = string.IsNullOrWhiteSpace(id) || !portalDestinations.ContainsKey(id) ? null : portalDestinations[id];
                    var node = _fiberNode.NextNode(root: _fiberNode);
                    while (node != null)
                    {
                        if (node.NativeNode != null)
                        {
                            if (node.PortalDestination != portalDestination)
                            {
                                _operationsQueue.Enqueue(new SetPortalDestinationOperation(node: node, portalDestination: portalDestination));
                            }
                            node = node.NextNode(root: _fiberNode, skipChildren: true);
                        }
                        else if (node.PortalDestination != null)
                        {
                            node = node.NextNode(root: _fiberNode, skipChildren: true);
                        }
                        else
                        {
                            node = node.NextNode(root: _fiberNode, skipChildren: false);
                        }
                    }
                }

                public override void Cleanup()
                {
                    // Hacky, but prevents effect from running again when the id is the same as last time,
                    // but still runs when the fiber node is unmounted.
                    if (_lastId != null && _lastId.Equals(_signal1.Get()) && _fiberNode.Phase == FiberNodePhase.Mounted && _fiberNode.PortalDestinations.ContainsKey(_lastId))
                    {
                        return;
                    }

                    var node = _fiberNode.NextNode(root: _fiberNode);
                    while (node != null)
                    {
                        if (node.NativeNode != null)
                        {
                            _operationsQueue.Enqueue(new SetPortalDestinationOperation(node: node, portalDestination: null));
                            node = node.NextNode(root: _fiberNode, skipChildren: true);
                        }
                        else if (node.PortalDestination != null)
                        {
                            node = node.NextNode(root: _fiberNode, skipChildren: true);
                        }
                        else
                        {
                            node = node.NextNode(root: _fiberNode, skipChildren: false);
                        }
                    }
                }
            }

            private class PortalMoveBeforeRemovalEffect : Effect
            {
                private readonly FiberNode _fiberNode;
                private readonly BaseSignal<string> _id;
                private readonly ShallowSignalDictionary<string, FiberNode> _portalDestinations;
                public PortalMoveBeforeRemovalEffect(
                    FiberNode fiberNode,
                    BaseSignal<string> id,
                    ShallowSignalDictionary<string, FiberNode> portalDestinations
                ) : base()
                {
                    _fiberNode = fiberNode;
                    _id = id;
                    _portalDestinations = portalDestinations;
                }

                protected override void Run() { }

                public override void Cleanup()
                {
                    var id = _id.Get();
                    if (_fiberNode.PortalDestination == null || string.IsNullOrWhiteSpace(id) || !_portalDestinations.ContainsKey(id))
                    {
                        return;
                    }

                    // Move the native node back to the its original parent. Before being removed again by Fiber.
                    _fiberNode.PortalDestination.NativeNode.RemoveChild(_fiberNode, destroyInstance: false);
                    _fiberNode.PortalDestination.NativeNodeChildren.Remove(_fiberNode);

                    var closestParentWithNativeNode = _fiberNode.FindClosestAncestorWithNativeNodeOrPortalDestination();
                    var index = closestParentWithNativeNode.FindNativeNodeIndex(_fiberNode);
                    closestParentWithNativeNode.NativeNodeChildren.Insert(index, _fiberNode);
                    closestParentWithNativeNode.NativeNode.AddChild(_fiberNode, index);
                }
            }

            public VirtualBody Render(FiberNode fiberNode)
            {
                var idSignal = _destinationId.ToSignal();
                fiberNode.PushEffect(new PortalEffect(idSignal, fiberNode.PortalDestinations, fiberNode, _operationsQueue));
                fiberNode.PushEffect(new PortalMoveBeforeRemovalEffect(fiberNode, idSignal, fiberNode.PortalDestinations));
                return Children;
            }
        }
    }


    public abstract class PortalDestinationBaseComponent : VirtualNode
    {
        protected readonly string _id;
        public PortalDestinationBaseComponent(
            string id
        ) : base(VirtualBody.Empty, VirtualNodeType.PortalDestinationComponent)
        {
            _id = id;
        }

        private class PortalDefinitionEffect : Effect
        {
            private readonly string _id;
            private readonly FiberNode _fiberNode;
            public PortalDefinitionEffect(string id, FiberNode fiberNode) : base()
            {
                _id = id;
                _fiberNode = fiberNode;
            }

            protected override void Run()
            {
                var child = _fiberNode.Child;
                _fiberNode.RegisterPortalDestination(_id, child);
            }

            public override void Cleanup()
            {
                _fiberNode.UnregisterPortalDestination(_id);
            }
        }

        protected void BaseImplementation(FiberNode fiberNode)
        {
            fiberNode.PushEffect(new PortalDefinitionEffect(_id, fiberNode));
        }
    }
}
