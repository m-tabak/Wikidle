using System.Xml.Linq;

namespace WikiDataExtractor.Wikipedia
{
    internal class PageData
    {
        internal PageData(string title, string text)
        {
            Title = title;
            Text = text;
        }

        internal PageData() :this("", "") 
        { }

        internal string Title { get; set; }
        internal string Text { get; set; }
        internal List<Element>? Infoboxes { get; set; }
        internal List<string>? Files { get; set; }

    }
}
