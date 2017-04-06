namespace GraphLib
{
    class Vertex<T>: IVertex
    {
        readonly string _name;
        public T Tag;

        public Vertex(string name)
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