using System;
using System.Collections.Generic;

namespace GraphLib.Primitives
{
    public class MinHeap<TKey, TElement> where TKey: IComparable
    {
        private readonly Func<TElement, TKey> _keyProvider;
        protected readonly List<Item> _elements;

        public MinHeap(Func<TElement, TKey> keyProvider)
        {
            _keyProvider = keyProvider;

            _elements = new List<Item>();
        }

        public void Insert(TElement element)
        {
            InsertReturnPosition(element);
        }

        protected virtual int InsertReturnPosition(TElement element)
        {
            var newItem = new Item(_keyProvider(element), element);
            _elements.Add(newItem);
            return BubbleUp(_elements.Count - 1);
        }

        public TElement Peek()
        {
            return _elements[0].Value;
        }

        public TElement Extract()
        {
            var result = Peek();
            Delete(0);
            return result;
        }

        public int Count => _elements.Count;

        protected void Delete(int index)
        {
            Swap(index, _elements.Count - 1);
            _elements.RemoveAt(_elements.Count - 1);
            BubbleDown(index);
        }

        private int BubbleUp(int fromIndex)
        {
            int currentIndex = fromIndex;
            bool swapOccured;
            do
            {
                swapOccured = false;
                int parentIndex = (currentIndex - 1) / 2;
                if (currentIndex > 0 && _elements[currentIndex].Key.CompareTo(_elements[parentIndex].Key) < 0)
                {
                    Swap(currentIndex, parentIndex);
                    currentIndex = parentIndex;
                    swapOccured = true;
                }
            } while (swapOccured);

            return currentIndex;
        }

        protected virtual void Swap(int fromIndex, int toIndex)
        {
            var t = _elements[fromIndex];
            _elements[fromIndex] = _elements[toIndex];
            _elements[toIndex] = t;
        }

        private void BubbleDown(int fromIndex)
        {
            int currentIndex = fromIndex;
            bool swapOccured;
            do
            {
                swapOccured = false;
                int indexLeft = currentIndex * 2 + 1;
                int indexRight = currentIndex * 2 + 2;
                if ((indexLeft < _elements.Count && _elements[currentIndex].Key.CompareTo(_elements[indexLeft].Key) > 0) ||
                    (indexRight < _elements.Count && _elements[currentIndex].Key.CompareTo(_elements[indexRight].Key) > 0))
                {
                    int indexToSwap = indexLeft;
                    if (indexRight < _elements.Count && _elements[indexRight].Key.CompareTo(_elements[indexLeft].Key) < 0)
                        indexToSwap = indexRight;

                    Swap(currentIndex, indexToSwap);

                    currentIndex = indexToSwap;
                    swapOccured = true;
                }
            } while (swapOccured);
        }

        protected struct Item
        {
            internal readonly TKey Key;
            internal readonly TElement Value;

            public Item(TKey key, TElement value)
            {
                Key = key;
                Value = value;
            }
        }
    }

    internal class MihHeapRemovable<TKey, TElement>: MinHeap<TKey, TElement> where TKey: IComparable
    {
        private readonly Dictionary<TElement, int> _positions;

        public MihHeapRemovable(Func<TElement, TKey> keyProvider) : base(keyProvider)
        {
            _positions = new Dictionary<TElement, int>();
        }

        protected override void Swap(int fromIndex, int toIndex)
        {
            _positions[_elements[fromIndex].Value] = toIndex;
            _positions[_elements[toIndex].Value] = fromIndex;

            base.Swap(fromIndex, toIndex);
        }

        public void Remove(TElement element)
        {
            int position;
            
            if (_positions.TryGetValue(element, out position))
            {
                Delete(position);
                _positions.Remove(element);
            }
        }

        protected override int InsertReturnPosition(TElement element)
        {
            var result = base.InsertReturnPosition(element);
            _positions[element] = result;
            return result;
        }
    }
}

