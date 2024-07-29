using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WikiDataExtractor.Wikipedia
{
    internal class PageReader
    {

        internal static PageData ReadPage(string title, string rawContent)
        {
            var page = new PageData();
            IEnumerable<string> files = [];
            List<Element> infoboxes = [];
            string text = HttpUtility.HtmlDecode(rawContent);

            // Remove Templates
            // In the process extract Infoboxes and the filenames in them.
            string templateSelector(Element t)
            {
                if (t.Name.StartsWith("infobox", StringComparison.OrdinalIgnoreCase))
                {
                    infoboxes.Add(t);
                    files = files.Concat(ExtractFilenames(t.Content));
                    var photomontage = t.Children.FirstOrDefault(i =>
                    i.Name.StartsWith("photomontage", StringComparison.OrdinalIgnoreCase));
                    if (photomontage != null)
                        files = files.Concat(ExtractFilenames(photomontage.Content));
                }
                return string.Empty;
            }
            ElementParser.ParseAndFormat(text, "{{", "}}", templateSelector, out text);
            // Remove Classes
            ElementParser.ParseAndFormat(text, "{{", "}}", e=>"", out text);


            // Remove links' encoding
            // In the process extract filenames from them.
            string linkSelector(Element link)
            {
                if (link.Name.StartsWith("File:"))
                {
                    files = files.Append(link.Name.Substring(5));
                    return string.Empty;
                }
                if (link.Name.StartsWith("Category:"))
                    return string.Empty;
                var indexOfPipe = link.Content.IndexOf('|');
                if (indexOfPipe == -1)
                    return link.Content;
                else
                    return link.Content.Substring(indexOfPipe + 1);
            }
            ElementParser.ParseAndFormat(text,"[[","]]",linkSelector, out text);

            //Remove Comments
            ElementParser.ParseAndFormat(text, "<!--", "-->", e=>"", out text);


            page.Title = title;
            page.Text = text; ;
            if (infoboxes.Count != 0)
                page.Infoboxes = infoboxes;
            if (files.Any())
                page.Files = files.ToList();
            return page;
        }


        private static IEnumerable<string> ExtractFilenames(string elementText)
        {
            List<string> result = [];
            List<string> extensions = ["jpg", "svg", "png", "jpeg", "webp", "ogg"];
            var chunks = elementText.Split(['=', '|', ':']);
            foreach (var chunk in chunks)
            {
                string? ext = Path.GetExtension(chunk);
                if (string.IsNullOrEmpty(ext))
                    continue;
                else
                {
                    ext = ext.Trim('.', ' ', '\n','\r');
                    if (extensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                        result.Add(chunk.Trim('.', ' ', '\n'));
                }
            }
            return result;
        }
    }
}
