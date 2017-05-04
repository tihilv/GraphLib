using GraphLib.VertexCreation;

namespace GraphLib.Huffman
{
    class LiteralVertextTag : VertexTag
    {
        public Literal Literal { get; set; }

        public LiteralVertextTag(string name) : base(name)
        {
        }

        public void SetLiteral(Literal literal)
        {
            Literal = literal;
        }
    }
}