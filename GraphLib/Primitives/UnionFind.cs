using System;
using System.Collections.Generic;
using GraphLib.Vertices;
using System.Linq;

namespace GraphLib.Primitives
{
    public class UnionFind<T>
    {
        private readonly Dictionary<T, int> _map;
        private readonly Data[] _datas;
        private int _unionCount;

        public UnionFind(IEnumerable<T> items)
        {
            var enumerable = items as T[] ?? items.ToArray();

            _map = new Dictionary<T, int>();
            _datas = new Data[enumerable.Length];
            for (int index = 0; index < enumerable.Length; index++)
            {
                var item = enumerable[index];
                _map.Add(item, index);
                _datas[index] = new Data(item, index, 0);
                _unionCount++;
            }
        }

        public int UnionCount => _unionCount;

        private int FindRootIndex(T item)
        {
            var originalIndex = _map[item];
            var index = originalIndex;
            while (_datas[index].ParentIndex != index)
                index = _datas[index].ParentIndex;

            var originalData = _datas[originalIndex];
            while (index != originalData.ParentIndex)
            {
                _datas[originalIndex] = new Data(originalData.Item, index, originalData.Rank);
                originalIndex = originalData.ParentIndex;
                originalData = _datas[originalIndex];
            }

            return index;
        }

        private Data FindRoot(T item)
        {
            return _datas[FindRootIndex(item)];
        }

        public T Find(T item)
        {
            return FindRoot(item).Item;
        }

        public bool SameUnion(T item1, T item2)
        {
            return ReferenceEquals(FindRoot(item1).Item, FindRoot(item2).Item);
        }

        public void Union(T item1, T item2)
        {
            var parentIndex1 = FindRootIndex(item1);
            var parentIndex2 = FindRootIndex(item2);

            var parent1 = _datas[parentIndex1];
            var parent2 = _datas[parentIndex2];

            if (ReferenceEquals(parent1.Item, parent2.Item))
                throw new Exception("Items already fused.");

            if (parent1.Rank > parent2.Rank)
                Rewire(parentIndex2, parentIndex1, false);
            else
                Rewire(parentIndex2, parentIndex1, parent1.Rank == parent2.Rank);

            _unionCount--;
        }

        void Rewire(int fromIndex, int toIndex, bool increaseRank)
        {
            _datas[fromIndex] = new Data(_datas[fromIndex].Item, toIndex, 0);
            if (increaseRank)
            {
                Data toData = _datas[toIndex];
                _datas[toIndex] = new Data(toData.Item, toIndex, toData.Rank+1);
            }
        }

        struct Data
        {
            public readonly T Item;
            public readonly int ParentIndex;
            public readonly int Rank;

            public Data(T item, int parentIndex, int rank)
            {
                Item = item;
                ParentIndex = parentIndex;
                Rank = rank;
            }
        }
    }
}
