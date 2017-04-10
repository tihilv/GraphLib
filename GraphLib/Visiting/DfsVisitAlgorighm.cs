using System.Collections.Generic;
using GraphLib.Vertices;

namespace GraphLib.Visiting
{
    public class DfsVisitAlgorighm : IVisitAlgorithm
    {
        readonly Stack<DequeueResult> _vertices = new Stack<DequeueResult>();

        public void EnqueueVertices(IEnumerable<IVertex> vertices, IVertex parentVertex)
        {
            if (parentVertex != null)
                _vertices.Push(new DequeueResult(parentVertex, VertexDequeueType.Finishing));

            foreach (IVertex vertex in vertices)
                _vertices.Push(new DequeueResult(vertex, VertexDequeueType.Processing));
        }

        public DequeueResult? DequeueVertex()
        {
            if (_vertices.Count > 0)
                return _vertices.Pop();

            return null;
        }
    }
}