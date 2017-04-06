using System.Collections.Generic;
using GraphLib.Visiting;

namespace GraphLib.SccDetection
{
    public class SccDetector
    {
        private readonly Graph _graph;

        public SccDetector(Graph graph)
        {
            _graph = graph;
        }

        public List<List<VertexData>> Process()
        {
            var reversedGraph = _graph.GetReversed();

            FinishingTimeVisitor finishingTimeVisitor = new FinishingTimeVisitor(reversedGraph.Vertices.Length);

            reversedGraph.Visit(new DfsVisitAlgorighm(), finishingTimeVisitor);

            SccVisitor sccVisitor = new SccVisitor();
            _graph.Visit(new DfsVisitAlgorighm(), sccVisitor, finishingTimeVisitor.SortedVertices);

            return sccVisitor.Result;
        }
    }
}
