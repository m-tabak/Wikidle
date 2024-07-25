namespace WikiDataExtractor.Wikipedia
{
    internal class Element
    {
        internal string Name = "";
        internal string Content = "";
        internal int Level;
        internal Element? Parent;
        internal List<Element> Children = [];
    }
}
