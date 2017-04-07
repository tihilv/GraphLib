using System.Collections.Generic;

namespace GraphLib.EdgeKeeping
{
    internal class FakeEdgeKeeper : IEdgeKeeper
    {
        public void Add(Edge edge)
        {
            
        }

        public void Remove(Edge edge)
        {
            
        }

        public IEnumerable<Edge> GetEdges()
        {
            return null;
        }
    }
}