using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

public class OnElementEndTest
{
    [Theory]
    [InlineData("</element>", "element")]
    [InlineData("</el-ement>", "el-ement")]
    [InlineData("</el_ement>", "el_ement")]
    [InlineData("</el:ement>", "el:ement")]
    [InlineData("</el:em_ent>", "el:em_ent")]
    [InlineData("</el-em_ent>", "el-em_ent")]
    [InlineData("</el:em-ent>", "el:em-ent")]
    [InlineData("</el:em-en_t>", "el:em-en_t")]
    [InlineData("</element1>", "element1")]
    [InlineData("</element >", "element")]
    [InlineData("</el-ement >", "el-ement")]
    [InlineData("</el_ement >", "el_ement")]
    [InlineData("</el:ement >", "el:ement")]
    [InlineData("</el:em_ent >", "el:em_ent")]
    [InlineData("</el-em_ent >", "el-em_ent")]
    [InlineData("</el:em-ent >", "el:em-ent")]
    [InlineData("</el:em-en_t >", "el:em-en_t")]
    [InlineData("</element1 >", "element1")]
    [InlineData("</element\n >", "element")]
    [InlineData("</el-ement\n >", "el-ement")]
    [InlineData("</el_ement\n >", "el_ement")]
    [InlineData("</el:ement\n >", "el:ement")]
    [InlineData("</el:em_ent\n >", "el:em_ent")]
    [InlineData("</el-em_ent\n >", "el-em_ent")]
    [InlineData("</el:em-ent\n >", "el:em-ent")]
    [InlineData("</el:em-en_t\n >", "el:em-en_t")]
    [InlineData("</element1\n >", "element1")]
    public void MatchOnCallback(string input, string expected)
    {
        DelegateXMLEventHandler handler = new()
        {
            OnElementEndCallback = (element, line, column) =>
            {
                Assert.Equal(expected, element);
            },
        };
        SaxParser.Parse(input, handler);
    }
}
