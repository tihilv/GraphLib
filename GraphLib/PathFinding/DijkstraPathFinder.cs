using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.Primitives;
using GraphLib.VertexCreation;
using GraphLib.Vertices;

namespace GraphLib.PathFinding
{
    public class DijkstraPathFinder
    {
        private readonly Graph _graph;

        readonly Dictionary<IVertex, PathDistanceInfo> _vertexToPathDictionary;

        public DijkstraPathFinder(Graph graph)
        {
            _graph = graph;

            _vertexToPathDictionary = new Dictionary<IVertex, PathDistanceInfo>();
        }

        PathDistanceInfo GetPathDistanceInfo(IVertex vertex, long score)
        {
            var result = new PathDistanceInfo(vertex, score);
            _vertexToPathDictionary[vertex] = result;
            return result;
        }

        public void Process(IVertex srcVertex)
        {
            if (_graph.Options.VerticesStoreMode != VerticesStoreMode.All)
                throw new Exception($"Unable to proceed graph with option VerticesStoreMode={_graph.Options.VerticesStoreMode}");

            MihHeapRemovable<long, PathDistanceInfo> unobserved= new MihHeapRemovable<long, PathDistanceInfo>(i=>i.Distance);

            GetPathDistanceInfo(srcVertex, 0);

            foreach (var vertex in _graph.Vertices)
                if (vertex != srcVertex)
                {
                    long score = long.MaxValue;
                    var toSourceEdges = vertex.GetIncomeEdges().Where(e => e.Tail == srcVertex);
                    foreach (Edge edge in toSourceEdges)
                        score = Math.Min(score, edge.Length);

                    unobserved.Insert(GetPathDistanceInfo(vertex, score));
                }

            while (unobserved.Count > 0 && unobserved.Peek().Distance < long.MaxValue)
            {
                var current = unobserved.Peek();
                unobserved.Remove(current);

                foreach (var edge in current.Vertex.GetOutcomeEdges())
                {
                    var newScore = current.Distance + edge.Length;
                    var otherPathInfo = _vertexToPathDictionary[edge.Head];
                    if (otherPathInfo.Distance > newScore)
                    {
                        unobserved.Remove(otherPathInfo);
                        otherPathInfo = GetPathDistanceInfo(otherPathInfo.Vertex, newScore);
                        unobserved.Insert(otherPathInfo);
                    }
                }
            }
        }

        public void ProcessNaive(IVertex srcVertex)
        {
            List<IVertex> unobserved = new List<IVertex>();

            GetPathDistanceInfo(srcVertex, 0);

            foreach (IVertex vertex in _graph.Vertices)
            {
                if (vertex != srcVertex)
                    unobserved.Add(vertex);
            }

            while (unobserved.Any())
            {
                long bestScore = long.MaxValue;
                IVertex nextVertex = null;
                foreach (var pair in _vertexToPathDictionary)
                    foreach (var edge in pair.Key.GetOutcomeEdges())
                        if (unobserved.Contains(edge.Head))
                    {
                        var score = pair.Value.Distance + edge.Length;
                        if (score < bestScore)
                        {
                            nextVertex = edge.Head;
                            bestScore = score;
                        }
                    }

                if (nextVertex == null)
                    break;

                unobserved.Remove(nextVertex);
                GetPathDistanceInfo(nextVertex, bestScore);
            }
        }

        public Dictionary<IVertex, PathDistanceInfo> VertexToPathDictionary => _vertexToPathDictionary;

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.All, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }
    }

    public struct PathDistanceInfo
    {
        public readonly IVertex Vertex;
        public readonly long Distance;

        public PathDistanceInfo(IVertex vertex, long score) : this()
        {
            Vertex = vertex;
            Distance = score;
        }

        private sealed class ScoreEqualityComparer : IComparer<PathDistanceInfo>
        {
            public int Compare(PathDistanceInfo x, PathDistanceInfo y)
            {
                return Math.Sign(x.Distance - y.Distance);
            }
        }

        private static readonly IComparer<PathDistanceInfo> ScoreComparerInstance = new ScoreEqualityComparer();

        public static IComparer<PathDistanceInfo> ScoreComparer => ScoreComparerInstance;
    }
}
