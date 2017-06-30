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
        }

        public void Process(IVertex srcVertex)
        {
        }

        public Dictionary<IVertex, PathDistanceInfo> VertexToPathDictionary => _vertexToPathDictionary;

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.Outcome, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }
    }
}

