namespace GraphLib.Visiting
{
    public interface IGraphVisitor
    {
        void StartVisit(VertexData vertexOfWholeList);
        void VisitVertex(VertexData vertex);
        void FinishVertex(VertexData vertex);
    }
}
