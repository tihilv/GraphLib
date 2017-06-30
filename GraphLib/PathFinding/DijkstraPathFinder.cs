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

        PathDistanceInfo GetPathDistanceInfo(IVertex vertex, long score, IVertex parent)
        {
            var result = new PathDistanceInfo(vertex, score, parent);
            _vertexToPathDictionary[vertex] = result;
            return result;
        }

        public void Process(IVertex srcVertex)
        {
            _vertexToPathDictionary.Clear();

            MihHeapRemovable<long, PathDistanceInfo> unobserved = new MihHeapRemovable<long, PathDistanceInfo>(i=>i.Distance);

            GetPathDistanceInfo(srcVertex, 0, null);

            foreach(var vertex in _graph.Vertices)
                if (vertex != srcVertex)
                    unobserved.Insert(GetPathDistanceInfo(vertex, Int64.MaxValue, null));

            foreach (var edge in srcVertex.GetOutcomeEdges())
                    unobserved.Insert(GetPathDistanceInfo(edge.Head, edge.Length, srcVertex));

            while (unobserved.Count > 0 && unobserved.Peek().Distance < long.MaxValue)
            {
                var current = unobserved.Extract();

                foreach (var edge in current.Vertex.GetOutcomeEdges())
                {
                    var newScore = current.Distance + edge.Length;
                    var otherPathInfo = _vertexToPathDictionary[edge.Head];
                    if (otherPathInfo.Distance > newScore)
                    {
                        unobserved.Remove(otherPathInfo);
                        otherPathInfo = GetPathDistanceInfo(otherPathInfo.Vertex, newScore, current.Vertex);
                        unobserved.Insert(otherPathInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Only for test reasons 
        /// </summary>
        /// <param name="srcVertex"></param>
        internal void ProcessNaive(IVertex srcVertex)
        {
            List<IVertex> unobserved = new List<IVertex>();

            GetPathDistanceInfo(srcVertex, 0, null);

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
                GetPathDistanceInfo(nextVertex, bestScore, null);
            }
        }

        public Dictionary<IVertex, PathDistanceInfo> VertexToPathDictionary => _vertexToPathDictionary;

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.Outcome, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }
    }
}

