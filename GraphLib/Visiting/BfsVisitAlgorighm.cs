using System.Collections.Generic;
using GraphLib.Vierticies;

namespace GraphLib.Visiting
{
    public class BfsVisitAlgorighm : IVisitAlgorithm
    {
        readonly Queue<DequeueResult> _vertices = new Queue<DequeueResult>();

        public void EnqueueVertices(IEnumerable<IVertex> vertices, IVertex parentVertex)
        {
            foreach (IVertex vertex in vertices)
                _vertices.Enqueue(new DequeueResult(vertex, VertexDequeueType.Processing));

            if (parentVertex != null)
                _vertices.Enqueue(new DequeueResult(parentVertex, VertexDequeueType.Finishing));
        }

        public DequeueResult? DequeueVertex()
        {
            if (_vertices.Count > 0)
                return _vertices.Dequeue();

            return null;
        }
    }
}