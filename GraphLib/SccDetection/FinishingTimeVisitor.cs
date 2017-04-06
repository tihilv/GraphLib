using GraphLib.Visiting;

namespace GraphLib.SccDetection
{
    public class FinishingTimeVisitor: IGraphVisitor
    {
        private readonly IVertex[] _vertices;
        private int _position = 0;

        public FinishingTimeVisitor(long verticesCount)
        {
            _vertices = new IVertex[verticesCount];
        }

        public void StartVisit(VertexData vertexOfWholeList)
        {
            
        }

        public void VisitVertex(VertexData vertex)
        {
            
        }

        public void FinishVertex(VertexData vertex)
        {
            _position++;
            _vertices[_vertices.Length - _position] = vertex.Vertex;
        }

        public IVertex[] SortedVertices => _vertices;
    }
}
