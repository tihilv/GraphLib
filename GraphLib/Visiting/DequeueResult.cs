using GraphLib.Vierticies;

namespace GraphLib.Visiting
{
    public struct DequeueResult
    {
        public readonly IVertex Vertex;
        public readonly VertexDequeueType Type;

        public DequeueResult(IVertex vertex, VertexDequeueType type)
        {
            Vertex = vertex;
            Type = type;
        }
    }
}