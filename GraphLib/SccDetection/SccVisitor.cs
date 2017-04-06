using System.Collections.Generic;
using GraphLib.Visiting;
using Scc = System.Collections.Generic.List<GraphLib.VertexData>;

namespace GraphLib.SccDetection
{
    public class SccVisitor: IGraphVisitor
    {
        private readonly List<Scc> _sccs;
        private Scc _currentScc;

        public SccVisitor()
        {
            _sccs = new List<Scc>();
        }

        public void StartVisit(VertexData vertexOfWholeList)
        {
            _currentScc = new Scc();
            _sccs.Add(_currentScc);
        }

        public void VisitVertex(VertexData vertex)
        {
            _currentScc.Add(vertex);
        }

        public void FinishVertex(VertexData vertex)
        {
            
        }

        public List<Scc> Result => _sccs;
    }
}
