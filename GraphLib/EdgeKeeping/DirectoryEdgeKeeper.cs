using System.Collections.Generic;

namespace GraphLib.EdgeKeeping
{
    public class DirectoryEdgeKeeper: IEdgeKeeper
    {
        private readonly Dictionary<Edge, Edge> _edges;

        public DirectoryEdgeKeeper()
        {
            _edges = new Dictionary<Edge, Edge>();
        }

        public void Add(Edge edge)
        {
            _edges.Add(edge, edge);
        }

        public void Remove(Edge edge)
        {
            _edges.Remove(edge);
        }

        public IEnumerable<Edge> GetEdges()
        {
            return _edges.Values;
        }
    }
}
