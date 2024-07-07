using WikiDataExtractor.Data;
using WikiDataExtractor.Tests.Util;

namespace WikiDataExtractor.Tests
{
    public class XmlParser_ReadTextNodesAsync
    {
        [Fact]
        public async Task WhenReadXmlStream_IterateThroughAllTextNodes()
        {
            var xmlStream = ResourceReader.ReadTextResAsStream("xml_text_nodes");
            string result = "";

            await foreach (var nodeTuple in XmlParser.ReadTextNodesAsync(xmlStream))
            {
                result += nodeTuple.Item2;
            }

            Assert.Equal("123", result);
        }
    }
}
