namespace GraphLib.EdgeKeeping
{
    internal class EdgeKeepingFactory
    {
        private readonly GraphOptions _graphOptions;

        public EdgeKeepingFactory(GraphOptions graphOptions)
        {
            _graphOptions = graphOptions;
        }

        public IEdgeKeeper Create()
        {
            if (_graphOptions.PreferedUsage == GraphPreferedUsage.OptimizedForRemove)
                return new DirectoryEdgeKeeper();

            return new ListEdgeKeeper();
        }

        public IEdgeKeeper CreateGlobal()
        {
            if (_graphOptions.KeepEdgeList)
                return Create();

            return new FakeEdgeKeeper();
        }
    }
}
