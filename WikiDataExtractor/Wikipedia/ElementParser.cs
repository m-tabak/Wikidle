
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Web;

namespace WikiDataExtractor.Wikipedia
{
    internal class ElementParser
    {

        /// <summary>
        /// Parse text for elements between two tags. 
        /// </summary>
        /// <param name="rawText">The text to process.</param>
        /// <param name="startTag">What marks the start of the element.</param>
        /// <param name="endTag">What marks the end of the element.</param>
        /// <returns>A collection of all elements found.</returns>
        internal static List<Element> Parse(
            string rawText,
            string startTag,
            string endTag
            )
        {
            return ParseAndFormat(rawText, startTag, endTag, e => "", out _);
        }

        /// <summary>
        /// Parse text for elements between two tags. And replace the elements according to a function.
        /// </summary>
        /// <param name="rawText">The text to process.</param>
        /// <param name="startTag">What marks the start of the element.</param>
        /// <param name="endTag">What marks the end of the element.</param>
        /// <param name="formatter">The function to selects first level elements and replaces it with different text.</param>
        /// <param name="formattedText">The result text.</param>
        /// <returns>A collection of all elements found.</returns>
        internal static List<Element> ParseAndFormat(
            string rawText,
            string startTag,
            string endTag,
            Func<Element, string> formatter,
            out string formattedText)
        {
            var segments = Regex.Split(rawText, $"({Regex.Escape(startTag)})|({Regex.Escape(endTag)})");
            List<Element> result = [];
            formattedText = "";
            Element? currentElement = null;
            string prevSegment = "";
            foreach (var segment in segments)
            {
                if (prevSegment == startTag)
                {
                    var name = GetName(segment, out string content);
                    if (currentElement == null)
                    {
                        currentElement = new()
                        {
                            Name = name,
                            Content = content,
                            Level = 1,
                        };
                    }
                    else
                    {
                        Element newElement = new()
                        {
                            Name = name,
                            Content = content,
                            Level = currentElement.Level + 1,
                            Parent = currentElement
                        };
                        currentElement.Children.Add(newElement);
                        currentElement = newElement;
                    }
                }
                else if (segment == endTag)
                {
                    if (currentElement != null)
                    {
                        if (currentElement.Level == 1)
                        {
                            result.Add(currentElement);
                            formattedText += formatter(currentElement);
                            currentElement = null;
                        }
                        else
                        {
                            currentElement = currentElement.Parent;
                        }
                    }
                }
                else
                {
                    if (segment != startTag)
                    {
                        if (currentElement == null)
                            formattedText += segment;
                        else
                            currentElement.Content += segment;
                    }
                }
                prevSegment = segment;
            }

            return result;
        }

        private static string GetName(string elementText, out string content)
        {
            string name = "";
            if (elementText.Contains('|'))
            {
                name = Regex.Match(elementText, @"^[^/|]*", RegexOptions.Singleline).Value.Trim();
                content = Regex.Match(elementText, @"(?:(\|).+)",RegexOptions.Singleline).Value;
            }
            else
            {
                content = elementText;
            }
            return name;
        }
    }
}
