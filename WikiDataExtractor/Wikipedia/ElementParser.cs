
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

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
        internal static List<Element> Parse(string rawText, string startTag, string endTag)
        {
            var segments = Regex.Split(rawText, $"({Regex.Escape(startTag)})|({Regex.Escape(endTag)})");

            List<Element> result = [];
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
                            currentElement = null;
                        }
                        else
                        {
                            currentElement = currentElement.Parent;
                        }
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
                name = Regex.Match(elementText, @"^.+(?=\|)").Value.Trim();
                content = Regex.Match(elementText, @"(?:(\|).+)").Value;
            }
            else
            {
                content = elementText;
            }
            return name;
        }
    }
}
