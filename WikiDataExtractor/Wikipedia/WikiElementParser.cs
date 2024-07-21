
using System.Text.RegularExpressions;

namespace WikiDataExtractor.Wikipedia
{
    internal class WikiElementParser
    {
        static readonly char[] Delimiters = [' ', '\'', ';', '&', '|', '\n'];
        internal static string[] Tokenize(string text)
        {
            var regexPattern = "";
            foreach (char ch in Delimiters)
            {
                regexPattern += ch;
            }
            regexPattern = $"(?=[{Regex.Escape(regexPattern)}])";
            return Regex.Split(text, regexPattern)
                .Where(s => s != " ")
                .ToArray();
        }

        /// <summary>
        /// Parse tokens for elements between two tags or characters. 
        /// A tag can not be recognized if the token containing it has non-delimiter characters surrounding it from both sides.
        /// </summary>
        /// <param name="tokens">The array of tokens to process.</param>
        /// <param name="startPattern">Start tag.</param>
        /// <param name="endPattern">End tag.</param>
        /// <returns>A collection of all elements found.</returns>
        internal static List<Element> FindElements(string[] tokens, string startPattern, string endPattern)
        {
            var startChars = startPattern.Distinct().ToArray();
            var endChars = endPattern.Distinct().ToArray();
            var charsToTrim = Delimiters.Concat(startChars).Concat(endChars).ToArray();

            var resultElements = new List<Element>();
            Element? currentElement = null;
            for (var i = 0; i < tokens.Length; i++)
            {
                if (PatternMatch(tokens[i], startPattern))
                {
                    if (currentElement == null)
                    {
                        currentElement = new()
                        {
                            StartTokenIndex = i,
                            Name = tokens[i].Trim(charsToTrim),
                            Level = 1,
                        };
                    }
                    else
                    {
                        Element newElement = new()
                        {
                            StartTokenIndex = i,
                            Name = tokens[i].Trim(charsToTrim),
                            Level = currentElement.Level + 1,
                            Parent = currentElement
                        };
                        currentElement.Children.Add(newElement);
                        currentElement = newElement;
                    }
                }

                //Add token text
                if (currentElement != null)
                {
                    currentElement.Text += tokens[i];
                }

                if (PatternMatch(tokens[i], endPattern))
                {
                    if (currentElement != null)
                    {
                        currentElement.Text = currentElement.Text.Trim(charsToTrim);
                        currentElement.EndTokenIndex = i;

                        if (currentElement.Level == 1)
                        {
                            resultElements.Add(currentElement);
                            currentElement = null;
                        }
                        else
                        {
                            currentElement = currentElement.Parent;
                        }
                    }
                }
            }
            return resultElements;
        }


        private static bool PatternMatch(string token, string patern)
        {
            token = token.Trim(Delimiters);
            return (token.StartsWith(patern) || token.EndsWith(patern));
        }
    }
}
