using System;
using System.Collections.Generic;
using UnityEngine;

namespace FiberUtils
{
    [Serializable]
    public struct SerializableKVP<KeyT, ValueT>
    {
        public KeyT Key;
        public ValueT Value;

        public SerializableKVP(KeyT key, ValueT value)
        {
            Key = key;
            Value = value;
        }
    }

    [Serializable]
    public class IndexedDictionary<K, V>
    {
        [SerializeField]
        protected List<SerializableKVP<K, V>> _list;
        protected Dictionary<K, V> _dictionary;
        public int Count => _list.Count;
        public List<SerializableKVP<K, V>> List { get => _list; }
        public Dictionary<K, V> Dictionary { get => _dictionary; }

        public IndexedDictionary()
        {
            _list = new(0);
            _dictionary = new(0);
        }

        public IndexedDictionary(int capacity)
        {
            _list = new(capacity);
            _dictionary = new(capacity);
        }

        public IndexedDictionary(IndexedDictionary<K, V> other)
        {
            _list = new(other.List);
            _dictionary = new(other.Dictionary);
        }


        public V this[int i]
        {
            get { return GetByIndex(i); }
        }

        public V this[K key]
        {
            get { return GetByKey(key); }
        }

        public void SetByKey(K key, V value)
        {
            for (var i = 0; i < _list.Count; ++i)
            {
                if (_list[i].Key.Equals(key))
                {
                    _list[i] = new(key, value);
                    break;
                }
            }
            _dictionary[key] = value;
        }

        public V GetByKey(K key)
        {
            return _dictionary[key];
        }

        public V GetByIndex(int i)
        {
            return _list[i].Value;
        }

        public SerializableKVP<K, V> GetKVPAt(int index)
        {
            return _list[index];
        }


        public IndexedDictionary<K, V> AddIndexedDictionary(IndexedDictionary<K, V> other)
        {
            for (var i = 0; i < other.Count; ++i)
            {
                var kvp = other.GetKVPAt(i);
                Add(kvp.Key, kvp.Value);
            }
            return this;
        }

        public V Add(K key, V value)
        {
            _dictionary.Add(key, value);
            _list.Add(new(key, value));
            return value;
        }

        public V Remove(K key)
        {
            for (var i = 0; i < _list.Count; ++i)
            {
                if (_list[i].Key.Equals(key))
                {
                    _list.RemoveAt(i);
                    break;
                }
            }

            var value = _dictionary[key];
            _dictionary.Remove(key);
            return value;
        }

        public void Clear()
        {
            _list.Clear();
            _dictionary.Clear();
        }

        public bool ContainsKey(K key)
        {
            return _dictionary.ContainsKey(key);
        }
    }

    [Serializable]
    public class SerializableIndexedStringDictionary<V> : IndexedDictionary<string, V>, ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
            _list.Clear();
            foreach (var kvp in _dictionary)
            {
                _list.Add(new SerializableKVP<string, V>(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            for (int i = 0; i < _list.Count; ++i)
            {
                var kvp = _list[i];
                if (!_dictionary.ContainsKey(kvp.Key))
                {
                    _dictionary.Add(kvp.Key, kvp.Value);
                }
                else if (!_dictionary.ContainsKey(string.Empty))
                {
                    _dictionary.Add(string.Empty, kvp.Value);
                }
                else
                {
                    Debug.LogError("Duplicate key in IndexedDictionary: " + kvp.Key);
                }
            }
        }
    }
}