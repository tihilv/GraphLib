using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Reflection;
using GraphLib;
using GraphLib.PathFinding;
using GraphLib.Vertices;

namespace GraphLibTests
{
    public class PathFindingTests
    {
        [Fact]
        public void SimpleTest()
        {
            Graph graph = new Graph(DijkstraPathFinder.OptimizedOptions());

            var s = graph.GetVertex("s");
            var v = graph.GetVertex("v");
            var w = graph.GetVertex("w");
            var t = graph.GetVertex("t");

            graph.AddEdge(s, v, 1);
            graph.AddEdge(s, w, 4);
            graph.AddEdge(v, w, 2);
            graph.AddEdge(v, t, 6);
            graph.AddEdge(w, t, 3);

            DijkstraPathFinder detector = new DijkstraPathFinder(graph);
            detector.ProcessNaive(s);
            var result = detector.VertexToPathDictionary;

            Assert.Equal(result[s].Distance, 0);
            Assert.Equal(result[v].Distance, 1);
            Assert.Equal(result[w].Distance, 3);
            Assert.Equal(result[t].Distance, 6);
        }

        [Fact]
        public void TestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "dijkstraData.txt");

            Graph graph = new Graph(DijkstraPathFinder.OptimizedOptions());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Split(new [] { '\t'}, StringSplitOptions.RemoveEmptyEntries);
                    var fromNode = str[0];

                    for (int i = 1; i < str.Length; i++)
                    {
                        var dstParsed = str[i].Split(',');
                        var dstNode = dstParsed[0];
                        var length = int.Parse(dstParsed[1]);
                        graph.AddEdge(fromNode, dstNode, length);
                    }
                }
            }

            DijkstraPathFinder detector = new DijkstraPathFinder(graph);
            detector.ProcessNaive(graph.GetVertex("1"));
            var result = detector.VertexToPathDictionary;

            var output = FormatOutput("7,37,59,82,99,115,133,165,188,197", graph, result);
            Assert.Equal("2599,2610,2947,2052,2367,2399,2029,2442,2505,3068", output);
        }

        string FormatOutput(string inputNodeNumbers, Graph graph, Dictionary<IVertex, PathDistanceInfo> data)
        {
            StringBuilder result = new StringBuilder();
            var splitted = inputNodeNumbers.Split(',');

            foreach (var s in splitted)
            {
                if (result.Length > 0)
                    result.Append(",");
                result.Append(data[graph.GetVertex(s)].Distance);
            }

            return result.ToString();
        }
    }
}
