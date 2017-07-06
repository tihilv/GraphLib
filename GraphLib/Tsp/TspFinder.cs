using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.VertexCreation;

namespace GraphLib.Tsp
{
    public class TspFinder
    {
        private readonly Graph _graph;

        public TspFinder(Graph graph)
        {
            _graph = graph;
        }

        public double Process()
        {
            if (_graph.Options.Direction != GraphDirection.Undirected)
                throw new ArgumentException("The graph should be undirected.");

            var vertices = _graph.Vertices;

            TspSubsets subsets = new TspSubsets((byte)vertices.Length);
            TspStorage prevStorage = new TspStorage();
            prevStorage.SetValue(new TspSubset(1), 0, 0);

            for (byte m = 2; m <= vertices.Length; m++)
            {
                TspStorage storage = new TspStorage();

                foreach (TspSubset subset in subsets.LengthOf(m))
                {
                    var s = subset.GetItems().ToArray();
                    foreach (byte j in s.Where(v => v != 0))
                    {
                        var secondSubset = subset.RemoveItem(j);

                        double minLength = long.MaxValue;

                        foreach (byte k in s)
                            if (k != j)
                            {
                                var value = prevStorage.GetValue(secondSubset, k);
                                if (value != long.MaxValue)
                                    value += _graph.GetEdge(vertices[k], vertices[j]).Length;

                                if (minLength > value)
                                    minLength = value;
                            }

                        storage.SetValue(subset, j, minLength);
                    }
                }

                prevStorage = storage;
            }

            var maxSubset = subsets.GetMaxSubset();
            double min = long.MaxValue;
            for (byte j = 1; j < vertices.Length; j++)
            {
                var value = prevStorage.GetValue(maxSubset, j);
                if (value != long.MaxValue)
                    value += _graph.GetEdge(vertices[0], vertices[j]).Length;

                if (min > value)
                    min = value;
            }

            return min;
        }

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Undirected, VerticesStoreMode.All, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }
    }

    class TspStorage
    {
        private readonly Dictionary<Tuple<long, byte>, double> _storage;

        public TspStorage()
        {
            _storage = new Dictionary<Tuple<long, byte>, double>();
        }

        internal double GetValue(TspSubset subset, byte index)
        {
            Tuple<long, byte> key = new Tuple<long, byte>(subset.Value, index);

            double result;
            if (_storage.TryGetValue(key, out result))
                return result;

            return long.MaxValue;
        }

        internal void SetValue(TspSubset subset, byte index, double value)
        {
            Tuple<long, byte> key = new Tuple<long, byte>(subset.Value, index);

            _storage[key] = value;
        }
    }

    class TspSubsets
    {
        private readonly byte _length;
        
        public TspSubsets(byte length)
        {
            if (length > 60)
                throw new ArgumentException("The length is too big.");
            _length = length;
        }


        internal IEnumerable<TspSubset> LengthOf(byte size)
        {
            for (long i = 1; i < Math.Pow(2, _length); i+=2)
                if (TspSubset.NumberOfSetBits(i) == size)
                    yield return new TspSubset(i);
        }

        internal TspSubset GetMaxSubset()
        {
            return new TspSubset((long)Math.Pow(2, _length)-1);
        }
    }

    struct TspSubset
    {
        private readonly long _subsetValue;

        public TspSubset(long subsetValue)
        {
            _subsetValue = subsetValue;
        }

        internal long NumberOfSetBits()
        {
            return NumberOfSetBits(_subsetValue);
        }

        internal long Value => _subsetValue;

        internal static long NumberOfSetBits(long i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56;
        }

        public IEnumerable<byte> GetItems()
        {
            long i = _subsetValue;
            byte number = 0;
            while (i > 0)
            {
                if (i % 2 == 1)
                    yield return number;

                i /= 2;
                number++;
            }
        }

        public TspSubset RemoveItem(byte position)
        {
            long intValue = _subsetValue;
            intValue &= ~(1 << position);
            return new TspSubset(intValue);
        }
    }
}
