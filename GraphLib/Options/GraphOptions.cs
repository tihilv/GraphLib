using GraphLib.VertexCreation;

namespace GraphLib
{
    public class GraphOptions
    {
        private readonly GraphDirection _direction;
        private readonly VerticesStoreMode _verticesStoreMode;
        private readonly GraphPreferedUsage _preferedUsage;
        private readonly IVertexTagFactory _customVertexTagFactory;
        private readonly bool _keepEdgeList;

        public GraphOptions(
            GraphDirection direction = GraphDirection.Undirected, 
            VerticesStoreMode verticesStoreMode = VerticesStoreMode.All, 
            GraphPreferedUsage preferedUsage = GraphPreferedUsage.OptimizedForInsert, 
            bool keepEdgeList = true,
            IVertexTagFactory customVertexTagFactory = null)
        {
            _keepEdgeList = keepEdgeList;
            _direction = direction;
            _verticesStoreMode = verticesStoreMode;
            _preferedUsage = preferedUsage;
            _customVertexTagFactory = customVertexTagFactory;
        }

        public GraphDirection Direction => _direction;

        public VerticesStoreMode VerticesStoreMode => _verticesStoreMode;

        public GraphPreferedUsage PreferedUsage => _preferedUsage;

        public IVertexTagFactory CustomVertexTagFactory => _customVertexTagFactory;

        public bool KeepEdgeList => _keepEdgeList;
    }
}
