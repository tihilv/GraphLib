namespace GraphLib.VertexCreation
{
    public class DefaultVertexTagFactory: IVertexTagFactory
    {
        public IVertexTag CreateVertex(string name)
        {
            return new VertexTag(name);
        }
    }
}