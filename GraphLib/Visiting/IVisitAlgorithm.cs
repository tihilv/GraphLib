using System.Collections.Generic;

namespace GraphLib.Visiting
{
    public interface IVisitAlgorithm
    {
        void EnqueueVertices(IEnumerable<VertexData> vertices, VertexData parentVertex);

        DequeueResult? DequeueVertex();
    }

    public struct DequeueResult
    {
        public readonly VertexData Vertex;
        public readonly VertexDequeueType Type;

        public DequeueResult(VertexData vertex, VertexDequeueType type)
        {
            Vertex = vertex;
            Type = type;
        }
    }

    public enum VertexDequeueType
    {
        Processing,
        Finishing
    }
}
