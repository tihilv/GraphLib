using System.Collections.Generic;
using GraphLib.VertexCreation;
using System.Linq;
using GraphLib.Primitives;
using GraphLib.Vertices;

namespace GraphLib.Mst
{
    public class KruskalMstFinder
    {
        private readonly Graph _graph;
        private UnionFind<IVertex> _union;

        public KruskalMstFinder(Graph graph)
        {
            _graph = graph;
            _edges = new List<Edge>();
        }

        private readonly List<Edge> _edges;
        private long _spacing;

        public UnionFind<IVertex> Union => _union;

        public List<Edge> Edges => _edges;

        public long Spacing => _spacing;

        public void Process(int? leaveClusters = 1)
        {
            _edges.Clear();
            var edges = _graph.Edges.OrderBy(e=>e.Length);
            _union = new UnionFind<IVertex>(_graph.Vertices);

            foreach (Edge edge in edges)
            {
                var v1 = edge.Tail;
                var v2 = edge.Head;

                if (!_union.SameUnion(v1, v2))
                {
                    if (_union.UnionCount > leaveClusters)
                    {
                        _union.Union(v1, v2);
                        _edges.Add(edge);
                    }
                    else
                    {
                        _spacing = edge.Length;
                        break;
                    }
                }
            }
        }

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Undirected, VerticesStoreMode.Outcome, GraphPreferedUsage.OptimizedForInsert, true, vertexTagFactory);
        }
    }
}
