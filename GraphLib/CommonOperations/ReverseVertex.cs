using System;
using GraphLib.Vierticies;
using GraphLib.Visiting;

namespace GraphLib.CommonOperations
{
    public class ReverseVertex: IGraphVisitor
    {
        private readonly Graph _outputGraph;

        public ReverseVertex(Graph outputGraph)
        {
            _outputGraph = outputGraph;
        }
        
        public void StartVisit(IVertex vertexOfWholeList)
        { }

        public void VisitVertex(IVertex vertex)
        {
            foreach (Edge edge in vertex.GetOutcomeEdges())
                _outputGraph.AddEdge(edge.Head.VertexTag, edge.Tail.VertexTag);
        }

        public void FinishVertex(IVertex vertex)
        { }

        public static Graph Execute(Graph graph)
        {
            if (graph.Options.Direction == GraphDirection.Undirected)
                throw new Exception("Undirected graph cannot be reversed");

            Graph result = graph.GetBlankForClone();
            if (graph.Options.KeepEdgeList)
            {
                foreach (var edge in graph.Edges)
                    result.AddEdge(edge.Head.VertexTag, edge.Tail.VertexTag);
            }
            else
            {
                ReverseVertex visitor = new ReverseVertex(result);
                graph.Visit(new BfsVisitAlgorighm(), visitor);
            }

            return result;
        }
    }
}
