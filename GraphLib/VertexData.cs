using System;
using System.Collections.Generic;
using GraphLib.EdgeKeeping;

namespace GraphLib
{
    public class VertexData : IEquatable<VertexData>
    {
        readonly IVertex _vertex;
        private readonly IEdgeKeeper _outcomeEdges;
        private readonly IEdgeKeeper _incomeEdges;

        public VertexData(IVertex vertex, EdgeKeepingFactory edgeKeepingFactory)
        {
            _vertex = vertex;
            _outcomeEdges = edgeKeepingFactory.Create();
            _incomeEdges = edgeKeepingFactory.Create();
        }

        public IVertex Vertex => _vertex;

        public IEdgeKeeper OutcomeEdges => _outcomeEdges;

        public IEdgeKeeper IncomeEdges => _incomeEdges;

        public string Name => _vertex.Name;

        public bool Equals(VertexData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_vertex, other._vertex) && Equals(_outcomeEdges, other._outcomeEdges) && Equals(_incomeEdges, other._incomeEdges);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VertexData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_vertex != null ? _vertex.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_outcomeEdges != null ? _outcomeEdges.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_incomeEdges != null ? _incomeEdges.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(VertexData left, VertexData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VertexData left, VertexData right)
        {
            return !Equals(left, right);
        }

        private sealed class VertexDataEqualityComparer : IEqualityComparer<VertexData>
        {
            public bool Equals(VertexData x, VertexData y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x._vertex, y._vertex) && Equals(x._outcomeEdges, y._outcomeEdges) && Equals(x._incomeEdges, y._incomeEdges);
            }

            public int GetHashCode(VertexData obj)
            {
                unchecked
                {
                    var hashCode = (obj._vertex != null ? obj._vertex.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj._outcomeEdges != null ? obj._outcomeEdges.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj._incomeEdges != null ? obj._incomeEdges.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<VertexData> VertexDataComparer { get; } = new VertexDataEqualityComparer();
    }
}
