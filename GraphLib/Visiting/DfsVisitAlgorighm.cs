using System.Collections.Generic;

namespace GraphLib.Visiting
{
    public class DfsVisitAlgorighm : IVisitAlgorithm
    {
        readonly Stack<DequeueResult> _vertices = new Stack<DequeueResult>();

        public void EnqueueVertices(IEnumerable<VertexData> vertices, VertexData parentVertex)
        {
            if (parentVertex != null)
                _vertices.Push(new DequeueResult(parentVertex, VertexDequeueType.Finishing));

            foreach (VertexData vertex in vertices)
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