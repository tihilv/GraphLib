﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using GraphLib;
using GraphLib.Mst;
using NUnit.Framework;

namespace GraphLibTests
{
    public class UnionTests
    {
        //[Test]
        //public void SimpleTest()
        //{
        //    Graph graph = new Graph(PrimsMstFinder.OptimizedOptions());

        //    graph.AddEdge("6", "3", 1);
        //    graph.AddEdge("6", "4", -1);

        //    graph.AddEdge("5", "6", -1);

        //    graph.AddEdge("1", "2", -5);
        //    graph.AddEdge("2", "3", -4);
        //    graph.AddEdge("3", "4", -9);
        //    graph.AddEdge("4", "1", -10);
        //    graph.AddEdge("1", "3", -11);
        //    graph.AddEdge("2", "4", -6);

        //    graph.AddEdge("5", "2", 1);
        //    graph.AddEdge("3", "5", 10);

        //    PrimsMstFinder detector = new PrimsMstFinder(graph);
        //    detector.Process();
        //    var result = detector.Edges.Sum(i => i.Length);

        //    Assert.Equal(-29, result);
        //}

        [Test]
        public void TestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "kruskalClustering.txt");

            Graph graph = new Graph(KruskalMstFinder.OptimizedOptions());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                textReader.ReadLine();
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var fromNode = str[0];
                    var toNode = str[1];
                    var length = Int32.Parse(str[2]);

                    graph.AddEdge(toNode, fromNode, length);
                }
            }

            KruskalMstFinder detector = new KruskalMstFinder(graph);
            detector.Process(4);

            Assert.AreEqual(106, detector.Spacing);
        }
    }
}
