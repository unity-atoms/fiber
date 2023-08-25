using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Signals
{
    public abstract class BaseSignal
    {
        // The deep dirty bit is used to track changes to the signal. Incrementing
        // this value indicates that the signal has changed. 
        [SerializeField]
        protected byte _dirtyBit = 0;
        public byte DirtyBit => _dirtyBit;

        ~BaseSignal()
        {
            if (_dependents != null)
            {
                if (_dependents is List<BaseSignal> listOfDependents)
                {
                    for (var i = listOfDependents.Count - 1; i >= 0; --i)
                    {
                        UnregisterDependent(listOfDependents[i]);
                    }
                }
                else
                {
                    var dependent = (BaseSignal)_dependents;
                    UnregisterDependent(dependent);
                }
            }
        }

        protected abstract void OnNotifySignalUpdate();

        public void NotifySignalUpdate()
        {
            OnNotifySignalUpdate();
            if (_dependents != null)
            {
                if (_dependents is List<BaseSignal> listOfDependents)
                {
                    for (var i = 0; i < listOfDependents.Count; ++i)
                    {
                        listOfDependents[i].NotifySignalUpdate();
                    }
                }
                else
                {
                    var dependent = (BaseSignal)_dependents;
                    dependent.NotifySignalUpdate();
                }
            }
        }

        protected object _dependents;
        public void RegisterDependent(BaseSignal dependant)
        {
            if (_dependents is List<BaseSignal> listOfDependentSignals)
            {
                listOfDependentSignals.Add(dependant);
            }
            else if (_dependents is BaseSignal)
            {
                _dependents = new List<BaseSignal>()
                {
                    (BaseSignal)_dependents,
                    dependant
                };
            }
            else
            {
                _dependents = dependant;
            }
        }

        public void UnregisterDependent(BaseSignal dependant)
        {
            if (_dependents is List<BaseSignal> listOfDependentSignals)
            {
                listOfDependentSignals.Remove(dependant);
            }
            else
            {
                _dependents = null;
            }
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
                _wrappedSignal.RegisterDependent(this);
            }
        }

        ~NullableSignal()
        {
            if (_wrappedSignal != null)
            {
                _wrappedSignal.UnregisterDependent(this);
            }
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
        public override sealed T Get() => _wrappedSignal == null ? default : _wrappedSignal.Get();
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
        public override sealed T Get() => _value;
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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
                NotifySignalUpdate();
            }
        }

        public Signal(T value = default, BaseSignal dependent = null)
        {
            _value = value;
            _dependents = dependent;
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
        public override sealed T Get() => _value;
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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
                    _value.UnregisterDependent(this);
                }
                _value = value;
                if (_value != null)
                {
                    _value.RegisterDependent(this);
                }
                NotifySignalUpdate();
            }
        }

        public Store(T value = default, BaseSignal dependent = null)
        {
            _value = value;
            if (_value != null)
            {
                _value.RegisterDependent(this);
            }
            _dependents = dependent;
        }

        ~Store()
        {
            if (_value != null)
            {
                _value.UnregisterDependent(this);
            }
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
        public override sealed T Get() => _value;
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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

        public ShallowSignalList(BaseSignal dependent = null)
        {
            _list = new(DEFAULT_CAPACITY);
            RegisterDependent(dependent);
        }

        public ShallowSignalList(int capacity = DEFAULT_CAPACITY, BaseSignal dependent = null)
        {
            _list = new(capacity);
            RegisterDependent(dependent);
        }

        public ShallowSignalList(IList<T> source, BaseSignal dependent = null)
        {
            _list = new(source?.Count ?? DEFAULT_CAPACITY);
            for (var i = 0; source != null && i < source.Count; ++i)
            {
                _list.Add(source[i]);
            }
            RegisterDependent(dependent);
        }

        public virtual T this[int i]
        {
            get { return _list[i]; }
            set { _list[i] = value; NotifySignalUpdate(); }
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
            NotifySignalUpdate();
        }

        public virtual void Add(T item)
        {
            _list.Add(item);
            NotifySignalUpdate();
        }

        public virtual void Insert(int index, T item)
        {
            _list.Insert(index, item);
            NotifySignalUpdate();
        }

        public virtual bool Remove(T item)
        {
            var result = _list.Remove(item);
            NotifySignalUpdate();
            return result;
        }

        public virtual void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            NotifySignalUpdate();
        }

        public virtual void Clear()
        {
            _list.Clear();
            NotifySignalUpdate();
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

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }

        public override sealed ShallowSignalList<T> Get()
        {
            return this;
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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

        public SignalList(BaseSignal dependent = null)
        {
            _list = new(DEFAULT_CAPACITY);
            RegisterDependent(dependent);
        }

        public SignalList(int capacity = DEFAULT_CAPACITY, BaseSignal dependent = null)
        {
            _list = new(capacity);
            RegisterDependent(dependent);
        }

        public SignalList(IList<T> source, BaseSignal dependent = null)
        {
            _list = new(source?.Count ?? DEFAULT_CAPACITY);
            for (var i = 0; source != null && i < source.Count; ++i)
            {
                _list.Add(source[i]);
                if (source[i] != null)
                {
                    source[i].RegisterDependent(this);
                }
            }
            RegisterDependent(dependent);
        }


        public T this[int i]
        {
            get { return _list[i]; }
            set
            {
                var oldItem = _list[i];
                if (oldItem != null)
                {
                    oldItem.UnregisterDependent(this);
                }

                _list[i] = value;
                if (_list[i] != null)
                {
                    _list[i].RegisterDependent(this);
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
                source[i].RegisterDependent(this);
            }
            NotifySignalUpdate();
        }

        public void Add(T item)
        {
            _list.Add(item);
            NotifySignalUpdate();
            if (item != null)
            {
                item.RegisterDependent(this);
            }
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            NotifySignalUpdate();
            if (item != null)
            {
                item.RegisterDependent(this);
            }
        }

        public bool Remove(T item)
        {
            var result = _list.Remove(item);
            NotifySignalUpdate();

            if (item != null)
            {
                item.UnregisterDependent(this);
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            NotifySignalUpdate();
            if (item != null)
            {
                item.UnregisterDependent(this);
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                _list[i].UnregisterDependent(this);
            }
            _list.Clear();
            NotifySignalUpdate();
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

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }

        public override sealed SignalList<T> Get()
        {
            return this;
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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

        public ShallowSignalDictionary(int capacity = DEFAULT_CAPACITY, BaseSignal dependent = null)
        {
            _dict = new(capacity);
            RegisterDependent(dependent);
        }

        public ShallowSignalDictionary(IDictionary<K, V> source, BaseSignal dependent = null)
        {
            _dict = new(source);
            RegisterDependent(dependent);
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
            NotifySignalUpdate();
        }


        public V Add(K key, V value)
        {
            _dict.Add(key, value);
            NotifySignalUpdate();
            return value;
        }

        public V Remove(K key)
        {
            var value = _dict[key];
            _dict.Remove(key);
            NotifySignalUpdate();
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
            NotifySignalUpdate();
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }

        public override sealed ShallowSignalDictionary<K, V> Get()
        {
            return this;
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
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

        public SignalDictionary(int capacity = DEFAULT_CAPACITY, BaseSignal dependent = null)
        {
            _dict = new(capacity);
            RegisterDependent(dependent);
        }

        public SignalDictionary(IDictionary<K, V> source, BaseSignal dependent = null)
        {
            _dict = new(source?.Count ?? DEFAULT_CAPACITY);
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                if (kvp.Value != null)
                {
                    kvp.Value.RegisterDependent(this);
                }
            }
            RegisterDependent(dependent);
        }

        public V this[K key]
        {
            get { return _dict[key]; }
            set
            {
                var oldItem = _dict[key];
                if (oldItem != null)
                {
                    oldItem.UnregisterDependent(this);
                }

                _dict[key] = value;
                if (_dict[key] != null)
                {
                    _dict[key].RegisterDependent(this);
                }

                NotifySignalUpdate();
            }
        }

        public void AddRange(IDictionary<K, V> source)
        {
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                kvp.Value.RegisterDependent(this);
            }
            NotifySignalUpdate();
        }


        public V Add(K key, V value)
        {
            _dict.Add(key, value);
            NotifySignalUpdate();

            if (value != null)
            {
                value.RegisterDependent(this);
            }

            return value;
        }

        public V Remove(K key)
        {
            var value = _dict[key];
            _dict.Remove(key);
            NotifySignalUpdate();
            if (value != null)
            {
                value.UnregisterDependent(this);
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
                kvp.Value.UnregisterDependent(this);
            }

            _dict.Clear();
            NotifySignalUpdate();
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }

        public override sealed SignalDictionary<K, V> Get()
        {
            return this;
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;

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

        public IndexedSignalDictionary(int capacity = DEFAULT_CAPACITY, BaseSignal dependent = null)
        {
            _dict = new(capacity);
            _list = new(capacity);
            RegisterDependent(dependent);
        }

        public IndexedSignalDictionary(IDictionary<K, V> source, BaseSignal dependent = null)
        {
            _dict = new(source?.Count ?? DEFAULT_CAPACITY);
            _list = new(source?.Count ?? DEFAULT_CAPACITY);
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                _list.Add(kvp.Value);
                if (kvp.Value != null)
                {
                    kvp.Value.RegisterDependent(this);
                }
            }
            RegisterDependent(dependent);
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
                    oldItem.UnregisterDependent(this);
                }

                _dict[key] = value;
                if (_dict[key] != null)
                {
                    _dict[key].RegisterDependent(this);
                }

                NotifySignalUpdate();
            }
        }

        public void AddRange(IDictionary<K, V> source)
        {
            foreach (var kvp in source)
            {
                _dict.Add(kvp.Key, kvp.Value);
                kvp.Value.RegisterDependent(this);
            }
            _list.AddRange(source.Values);
            NotifySignalUpdate();
        }


        public V Add(K key, V value)
        {
            _dict.Add(key, value);
            _list.Add(value);
            NotifySignalUpdate();

            if (value != null)
            {
                value.RegisterDependent(this);
            }

            return value;
        }

        public V Remove(K key)
        {
            var value = _dict[key];
            _dict.Remove(key);
            _list.Remove(value);
            NotifySignalUpdate();
            if (value != null)
            {
                value.UnregisterDependent(this);
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
                kvp.Value.UnregisterDependent(this);
            }

            _dict.Clear();
            _list.Clear();
            NotifySignalUpdate();
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        public bool Contains(V value)
        {
            return _list.Contains(value);
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }

        public override sealed IndexedSignalDictionary<K, V> Get()
        {
            return this;
        }
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;

        public IEnumerator GetEnumerator() => _dict.GetEnumerator();
    }
}
