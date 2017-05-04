using GraphLib.VertexCreation;

namespace GraphLib.Huffman
{
    class VertexTagFactory : IVertexTagFactory
    {
        public IVertexTag CreateVertex(string name)
        {
            return new LiteralVertextTag(name);
        }
    }
}