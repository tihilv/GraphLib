using System.IO;
using GraphLib;
using GraphLib.SccDetection;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace GraphLibTests
{
    public class SccTests
    {
        [Test]
        public void SimpleTest() 
        {
            Graph graph = new Graph(SccDetector.OptimizedOptions());

            graph.AddEdge("1", "7");
            graph.AddEdge("7", "4");
            graph.AddEdge("4", "1");
            graph.AddEdge("7", "9");
            graph.AddEdge("9", "6");
            graph.AddEdge("6", "3");
            graph.AddEdge("3", "9");
            graph.AddEdge("6", "8");
            graph.AddEdge("8", "2");
            graph.AddEdge("2", "5");
            graph.AddEdge("5", "8");

            SccDetector detector = new SccDetector(graph);
            var result = detector.Process();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(3, result[0].Count);
            Assert.AreEqual(3, result[1].Count);
            Assert.AreEqual(3, result[2].Count);
        }

        [Test]
        public void TestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(SccTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "sccTestData.txt");
            
            Graph graph = new Graph(SccDetector.OptimizedOptions());

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!textReader.EndOfStream)
                {
                    var str = textReader.ReadLine().Trim();
                    var index = str.IndexOf(' ');
                    var node1 = str.Substring(0, index);
                    var node2 = str.Substring(index + 1);

                    graph.AddEdge(node1, node2);
                }
            }

            SccDetector detector = new SccDetector(graph);
            var result = detector.Process();
            var combined = result.Select(r => r.Count).OrderBy(r=>-r).ToArray();

            Assert.AreEqual(434821, combined[0]);
            Assert.AreEqual(968, combined[1]);
            Assert.AreEqual(459, combined[2]);
            Assert.AreEqual(313, combined[3]);
            Assert.AreEqual(211, combined[4]);
        }
    }
}
