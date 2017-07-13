using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using GraphLib;
using GraphLib.Tsp;
using Xunit;

namespace GraphLibTests
{
    public class TspTests
    {
        [Fact]
        public void TestFromFile()
        {
            var graph = GetGraph("tsp1.txt", TspFinder.OptimizedOptions());

            TspFinder finder = new TspFinder(graph);
            var result = finder.Process();
            Assert.Equal(true, Math.Abs(4.41-result) < 0.01);
        }

        [Fact]
        public void HeuristicTestFromFile()
        {
            var graph = GetTspPoints("tsp_big.txt");

            HeuristicTspFinder finder = new HeuristicTspFinder(graph);
            var result = finder.Process();
            Assert.Equal(true, Math.Abs(1203406.5012708856 - result) < 0.01);
        }


        private Graph GetGraph(string fileName, GraphOptions options)
        {
            var points = GetTspPoints(fileName);

            Graph graph = new Graph(options);
            for (int i = 0; i < points.Count; i++)
                for (int j = i + 1; j < points.Count; j++)
                {
                    var dist = points[i].DistanceTo(points[j]);
                    graph.AddEdge(i.ToString(), j.ToString(), dist);
                }

            return graph;
        }

        private static List<TspPoint> GetTspPoints(string fileName)
        {
            var location = Path.GetDirectoryName(typeof(TspTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", fileName);


            List<TspPoint> points = new List<TspPoint>();

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                textReader.ReadLine();
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Trim();
                    var index = str.IndexOf(' ');
                    var x = double.Parse(str.Substring(0, index).Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
                    var y = double.Parse(str.Substring(index + 1).Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));

                    points.Add(new TspPoint(x, y));
                }
            }
            return points;
        }
    }
}
