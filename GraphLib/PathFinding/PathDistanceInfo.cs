using GraphLib.Vertices;

namespace GraphLib.PathFinding
{
    public struct PathDistanceInfo
    {
        public readonly IVertex Vertex;
        public readonly long Distance;
        public readonly IVertex Parent;

        public PathDistanceInfo(IVertex vertex, long score, IVertex parent) : this()
        {
            Vertex = vertex;
            Distance = score;
            Parent = parent;
        }
    }
}