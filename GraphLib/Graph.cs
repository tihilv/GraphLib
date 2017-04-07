using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.EdgeKeeping;
using GraphLib.VertexCreation;
using GraphLib.Vierticies;
using GraphLib.Visiting;

namespace GraphLib
{
    public class Graph
    {
        private readonly IVertexTagFactory _vertexTagFactory;
        private readonly bool _isDirected;

        private readonly Dictionary<string, IVertex> _vertices;
        private readonly IEdgeKeeper _edges;
        private readonly EdgeKeepingFactory _edgeKeepingFactory;
        private readonly VertexFactory _vertexFactory;

        public Graph(bool isDirected) : this(isDirected, new DefaultVertexTagFactory(), new EdgeKeepingFactory(false), new VertexFactory(false))
        {
            
        }

        public Graph(bool isDirected, IVertexTagFactory vertexTagFactory, EdgeKeepingFactory edgeKeepingFactory, VertexFactory vertexFactory, int? vertexCapacity = null)
        {
            _vertexTagFactory = vertexTagFactory;
            _isDirected = isDirected;

            _edgeKeepingFactory = edgeKeepingFactory;
            _vertexFactory = vertexFactory;

            if (vertexCapacity != null)
                _vertices = new Dictionary<string, IVertex>(vertexCapacity.Value);
            else
                _vertices = new Dictionary<string, IVertex>();

            _edges = edgeKeepingFactory.Create();
        }

        public IVertex[] Vertices => _vertices.Values.ToArray();
        public IVertexTag[] VertexTags => _vertices.Values.Select(v=>v.VertexTag).ToArray();
        public Edge[] Edges => _edges.GetEdges().ToArray();
        public int VertexCount => _vertices.Count;

        public void AddEdge(string tailName, string headName)
        {
            IVertex tail = GetVertexData(tailName);
            IVertex head = GetVertexData(headName);

            AddEdge(tail, head);
        }

        public void AddEdge(IVertexTag tail, IVertexTag head)
        {
            AddEdge(GetVertexData(tail.Name),GetVertexData(head.Name));
        }

        private void AddEdge(IVertex tail, IVertex head)
        {
            Edge result = new Edge(tail, head);

            _edges.Add(result);
            tail.AddOutcomeEdge(result);
            head.AddIncomeEdge(result);
        }

        public void RemoveEdge(Edge edge)
        {
            _edges.Remove(edge);
            edge.Tail.RemoveOutcomeEdge(edge);
            edge.Head.RemoveIncomeEdge(edge);
        }

        public void RemoveVertex(string name)
        {
            RemoveVertex(GetVertexData(name));
        }

        void RemoveVertex(IVertex vertexData)
        {
            foreach (var edge in vertexData.GetOutcomeEdges())
            {
                edge.Head.RemoveIncomeEdge(edge);
                _edges.Remove(edge);
            }

            foreach (var edge in vertexData.GetIncomeEdges())
            {
                edge.Tail.RemoveOutcomeEdge(edge);
                _edges.Remove(edge);
            }

            _vertices.Remove(vertexData.Name);
        }

        public IVertexTag GetVertexTag(string name)
        {
            return GetVertexData(name).VertexTag;
        }

        private IVertex GetVertexData(string name)
        {
            IVertex result;
            if (!_vertices.TryGetValue(name, out result))
                result = RegisterVertex(_vertexTagFactory.CreateVertex(name));

            return result;
        }

        private IVertex RegisterVertex(IVertexTag vertexTag)
        {
            var result = _vertexFactory.Create(vertexTag, _edgeKeepingFactory);
            _vertices.Add(vertexTag.Name, result);
            return result;
        }

        public Graph Clone()
        {
            Graph result = new Graph(_isDirected, _vertexTagFactory, _edgeKeepingFactory, _vertexFactory, VertexCount);
            foreach (var edge in _edges.GetEdges())
                result.AddEdge(edge.Tail.Name, edge.Head.Name);

            return result;
        }

        public Graph GetReversed()
        {
            if (!_isDirected)
                throw new Exception("Unidirected graph cannot be reversed");

            Graph result = new Graph(_isDirected, _vertexTagFactory, _edgeKeepingFactory, _vertexFactory, VertexCount);

            foreach (var vertexTag in VertexTags)
                result.RegisterVertex(vertexTag);

            foreach (var edge in _edges.GetEdges())
                result.AddEdge(edge.Head.VertexTag, edge.Tail.VertexTag);

            return result;
        }

        public void Merge(string vName1, string vName2)
        {
            Merge(GetVertexTag(vName1), GetVertexTag(vName2));
        }

        public void Merge(IVertexTag vertexTag1, IVertexTag vertexTag2)
        {
            var v1 = GetVertexData(vertexTag1.Name);
            var v2 = GetVertexData(vertexTag2.Name);
            Merge(v1, v2);
        }

        public void Merge(IVertex v1, IVertex v2)
        {
            var newVertex = GetVertexData(v1.Name + "_" + v2.Name);
            MergeInt(v1, v2, newVertex);
            MergeInt(v2, v1, newVertex);

            _vertices.Remove(v1.Name);
            _vertices.Remove(v2.Name);
        }
        
        private void MergeInt(IVertex v1, IVertex v2, IVertex newVertex)
        {
            var edges = v1.GetOutcomeEdges();
            foreach (Edge edge in edges)
            {
                if (edge.Head != v2)
                {
                    AddEdge(newVertex, edge.Head);
                }

                RemoveEdge(edge);
            }

            edges = v1.GetIncomeEdges();
            foreach (Edge edge in edges)
            {
                if (edge.Tail != v2)
                {
                    AddEdge(edge.Tail, newVertex);
                }

                RemoveEdge(edge);
            }
        }

        public bool HasEdge(IVertex fromVertex, IVertex toVertex)
        {
            if (fromVertex.GetOutcomeEdges().Any(e => e.Head == toVertex))
                return true;

            if (!_isDirected && toVertex.GetOutcomeEdges().Any(e => e.Head == fromVertex))
                return true;

            return false;
        }

        IEnumerable<IVertex> GetConnectedVertices(IVertex fromVertex)
        {
            foreach (var edge in fromVertex.GetOutcomeEdges())
                yield return edge.Head;

            if (!_isDirected)
                foreach (var edge in fromVertex.GetIncomeEdges())
                    yield return edge.Tail;
        }

        public void Visit(IVisitAlgorithm visitAlgorithm, IGraphVisitor visitor, IEnumerable<IVertexTag> vertexTags = null)
        {
            IEnumerable<IVertex> verticesToObserve = _vertices.Values;
            if (vertexTags != null)
                verticesToObserve = vertexTags.Select(v => _vertices[v.Name]);

            HashSet<IVertex> visitedVertices = new HashSet<IVertex>();

            foreach (IVertex vertexOfWholeList in verticesToObserve)
            {
                if (!visitedVertices.Contains(vertexOfWholeList))
                {
                    visitor.StartVisit(vertexOfWholeList);
                    visitAlgorithm.EnqueueVertices(new[] {vertexOfWholeList}, null);

                    DequeueResult? currentVertexResult;
                    while ((currentVertexResult = visitAlgorithm.DequeueVertex()) != null)
                    {
                        IVertex currentVertex = currentVertexResult.Value.Vertex;
                        if (currentVertexResult.Value.Type == VertexDequeueType.Finishing)
                            visitor.FinishVertex(currentVertex);
                        else
                        {
                            if (!visitedVertices.Contains(currentVertex))
                            {
                                visitedVertices.Add(currentVertex);
                                visitor.VisitVertex(currentVertex);
                                
                                visitAlgorithm.EnqueueVertices(GetConnectedVertices(currentVertex), currentVertex);
                            }
                        }
                    }
                }
            }
        }
    }
}
