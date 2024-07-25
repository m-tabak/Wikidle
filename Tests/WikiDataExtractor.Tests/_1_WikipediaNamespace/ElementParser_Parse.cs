using WikiDataExtractor.Tests.Util;
using WikiDataExtractor.Wikipedia;

namespace WikiDataExtractor.Tests.Wikipedia
{
    public class ElementParser_Parse
    {

        [Theory]
        [InlineData("{{","}}",20)]
        [InlineData("[[","]]",58)]
        public void ShouldParse_AllOuterElements(string startTag, string endTag, int count)
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var elements = ElementParser.Parse(text, startTag, endTag);

            Assert.Equal(count, elements.Count);
        }

        [Fact]
        public void ShouldParse_AllInnerElements()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var elements = ElementParser.Parse(text, "{{", "}}");

            Assert.Equal(9, elements[0].Children.Count);
        }
    }
}
