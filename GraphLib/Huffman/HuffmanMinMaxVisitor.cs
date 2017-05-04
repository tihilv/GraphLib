using System;
using System.Collections.Generic;
using System.Linq;
using GraphLib.Vertices;
using GraphLib.Visiting;

namespace GraphLib.Huffman
{
    class HuffmanVisitor : IGraphVisitor
    {
        private readonly Dictionary<string, bool[]> _codes;

        private readonly List<bool> _current;
        private bool _firstVertex;

        public Dictionary<string, bool[]> Codes
        {
            get { return _codes; }
        }

        public HuffmanVisitor()
        {
            _codes = new Dictionary<string, bool[]>();
            _current = new List<bool>();

            _firstVertex = true;
        }

        public void StartVisit(IVertex vertexOfWholeList)
        {
            
        }

        public void VisitVertex(IVertex vertex)
        {
            if (!_firstVertex)
            {
                bool left = vertex.GetIncomeEdges().First().Tail.GetOutcomeEdges().First().Head == vertex;
                _current.Add(left);
            }
            else
                _firstVertex = false;

            if (!String.IsNullOrEmpty(((LiteralVertextTag)vertex.VertexTag).Literal.Idenfitier))
            {
                _codes.Add(vertex.Name, _current.ToArray());
            }
        }

        public void FinishVertex(IVertex vertex)
        {
            if (_current.Any())
                _current.RemoveAt(_current.Count - 1);
        }
    }
}