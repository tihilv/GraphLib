using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.Primitives;

namespace GraphLib.Mst
{
    [Obsolete("Not for use. Only for testing purposes")]
    class NaiveHeap<TKey, TValue> where TKey : IComparable
    { 
        private List<TValue> _items;
        private readonly Func<TValue, TKey> _func;

        private readonly MihHeapRemovable<TKey, TValue> _test;

        public NaiveHeap(Func<TValue, TKey> func)
        {
            _func = func;
            _items = new List<TValue>();

            _test = new MihHeapRemovable<TKey, TValue>(func);
        }

        public int Count => _items.Count;

        public void Insert(TValue item)
        {
            _items.Add(item);
            _items = _items.OrderBy(_func).ToList();
            _test.Insert(item);
        }

        public TValue Peek()
        {
            if (!_items[0].Equals(_test.Peek()))
            {
                
            }
            return _items[0];
        }

        public TValue Extract()
        {
            var r = _items[0];
            _items.RemoveAt(0);
            if (!r.Equals(_test.Extract()))
            {
                
            }
            return r;
        }

        public void Remove(TValue item)
        {
            _items.Remove(item);
            _test.Remove(item);
        }
    }
}