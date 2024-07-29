using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Tests.Util;
using WikiDataExtractor.Wikipedia;

namespace WikiDataExtractor.Tests.Wikipedia
{
    public class PageReader_ReadPage
    {

        [Fact]
        public void ShouldRead_AllInfoboxes()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var pageData = PageReader.ReadPage("Warsaw", text);

            Assert.NotNull(pageData.Infoboxes);
            Assert.Equal(3, pageData.Infoboxes.Count);
        }

        [Fact]
        public void ShouldRead_AllFilenames()
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var pageData = PageReader.ReadPage("Warsaw", text);

            Assert.NotNull(pageData.Files);
            Assert.Equal(10, pageData.Files.Count);
        }

        [Theory]
        [InlineData("{{")]
        [InlineData("}}")]
        [InlineData("[[")]
        [InlineData("]]")]
        public void ResultText_ShouldNotContain_Elements(string elementTag)
        {
            var text = ResourceReader.ReadTextRes("wikitext1");

            var pageData = PageReader.ReadPage("Warsaw", text);

            Assert.DoesNotContain(elementTag, pageData.Text);
        }
    }
}
