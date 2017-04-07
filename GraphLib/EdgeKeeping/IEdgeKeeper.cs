using System.Collections.Generic;

namespace GraphLib.EdgeKeeping
{
    internal interface IEdgeKeeper
    {
        void Add(Edge edge);
        void Remove(Edge edge);
        IEnumerable<Edge> GetEdges();
    }
}
