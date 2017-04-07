using GraphLib.EdgeKeeping;

namespace GraphLib.Vierticies
{
    public class VertexFactory
    {
        private readonly bool _keepIncomeVertices;

        public VertexFactory(bool keepIncomeVertices)
        {
            _keepIncomeVertices = keepIncomeVertices;
        }

        public IVertex Create(IVertexTag vertexTag, EdgeKeepingFactory edgeKeepingFactory)
        {
            if (_keepIncomeVertices)
                return new OutcomeIncomeEdgeKeepingVertex(vertexTag, edgeKeepingFactory);

            return new OutcomeEdgeKeepingVertex(vertexTag, edgeKeepingFactory);
        }
    }
}
