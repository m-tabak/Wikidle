using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiDataExtractor.Tests.Util;
using WikiDataExtractor.Wikipedia;

namespace WikiDataExtractor.Tests._1_WikipediaNamespace
{
    public class ElementParser_ParseAndFormat
    {
        [Fact]
        public void ShouldFormat_Templates()
        {
            var text = ResourceReader.ReadTextRes("wikitext2");
            var expected = @"

== Geography ==
Warsaw is near the middle of Poland on both sides of the Vistula [[river]], and about 350 [[kilometre|km]] (225 [[mile]]s) from the [[Baltic Sea]]. It is about 100 [[metre|m]] (325 [[Foot (unit of length)|ft]]) above [[sea level]]. Warsaw has a [[humid continental climate]] (''Dfb'' in the [[Koeppen climate classification]]).

Warsaw is home to four [[university|universities]] and 62 [[college]]s, and many [[theatre]]s and art galleries.
";

            var elements = ElementParser.ParseAndFormat(text, "{{", "}}",e=>"",out var actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldFormat_Links()
        {
            var text = ResourceReader.ReadTextRes("wikitext2");
            var expected = @"{{For|the name of Warsaw in various languages|wikt:Warsaw}}

== Geography ==
Warsaw is near the middle of Poland on both sides of the Vistula LINK, and about 350 LINK (225 LINKs) from the LINK. It is about 100 LINK (325 LINK) above LINK. Warsaw has a LINK (''Dfb'' in the LINK).

Warsaw is home to four LINK and 62 LINKs, and many LINKs and art galleries.
";

            var elements = ElementParser.ParseAndFormat(text, "[[", "]]", e => "LINK", out var actual);

            Assert.Equal(expected, actual);
        }
    }
}
