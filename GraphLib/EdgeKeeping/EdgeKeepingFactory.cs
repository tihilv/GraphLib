namespace GraphLib.EdgeKeeping
{
    public class EdgeKeepingFactory
    {
        private readonly bool _optimizeForRemove;

        public EdgeKeepingFactory(bool optimizeForRemove)
        {
            _optimizeForRemove = optimizeForRemove;
        }

        public IEdgeKeeper Create()
        {
            if (_optimizeForRemove)
                return new DirectoryEdgeKeeper();

            return new ListEdgeKeeper();
        }
    }
}
