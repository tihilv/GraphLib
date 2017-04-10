using System.Collections.Generic;
using GraphLib.EdgeKeeping;

namespace GraphLib.Vertices
{
    internal class OutcomeIncomeEdgeKeepingVertex : IVertex
    {
        static readonly Edge[] _emptyEdges = new Edge[0];

        private readonly IVertexTag _vertexTag;
        private readonly EdgeKeepingFactory _edgeKeepingFactory;
        private IEdgeKeeper _outcomeEdges;
        private IEdgeKeeper _incomeEdges;

        public OutcomeIncomeEdgeKeepingVertex(IVertexTag vertexTag, EdgeKeepingFactory edgeKeepingFactory)
        {
            _vertexTag = vertexTag;
            _edgeKeepingFactory = edgeKeepingFactory;

            _outcomeEdges = null;
            _incomeEdges = null;
        }

        public IVertexTag VertexTag => _vertexTag;

        public string Name => _vertexTag.Name;


        public void RemoveOutcomeEdge(Edge edge)
        {
            _outcomeEdges.Remove(edge);
        }

        public void RemoveIncomeEdge(Edge edge)
        {
            _incomeEdges.Remove(edge);
        }

        public void AddOutcomeEdge(Edge edge)
        {
            if (_outcomeEdges == null)
                _outcomeEdges = _edgeKeepingFactory.Create();

            _outcomeEdges.Add(edge);
        }

        public void AddIncomeEdge(Edge edge)
        {
            if (_incomeEdges == null)
                _incomeEdges = _edgeKeepingFactory.Create();

            _incomeEdges.Add(edge);
        }

        public IEnumerable<Edge> GetOutcomeEdges()
        {
            if (_outcomeEdges == null)
                return _emptyEdges;

            return _outcomeEdges.GetEdges();
        }

        public IEnumerable<Edge> GetIncomeEdges()
        {
            if (_incomeEdges == null)
                return _emptyEdges;

            return _incomeEdges.GetEdges();
        }
    }
}