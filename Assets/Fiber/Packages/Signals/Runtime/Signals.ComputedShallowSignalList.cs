using System;
using System.Collections.Generic;

namespace Signals
{
    [Serializable]
    public abstract class ComputedShallowSignalList<T1, ItemType> : ComputedSignal<T1, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1
        ) : base(signal1)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1)
        {
            _list.Clear();
            ComputeList(value1, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, IList<ItemType> list);
    }

    [Serializable]
    public abstract class ComputedShallowSignalList<T1, T2, ItemType> : ComputedSignal<T1, T2, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1,
            ISignal<T2> signal2
        ) : base(signal1, signal2)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1, T2 value2)
        {
            _list.Clear();
            ComputeList(value1, value2, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, T2 value2, IList<ItemType> list);
    }

    [Serializable]
    public abstract class ComputedShallowSignalList<T1, T2, T3, ItemType> : ComputedSignal<T1, T2, T3, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3
        ) : base(signal1, signal2, signal3)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3)
        {
            _list.Clear();
            ComputeList(value1, value2, value3, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, T2 value2, T3 value3, IList<ItemType> list);
    }

    [Serializable]
    public abstract class ComputedShallowSignalList<T1, T2, T3, T4, ItemType> : ComputedSignal<T1, T2, T3, T4, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4
        ) : base(signal1, signal2, signal3, signal4)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _list.Clear();
            ComputeList(value1, value2, value3, value4, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, T2 value2, T3 value3, T4 value4, IList<ItemType> list);
    }

    [Serializable]
    public abstract class ComputedShallowSignalList<T1, T2, T3, T4, T5, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5
        ) : base(signal1, signal2, signal3, signal4, signal5)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            _list.Clear();
            ComputeList(value1, value2, value3, value4, value5, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, IList<ItemType> list);
    }

    [Serializable]
    public abstract class ComputedShallowSignalList<T1, T2, T3, T4, T5, T6, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6
        ) : base(signal1, signal2, signal3, signal4, signal5, signal6)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            _list.Clear();
            ComputeList(value1, value2, value3, value4, value5, value6, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, IList<ItemType> list);
    }

    [Serializable]
    public abstract class ComputedShallowSignalList<T1, T2, T3, T4, T5, T6, T7, ItemType> : ComputedSignal<T1, T2, T3, T4, T5, T6, T7, IList<ItemType>>, ISignalList<ItemType>
    {
        private readonly ShallowSignalList<ItemType> _list;
        public int Count => LastValue?.Count ?? 0;
        public ItemType GetAt(int index) => LastValue != null && LastValue.Count > index ? LastValue[index] : default;

        public ComputedShallowSignalList(
            ISignal<T1> signal1,
            ISignal<T2> signal2,
            ISignal<T3> signal3,
            ISignal<T4> signal4,
            ISignal<T5> signal5,
            ISignal<T6> signal6,
            ISignal<T7> signal7
        ) : base(signal1, signal2, signal3, signal4, signal5, signal6, signal7)
        {
            _list = new();
        }

        protected sealed override IList<ItemType> Compute(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            _list.Clear();
            ComputeList(value1, value2, value3, value4, value5, value6, value7, _list);
            return _list;
        }

        protected abstract void ComputeList(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, IList<ItemType> list);
    }
}