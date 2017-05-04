using System;
using GraphLib.Vertices;

namespace GraphLib.Mst
{
    public struct MstInfo
    {
        public readonly IVertex Vertex;
        public readonly Edge Edge;

        public MstInfo(IVertex vertex, Edge edge) : this()
        {
            Vertex = vertex;
            Edge = edge;
        }

        public long Length
        {
            get
            {
                if (Edge == null)
                    return Int64.MaxValue;

                return Edge.Length;
            }
        }
    }
}