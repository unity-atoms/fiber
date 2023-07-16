using System;
using FiberUtils;

namespace Signals
{
    // Manager class that lets you subscribe to changes to signals.
    public class SignalSubscribtionManager
    {
        private IndexedDictionary<int, Action> _subscriptions;
        private IntIdGenerator _idGenerator;
        private int _onUpdateSubscriptionId;

        public SignalSubscribtionManager()
        {
            _subscriptions = new IndexedDictionary<int, Action>();
            _idGenerator = new IntIdGenerator();
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
            for (var i = 0; i < _subscriptions.Count; ++i)
            {
                _subscriptions.GetByIndex(i)();
            }
        }

        public int Subscribe<T>(
            BaseSignal<T> signal,
            Action<T> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var initialValue = signal.Get();
            byte dirtyBit = signal.DirtyBit;

            Action subscriber = () =>
            {
                var newValue = signal.Get();
                if (dirtyBit != signal.DirtyBit)
                {
                    dirtyBit = signal.DirtyBit;
                    onChange(newValue);
                }
            };

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscriber);

            if (triggerHandlerOnSubscribe)
            {
                onChange(initialValue);
            }

            return id;
        }

        public int Subscribe<T1, T2>(
            BaseSignal<T1> signal1, BaseSignal<T2> signal2,
            Action<T1, T2> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var initialValue1 = signal1.Get();
            var initialValue2 = signal2.Get();
            byte dirtyBit1 = signal1.DirtyBit;
            byte dirtyBit2 = signal2.DirtyBit;

            Action subscriber = () =>
            {
                var newValue1 = signal1.Get();
                var newValue2 = signal2.Get();
                if (dirtyBit1 != signal1.DirtyBit || dirtyBit2 != signal2.DirtyBit)
                {
                    dirtyBit1 = signal1.DirtyBit;
                    dirtyBit2 = signal2.DirtyBit;
                    onChange(newValue1, newValue2);
                }
            };

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscriber);

            if (triggerHandlerOnSubscribe)
            {
                onChange(initialValue1, initialValue2);
            }

            return id;
        }

        public int Subscribe<T1, T2, T3, T4>(
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
            BaseSignal<T4> signal4,
            Action<T1, T2, T3, T4> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var initialValue1 = signal1.Get();
            var initialValue2 = signal2.Get();
            var initialValue3 = signal3.Get();
            var initialValue4 = signal4.Get();
            byte dirtyBit1 = signal1.DirtyBit;
            byte dirtyBit2 = signal2.DirtyBit;
            byte dirtyBit3 = signal3.DirtyBit;
            byte dirtyBit4 = signal4.DirtyBit;

            Action subscriber = () =>
            {
                var newValue1 = signal1.Get();
                var newValue2 = signal2.Get();
                var newValue3 = signal3.Get();
                var newValue4 = signal4.Get();
                if (dirtyBit1 != signal1.DirtyBit || dirtyBit2 != signal2.DirtyBit || dirtyBit3 != signal3.DirtyBit || dirtyBit4 != signal4.DirtyBit)
                {
                    dirtyBit1 = signal1.DirtyBit;
                    dirtyBit2 = signal2.DirtyBit;
                    dirtyBit3 = signal3.DirtyBit;
                    dirtyBit4 = signal4.DirtyBit;
                    onChange(newValue1, newValue2, newValue3, newValue4);
                }
            };

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscriber);

            if (triggerHandlerOnSubscribe)
            {
                onChange(initialValue1, initialValue2, initialValue3, initialValue4);
            }

            return id;
        }

        public int Subscribe<T1, T2, T3>(
            BaseSignal<T1> signal1,
            BaseSignal<T2> signal2,
            BaseSignal<T3> signal3,
            Action<T1, T2, T3> onChange,
            bool triggerHandlerOnSubscribe = true
        )
        {
            var initialValue1 = signal1.Get();
            var initialValue2 = signal2.Get();
            var initialValue3 = signal3.Get();
            byte dirtyBit1 = signal1.DirtyBit;
            byte dirtyBit2 = signal2.DirtyBit;
            byte dirtyBit3 = signal3.DirtyBit;

            Action subscriber = () =>
            {
                var newValue1 = signal1.Get();
                var newValue2 = signal2.Get();
                var newValue3 = signal3.Get();
                if (dirtyBit1 != signal1.DirtyBit || dirtyBit2 != signal2.DirtyBit || dirtyBit3 != signal3.DirtyBit)
                {
                    dirtyBit1 = signal1.DirtyBit;
                    dirtyBit2 = signal2.DirtyBit;
                    dirtyBit3 = signal3.DirtyBit;
                    onChange(newValue1, newValue2, newValue3);
                }
            };

            var id = _idGenerator.NextId();
            _subscriptions.Add(id, subscriber);

            if (triggerHandlerOnSubscribe)
            {
                onChange(initialValue1, initialValue2, initialValue3);
            }

            return id;
        }

        public void Unsubscribe(int id)
        {
            _subscriptions.Remove(id);
        }
    }
}