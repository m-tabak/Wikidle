namespace WikiDataExtractor.Wikipedia
{
    internal class Element
    {
        internal int StartTokenIndex;
        internal int EndTokenIndex;
        internal string Name = "";
        internal string Text = "";
        internal int Level;
        internal Element? Parent;
        internal List<Element> Children = [];
    }
}
