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
            Current = default(T);
        }
        public Ref(T initialValue)
        {
            Current = initialValue;
        }
    }

    public abstract class NativeNode
    {
        public abstract void AddChild(FiberNode node, int index);
        public abstract void RemoveChild(FiberNode node);
        public abstract void MoveChild(FiberNode node, int index);
        public abstract void WorkLoop();
        public abstract void SetVisible(bool visible);
    }

    public interface IComponentAPI
    {
        public VirtualNode ContextProvider<C>(C value, List<VirtualNode> children);
        public VirtualNode ContextProvider<C>(List<VirtualNode> children);
        public T GetGlobal<T>();
        public C GetContext<C>(FiberNode node);
        public NativeNode GetParentNativeNode();
        public void CreateEffect(BaseEffect effect);
        public void CreateEffect(Func<Action> effect);
        public void CreateEffect<T1>(Func<T1, Action> effect, BaseSignal<T1> signal1, bool runOnMount);
        public void CreateEffect<T1, T2>(
            Func<T1, T2, Action> effect,
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            bool runOnMount
        );
        public void CreateEffect<T1, T2, T3>(
            Func<T1, T2, T3, Action> effect,
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
            bool runOnMount
        );
        public void CreateUpdateEffect(Action<float> onUpdate);
        public ComputedSignal<T1, RT> CreateComputedSignal<T1, RT>(
            Func<T1, RT> compute, BaseSignal<T1> signal1
        );
        public ComputedSignal<T1, T2, RT> CreateComputedSignal<T1, T2, RT>(
            Func<T1, T2, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2
        );
        public ComputedSignal<T1, T2, T3, RT> CreateComputedSignal<T1, T2, T3, RT>(
            Func<T1, T2, T3, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3
        );
        public ComputedSignal<T1, T2, T3, T4, RT> CreateComputedSignal<T1, T2, T3, T4, RT>(
            Func<T1, T2, T3, T4, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4
        );
        public ComputedSignal<T1, T2, T3, T4, T5, RT> CreateComputedSignal<T1, T2, T3, T4, T5, RT>(
            Func<T1, T2, T3, T4, T5, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5
        );
        public ComputedSignal<T1, T2, T3, T4, T5, T6, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(
            Func<T1, T2, T3, T4, T5, T6, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6
        );
        public ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(
            Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6, BaseSignal<T7> signal7
        );
        public VirtualNode Fragment(List<VirtualNode> children);
        public VirtualNode Enable(BaseSignal<bool> whenSignal, List<VirtualNode> children);
        public VirtualNode Visible(BaseSignal<bool> whenSignal, List<VirtualNode> children);
        public VirtualNode Active(BaseSignal<bool> whenSignal, List<VirtualNode> children);
        public VirtualNode Mount(BaseSignal<bool> whenSignal, List<VirtualNode> children);
        public VirtualNode For<ItemType, SignalType, SignalReturnType, KeyType>(
            SignalType each,
            Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children
        )
            where SignalType : BaseSignal<SignalReturnType>
            where SignalReturnType : IList<ItemType>;
        public VirtualNode Switch(VirtualNode fallback, List<VirtualNode> children);
        public VirtualNode Match(BaseSignal<bool> when, List<VirtualNode> children);
    }

    public interface IEffectAPI
    {
        public C GetContext<C>(FiberNode node);
        public T GetGlobal<T>();
    }

    public abstract class BaseEffect
    {
        public IEffectAPI Api { private get; set; }
        public FiberNode FiberNode { private get; set; }

        public C GetContext<C>() => Api.GetContext<C>(FiberNode);
        public C C<C>() => GetContext<C>();
        public T GetGlobal<T>() => Api.GetGlobal<T>();
        public T G<T>() => GetGlobal<T>();

        public abstract void RunIfDirty();
        public abstract void Cleanup();
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
        private DynamicSignals<T> _dynamicSignals;
        bool _hasRun = false;

        public DynamicEffect(IList<BaseSignal<T>> signals, bool runOnMount = true)
        {
            _dynamicSignals = new DynamicSignals<T>(signals, runOnMount);
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

        protected abstract void Run(DynamicSignals<T> signals);
    }

    public abstract class Effect<T1> : BaseEffect
    {
        protected BaseSignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        bool _hasRun = false;

        public Effect(
            BaseSignal<T1> signal1,
            bool runOnMount = true
        )
        {
            _signal1 = signal1;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - (runOnMount ? 1 : 0));
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
        protected BaseSignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected BaseSignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        bool _hasRun = false;

        public Effect(
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            bool runOnMount = true
        )
        {
            _signal1 = signal1;
            _signal2 = signal2;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - (runOnMount ? 1 : 0));
            _lastDirtyBit2 = (byte)(signal2.DirtyBit - (runOnMount ? 1 : 0));
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
        protected BaseSignal<T1> _signal1;
        protected byte _lastDirtyBit1;
        protected BaseSignal<T2> _signal2;
        protected byte _lastDirtyBit2;
        protected BaseSignal<T3> _signal3;
        protected byte _lastDirtyBit3;
        bool _hasRun = false;

        public Effect(
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
            bool runOnMount = true
        )
        {
            _signal1 = signal1;
            _signal2 = signal2;
            _signal3 = signal3;
            _lastDirtyBit1 = (byte)(signal1.DirtyBit - (runOnMount ? 1 : 0));
            _lastDirtyBit2 = (byte)(signal2.DirtyBit - (runOnMount ? 1 : 0));
            _lastDirtyBit3 = (byte)(signal3.DirtyBit - (runOnMount ? 1 : 0));
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

        public InlineEffect(Func<T1, Action> effect, BaseSignal<T1> signal1, bool runOnMount)
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
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
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
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
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
        public InlineComputedSignal(Func<T1, RT> compute, BaseSignal<T1> signal1)
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
        public InlineComputedSignal(Func<T1, T2, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2)
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
        public InlineComputedSignal(Func<T1, T2, T3, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3)
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
        public InlineComputedSignal(Func<T1, T2, T3, T4, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4)
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
        public InlineComputedSignal(Func<T1, T2, T3, T4, T5, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5)
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
        public InlineComputedSignal(Func<T1, T2, T3, T4, T5, T6, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6)
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
        public InlineComputedSignal(Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6, BaseSignal<T7> signal7)
            : base(signal1, signal2, signal3, signal4, signal5, signal6, signal7)
        {
            _compute = compute;
        }
        protected override RT Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            return _compute(value1, value2, value3, value4, value5, value6, value7);
        }
    }

    public class VirtualNode
    {
        private List<VirtualNode> _children;
        public List<VirtualNode> children { get => _children; }

        public VirtualNode(List<VirtualNode> children)
        {
            _children = children;
        }
    }

    public abstract class BaseContextProvider : VirtualNode
    {
        public BaseContextProvider(List<VirtualNode> children) : base(children) { }
    }

    public class ContextProvider<C> : BaseContextProvider
    {
        public C Value { get; private set; }
        public ContextProvider(C value, List<VirtualNode> children) : base(children)
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

        public ContextProvider<C> ContextProvider(C value, List<VirtualNode> children)
        {
            return new ContextProvider<C>(value, children);
        }

        public ContextProvider<C> ContextProvider(List<VirtualNode> children)
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

        public BaseComponent() : base(null) { }
        public BaseComponent(List<VirtualNode> children) : base(children) { }
        public abstract VirtualNode Render();

        public VirtualNode ContextProvider<C>(C value, List<VirtualNode> children) => Api.ContextProvider<C>(value, children);
        public VirtualNode ContextProvider<C>(List<VirtualNode> children) => Api.ContextProvider<C>(children);
        public T GetGlobal<T>() => Api.GetGlobal<T>();
        public T G<T>() => Api.GetGlobal<T>();
        public C GetContext<C>() => Api.GetContext<C>(FiberNode);
        public C C<C>() => GetContext<C>();
        public NativeNode GetParentNativeNode() => Api.GetParentNativeNode();
        public void CreateEffect(BaseEffect effect) => Api.CreateEffect(effect);
        public void CreateEffect(Func<Action> effect) => Api.CreateEffect(effect);
        public void CreateEffect<T1>(Func<T1, Action> effect, BaseSignal<T1> signal1, bool runOnMount = true)
        {
            Api.CreateEffect(effect, signal1, runOnMount);
        }
        public void CreateEffect<T1, T2>(
            Func<T1, T2, Action> effect,
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            bool runOnMount = true
        )
        {
            Api.CreateEffect(effect, signal1, signal2, runOnMount);
        }
        public void CreateEffect<T1, T2, T3>(
            Func<T1, T2, T3, Action> effect,
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
            bool runOnMount = true
        )
        {
            Api.CreateEffect(effect, signal1, signal2, signal3, runOnMount);
        }
        public void CreateUpdateEffect(Action<float> onUpdate) => Api.CreateUpdateEffect(onUpdate);
        public ComputedSignal<T1, RT> CreateComputedSignal<T1, RT>(
            Func<T1, RT> compute, BaseSignal<T1> signal1
        ) => Api.CreateComputedSignal<T1, RT>(compute, signal1);
        public ComputedSignal<T1, T2, RT> CreateComputedSignal<T1, T2, RT>(
            Func<T1, T2, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2
        ) => Api.CreateComputedSignal<T1, T2, RT>(compute, signal1, signal2);
        public ComputedSignal<T1, T2, T3, RT> CreateComputedSignal<T1, T2, T3, RT>(
            Func<T1, T2, T3, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3
        ) => Api.CreateComputedSignal<T1, T2, T3, RT>(compute, signal1, signal2, signal3);
        public ComputedSignal<T1, T2, T3, T4, RT> CreateComputedSignal<T1, T2, T3, T4, RT>(
            Func<T1, T2, T3, T4, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, RT>(compute, signal1, signal2, signal3, signal4);
        public ComputedSignal<T1, T2, T3, T4, T5, RT> CreateComputedSignal<T1, T2, T3, T4, T5, RT>(
            Func<T1, T2, T3, T4, T5, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, T5, RT>(compute, signal1, signal2, signal3, signal4, signal5);
        public ComputedSignal<T1, T2, T3, T4, T5, T6, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(
            Func<T1, T2, T3, T4, T5, T6, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        public ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(
            Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6, BaseSignal<T7> signal7
        ) => Api.CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
        public VirtualNode Fragment(List<VirtualNode> children) => Api.Fragment(children);
        public VirtualNode Enable(BaseSignal<bool> when, List<VirtualNode> children) => Api.Enable(when, children);
        public VirtualNode Visible(BaseSignal<bool> when, List<VirtualNode> children) => Api.Visible(when, children);
        public VirtualNode Active(BaseSignal<bool> when, List<VirtualNode> children) => Api.Active(when, children);
        public VirtualNode Mount(BaseSignal<bool> when, List<VirtualNode> children) => Api.Mount(when, children);
        public VirtualNode For<ItemType, SignalType, SignalReturnType, KeyType>(
            SignalType each,
            Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children
        )
            where SignalType : BaseSignal<SignalReturnType>
            where SignalReturnType : IList<ItemType>
        {
            return Api.For<ItemType, SignalType, SignalReturnType, KeyType>(each, children);
        }
        public VirtualNode Switch(VirtualNode fallback, List<VirtualNode> children) => Api.Switch(fallback, children);
        public VirtualNode Match(BaseSignal<bool> when, List<VirtualNode> children) => Api.Match(when, children);
        public List<VirtualNode> Children()
        {
            var children = new List<VirtualNode>();
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            children.Add(c5);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            children.Add(c5);
            children.Add(c6);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            children.Add(c5);
            children.Add(c6);
            children.Add(c7);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7, VirtualNode c8)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            children.Add(c5);
            children.Add(c6);
            children.Add(c7);
            children.Add(c8);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7, VirtualNode c8, VirtualNode c9)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            children.Add(c5);
            children.Add(c6);
            children.Add(c7);
            children.Add(c8);
            children.Add(c9);
            return children;
        }
        public List<VirtualNode> Children(VirtualNode c1, VirtualNode c2, VirtualNode c3, VirtualNode c4, VirtualNode c5, VirtualNode c6, VirtualNode c7, VirtualNode c8, VirtualNode c9, VirtualNode c10)
        {
            var children = new List<VirtualNode>();
            children.Add(c1);
            children.Add(c2);
            children.Add(c3);
            children.Add(c4);
            children.Add(c5);
            children.Add(c6);
            children.Add(c7);
            children.Add(c8);
            children.Add(c9);
            children.Add(c10);
            return children;
        }
    }

    public abstract class Component<P> : BaseComponent
    {
        public P Props { get; set; }

        public Component(P props, List<VirtualNode> children = null) : base(children)
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

    public class FiberNode
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
        public bool IsEnabled { get; set; } = true;

        private List<BaseEffect> _effects = new List<BaseEffect>();

        public FiberNode(
            NativeNode nativeNode,
            VirtualNode virtualNode,
            FiberNode parent,
            FiberNode sibling
        )
        {
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

        public void WorkLoop()
        {
            if (NativeNode != null)
            {
                NativeNode.WorkLoop();
            }

            RunEffects();
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

        public FiberNode NextEnabledNode(FiberNode root = null)
        {
            FiberNode current = this;
            do
            {
                current = current.NextNode(root);
            }
            while (current != null && !current.IsEnabled && current.Phase != FiberNodePhase.Mounted);

            return current;
        }
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
        private FiberNode _currentWorkLoopNode = null;
        public void WorkLoop(bool immediatelyExecuteRemainingWork = false)
        {
            _stopWatch.Restart();

            // Reset the current work loop node to the root
            if (_currentWorkLoopNode == null)
            {
                _currentWorkLoopNode = _root;
            }

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
                else if (_currentWorkLoopNode != null)
                {
                    _currentWorkLoopNode.WorkLoop();
                    _currentWorkLoopNode = _currentWorkLoopNode.NextEnabledNode();
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
                var child = component.Render();
                if (child != null)
                {
                    var childFiberNode = new FiberNode(
                        nativeNode: null,
                        virtualNode: child,
                        parent: fiberNode,
                        sibling: null
                    );
                    fiberNode.Child = childFiberNode;
                    _renderQueue.Enqueue(childFiberNode);
                }
            }
            else if (fiberNode.VirtualNode is EnableComponent enableComponent)
            {
                var children = enableComponent.Render(fiberNode);
                RenderChildren(fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode is VisibleComponent visibleComponent)
            {
                var children = visibleComponent.Render(fiberNode);
                RenderChildren(fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode is ActiveComponent activeComponent)
            {
                var child = activeComponent.Render(fiberNode);
                if (child != null)
                {
                    var childFiberNode = new FiberNode(
                        nativeNode: null,
                        virtualNode: child,
                        parent: fiberNode,
                        sibling: null
                    );
                    fiberNode.Child = childFiberNode;
                    _renderQueue.Enqueue(childFiberNode);
                }
            }
            else if (fiberNode.VirtualNode is MountComponent mountComponent)
            {
                var children = mountComponent.Render(fiberNode);
                RenderChildren(fiberNode, children, _renderQueue);
            }
            else if (fiberNode.VirtualNode is BaseForComponent forComponent)
            {
                // For component will handle the rendering of its children by itself.
                // The reason is that it need to keep track of its children and their
                // corresponding keys.
                forComponent.Render(fiberNode);
            }
            else if (fiberNode.VirtualNode is SwitchComponent switchComponent)
            {
                var child = switchComponent.Render(fiberNode);
                if (child != null)
                {
                    var childFiberNode = new FiberNode(
                        nativeNode: null,
                        virtualNode: child,
                        parent: fiberNode,
                        sibling: null
                    );
                    fiberNode.Child = childFiberNode;
                    _renderQueue.Enqueue(childFiberNode);
                }
            }
            else
            {
                // OPEN POINT: Should we really create the native node here? Isn't better to that when we commit the work?
                fiberNode.NativeNode = CreateNativeNode(fiberNode);
                if (fiberNode.NativeNode != null)
                {
                    fiberNode.NativeNode.SetVisible(false);
                }
                RenderChildren(fiberNode, fiberNode.VirtualNode.children, _renderQueue);
            }

            fiberNode.Phase = FiberNodePhase.Rendered;
            _operationsQueue.Enqueue(new MountOperation(node: fiberNode));
        }

        public static void RenderChildren(FiberNode fiberNode, List<VirtualNode> children, Queue<FiberNode> renderQueue)
        {
            FiberNode previousChildFiberNode = null;
            for (var i = 0; children != null && i < children.Count; ++i)
            {
                var child = children[i];
                if (child != null)
                {
                    var childFiberNode = new FiberNode(
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

            if (virtualNode is FragmentComponent || virtualNode is BaseContextProvider || virtualNode is MatchComponent)
            {
                return null;
            }

            throw new Exception($"Unknown virtual node {virtualNode}.");
        }

        public T GetGlobal<T>()
        {
            var type = typeof(T);
            if (_globals.ContainsKey(type))
            {
                return (T)_globals[type];
            }

            throw new Exception($"Global of type {type} not found. Did you forget to add it to the renderer?");
        }

        public VirtualNode ContextProvider<C>(C value, List<VirtualNode> children)
        {
            return _contextsAPI.GetContext<C>().ContextProvider(value, children);
        }

        public VirtualNode ContextProvider<C>(List<VirtualNode> children)
        {
            return _contextsAPI.GetContext<C>().ContextProvider(children);
        }

        public C GetContext<C>(FiberNode node)
        {
            var fiberNode = node;

            while (fiberNode != null && !(fiberNode.VirtualNode is ContextProvider<C>))
            {
                fiberNode = fiberNode.Parent;
            }

            if (fiberNode == null)
            {
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
            var inlineEffect = new InlineEffect(effect);
            inlineEffect.Api = this;
            inlineEffect.FiberNode = _currentFiberNode;
            _currentFiberNode.PushEffect(inlineEffect);
        }

        public void CreateEffect<T1>(Func<T1, Action> effect, BaseSignal<T1> signal1, bool runOnMount)
        {
            var inlineEffect = new InlineEffect<T1>(effect, signal1, runOnMount);
            inlineEffect.Api = this;
            inlineEffect.FiberNode = _currentFiberNode;
            _currentFiberNode.PushEffect(inlineEffect);
        }

        public void CreateEffect<T1, T2>(
            Func<T1, T2, Action> effect,
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            bool runOnMount
        )
        {
            var inlineEffect = new InlineEffect<T1, T2>(effect, signal1, signal2, runOnMount);
            inlineEffect.Api = this;
            inlineEffect.FiberNode = _currentFiberNode;
            _currentFiberNode.PushEffect(inlineEffect);
        }

        public void CreateEffect<T1, T2, T3>(
            Func<T1, T2, T3, Action> effect,
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
            bool runOnMount
        )
        {
            var inlineEffect = new InlineEffect<T1, T2, T3>(effect, signal1, signal2, signal3, runOnMount);
            inlineEffect.Api = this;
            inlineEffect.FiberNode = _currentFiberNode;
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

        public ComputedSignal<T1, RT> CreateComputedSignal<T1, RT>(
            Func<T1, RT> compute, BaseSignal<T1> signal1
        )
        {
            return new InlineComputedSignal<T1, RT>(compute, signal1);
        }

        public ComputedSignal<T1, T2, RT> CreateComputedSignal<T1, T2, RT>(
            Func<T1, T2, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2
        )
        {
            return new InlineComputedSignal<T1, T2, RT>(compute, signal1, signal2);
        }

        public ComputedSignal<T1, T2, T3, RT> CreateComputedSignal<T1, T2, T3, RT>(
            Func<T1, T2, T3, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3
        )
        {
            return new InlineComputedSignal<T1, T2, T3, RT>(compute, signal1, signal2, signal3);
        }

        public ComputedSignal<T1, T2, T3, T4, RT> CreateComputedSignal<T1, T2, T3, T4, RT>(
            Func<T1, T2, T3, T4, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, RT>(compute, signal1, signal2, signal3, signal4);
        }

        public ComputedSignal<T1, T2, T3, T4, T5, RT> CreateComputedSignal<T1, T2, T3, T4, T5, RT>(
            Func<T1, T2, T3, T4, T5, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, T5, RT>(compute, signal1, signal2, signal3, signal4, signal5);
        }

        public ComputedSignal<T1, T2, T3, T4, T5, T6, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, RT>(
            Func<T1, T2, T3, T4, T5, T6, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, T5, T6, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6);
        }

        public ComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT> CreateComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(
            Func<T1, T2, T3, T4, T5, T6, T7, RT> compute, BaseSignal<T1> signal1, BaseSignal<T2> signal2, BaseSignal<T3> signal3, BaseSignal<T4> signal4, BaseSignal<T5> signal5, BaseSignal<T6> signal6, BaseSignal<T7> signal7
        )
        {
            return new InlineComputedSignal<T1, T2, T3, T4, T5, T6, T7, RT>(compute, signal1, signal2, signal3, signal4, signal5, signal6, signal7);
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

        public VirtualNode Fragment(List<VirtualNode> children)
        {
            return new FragmentComponent(children);
        }

        private class FragmentComponent : VirtualNode
        {
            public FragmentComponent(List<VirtualNode> children) : base(children) { }
        }

        public VirtualNode Enable(BaseSignal<bool> whenSignal, List<VirtualNode> children)
        {
            return new EnableComponent(whenSignal, children);
        }

        private class EnableComponent : VirtualNode
        {
            private readonly BaseSignal<bool> _whenSignal;

            public EnableComponent(BaseSignal<bool> whenSignal, List<VirtualNode> children) : base(children)
            {
                _whenSignal = whenSignal;
            }

            private class EnabledEffect : Effect<bool>
            {
                private readonly FiberNode _fiberNode;
                public EnabledEffect(
                    BaseSignal<bool> whenSignal,
                    FiberNode fiberNode
                )
                    : base(whenSignal, runOnMount: true)
                {
                    _fiberNode = fiberNode;
                }
                protected override void Run(bool enabled)
                {
                    _fiberNode.IsEnabled = enabled;
                }
                public override void Cleanup() { }
            }

            public List<VirtualNode> Render(FiberNode fiberNode)
            {
                fiberNode.PushEffect(new EnabledEffect(_whenSignal, fiberNode));
                return children;
            }
        }

        public VirtualNode Visible(BaseSignal<bool> whenSignal, List<VirtualNode> children)
        {
            return new VisibleComponent(whenSignal, children);
        }

        private class VisibleComponent : VirtualNode
        {
            public bool IsVisible { get => _whenSignal.Get(); }
            private readonly BaseSignal<bool> _whenSignal;
            public VisibleComponent(BaseSignal<bool> whenSignal, List<VirtualNode> children) : base(children)
            {
                _whenSignal = whenSignal;
            }

            private class VisibleEffect : Effect<bool>
            {
                private readonly FiberNode _fiberNode;
                public VisibleEffect(
                    BaseSignal<bool> whenSignal,
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

            public List<VirtualNode> Render(FiberNode fiberNode)
            {
                fiberNode.PushEffect(new VisibleEffect(_whenSignal, fiberNode));
                return children;
            }
        }

        public VirtualNode Active(BaseSignal<bool> whenSignal, List<VirtualNode> children)
        {
            return new ActiveComponent(whenSignal, children);
        }

        private class ActiveComponent : VirtualNode
        {
            private readonly BaseSignal<bool> _whenSignal;
            public ActiveComponent(BaseSignal<bool> whenSignal, List<VirtualNode> children) : base(children)
            {
                _whenSignal = whenSignal;
            }

            public VirtualNode Render(FiberNode fiberNode)
            {
                return new VisibleComponent(
                    _whenSignal,
                    new List<VirtualNode> {
                        new EnableComponent(
                            _whenSignal,
                            children
                        )
                    }
                );
            }
        }

        public VirtualNode Mount(BaseSignal<bool> whenSignal, List<VirtualNode> children)
        {
            return new MountComponent(whenSignal, children, _renderQueue, _operationsQueue);
        }

        private class MountComponent : VirtualNode
        {
            private readonly BaseSignal<bool> _whenSignal;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;

            public MountComponent(
                BaseSignal<bool> whenSignal,
                List<VirtualNode> children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue
            ) : base(children)
            {
                _whenSignal = whenSignal;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;
            }

            private class MountEffect : Effect<bool>
            {
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly FiberNode _mountFiberNode;
                private readonly List<VirtualNode> _children;
                private bool _valueLastTime;

                public MountEffect(
                    BaseSignal<bool> whenSignal,
                    List<VirtualNode> children,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    FiberNode mountFiberNode
                )
                    : base(whenSignal, runOnMount: false)
                {
                    _children = children;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
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
                        Renderer.RenderChildren(_mountFiberNode, _children, _renderQueue);
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

            public List<VirtualNode> Render(FiberNode mountFiberNode)
            {
                mountFiberNode.PushEffect(new MountEffect(_whenSignal, children, _renderQueue, _operationsQueue, mountFiberNode));
                return _whenSignal.Get() ? children : null;
            }
        }

        public VirtualNode For<ItemType, SignalType, SignalReturnType, KeyType>(
            SignalType each,
            Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children
        )
            where SignalType : BaseSignal<SignalReturnType>
            where SignalReturnType : IList<ItemType>
        {
            return new ForComponent<ItemType, SignalType, SignalReturnType, KeyType>(each, children, _renderQueue, _operationsQueue);
        }

        // Class is only added in order to be able to type check when rendering (not possible with generic class)
        private abstract class BaseForComponent : VirtualNode
        {
            protected BaseForComponent() : base(null) { }
            public abstract void Render(FiberNode fiberNode);
        }

        private class ForComponent<ItemType, SignalType, SignalReturnType, KeyType> : BaseForComponent
            where SignalType : BaseSignal<SignalReturnType>
            where SignalReturnType : IList<ItemType>
        {
            private readonly SignalType _eachSignal;
            private readonly Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> _children;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;
            private readonly Dictionary<KeyType, int> _currentKeyToIdMap;
            private readonly Dictionary<int, KeyType> _currentIdToKeyMap;

            public ForComponent(
                SignalType eachSignal,
                Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue
            ) : base()
            {
                _eachSignal = eachSignal;
                _children = children;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;

                _currentKeyToIdMap = new();
                _currentIdToKeyMap = new();
            }

            private class ForEffect : Effect<SignalReturnType>
            {
                private readonly Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> _children;
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly FiberNode _fiberNode;
                private readonly Dictionary<KeyType, int> _currentKeyToIdMap;
                private readonly Dictionary<int, KeyType> _currentIdToKeyMap;
                private readonly HashSet<KeyType> _allKeys;

                public ForEffect(
                    SignalType eachSignal,
                    Func<ItemType, int, ValueTuple<KeyType, VirtualNode>> children,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    FiberNode fiberNode,
                    Dictionary<KeyType, int> currentKeyToIdMap,
                    Dictionary<int, KeyType> currentIdToKeyMap
                )
                    : base(eachSignal, runOnMount: false)
                {
                    _children = children;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _fiberNode = fiberNode;
                    _currentKeyToIdMap = currentKeyToIdMap;
                    _currentIdToKeyMap = currentIdToKeyMap;
                    _allKeys = new();
                }

                protected override void Run(SignalReturnType each)
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
                fiberNode.PushEffect(new ForEffect(_eachSignal, _children, _renderQueue, _operationsQueue, fiberNode, _currentKeyToIdMap, _currentIdToKeyMap));

                var each = _eachSignal.Get();
                FiberNode previousChildFiberNode = null;
                for (var i = 0; i < each.Count; ++i)
                {
                    var (key, child) = _children(each[i], i);

                    if (child != null)
                    {
                        var childFiberNode = new FiberNode(
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

        public VirtualNode Switch(VirtualNode fallback, List<VirtualNode> children)
        {
            return new SwitchComponent(fallback, children, _renderQueue, _operationsQueue);
        }

        private class SwitchComponent : VirtualNode
        {
            private readonly VirtualNode _fallback;
            private readonly SignalList<BaseSignal<bool>> _matchSignals;
            private readonly Queue<FiberNode> _renderQueue;
            private readonly MixedQueue _operationsQueue;

            public SwitchComponent(
                VirtualNode fallback,
                List<VirtualNode> children,
                Queue<FiberNode> renderQueue,
                MixedQueue operationsQueue
            )
                : base(children)
            {
                _fallback = fallback;
                _renderQueue = renderQueue;
                _operationsQueue = operationsQueue;

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
                private readonly List<VirtualNode> _children;
                private readonly VirtualNode _fallback;
                private readonly FiberNode _fiberNode;
                private readonly Queue<FiberNode> _renderQueue;
                private readonly MixedQueue _operationsQueue;
                private readonly Ref<int> _lastRenderedIndexRef;

                public SwitchEffect(
                    SignalList<BaseSignal<bool>> matchSignals,
                    List<VirtualNode> children,
                    VirtualNode fallback,
                    FiberNode fiberNode,
                    Queue<FiberNode> renderQueue,
                    MixedQueue operationsQueue,
                    Ref<int> lastRenderedIndexRef
                )
                : base(matchSignals, runOnMount: false)
                {
                    _children = children;
                    _fallback = fallback;
                    _fiberNode = fiberNode;
                    _renderQueue = renderQueue;
                    _operationsQueue = operationsQueue;
                    _lastRenderedIndexRef = lastRenderedIndexRef;
                }
                protected override void Run(DynamicSignals<bool> matchSignals)
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
                            nativeNode: null,
                            virtualNode: _fallback,
                            parent: _fiberNode,
                            sibling: null
                        );
                        _fiberNode.Child = childFiberNode;
                        _renderQueue.Enqueue(childFiberNode);
                    }
                }
                public override void Cleanup() { }
            }

            public VirtualNode Render(FiberNode fiberNode)
            {
                var _lastRenderedIndexRef = new Ref<int>(-1);
                fiberNode.PushEffect(new SwitchEffect(_matchSignals, children, _fallback, fiberNode, _renderQueue, _operationsQueue, _lastRenderedIndexRef));

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

        public VirtualNode Match(BaseSignal<bool> when, List<VirtualNode> children)
        {
            return new MatchComponent(when, children);
        }

        private class MatchComponent : VirtualNode
        {
            public BaseSignal<bool> When { get; private set; }

            public MatchComponent(BaseSignal<bool> when, List<VirtualNode> children)
                : base(children)
            {
                When = when;
            }

            public List<VirtualNode> Render()
            {
                return children;
            }
        }
    }
}
