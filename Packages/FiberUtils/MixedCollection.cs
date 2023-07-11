using System;
using System.Collections;
using System.Collections.Generic;

// Collections that can be used to store different types of items
// without boxing/unboxing or casting.
namespace FiberUtils
{
    public class MixedQueue
    {
        private Dictionary<Type, object> _itemsByType;
        private List<Type> _order;

        public MixedQueue()
        {
            _itemsByType = new();
            _order = new();
        }

        public Type PeekType()
        {
            return _order[0];
        }

        public void Enqueue<T>(T item)
        {
            var type = typeof(T);
            if (!_itemsByType.ContainsKey(type))
            {
                _itemsByType.Add(type, new List<T>(1) { item });
            }
            else
            {
                var itemsOfType = _itemsByType[type] as List<T>;
                itemsOfType.Add(item);
            }
            _order.Add(type);
        }

        public T Dequeue<T>()
        {
            var type = typeof(T);
            if (PeekType() != type)
            {
                throw new Exception($"Next in queue is not of type {type}");
            }

            var items = _itemsByType[type] as List<T>;
            var itemToReturn = items[0];
            items.RemoveAt(0);
            _order.RemoveAt(0);
            return itemToReturn;
        }

        public T Peek<T>()
        {
            var type = typeof(T);
            if (PeekType() != type)
            {
                throw new Exception($"Next in queue is not of type {type}");
            }

            var items = _itemsByType[type] as List<T>;
            var itemToReturn = items[0];
            return itemToReturn;
        }

        public int Count => _order.Count;
    }

    public class MixedStack
    {
        private Dictionary<Type, object> _itemsByType;
        private List<Type> _order;

        public MixedStack()
        {
            _itemsByType = new();
            _order = new();
        }

        public Type PeekType()
        {
            return _order[_order.Count - 1];
        }

        public void Push<T>(T item)
        {
            var type = typeof(T);
            if (!_itemsByType.ContainsKey(type))
            {
                _itemsByType.Add(type, new List<T>(1) { item });
            }
            else
            {
                var itemsOfType = _itemsByType[type] as List<T>;
                itemsOfType.Add(item);
            }
            _order.Add(type);
        }

        public T Pop<T>()
        {
            var type = typeof(T);
            if (PeekType() != type)
            {
                throw new Exception($"Top of stack is not of type {type}");
            }

            var items = _itemsByType[type] as List<T>;
            var itemIndex = items.Count - 1;
            var itemToReturn = items[itemIndex];
            items.RemoveAt(itemIndex);
            _order.RemoveAt(_order.Count - 1);
            return itemToReturn;
        }

        public void Pop()
        {
            var type = PeekType();
            var items = _itemsByType[type] as IList;
            items.RemoveAt(items.Count - 1);
            _order.RemoveAt(_order.Count - 1);
        }

        public T Peek<T>()
        {
            var type = typeof(T);
            if (PeekType() != type)
            {
                throw new Exception($"Top of stack is not of type {type}");
            }

            var items = _itemsByType[type] as List<T>;
            var itemToReturn = items[items.Count - 1];
            return itemToReturn;
        }

        public int Count => _order.Count;

        // Get type at index. Index 0 is at the bottom of the stack.
        public Type GetTypeAt(int index)
        {
            return _order[index];
        }

        // Get item at index. Index 0 is at the bottom of the stack.
        public T GetAt<T>(int index)
        {
            var type = typeof(T);
            if (GetTypeAt(index) != type)
            {
                throw new Exception($"Item at index {index} is not of type {type}");
            }

            var items = _itemsByType[type] as List<T>;

            // Find index
            var itemByTypeIndex = 0;
            for (var i = 0; i < index; ++i)
            {
                if (_order[i] == type)
                {
                    ++itemByTypeIndex;
                }
            }

            var itemToReturn = items[itemByTypeIndex];
            return itemToReturn;
        }
    }
}