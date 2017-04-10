using GraphLib.Vertices;

namespace GraphLib.Visiting
{
    public interface IGraphVisitor
    {
        void StartVisit(IVertex vertexOfWholeList);
        void VisitVertex(IVertex vertex);
        void FinishVertex(IVertex vertex);
    }
}
