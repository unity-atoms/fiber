using System;
using System.Collections.Generic;
using FiberUtils;

namespace Signals
{
    public interface ISignalSubscription
    {
        void Invoke();
        void OnUnsubscribe();
    }

    public class SignalSubscription<T1> : BaseSignal, ISignalSubscription
    {
        private readonly Action<ISignalSubscription> _addToDirtySubscriptions;
        private readonly ISignal<T1> _signal1;
        private byte _dirtyBit1;
        private readonly Action<T1> _onChange;

        public SignalSubscription(
            Action<ISignalSubscription> addToDirtySubscriptions,
            ISignal<T1> signal1,
            Action<T1> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            _addToDirtySubscriptions = addToDirtySubscriptions;
            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _dirtyBit1 = _signal1.DirtyBit;
            _onChange = onChange;

            if (triggerHandlerOnSubscribe)
            {
                _onChange(_signal1.Get());
            }
        }

        protected override void OnNotifySignalUpdate()
        {
            _addToDirtySubscriptions(this);
        }
        public void Invoke()
        {
            var newValue = _signal1.Get();
            if (_dirtyBit1 != _signal1.DirtyBit)
            {
                _dirtyBit1 = _signal1.DirtyBit;
                _onChange(newValue);
            }
        }
        public void OnUnsubscribe()
        {
            _signal1.UnregisterDependent(this);
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    public class SignalSubscription<T1, T2> : BaseSignal, ISignalSubscription
    {
        private readonly Action<ISignalSubscription> _addToDirtySubscriptions;
        private readonly ISignal<T1> _signal1;
        private byte _dirtyBit1;
        private readonly ISignal<T2> _signal2;
        private byte _dirtyBit2;
        private readonly Action<T1, T2> _onChange;

        public SignalSubscription(
            Action<ISignalSubscription> addToDirtySubscriptions,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            Action<T1, T2> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            _addToDirtySubscriptions = addToDirtySubscriptions;
            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _dirtyBit1 = _signal1.DirtyBit;
            _signal2 = signal2;
            _signal2.RegisterDependent(this);
            _dirtyBit2 = _signal2.DirtyBit;
            _onChange = onChange;

            if (triggerHandlerOnSubscribe)
            {
                _onChange(_signal1.Get(), _signal2.Get());
            }
        }

        protected override void OnNotifySignalUpdate()
        {
            _addToDirtySubscriptions(this);
        }
        public void Invoke()
        {
            var newValue1 = _signal1.Get();
            var newValue2 = _signal2.Get();
            if (_dirtyBit1 != _signal1.DirtyBit || _dirtyBit2 != _signal2.DirtyBit)
            {
                _dirtyBit1 = _signal1.DirtyBit;
                _dirtyBit2 = _signal2.DirtyBit;
                _onChange(newValue1, newValue2);
            }
        }
        public void OnUnsubscribe()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    public class SignalSubscription<T1, T2, T3> : BaseSignal, ISignalSubscription
    {
        private readonly Action<ISignalSubscription> _addToDirtySubscriptions;
        private readonly ISignal<T1> _signal1;
        private byte _dirtyBit1;
        private readonly ISignal<T2> _signal2;
        private byte _dirtyBit2;
        private readonly ISignal<T3> _signal3;
        private byte _dirtyBit3;
        private readonly Action<T1, T2, T3> _onChange;

        public SignalSubscription(
            Action<ISignalSubscription> addToDirtySubscriptions,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            Action<T1, T2, T3> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            _addToDirtySubscriptions = addToDirtySubscriptions;
            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _dirtyBit1 = _signal1.DirtyBit;
            _signal2 = signal2;
            _signal2.RegisterDependent(this);
            _dirtyBit2 = _signal2.DirtyBit;
            _signal3 = signal3;
            _signal3.RegisterDependent(this);
            _dirtyBit3 = _signal3.DirtyBit;
            _onChange = onChange;

            if (triggerHandlerOnSubscribe)
            {
                _onChange(_signal1.Get(), _signal2.Get(), _signal3.Get());
            }
        }

        protected override void OnNotifySignalUpdate()
        {
            _addToDirtySubscriptions(this);
        }
        public void Invoke()
        {
            var newValue1 = _signal1.Get();
            var newValue2 = _signal2.Get();
            var newValue3 = _signal3.Get();
            if (_dirtyBit1 != _signal1.DirtyBit || _dirtyBit2 != _signal2.DirtyBit || _dirtyBit3 != _signal3.DirtyBit)
            {
                _dirtyBit1 = _signal1.DirtyBit;
                _dirtyBit2 = _signal2.DirtyBit;
                _dirtyBit3 = _signal3.DirtyBit;
                _onChange(newValue1, newValue2, newValue3);
            }
        }
        public void OnUnsubscribe()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    public class SignalSubscription<T1, T2, T3, T4> : BaseSignal, ISignalSubscription
    {
        private readonly Action<ISignalSubscription> _addToDirtySubscriptions;
        private readonly ISignal<T1> _signal1;
        private byte _dirtyBit1;
        private readonly ISignal<T2> _signal2;
        private byte _dirtyBit2;
        private readonly ISignal<T3> _signal3;
        private byte _dirtyBit3;
        private readonly ISignal<T4> _signal4;
        private byte _dirtyBit4;
        private readonly Action<T1, T2, T3, T4> _onChange;

        public SignalSubscription(
            Action<ISignalSubscription> addToDirtySubscriptions,
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            Action<T1, T2, T3, T4> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            _addToDirtySubscriptions = addToDirtySubscriptions;
            _signal1 = signal1;
            _signal1.RegisterDependent(this);
            _dirtyBit1 = _signal1.DirtyBit;
            _signal2 = signal2;
            _signal2.RegisterDependent(this);
            _dirtyBit2 = _signal2.DirtyBit;
            _signal3 = signal3;
            _signal3.RegisterDependent(this);
            _dirtyBit3 = _signal3.DirtyBit;
            _signal4 = signal4;
            _signal4.RegisterDependent(this);
            _dirtyBit4 = _signal4.DirtyBit;
            _onChange = onChange;

            if (triggerHandlerOnSubscribe)
            {
                _onChange(_signal1.Get(), _signal2.Get(), _signal3.Get(), _signal4.Get());
            }
        }

        protected override void OnNotifySignalUpdate()
        {
            _addToDirtySubscriptions(this);
        }
        public void Invoke()
        {
            var newValue1 = _signal1.Get();
            var newValue2 = _signal2.Get();
            var newValue3 = _signal3.Get();
            var newValue4 = _signal4.Get();
            if (_dirtyBit1 != _signal1.DirtyBit || _dirtyBit2 != _signal2.DirtyBit || _dirtyBit3 != _signal3.DirtyBit || _dirtyBit4 != _signal4.DirtyBit)
            {
                _dirtyBit1 = _signal1.DirtyBit;
                _dirtyBit2 = _signal2.DirtyBit;
                _dirtyBit3 = _signal3.DirtyBit;
                _dirtyBit4 = _signal4.DirtyBit;
                _onChange(newValue1, newValue2, newValue3, newValue4);
            }
        }
        public void OnUnsubscribe()
        {
            _signal1.UnregisterDependent(this);
            _signal2.UnregisterDependent(this);
            _signal3.UnregisterDependent(this);
            _signal4.UnregisterDependent(this);
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

    // Manager class that lets you subscribe to changes to signals.
    public class SignalSubscribtionManager
    {
        private Dictionary<int, ISignalSubscription> _subscriptions;
        private Queue<ISignalSubscription> _dirtySubscriptions;
        private IntIdGenerator _idGenerator;
        private int _onUpdateSubscriptionId;

        public SignalSubscribtionManager()
        {
            _subscriptions = new();
            _dirtySubscriptions = new();
            _idGenerator = new();
        }

        public void Initialize()
        {
            _onUpdateSubscriptionId = MonoBehaviourHelper.AddOnUpdateHandler(OnUpdate);
        }

        public void Cleanup()
        {
            MonoBehaviourHelper.RemoveOnUpdateHandler(_onUpdateSubscriptionId);
        }

        void OnUpdate(float deltaTime)
        {
            while (_dirtySubscriptions.Count > 0)
            {
                var subscription = _dirtySubscriptions.Dequeue();
                subscription.Invoke();
            }
        }

        public int Subscribe<T>(
            ISignal<T> signal1,
            Action<T> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var subscription = new SignalSubscription<T>(
                addToDirtySubscriptions: (s) => { _dirtySubscriptions.Enqueue(s); },
                signal1: signal1,
                onChange: onChange,
                triggerHandlerOnSubscribe: triggerHandlerOnSubscribe
            );

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscription);

            return id;
        }

        public int Subscribe<T1, T2>(
            ISignal<T1> signal1, ISignal<T2> signal2,
            Action<T1, T2> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var subscription = new SignalSubscription<T1, T2>(
                addToDirtySubscriptions: (s) => { _dirtySubscriptions.Enqueue(s); },
                signal1: signal1,
                signal2: signal2,
                onChange: onChange,
                triggerHandlerOnSubscribe: triggerHandlerOnSubscribe
            );

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscription);

            return id;
        }

        public int Subscribe<T1, T2, T3>(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            Action<T1, T2, T3> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var subscription = new SignalSubscription<T1, T2, T3>(
                addToDirtySubscriptions: (s) => { _dirtySubscriptions.Enqueue(s); },
                signal1: signal1,
                signal2: signal2,
                signal3: signal3,
                onChange: onChange,
                triggerHandlerOnSubscribe: triggerHandlerOnSubscribe
            );

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscription);

            return id;
        }

        public int Subscribe<T1, T2, T3, T4>(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            Action<T1, T2, T3, T4> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var subscription = new SignalSubscription<T1, T2, T3, T4>(
                addToDirtySubscriptions: (s) => { _dirtySubscriptions.Enqueue(s); },
                signal1: signal1,
                signal2: signal2,
                signal3: signal3,
                signal4: signal4,
                onChange: onChange,
                triggerHandlerOnSubscribe: triggerHandlerOnSubscribe
            );

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscription);

            return id;
        }

        public void Unsubscribe(int id)
        {
            var subsciption = _subscriptions[id];
            subsciption.OnUnsubscribe();
            _subscriptions.Remove(id);
        }
    }
}