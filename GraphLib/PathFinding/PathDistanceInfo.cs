using GraphLib.Vertices;

namespace GraphLib.PathFinding
{
    public struct PathDistanceInfo
    {
        public readonly IVertex Vertex;
        public readonly double Distance;
        public readonly IVertex Parent;

        public PathDistanceInfo(IVertex vertex, double score, IVertex parent) : this()
        {
            Vertex = vertex;
            Distance = score;
            Parent = parent;
        }
    }
}