using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
