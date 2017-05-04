using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.Primitives;
using GraphLib.VertexCreation;
using GraphLib.Visiting;

namespace GraphLib.Huffman
{
    public class HuffmanTreeGenerator
    {
        private Graph _graph;

        private LiteralVertextTag _head;

        private Dictionary<string, bool[]> _codes;

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.All, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }

        public Graph Graph => _graph;

        public Dictionary<string, bool[]> Codes => _codes;

        public void Process(IEnumerable<Literal> literals)
        {
            _graph = new Graph(OptimizedOptions(new VertexTagFactory()));

            MinHeap<long, Literal> heap = new MinHeap<long, Literal>(literal => literal.Weight);

            foreach (Literal literal in literals)
            {
                var vertex = (LiteralVertextTag)_graph.GetVertexTag(literal.Idenfitier);
                vertex.SetLiteral(literal);
                heap.Insert(literal);
            }

            do
            {
                var l1 = heap.Extract();
                if (heap.Count == 0)
                    break;
                var l2 = heap.Extract();

                var r = new Literal(l1.Idenfitier+"|"+l2.Idenfitier, l1.Weight+l2.Weight);
                _head = (LiteralVertextTag)_graph.GetVertexTag(r.Idenfitier);
                heap.Insert(r);
                _graph.AddEdge(r.Idenfitier, l1.Idenfitier);
                _graph.AddEdge(r.Idenfitier, l2.Idenfitier);

            } while (true);
        }

        public void FillCodes()
        {
            if (_graph == null)
                throw new Exception("Huffman tree haven't built.");

            HuffmanVisitor visitor = new HuffmanVisitor();
            _graph.Visit(new DfsVisitAlgorighm(), visitor, new []{ _head }.Concat(_graph.Vertices.Select(v=>v.VertexTag).Where(v=>v != _head)));
            _codes = visitor.Codes;
        }
    }
}

