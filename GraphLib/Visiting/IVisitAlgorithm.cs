using System.Collections.Generic;
using GraphLib.Vertices;

namespace GraphLib.Visiting
{
    public interface IVisitAlgorithm
    {
        void EnqueueVertices(IEnumerable<IVertex> vertices, IVertex parentVertex);

        DequeueResult? DequeueVertex();
    }

    public enum VertexDequeueType
    {
        Processing,
        Finishing
    }
}
