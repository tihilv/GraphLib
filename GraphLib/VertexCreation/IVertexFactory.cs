namespace GraphLib.VertexCreation
{
    public interface IVertexFactory
    {
        IVertex CreateVertex(string name);
    }

    public class DefaultVertexFactory<T>: IVertexFactory
    {
        public IVertex CreateVertex(string name)
        {
            return new Vertex<T>(name);
        }
    }
}
