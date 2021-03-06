﻿using System;

namespace GraphLib.MinCutting
{
    public class MinCutDetector
    {
        private readonly Graph _graph;
        private readonly Random _random;

        public MinCutDetector(Graph graph)
        {
            _graph = graph;
            _random = new Random();
        }

        public void Process()
        {
            int best = int.MaxValue;

            int count = (int)(_graph.VertexTags.Length * _graph.VertexTags.Length * Math.Log(_graph.VertexTags.Length));
            for (int i = 0; i < count; i++)
            {
                var value = TryCut(_graph);
                if (best > value)
                    best = value;
            }
        }

        int TryCut(Graph graph)
        {
            var g = graph.Clone();

            while (g.VertexTags.Length > 2)
            {
                var e = g.Edges[_random.Next(0, g.Edges.Length)];
                g.Merge(e.Tail, e.Head);
            }

            return g.Edges.Length;
        }

    }
}
