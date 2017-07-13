using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphLib.Tsp
{
    public class HeuristicTspFinder
    {
        private readonly List<TspPoint> _points;

        public HeuristicTspFinder(List<TspPoint> points)
        {
            _points = points;
        }

        public double Process()
        {
            var firstVertex = _points[0];
            HashSet<TspPoint> visitedVertices = new HashSet<TspPoint>();

            double distance = 0;

            var currentVertex = firstVertex;
            for (int i = 0; i < _points.Count-1; i++)
            {
                visitedVertices.Add(currentVertex);

                var closestVertex = _points.Where(v => !visitedVertices.Contains(v)).OrderBy(v => v.DistanceToSquared(currentVertex)).First();

                distance += closestVertex.DistanceTo(currentVertex);
                currentVertex = closestVertex;
            }

            distance += currentVertex.DistanceTo(firstVertex);

            return distance;
        }
    }

    public struct TspPoint
    {
        public readonly double X;
        public readonly double Y;

        public TspPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(TspPoint p)
        {
            return Math.Sqrt(DistanceToSquared(p));
        }

        public double DistanceToSquared(TspPoint p)
        {
            return Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2);
        }
    }
}

