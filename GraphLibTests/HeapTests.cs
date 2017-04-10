using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GraphLib;
using GraphLib.TmpPrimitives;
using Xunit;
using Xunit.Abstractions;

namespace GraphLibTests
{
    public class HeapTests
    {
        private readonly ITestOutputHelper _output;

        public HeapTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SimpleTest()
        {
            var input = "32 45 79 52 65 98 25 68 90 92".Split(' ').Select(n => int.Parse(n));

            MinHeap<int, int> heap = new MinHeap<int, int>(e => e);
            List<int> numbers = new List<int>();

            foreach (var n in input)
            {
                heap.Insert(n);
                numbers.Add(n);
            }

            numbers.Sort();
            for (int j = 0; j < numbers.Count; j++)
            {
                int fromHeap = heap.Extract();
                Assert.Equal(numbers[j], fromHeap);
            }
        }
        
        [Fact]
        public void RandomizedTest()
        {
            Random random = new Random();

            int iterationsCount = 1000;
            int elementCount = 500;
            int maxValue = 250;

            for (int i = 0; i < iterationsCount; i++)
            {
                MinHeap<int, int> heap = new MinHeap<int, int>(e=>e);
                List<int> numbers = new List<int>();

                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < elementCount; j++)
                {
                    int n = random.Next(0, maxValue);
                    heap.Insert(n);
                    numbers.Add(n);
                    sb.Append(n.ToString());
                    sb.Append(" ");
                }

                numbers.Sort();
                _output.WriteLine(sb.ToString());
                for (int j = 0; j < numbers.Count; j++)
                {
                    int fromHeap = heap.Extract();
                    Assert.Equal(numbers[j], fromHeap);
                }
            }
        }

        [Fact]
        public void MedianTestFromFile()
        {
            var location = Path.GetDirectoryName(typeof(HeapTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "median.txt");

            var heapForMax = new MinHeap<int, int>(i => i);
            var heapForMin = new MinHeap<int, int>(i => -i);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                int result = 0;

                while (!textReader.EndOfStream)
                {
                    var n = int.Parse(textReader.ReadLine());

                    if ((heapForMax.Count == 0 || heapForMin.Count == 0) || (heapForMax.Peek() <= n))
                        heapForMax.Insert(n);
                    else
                        heapForMin.Insert(n);

                    if (heapForMax.Count > (heapForMin.Count + 1))
                        heapForMin.Insert(heapForMax.Extract());
                    else if (heapForMin.Count > (heapForMax.Count + 1))
                        heapForMax.Insert(heapForMin.Extract());

                    int median;
                    if (heapForMin.Count < heapForMax.Count)
                        median = heapForMax.Peek();
                    else
                        median = heapForMin.Peek();

                    result = (result + median) % 10000;
                }

                Assert.Equal(1213, result);
            }
           
        }

    }
}
