using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WikiDataExtractor.Wikipedia;

namespace WikiDataExtractor.Services
{
    internal class XmlParser
    {
        /// <summary>
        /// Iterate through text nodes in an XML file.
        /// </summary>
        /// <param name="stream">A stream containing the XML text.</param>
        /// <returns>The node's name and value respectively.</returns>
        internal async IAsyncEnumerable<(string, string)> ReadTextNodesAsync(Stream stream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                while (await reader.ReadAsync())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        yield return (reader.Name, reader.Value);
                    }
                }
            }
        }
    }
}
