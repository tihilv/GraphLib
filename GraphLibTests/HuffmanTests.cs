using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GraphLib.Huffman;
using NUnit.Framework;

namespace GraphLibTests
{
    public class HuffmanTests
    {
        [Test]
        public void SimpleTest1()
        {
            List<Literal> literals = new List<Literal>();
            literals.Add(new Literal("A", 28));
            literals.Add(new Literal("B", 27));
            literals.Add(new Literal("C", 20));
            literals.Add(new Literal("D", 15));
            literals.Add(new Literal("E", 10));

            HuffmanTreeGenerator treeGenerator = new HuffmanTreeGenerator();
            treeGenerator.Process(literals);
            treeGenerator.FillCodes();

            var r = treeGenerator.Codes["A"].Length * 28 + treeGenerator.Codes["B"].Length * 27 +
                    treeGenerator.Codes["C"].Length * 20 + treeGenerator.Codes["D"].Length * 15 +
                    treeGenerator.Codes["E"].Length * 10;
        }


        [Test]
        public void SimpleTest()
        {
            List<Literal> literals = new List<Literal>();
            literals.Add(new Literal("A", 3));
            literals.Add(new Literal("B", 2));
            literals.Add(new Literal("C", 6));
            literals.Add(new Literal("D", 8));
            literals.Add(new Literal("E", 2));
            literals.Add(new Literal("F", 6));

            HuffmanTreeGenerator treeGenerator = new HuffmanTreeGenerator();
            treeGenerator.Process(literals);
            treeGenerator.FillCodes();

            Assert.AreEqual(3, treeGenerator.Codes["A"].Length);
            Assert.AreEqual(4, treeGenerator.Codes["B"].Length);
            Assert.AreEqual(2, treeGenerator.Codes["C"].Length);
            Assert.AreEqual(2, treeGenerator.Codes["D"].Length);
            Assert.AreEqual(4, treeGenerator.Codes["E"].Length);
            Assert.AreEqual(2, treeGenerator.Codes["F"].Length);
        }

        [Test]
        public void TestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "huffman.txt");

            List<Literal> literals = new List<Literal>();

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                textReader.ReadLine();
                int name = 0;
                while (!textReader.EndOfStream)
                {
                    var weight = long.Parse(textReader.ReadLine());
                    literals.Add(new Literal(name.ToString(), weight));
                    name++;
                }
            }

            HuffmanTreeGenerator treeGenerator = new HuffmanTreeGenerator();
            treeGenerator.Process(literals);
            treeGenerator.FillCodes();

            var min = treeGenerator.Codes.Values.Min(v => v.Length);
            var max = treeGenerator.Codes.Values.Max(v => v.Length);
            Assert.AreEqual(9, min);
            Assert.AreEqual(19, max);
        }
    }
}
