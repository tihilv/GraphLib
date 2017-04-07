namespace GraphLib.VertexCreation
{
    class VertexTag: IVertexTag
    {
        readonly string _name;
        public VertexTag(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public string Name => _name;
    }
}