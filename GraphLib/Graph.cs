using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.EdgeKeeping;
using GraphLib.VertexCreation;
using GraphLib.Visiting;

namespace GraphLib
{
    public class Graph
    {
        private readonly IVertexFactory _vertexFactory;
        private readonly bool _isDirected;

        private readonly Dictionary<string, VertexData> _vertices;
        private readonly Dictionary<Edge, Edge> _edges;
        private readonly EdgeKeepingFactory _edgeKeepingFactory;

        public Graph(bool isDirected) : this(isDirected, new DefaultVertexFactory<object>(), new EdgeKeepingFactory(false))
        {
            
        }

        public Graph(bool isDirected, IVertexFactory vertexFactory, EdgeKeepingFactory edgeKeepingFactory)
        {
            _vertexFactory = vertexFactory;
            _isDirected = isDirected;

            _edgeKeepingFactory = edgeKeepingFactory;
            _vertices = new Dictionary<string, VertexData>();
            _edges = new Dictionary<Edge, Edge>();
        }

        public IVertex[] Vertices => _vertices.Values.Select(v=>v.Vertex).ToArray();
        public List<Edge> Edges => _edges.Values.ToList();

        public void AddEdge(string tailName, string headName)
        {
            VertexData tail = GetVertexData(tailName);
            VertexData head = GetVertexData(headName);

            AddEdge(tail, head);
        }

        public void AddEdge(IVertex tail, IVertex head)
        {
            AddEdge(GetVertexData(tail.Name),GetVertexData(head.Name));
        }

        private void AddEdge(VertexData tail, VertexData head)
        {
            Edge result = new Edge(tail, head);

            _edges.Add(result, result);
            tail.OutcomeEdges.Add(result);
            head.IncomeEdges.Add(result);
        }

        public void RemoveEdge(Edge edge)
        {
            _edges.Remove(edge);
            edge.Tail.OutcomeEdges.Remove(edge);
            edge.Head.IncomeEdges.Remove(edge);
        }

        public IVertex GetVertex(string name)
        {
            return GetVertexData(name).Vertex;
        }

        private VertexData GetVertexData(string name)
        {
            VertexData result;
            if (!_vertices.TryGetValue(name, out result))
                result = RegisterVertex(_vertexFactory.CreateVertex(name));

            return result;
        }

        private VertexData RegisterVertex(IVertex vertex)
        {
            var result = new VertexData(vertex, _edgeKeepingFactory);
            _vertices.Add(vertex.Name, result);
            return result;
        }

        public Graph Clone()
        {
            Graph result = new Graph(_isDirected, _vertexFactory, _edgeKeepingFactory);
            foreach (var edge in _edges.Values)
                result.AddEdge(edge.Tail.Name, edge.Head.Name);

            return result;
        }

        public Graph GetReversed()
        {
            if (!_isDirected)
                throw new Exception("Unidirected graph cannot be reversed");

            Graph result = new Graph(_isDirected, _vertexFactory, _edgeKeepingFactory);

            foreach (var vertex in Vertices)
                result.RegisterVertex(vertex);

            foreach (var edge in _edges.Values)
                result.AddEdge(edge.Head.Vertex, edge.Tail.Vertex);

            return result;
        }

        public void Merge(string vName1, string vName2)
        {
            Merge(GetVertex(vName1), GetVertex(vName2));
        }

        public void Merge(IVertex vertex1, IVertex vertex2)
        {
            var v1 = GetVertexData(vertex1.Name);
            var v2 = GetVertexData(vertex2.Name);
            Merge(v1, v2);
        }

        public void Merge(VertexData v1, VertexData v2)
        {
            var newVertex = GetVertexData(v1.Name + "_" + v2.Name);
            MergeInt(v1, v2, newVertex);
            MergeInt(v2, v1, newVertex);

            _vertices.Remove(v1.Name);
            _vertices.Remove(v2.Name);
        }
        
        private void MergeInt(VertexData v1, VertexData v2, VertexData newVertex)
        {
            var edges = v1.OutcomeEdges.GetEdges();
            foreach (Edge edge in edges)
            {
                if (edge.Head != v2)
                {
                    AddEdge(newVertex, edge.Head);
                }

                RemoveEdge(edge);
            }

            edges = v1.IncomeEdges.GetEdges();
            foreach (Edge edge in edges)
            {
                if (edge.Tail != v2)
                {
                    AddEdge(edge.Tail, newVertex);
                }

                RemoveEdge(edge);
            }

        }

        //public bool HasEdge(IVertex fromVertex, IVertex toVertex)
        //{
        //    if (_edgesFromTails.GetEdges(fromVertex).Any(e => e.Head == toVertex))
        //        return true;

        //    if (!_isDirected && _edgesFromTails.GetEdges(toVertex).Any(e => e.Head == fromVertex))
        //        return true;

        //    return false;
        //}

        IEnumerable<VertexData> GetConnectedVertices(VertexData fromVertex)
        {
            foreach (var edge in fromVertex.OutcomeEdges.GetEdges())
                yield return edge.Head;

            if (!_isDirected)
                foreach (var edge in fromVertex.IncomeEdges.GetEdges())
                    yield return edge.Tail;
        }

        public void Visit(IVisitAlgorithm visitAlgorithm, IGraphVisitor visitor, IEnumerable<IVertex> vertices = null)
        {
            IEnumerable<VertexData> verticesToObserve = _vertices.Values;
            if (vertices != null)
                verticesToObserve = vertices.Select(v => _vertices[v.Name]);

            HashSet<VertexData> visitedVertices = new HashSet<VertexData>();

            foreach (VertexData vertexOfWholeList in verticesToObserve)
            {
                if (!visitedVertices.Contains(vertexOfWholeList))
                {
                    visitor.StartVisit(vertexOfWholeList);
                    visitAlgorithm.EnqueueVertices(new[] {vertexOfWholeList}, null);

                    DequeueResult? currentVertexResult;
                    while ((currentVertexResult = visitAlgorithm.DequeueVertex()) != null)
                    {
                        VertexData currentVertex = currentVertexResult.Value.Vertex;
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
