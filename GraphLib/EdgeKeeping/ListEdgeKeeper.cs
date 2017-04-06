using System.Collections.Generic;

namespace GraphLib.EdgeKeeping
{
    public class ListEdgeKeeper : IEdgeKeeper
    {
        private readonly List<Edge> _edges;

        public ListEdgeKeeper()
        {
            _edges = new List<Edge>();
        }

        public void Add(Edge edge)
        {
            _edges.Add(edge);
        }

        public void Remove(Edge edge)
        {
            _edges.Remove(edge);
        }

        public IEnumerable<Edge> GetEdges()
        {
            return _edges;
        }
    }
}