using System;
using System.Collections.Generic;
using GraphLib.VertexCreation;
using GraphLib.Vertices;
using System.Linq;

namespace GraphLib.PathFinding
{
    public class JohnsonPathFinder
    {
        private readonly Graph _graph;

        readonly Dictionary<IVertex, Dictionary<IVertex, PathDistanceInfo>> _vertexToPathDictionary;

        public JohnsonPathFinder(Graph graph)
        {
            _graph = graph;
            _vertexToPathDictionary = new Dictionary<IVertex, Dictionary<IVertex, PathDistanceInfo>>();
        }

        public void Process()
        {
            _vertexToPathDictionary.Clear();

            Graph bfGraph = new Graph(BellmanFordPathFinder.OptimizedOptions());
            foreach (Edge edge in _graph.Edges)
                bfGraph.AddEdge(edge.Tail.VertexTag, edge.Head.VertexTag, edge.Length);

            VertexTag newVertex = new VertexTag(Guid.NewGuid().ToString());
            foreach (var vertex in _graph.Vertices)
                bfGraph.AddEdge(newVertex, vertex.VertexTag, 0);

            BellmanFordPathFinder bfDetector = new BellmanFordPathFinder(bfGraph);
            bfDetector.Process(bfGraph.GetVertex(newVertex.Name));

            if (!bfDetector.HasNegativeCycle)
            {
                HasNegativeCycle = false;

                Graph dGraph = new Graph(BellmanFordPathFinder.OptimizedOptions());
                foreach (Edge edge in _graph.Edges)
                {
                    var newLength = edge.Length
                                    + bfDetector.VertexToPathDictionary[bfGraph.GetVertex(edge.Tail.Name)].Distance
                                    - bfDetector.VertexToPathDictionary[bfGraph.GetVertex(edge.Head.Name)].Distance;
                    dGraph.AddEdge(edge.Tail.VertexTag, edge.Head.VertexTag, newLength);
                }

                foreach (var tail in dGraph.Vertices)
                {
                    DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder(dGraph);
                    dijkstraPathFinder.Process(tail);
                    foreach (IVertex head in dijkstraPathFinder.VertexToPathDictionary.Keys.ToArray())
                    {
                        var current = dijkstraPathFinder.VertexToPathDictionary[head];
                        var newLength = current.Distance;
                        if (newLength != long.MaxValue)
                            newLength += bfDetector.VertexToPathDictionary[bfGraph.GetVertex(head.Name)].Distance
                                         - bfDetector.VertexToPathDictionary[bfGraph.GetVertex(tail.Name)].Distance;
                        dijkstraPathFinder.VertexToPathDictionary[head] = new PathDistanceInfo(current.Vertex, newLength, current.Parent);

                    }
                    _vertexToPathDictionary[tail] = dijkstraPathFinder.VertexToPathDictionary;
                }
            }
            else
                HasNegativeCycle = true;
        }

        public bool HasNegativeCycle { get; private set; }

        public Dictionary<IVertex, Dictionary<IVertex, PathDistanceInfo>> VertexToPathDictionary => _vertexToPathDictionary;

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.Outcome, GraphPreferedUsage.OptimizedForInsert, true, vertexTagFactory);
        }
    }
}

