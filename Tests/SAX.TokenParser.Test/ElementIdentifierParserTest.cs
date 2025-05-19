using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ElementIdentifierParserTest
{
    [Theory]
    [InlineData("<element", "element")]
    [InlineData("<el-ement", "el-ement")]
    [InlineData("<el_ement", "el_ement")]
    [InlineData("<el:ement", "el:ement")]
    [InlineData("<el:em_ent", "el:em_ent")]
    [InlineData("<el-em_ent", "el-em_ent")]
    [InlineData("<el:em-ent", "el:em-ent")]
    [InlineData("<el:em-en_t", "el:em-en_t")]
    [InlineData("<element1", "element1")]
    public void TestElementIdentifier(string input, string expected)
    {
        var result = XmlTokenParser.ElementIdentifierForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var chars = result.Value;
        Assert.NotEmpty(chars.ToStringValue());
        Assert.True(chars.EqualsValue(expected));
    }
}
