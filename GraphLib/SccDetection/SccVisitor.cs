using System.Collections.Generic;
using GraphLib.Vertices;
using GraphLib.Visiting;
using Scc = System.Collections.Generic.List<GraphLib.Vertices.IVertex>;

namespace GraphLib.SccDetection
{
    internal class SccVisitor: IGraphVisitor
    {
        private readonly List<Scc> _sccs;
        private Scc _currentScc;

        public SccVisitor()
        {
            _sccs = new List<Scc>();
        }

        public void StartVisit(IVertex vertexOfWholeList)
        {
            _currentScc = new Scc();
            _sccs.Add(_currentScc);
        }

        public void VisitVertex(IVertex vertex)
        {
            _currentScc.Add(vertex);
        }

        public void FinishVertex(IVertex vertex)
        {
            
        }

        public List<Scc> Result => _sccs;
    }
}
