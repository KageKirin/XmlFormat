using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ElementStartParserTest
{
    [Theory]
    [InlineData("<element>", "element")]
    [InlineData("<el-ement>", "el-ement")]
    [InlineData("<el_ement>", "el_ement")]
    [InlineData("<el:ement>", "el:ement")]
    [InlineData("<el:em_ent>", "el:em_ent")]
    [InlineData("<el-em_ent>", "el-em_ent")]
    [InlineData("<el:em-ent>", "el:em-ent")]
    [InlineData("<el:em-en_t>", "el:em-en_t")]
    [InlineData("<element1>", "element1")]
    [InlineData("<element >", "element")]
    [InlineData("<el-ement >", "el-ement")]
    [InlineData("<el_ement >", "el_ement")]
    [InlineData("<el:ement >", "el:ement")]
    [InlineData("<el:em_ent >", "el:em_ent")]
    [InlineData("<el-em_ent >", "el-em_ent")]
    [InlineData("<el:em-ent >", "el:em-ent")]
    [InlineData("<el:em-en_t >", "el:em-en_t")]
    [InlineData("<element1 >", "element1")]
    [InlineData("<element\n >", "element")]
    [InlineData("<el-ement\n >", "el-ement")]
    [InlineData("<el_ement\n >", "el_ement")]
    [InlineData("<el:ement\n >", "el:ement")]
    [InlineData("<el:em_ent\n >", "el:em_ent")]
    [InlineData("<el-em_ent\n >", "el-em_ent")]
    [InlineData("<el:em-ent\n >", "el:em-ent")]
    [InlineData("<el:em-en_t\n >", "el:em-en_t")]
    [InlineData("<element1\n >", "element1")]
    public void TestElementStartNoAttributes(string input, string expected)
    {
        var result = XmlTokenParser.ElementStart(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expected, element.Identifier.ToStringValue());
        Assert.Empty(element.Attributes);
    }

    [Theory]
    [InlineData("<element attribute>", "element", "attribute")]
    [InlineData("<el-ement at-tribute>", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute>", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute>", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute>", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute>", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute>", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e>", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0>", "element1", "attribute0")]
    [InlineData("<element attribute >", "element", "attribute")]
    [InlineData("<el-ement at-tribute >", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute >", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute >", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute >", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute >", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute >", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e >", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0 >", "element1", "attribute0")]
    [InlineData("<element\nattribute \n>", "element", "attribute")]
    [InlineData("<el-ement\nat-tribute \n>", "el-ement", "at-tribute")]
    [InlineData("<el_ement\nat_tribute \n>", "el_ement", "at_tribute")]
    [InlineData("<el:ement\nat:tribute \n>", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent\nat:tri_bute \n>", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent\nat-tri_bute \n>", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent\nat:tri-bute \n>", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t\nat:tri-but_e \n>", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1\nattribute0 \n>", "element1", "attribute0")]
    public void TestElementStartWithAttributeNoValue(string input, string expectedElement, string expectedAttribute)
    {
        var result = XmlTokenParser.ElementStart(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Single(element.Attributes);
        var attribute = element.Attributes[0];
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedAttribute, attribute.Name.ToStringValue());
        Assert.Null(attribute.Value);
    }

    [Theory]
    [InlineData("<element attribute=\"\">", "element", "attribute")]
    [InlineData("<el-ement at-tribute=\"\">", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute=\"\">", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute=\"\">", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute=\"\">", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute=\"\">", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute=\"\">", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e=\"\">", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0=\"\">", "element1", "attribute0")]
    [InlineData("<element attribute=\"\" >", "element", "attribute")]
    [InlineData("<el-ement at-tribute=\"\" >", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute=\"\" >", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute=\"\" >", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute=\"\" >", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute=\"\" >", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute=\"\" >", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e=\"\" >", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0=\"\" >", "element1", "attribute0")]
    [InlineData("<element\nattribute=\"\"\n>", "element", "attribute")]
    [InlineData("<el-ement\nat-tribute=\"\"\n>", "el-ement", "at-tribute")]
    [InlineData("<el_ement\nat_tribute=\"\"\n>", "el_ement", "at_tribute")]
    [InlineData("<el:ement\nat:tribute=\"\"\n>", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent\nat:tri_bute=\"\"\n>", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent\nat-tri_bute=\"\"\n>", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent\nat:tri-bute=\"\"\n>", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t\nat:tri-but_e=\"\"\n>", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1\nattribute0=\"\"\n>", "element1", "attribute0")]
    public void TestElementAttributeEmptyValue(string input, string expectedElement, string expectedAttribute)
    {
        var result = XmlTokenParser.ElementStart(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Single(element.Attributes);
        var attribute = element.Attributes[0];
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedAttribute, attribute.Name.ToStringValue());
        Assert.NotNull(attribute.Value);
        Assert.Empty(((TextSpan)attribute.Value!).ToStringValue());
    }

    [Theory]
    [InlineData("<element attribute=\"hogehoge\">", "element", "attribute", "hogehoge")]
    [InlineData("<el-ement at-tribute=\"hogehoge\">", "el-ement", "at-tribute", "hogehoge")]
    [InlineData("<el_ement at_tribute=\"hogehoge\">", "el_ement", "at_tribute", "hogehoge")]
    [InlineData("<el:ement at:tribute=\"hogehoge\">", "el:ement", "at:tribute", "hogehoge")]
    [InlineData("<el:em_ent at:tri_bute=\"hogehoge\">", "el:em_ent", "at:tri_bute", "hogehoge")]
    [InlineData("<el-em_ent at-tri_bute=\"hogehoge\">", "el-em_ent", "at-tri_bute", "hogehoge")]
    [InlineData("<el:em-ent at:tri-bute=\"hogehoge\">", "el:em-ent", "at:tri-bute", "hogehoge")]
    [InlineData("<el:em-en_t at:tri-but_e=\"hogehoge\">", "el:em-en_t", "at:tri-but_e", "hogehoge")]
    [InlineData("<element1 attribute0=\"hogehoge\">", "element1", "attribute0", "hogehoge")]
    [InlineData("<element attribute=\"{hogehoge}\">", "element", "attribute", "{hogehoge}")]
    [InlineData("<el-ement at-tribute=\"{hogehoge}\">", "el-ement", "at-tribute", "{hogehoge}")]
    [InlineData("<el_ement at_tribute=\"{hogehoge}\">", "el_ement", "at_tribute", "{hogehoge}")]
    [InlineData("<el:ement at:tribute=\"{hogehoge}\">", "el:ement", "at:tribute", "{hogehoge}")]
    [InlineData("<el:em_ent at:tri_bute=\"{hogehoge}\">", "el:em_ent", "at:tri_bute", "{hogehoge}")]
    [InlineData("<el-em_ent at-tri_bute=\"{hogehoge}\">", "el-em_ent", "at-tri_bute", "{hogehoge}")]
    [InlineData("<el:em-ent at:tri-bute=\"{hogehoge}\">", "el:em-ent", "at:tri-bute", "{hogehoge}")]
    [InlineData("<el:em-en_t at:tri-but_e=\"{hogehoge}\">", "el:em-en_t", "at:tri-but_e", "{hogehoge}")]
    [InlineData("<element1 attribute0=\"{hogehoge}\">", "element1", "attribute0", "{hogehoge}")]
    public void TestElementAttribute(string input, string expectedElement, string expectedAttribute, string expectedValue)
    {
        var result = XmlTokenParser.ElementStart(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Single(element.Attributes);
        var attribute = element.Attributes[0];
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedAttribute, attribute.Name.ToStringValue());
        Assert.NotNull(attribute.Value);
        Assert.NotEmpty(((TextSpan)attribute.Value!).ToStringValue());
        Assert.Equal(expectedValue, ((TextSpan)attribute.Value!).ToStringValue());
    }
}
