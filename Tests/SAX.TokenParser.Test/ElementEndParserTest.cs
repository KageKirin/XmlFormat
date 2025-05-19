using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ElementEndParserTest
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
    public void TestElementEnd(string input, string expected)
    {
        var result = XmlTokenParser.ElementEnd(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.ToStringValue());
        Assert.Equal(expected, element.ToStringValue());
    }
}
