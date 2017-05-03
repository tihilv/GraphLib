using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NuGet.Packaging;
using Xunit;

namespace GraphLibTests
{
    public class HammingDistanceExample
    {
        [Fact]
        public void Execute()
        {
            var location = Path.GetDirectoryName(typeof(PathFindingTests).GetTypeInfo().Assembly.Location);
            var path = Path.Combine(location, "..", "..", "..", "data", "hammingClustering.txt");

            List<Cluster> clusters = new List<Cluster>();

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                int index = 0;
                textReader.ReadLine();
                while (!textReader.EndOfStream)
                {
                    index++;
                    var bits = textReader.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(e=>e=="1");
                    var code = new HammingCode(bits);

                    List<Cluster> clustersToMerge = new List<Cluster>();
                    foreach (Cluster cluster in clusters)
                        if (cluster.Contains(code))
                            clustersToMerge.Add(cluster);

                    if (!clustersToMerge.Any())
                    {
                        Cluster newCluster = new Cluster();
                        newCluster.Enlarge(code);
                        clusters.Add(newCluster);
                    }
                    else
                    {
                        var first = clustersToMerge.First();
                        first.Enlarge(code);
                        foreach (var cluster in clustersToMerge.Skip(1))
                        {
                            clusters.Remove(cluster);
                            first.Enlarge(cluster);
                        }
                    }

                }

                Assert.Equal(6118, clusters.Count);
            }
        }
    }

    internal class Cluster
    {
        private readonly HashSet<int> _possibleCodes;

        public Cluster()
        {
            _possibleCodes = new HashSet<int>();
        }

        internal bool Contains(HammingCode code)
        {
            return _possibleCodes.Contains(code.AsInteger());
        }

        public void Enlarge(HammingCode code)
        {
            _possibleCodes.Add(code.AsInteger());

            foreach (HammingCode mutation in code.MutateForTwo())
                _possibleCodes.Add(mutation.AsInteger());
        }

        public void Enlarge(Cluster cluster)
        {
            _possibleCodes.AddRange(cluster._possibleCodes);
        }
    }

    internal class HammingCode
    {
        private readonly bool[] _bits;

        public HammingCode(IEnumerable<bool> bits)
        {
            _bits = bits.ToArray();
        }

        public Int32 AsInteger()
        {
            int result = 0;
            int mult = 1;
            foreach (bool b in _bits)
            {
                result += mult * (b ? 1 : 0);
                mult *= 2;
            }

            return result;
        }

        public IEnumerable<HammingCode> MutateForTwo()
        {
            for (int i = 0; i < _bits.Length; i++)
            {
                var firstMutate = (bool[])_bits.Clone();
                firstMutate[i] = !firstMutate[i];
                yield return new HammingCode(firstMutate);
                for (int j = i + 1; j < _bits.Length; j++)
                {
                    var secondMutate = (bool[]) firstMutate.Clone();
                    secondMutate[j] = !secondMutate[j];
                    yield return new HammingCode(secondMutate);
                }
            }
        }
    }
}

