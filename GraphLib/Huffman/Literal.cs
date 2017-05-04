namespace GraphLib.Huffman
{
    public struct Literal
    {
        public readonly string Idenfitier;
        public readonly long Weight;

        public Literal(string idenfitier, long weight)
        {
            Idenfitier = idenfitier;
            Weight = weight;
        }
    }
}