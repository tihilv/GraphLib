using System;
using System.Collections.Generic;
using GraphLib.Vertices;

namespace GraphLib
{
    public class Edge : IEquatable<Edge>
    {
        public readonly IVertex Tail;
        public readonly IVertex Head;
        public readonly long Length;

        internal Edge(IVertex tail, IVertex head, long length)
        {
            Tail = tail;
            Head = head;
            Length = length;
        }

        public override string ToString()
        {
            return Tail.Name + " - " + Head.Name;
        }

        public bool Equals(Edge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Tail, other.Tail) && Equals(Head, other.Head);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Tail != null ? Tail.GetHashCode() : 0) * 397) ^ (Head != null ? Head.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Edge left, Edge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Edge left, Edge right)
        {
            return !Equals(left, right);
        }

        private sealed class TailHeadEqualityComparer : IEqualityComparer<Edge>
        {
            public bool Equals(Edge x, Edge y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.Tail, y.Tail) && Equals(x.Head, y.Head);
            }

            public int GetHashCode(Edge obj)
            {
                unchecked
                {
                    return ((obj.Tail != null ? obj.Tail.GetHashCode() : 0) * 397) ^ (obj.Head != null ? obj.Head.GetHashCode() : 0);
                }
            }
        }

        public static IEqualityComparer<Edge> TailHeadComparer { get; } = new TailHeadEqualityComparer();
    }
}