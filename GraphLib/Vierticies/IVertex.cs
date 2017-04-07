using System.Collections.Generic;

namespace GraphLib.Vierticies
{
    public interface IVertex
    {
        IVertexTag VertexTag { get; }
        string Name { get; }
        void RemoveOutcomeEdge(Edge edge);
        void RemoveIncomeEdge(Edge edge);
        void AddOutcomeEdge(Edge edge);
        void AddIncomeEdge(Edge edge);
        IEnumerable<Edge> GetOutcomeEdges();
        IEnumerable<Edge> GetIncomeEdges();
    }
}
