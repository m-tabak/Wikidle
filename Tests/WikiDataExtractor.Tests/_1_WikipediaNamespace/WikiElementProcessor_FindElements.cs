using WikiDataExtractor.Tests.Util;
using WikiDataExtractor.Wikipedia;

namespace WikiDataExtractor.Tests.Wikipedia
{
    public class WikiElementProcessor_FindElements
    {

        [Fact]
        public void ShouldParse_AllOuterElements()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var tokens = WikiElementParser.Tokenize(text);
            var elements = WikiElementParser.FindElements(tokens, "{{", "}}");

            Assert.Equal(20, elements.Count);
        }

        [Fact]
        public void ShouldParse_AllInnerElements()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var tokens = WikiElementParser.Tokenize(text);
            var elements = WikiElementParser.FindElements(tokens, "{{", "}}");

            Assert.Equal(9, elements[0].Children.Count);
        }
    }
}
