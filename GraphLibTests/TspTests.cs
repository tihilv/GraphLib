using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using GraphLib;
using GraphLib.SccDetection;
using GraphLib.Tsp;
using Xunit;

namespace GraphLibTests
{
    public class TspTests
    {
        [Fact]
        public void TestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(TspTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "tsp1.txt");


            List<Point> points = new List<Point>();

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                textReader.ReadLine();
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Trim();
                    var index = str.IndexOf(' ');
                    var x = double.Parse(str.Substring(0, index));
                    var y = double.Parse(str.Substring(index + 1));

                    points.Add(new Point(x, y));
                }
            }

            Graph graph = new Graph(TspFinder.OptimizedOptions());
            for (int i = 0; i < points.Count; i++)
            for (int j = i + 1; j < points.Count; j++)
            {
                var dist = points[i].DistanceTo(points[j]);
                graph.AddEdge(i.ToString(), j.ToString(), dist);
            }

            TspFinder finder = new TspFinder(graph);
            var result = finder.Process();
            Assert.Equal(true, Math.Abs(4.41-result) < 0.01);
        }


        struct Point
        {
            public readonly double X;
            public readonly double Y;

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }

            public double DistanceTo(Point p)
            {
                return Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2));
            }
        }

    }
}
