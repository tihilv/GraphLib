using System.Collections.Generic;
using GraphLib.VertexCreation;
using GraphLib.Vertices;

namespace GraphLib.PathFinding
{
    public class BellmanFordPathFinder
    {
        private readonly Graph _graph;

        readonly Dictionary<IVertex, PathDistanceInfo> _vertexToPathDictionary;

        public BellmanFordPathFinder(Graph graph)
        {
            _graph = graph;
            _vertexToPathDictionary = new Dictionary<IVertex, PathDistanceInfo>();
        }

        public void Process(IVertex srcVertex)
        {
            HasNegativeCycle = false;
            _vertexToPathDictionary.Clear();

            Dictionary<IVertex, int> vertexPosition = new Dictionary<IVertex, int>();
            for (int index = 0; index < _graph.VertexCount; index++)
                vertexPosition[_graph.Vertices[index]] = index;

            long[] aPrev = new long[_graph.VertexCount];
            for (int index = 0; index < aPrev.Length; index++)
                aPrev[index] = long.MaxValue;
            aPrev[vertexPosition[srcVertex]] = 0;

            for (int iteration = 0; iteration < _graph.VertexCount; iteration++)
            {
                long[] aCurrent = new long[_graph.VertexCount];

                for (int index = 0; index < _graph.VertexCount; index++)
                {
                    var v = _graph.Vertices[index];

                    long minValue = aPrev[index];
                    foreach (Edge edge in v.GetIncomeEdges())
                    {
                        var w = edge.Tail;
                        var value = aPrev[vertexPosition[w]];
                        if (value != long.MaxValue)
                            value += edge.Length;

                        if (minValue > value)
                            minValue = value;
                    }

                    aCurrent[index] = minValue;
                }

                bool equals = true;
                for (int index = 0; index < _graph.VertexCount; index++)
                {
                    if (aPrev[index] != aCurrent[index])
                    {
                        equals = false;
                        break;
                    }
                }

                aPrev = aCurrent;

                if (equals)
                    break;

                if (iteration == _graph.VertexCount - 1)
                    HasNegativeCycle = true;
            }

            if (!HasNegativeCycle)
            {
                for (int index = 0; index < _graph.VertexCount; index++)
                    _vertexToPathDictionary[_graph.Vertices[index]] = new PathDistanceInfo(_graph.Vertices[index], aPrev[index], null);
            }
        }

        public bool HasNegativeCycle { get; private set; }

        public Dictionary<IVertex, PathDistanceInfo> VertexToPathDictionary => _vertexToPathDictionary;

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.All, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }
    }
}

