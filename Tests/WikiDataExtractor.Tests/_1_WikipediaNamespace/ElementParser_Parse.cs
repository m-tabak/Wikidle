using WikiDataExtractor.Tests.Util;
using WikiDataExtractor.Wikipedia;

namespace WikiDataExtractor.Tests.Wikipedia
{
    public class ElementParser_Parse
    {

        [Theory]
        [InlineData("{{", "}}", 20)]
        [InlineData("[[", "]]", 58)]
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

        [Fact]
        public void ShouldParse_ElementName()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");
           
            var elements = ElementParser.Parse(text, "{{", "}}");

            Assert.Equal("Infobox settlement", elements[1].Name);
        }

        [Fact]
        public void ShouldParse_ElementContent()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");
            var expected = @"| name                            = Warsaw
| native_name                     = &lt;small&gt;''Warszawa''&lt;/small&gt;
| native_name_lang                = pl
| settlement_type                 = [[City with powiat rights|Capital city and county]]
";

            var elements = ElementParser.Parse(text, "{{", "}}");

            Assert.Equal(expected, elements[1].Content);
        }
    }
}
