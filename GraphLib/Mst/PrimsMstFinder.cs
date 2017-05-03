using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.Primitives;
using GraphLib.VertexCreation;
using GraphLib.Vertices;

namespace GraphLib.Mst
{
    public class PrimsMstFinder
    {
        private readonly Graph _graph;

        private readonly List<Edge> _edges;

        public PrimsMstFinder(Graph graph)
        {
            _graph = graph;
            _edges = new List<Edge>();
        }

        MstInfo GetPathDistanceInfo(IVertex vertex, Edge bestEdge, Dictionary<IVertex, MstInfo> vertexToMstDictionary)
        {
            var result = new MstInfo(vertex, bestEdge);
            vertexToMstDictionary[vertex] = result;
            return result;
        }

        public void Process()
        {
            _edges.Clear();
            Dictionary<IVertex, MstInfo> vertexToMstDictionary = new Dictionary<IVertex, MstInfo>();
            MihHeapRemovable<long, MstInfo> unobserved = new MihHeapRemovable<long, MstInfo>(i => i.Length);
            HashSet<IVertex> observedVertices = new HashSet<IVertex>();

            IVertex srcVertex = _graph.Vertices[0];
            observedVertices.Add(srcVertex);

            GetPathDistanceInfo(srcVertex, null, vertexToMstDictionary);

            foreach (var vertex in _graph.Vertices)
                if (vertex != srcVertex)
                    unobserved.Insert(GetPathDistanceInfo(vertex, null, vertexToMstDictionary));

            foreach (var edge in srcVertex.GetOutcomeEdges())
                unobserved.Insert(GetPathDistanceInfo(edge.Other(srcVertex), edge, vertexToMstDictionary));

            while (unobserved.Count > 0 && unobserved.Peek().Length < long.MaxValue)
            {
                var current = unobserved.Peek();
                unobserved.Remove(current);
                _edges.Add(current.Edge);
                observedVertices.Add(current.Vertex);

                foreach (var edge in current.Vertex.GetOutcomeEdges())
                {
                    var other = edge.Other(current.Vertex);
                    if (!observedVertices.Contains(other))
                    {
                        var otherPathInfo = vertexToMstDictionary[other];
                        if (otherPathInfo.Length > edge.Length)
                        {
                            unobserved.Remove(otherPathInfo);
                            otherPathInfo = GetPathDistanceInfo(other, edge, vertexToMstDictionary);
                            unobserved.Insert(otherPathInfo);
                        }
                    }
                }
            }
        }

        public List<Edge> Edges
        {
            get { return _edges; }
        }

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Undirected, VerticesStoreMode.Outcome, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }
    }

    public struct MstInfo
    {
        public readonly IVertex Vertex;
        public readonly Edge Edge;

        public MstInfo(IVertex vertex, Edge edge) : this()
        {
            Vertex = vertex;
            Edge = edge;
        }

        public long Length
        {
            get
            {
                if (Edge == null)
                    return Int64.MaxValue;

                return Edge.Length;
            }
        }
    }

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



