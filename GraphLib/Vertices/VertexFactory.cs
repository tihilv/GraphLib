using GraphLib.EdgeKeeping;

namespace GraphLib.Vertices
{
    internal class VertexFactory
    {
        private readonly GraphOptions _graphOptions;

        public VertexFactory(GraphOptions graphOptions)
        {
            _graphOptions = graphOptions;
        }

        public IVertex Create(IVertexTag vertexTag, EdgeKeepingFactory edgeKeepingFactory)
        {
            if (_graphOptions.VerticesStoreMode == VerticesStoreMode.All)
                return new OutcomeIncomeEdgeKeepingVertex(vertexTag, edgeKeepingFactory);

            return new OutcomeEdgeKeepingVertex(vertexTag, edgeKeepingFactory);
        }
    }
}
