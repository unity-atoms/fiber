using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Signals
{
    public abstract class BaseSignal
    {
        [SerializeField]
        protected byte _dirtyBit = 0;
        public byte DirtyBit
        {
            get => _dirtyBit;
            set
            {
                _dirtyBit = value;
                if (_parent != null)
                {
                    _parent.DirtyBit = (byte)(_parent.DirtyBit + 1);
                }
            }
        }

        protected BaseSignal _parent;
        public void RegisterParent(BaseSignal parent)
        {
            _parent = parent;
        }

        public void UnregisterParent()
        {
            _parent = null;
        }

        public abstract bool IsDirty(byte otherDirtyBit);
    }

    [Serializable]
    public abstract class BaseSignal<T> : BaseSignal
    {
        public abstract T Get();
    }

    public class NullableSignal<T, ST> : BaseSignal<T>
        where ST : BaseSignal<T>
    {
        private ST _wrappedSignal;
        public NullableSignal(ST wrappedSignal)
        {
            _wrappedSignal = wrappedSignal;
            if (_wrappedSignal != null)
            {
                _wrappedSignal.RegisterParent(this);
            }
        }
        ~NullableSignal()
        {
            if (_wrappedSignal != null)
            {
                _wrappedSignal.UnregisterParent();
            }
        }

        public override T Get()
        {
            return _wrappedSignal == null ? default(T) : _wrappedSignal.Get();
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            Get(); // We need to run the signal in order to update the dirty bit
            return DirtyBit != otherDirtyBit;
        }
    }

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

        public override T Get()
        {
            return _value;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    [Serializable]
    public class Signal<T> : BaseSignal<T>
    {
        [SerializeField]
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                DirtyBit = (byte)(DirtyBit + 1);
            }
        }

        public Signal(T value = default(T), BaseSignal parent = null)
        {
            _value = value;
            _dirtyBit = 0;
            _parent = parent;
        }

        public override T Get()
        {
            return Value;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    [Serializable]
    public class Store<T> : BaseSignal<T>
        where T : BaseSignal
    {
        [SerializeField]
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (_value != null)
                {
                    _value.UnregisterParent();
                }
                _value = value;
                if (_value != null)
                {
                    _value.RegisterParent(this);
                }
                DirtyBit = (byte)(DirtyBit + 1);
            }
        }

        public Store(T value = default(T), BaseSignal parent = null)
        {
            _value = value;
            if (_value != null)
            {
                _value.RegisterParent(this);
            }
            _dirtyBit = 0;
            _parent = parent;
        }

        public override T Get()
        {
            return Value;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    [Serializable]
    public abstract class BaseSignalList<T, LT> : BaseSignal<LT> where LT : ISignalList<T>
    {
        [SerializeField]
        protected List<T> _list;

        public int Count => _list.Count;
        public bool IsReadOnly { get => false; }

        public T GetAt(int index)
        {
            return _list[index];
        }

        public override int GetHashCode()
        {
            return _list.GetHashCode();
        }
    }


    public interface ISignalList<T>
    {
        int Count { get; }
        T GetAt(int index);
    }

    // Doesn't track changes of items, only mutations to the list itself
    [Serializable]
    public class ShallowSignalList<T> : BaseSignalList<T, ShallowSignalList<T>>, IList<T>, ISignalList<T>
    {
        const int DEFAULT_CAPACITY = 5;

        public ShallowSignalList()
        {
            _list = new(DEFAULT_CAPACITY);
        }

        public ShallowSignalList(BaseSignal parent = null)
        {
            _list = new(DEFAULT_CAPACITY);
            RegisterParent(parent);
        }

        public ShallowSignalList(int capacity = DEFAULT_CAPACITY, BaseSignal parent = null)
        {
            _list = new(capacity);
            RegisterParent(parent);
        }

        public ShallowSignalList(IList<T> source, BaseSignal parent = null)
        {
            _list = new(source?.Count ?? DEFAULT_CAPACITY);
            for (var i = 0; source != null && i < source.Count; ++i)
            {
                _list.Add(source[i]);
            }
            RegisterParent(parent);
        }

        public virtual T this[int i]
        {
            get { return _list[i]; }
            set { _list[i] = value; DirtyBit++; }
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public virtual void AddRange(IList<T> source)
        {
            for (var i = 0; i < source.Count; ++i)
            {
                _list.Add(source[i]);
            }
            DirtyBit++;
        }

        public virtual void Add(T item)
        {
            _list.Add(item);
            DirtyBit++;
        }

        public virtual void Insert(int index, T item)
        {
            _list.Insert(index, item);
            DirtyBit++;
        }

        public virtual bool Remove(T item)
        {
            var result = _list.Remove(item);
            DirtyBit++;
            return result;
        }

        public virtual void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            DirtyBit++;
        }

        public virtual void Clear()
        {
            _list.Clear();
            DirtyBit++;
        }
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public override ShallowSignalList<T> Get()
        {
            return this;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    // Tracks both mutations to the list and changes to the items in the list.
    [Serializable]
    public class SignalList<T> : BaseSignalList<T, SignalList<T>>, IList<T>, ISignalList<T>
        where T : BaseSignal
    {
        const int DEFAULT_CAPACITY = 5;

        public SignalList()
        {
            _list = new(DEFAULT_CAPACITY);
        }

        public SignalList(int capacity = DEFAULT_CAPACITY, BaseSignal parent = null)
        {
            _list = new(capacity);
            RegisterParent(parent);
        }

        public SignalList(IList<T> source, BaseSignal parent = null)
        {
            _list = new(source?.Count ?? DEFAULT_CAPACITY);
            for (var i = 0; source != null && i < source.Count; ++i)
            {
                _list.Add(source[i]);
                if (source[i] != null)
                {
                    source[i].RegisterParent(this);
                }
            }
            RegisterParent(parent);
        }


        public T this[int i]
        {
            get { return _list[i]; }
            set
            {
                var oldItem = _list[i];
                if (oldItem != null)
                {
                    oldItem.UnregisterParent();
                }

                _list[i] = value;
                if (_list[i] != null)
                {
                    _list[i].RegisterParent(this);
                }
            }
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void AddRange(IList<T> source)
        {
            for (var i = 0; i < source.Count; ++i)
            {
                _list.Add(source[i]);
                source[i].RegisterParent(this);
            }
            DirtyBit++;
        }

        public void Add(T item)
        {
            _list.Add(item);
            DirtyBit++;
            if (item != null)
            {
                item.RegisterParent(this);
            }
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            DirtyBit++;
            if (item != null)
            {
                item.RegisterParent(this);
            }
        }

        public bool Remove(T item)
        {
            var result = _list.Remove(item);
            DirtyBit++;

            if (item != null)
            {
                item.UnregisterParent();
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            DirtyBit++;
            if (item != null)
            {
                item.UnregisterParent();
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                _list[i].UnregisterParent();
            }
            _list.Clear();
            DirtyBit++;
        }
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public override SignalList<T> Get()
        {
            return this;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    // Doesn't track changes of values, only mutations to the dictionary itself
    [Serializable]
    public class ShallowSignalDictionary<K, V> : BaseSignal<ShallowSignalDictionary<K, V>>
    {
        protected Dictionary<K, V> _dict;

        public int Count => _dict.Count;

        const int DEFAULT_CAPACITY = 5;

        public ShallowSignalDictionary()
        {
            _dict = new(DEFAULT_CAPACITY);
        }

        public ShallowSignalDictionary(int capacity = DEFAULT_CAPACITY, BaseSignal parent = null)
        {
            _dict = new(capacity);
            RegisterParent(parent);
        }

        public ShallowSignalDictionary(IDictionary<K, V> source, BaseSignal parent = null)
        {
            _dict = new(source);
            RegisterParent(parent);
        }

        public V this[K key]
        {
            get { return _dict[key]; }
            set { _dict[key] = value; }
        }

        public void AddRange(IDictionary<K, V> source)
        {
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
            }
            DirtyBit++;
        }


        public V Add(K key, V value)
        {
            _dict.Add(key, value);
            DirtyBit++;
            return value;
        }

        public V Remove(K key)
        {
            var value = _dict[key];
            _dict.Remove(key);
            DirtyBit++;
            return value;
        }

        public V GetByKey(K key)
        {
            if (!_dict.ContainsKey(key))
            {
                return default(V);
            }
            return _dict[key];
        }

        public void Clear()
        {
            _dict.Clear();
            DirtyBit++;
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        public override ShallowSignalDictionary<K, V> Get()
        {
            return this;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }

    // Tracks both mutations to the dictionary and changes to the values in the dictionary.
    [Serializable]
    public class SignalDictionary<K, V> : BaseSignal<SignalDictionary<K, V>>, IEnumerable
        where V : BaseSignal
    {
        protected Dictionary<K, V> _dict;

        public int Count => _dict.Count;

        const int DEFAULT_CAPACITY = 5;

        public SignalDictionary()
        {
            _dict = new(DEFAULT_CAPACITY);
        }

        public SignalDictionary(int capacity = DEFAULT_CAPACITY, BaseSignal parent = null)
        {
            _dict = new(capacity);
            RegisterParent(parent);
        }

        public SignalDictionary(IDictionary<K, V> source, BaseSignal parent = null)
        {
            _dict = new(source?.Count ?? DEFAULT_CAPACITY);
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                if (kvp.Value != null)
                {
                    kvp.Value.RegisterParent(this);
                }
            }
            RegisterParent(parent);
        }

        public V this[K key]
        {
            get { return _dict[key]; }
            set
            {
                var oldItem = _dict[key];
                if (oldItem != null)
                {
                    oldItem.UnregisterParent();
                }

                _dict[key] = value;
                if (_dict[key] != null)
                {
                    _dict[key].RegisterParent(this);
                }

                DirtyBit++;
            }
        }

        public void AddRange(IDictionary<K, V> source)
        {
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                kvp.Value.RegisterParent(this);
            }
            DirtyBit++;
        }


        public V Add(K key, V value)
        {
            _dict.Add(key, value);
            DirtyBit++;

            if (value != null)
            {
                value.RegisterParent(this);
            }

            return value;
        }

        public V Remove(K key)
        {
            var value = _dict[key];
            _dict.Remove(key);
            DirtyBit++;
            if (value != null)
            {
                value.UnregisterParent();
            }
            return value;
        }

        public V GetByKey(K key)
        {
            if (!_dict.ContainsKey(key))
            {
                return default(V);
            }
            return _dict[key];
        }

        public void Clear()
        {
            foreach (var kvp in _dict)
            {
                kvp.Value.UnregisterParent();
            }

            _dict.Clear();
            DirtyBit++;
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        public override SignalDictionary<K, V> Get()
        {
            return this;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        public IEnumerator GetEnumerator() => _dict.GetEnumerator();
    }

    [Serializable]
    public class IndexedSignalDictionary<K, V> : BaseSignalList<V, IndexedSignalDictionary<K, V>>, ISignalList<V>, IEnumerable
        where V : BaseSignal
    {
        protected Dictionary<K, V> _dict;

        const int DEFAULT_CAPACITY = 5;

        public IndexedSignalDictionary()
        {
            _dict = new(DEFAULT_CAPACITY);
            _list = new(DEFAULT_CAPACITY);
        }

        public IndexedSignalDictionary(int capacity = DEFAULT_CAPACITY, BaseSignal parent = null)
        {
            _dict = new(capacity);
            _list = new(capacity);
            RegisterParent(parent);
        }

        public IndexedSignalDictionary(IDictionary<K, V> source, BaseSignal parent = null)
        {
            _dict = new(source?.Count ?? DEFAULT_CAPACITY);
            _list = new(source?.Count ?? DEFAULT_CAPACITY);
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                _list.Add(kvp.Value);
                if (kvp.Value != null)
                {
                    kvp.Value.RegisterParent(this);
                }
            }
            RegisterParent(parent);
        }

        public V this[int i]
        {
            get { return _list[i]; }
        }

        public V this[K key]
        {
            get { return _dict[key]; }
            set
            {
                var oldItem = _dict[key];
                if (oldItem != null)
                {
                    oldItem.UnregisterParent();
                }

                _dict[key] = value;
                if (_dict[key] != null)
                {
                    _dict[key].RegisterParent(this);
                }

                DirtyBit++;
            }
        }

        public void AddRange(IDictionary<K, V> source)
        {
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                kvp.Value.RegisterParent(this);
            }
            _list.AddRange(source.Values);
            DirtyBit++;
        }


        public V Add(K key, V value)
        {
            _dict.Add(key, value);
            _list.Add(value);
            DirtyBit++;

            if (value != null)
            {
                value.RegisterParent(this);
            }

            return value;
        }

        public V Remove(K key)
        {
            var value = _dict[key];
            _dict.Remove(key);
            _list.Remove(value);
            DirtyBit++;
            if (value != null)
            {
                value.UnregisterParent();
            }
            return value;
        }

        public V GetByKey(K key)
        {
            if (!_dict.ContainsKey(key))
            {
                return default(V);
            }
            return _dict[key];
        }

        public void Clear()
        {
            foreach (var kvp in _dict)
            {
                kvp.Value.UnregisterParent();
            }

            _dict.Clear();
            _list.Clear();
            DirtyBit++;
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        public bool Contains(V value)
        {
            return _list.Contains(value);
        }

        public override IndexedSignalDictionary<K, V> Get()
        {
            return this;
        }

        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
        public IEnumerator GetEnumerator() => _dict.GetEnumerator();
    }
}
