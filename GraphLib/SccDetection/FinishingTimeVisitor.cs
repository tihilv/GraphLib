using GraphLib.Vertices;
using GraphLib.Visiting;

namespace GraphLib.SccDetection
{
    internal class FinishingTimeVisitor: IGraphVisitor
    {
        private readonly IVertexTag[] _vertexTags;
        private int _position = 0;

        public FinishingTimeVisitor(long verticesCount)
        {
            _vertexTags = new IVertexTag[verticesCount];
        }

        public void StartVisit(IVertex vertexOfWholeList)
        {
            
        }

        public void VisitVertex(IVertex vertex)
        {
            
        }

        public void FinishVertex(IVertex vertex)
        {
            _position++;
            _vertexTags[_vertexTags.Length - _position] = vertex.VertexTag;
        }

        public IVertexTag[] SortedVertexTags => _vertexTags;
    }
}
