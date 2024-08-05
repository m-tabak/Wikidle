using WikiDataExtractor.Services;
using WikiDataExtractor.Tests.Util;

namespace WikiDataExtractor.Tests.Services
{
    public class XmlParser_ReadTextNodesAsync
    {
        [Fact]
        public async Task WhenReadXmlStream_IterateThroughAllTextNodes()
        {
            var xmlParser = new XmlParser();
            var xmlStream = ResourceReader.ReadTextResAsStream("xml_text_nodes");
            string result = "";

            await foreach (var nodeTuple in xmlParser.ReadTextNodesAsync(xmlStream))
            {
                result += nodeTuple.Item2;
            }

            Assert.Equal("123", result);
        }
    }
}
