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
        public void SimpleDijkstraTest()
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
            detector.Process(s);
            var result = detector.VertexToPathDictionary;

            Assert.Equal(result[s].Distance, 0);
            Assert.Equal(result[v].Distance, 1);
            Assert.Equal(result[w].Distance, 3);
            Assert.Equal(result[t].Distance, 6);
        }

        [Fact]
        public void DijkstraTestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "dijkstraData.txt");

            Graph graph = new Graph(DijkstraPathFinder.OptimizedOptions());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
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
            detector.Process(graph.GetVertex("1"));
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

        [Fact]
        public void SimpleBellmanFordTest()
        {
            Graph graph = new Graph(BellmanFordPathFinder.OptimizedOptions());

            var s = graph.GetVertex("s");
            var v = graph.GetVertex("v");
            var w = graph.GetVertex("w");
            var t = graph.GetVertex("t");

            graph.AddEdge(s, v, 1);
            graph.AddEdge(s, w, 4);
            graph.AddEdge(v, w, 2);
            graph.AddEdge(v, t, 6);
            graph.AddEdge(w, t, 3);

            BellmanFordPathFinder detector = new BellmanFordPathFinder(graph);
            detector.Process(s);
            var result = detector.VertexToPathDictionary;

            Assert.Equal(result[s].Distance, 0);
            Assert.Equal(result[v].Distance, 1);
            Assert.Equal(result[w].Distance, 3);
            Assert.Equal(result[t].Distance, 6);
        }

        [Fact]
        public void BellmanFordTestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "dijkstraData.txt");

            Graph graph = new Graph(BellmanFordPathFinder.OptimizedOptions());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
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

            BellmanFordPathFinder detector = new BellmanFordPathFinder(graph);
            detector.Process(graph.GetVertex("1"));
            var result = detector.VertexToPathDictionary;

            var output = FormatOutput("7,37,59,82,99,115,133,165,188,197", graph, result);
            Assert.Equal("2599,2610,2947,2052,2367,2399,2029,2442,2505,3068", output);
        }
        
       
        [Fact]
        public void JohnsonTestFromFile()
        {
            var result1 = JohnsonTestFromFile("johnsons_g1.txt");
            Assert.Equal(long.MaxValue, result1);

            var result2 = JohnsonTestFromFile("johnsons_g2.txt");
            Assert.Equal(long.MaxValue, result2);

            var result3 = JohnsonTestFromFile("johnsons_g3.txt");
            Assert.Equal(-19, result3);
        }

        private double JohnsonTestFromFile(string filename)
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", filename);

            Graph graph = new Graph(JohnsonPathFinder.OptimizedOptions());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                textReader.ReadLine();
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    var fromNode = str[0];
                    var toNode = str[1];
                    var length = Int32.Parse(str[2]);

                    graph.AddEdge(fromNode, toNode, length);
                }
            }

            JohnsonPathFinder finder = new JohnsonPathFinder(graph);
            finder.Process();

            double result = long.MaxValue;
            if (!finder.HasNegativeCycle)
            foreach (var tailPair in finder.VertexToPathDictionary)
                foreach (var headPair in tailPair.Value)
                {
                    if (result > headPair.Value.Distance)
                        result = headPair.Value.Distance;
                }

            return result;
        }
    }
}