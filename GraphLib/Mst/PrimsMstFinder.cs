﻿using System.Collections.Generic;
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
            MihHeapRemovable<double, MstInfo> unobserved = new MihHeapRemovable<double, MstInfo>(i => i.Length);
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
}



