﻿using System.Collections.Generic;
using GraphLib.CommonOperations;
using GraphLib.VertexCreation;
using GraphLib.Vertices;
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

        public List<List<IVertex>> Process()
        {
            var reversedGraph = ReverseVertex.Execute(_graph);

            FinishingTimeVisitor finishingTimeVisitor = new FinishingTimeVisitor(reversedGraph.VertexCount);

            reversedGraph.Visit(new DfsVisitAlgorighm(), finishingTimeVisitor);

            SccVisitor sccVisitor = new SccVisitor();
            _graph.Visit(new DfsVisitAlgorighm(), sccVisitor, finishingTimeVisitor.SortedVertexTags);

            return sccVisitor.Result;
        }

        public static GraphOptions OptimizedOptions(IVertexTagFactory vertexTagFactory = null)
        {
            return new GraphOptions(GraphDirection.Directed, VerticesStoreMode.Outcome, GraphPreferedUsage.OptimizedForInsert, false, vertexTagFactory);
        }

    }
}
