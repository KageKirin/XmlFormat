using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ElementAttributeParserTest
{
    [Theory]
    [InlineData("attribute")]
    [InlineData("at-tribute")]
    [InlineData("at_tribute")]
    [InlineData("at:tribute")]
    [InlineData("at:tri_bute")]
    [InlineData("at-tri_bute")]
    [InlineData("at:tri-bute")]
    [InlineData("at:tri-but_e")]
    [InlineData("attribute0")]
    public void TestElementAttributeNoValue(string input)
    {
        var result = XmlTokenParser.ElementAttributeForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attribute = result.Value;
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(input, attribute.Name.ToStringValue());
        Assert.Null(attribute.Value);
    }

    [Theory]
    [InlineData("attribute=\"\"", "attribute")]
    [InlineData("at-tribute=\"\"", "at-tribute")]
    [InlineData("at_tribute=\"\"", "at_tribute")]
    [InlineData("at:tribute=\"\"", "at:tribute")]
    [InlineData("at:tri_bute=\"\"", "at:tri_bute")]
    [InlineData("at-tri_bute=\"\"", "at-tri_bute")]
    [InlineData("at:tri-bute=\"\"", "at:tri-bute")]
    [InlineData("at:tri-but_e=\"\"", "at:tri-but_e")]
    [InlineData("attribute0=\"\"", "attribute0")]
    public void TestElementAttributeEmptyValue(string input, string expected)
    {
        var result = XmlTokenParser.ElementAttributeForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attribute = result.Value;
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expected, attribute.Name.ToStringValue());

        Assert.NotNull(attribute.Value);
        TextSpan attribValue = (TextSpan)attribute.Value!;
        Assert.Empty(attribValue.ToStringValue());
    }

    [Theory]
    [InlineData("attribute=\"hogehoge\"", "attribute", "hogehoge")]
    [InlineData("at-tribute=\"hogehoge\"", "at-tribute", "hogehoge")]
    [InlineData("at_tribute=\"hogehoge\"", "at_tribute", "hogehoge")]
    [InlineData("at:tribute=\"hogehoge\"", "at:tribute", "hogehoge")]
    [InlineData("at:tri_bute=\"hogehoge\"", "at:tri_bute", "hogehoge")]
    [InlineData("at-tri_bute=\"hogehoge\"", "at-tri_bute", "hogehoge")]
    [InlineData("at:tri-bute=\"hogehoge\"", "at:tri-bute", "hogehoge")]
    [InlineData("at:tri-but_e=\"hogehoge\"", "at:tri-but_e", "hogehoge")]
    [InlineData("attribute0=\"hogehoge\"", "attribute0", "hogehoge")]
    [InlineData("attribute=\"{hogehoge}\"", "attribute", "{hogehoge}")]
    [InlineData("at-tribute=\"{hogehoge}\"", "at-tribute", "{hogehoge}")]
    [InlineData("at_tribute=\"{hogehoge}\"", "at_tribute", "{hogehoge}")]
    [InlineData("at:tribute=\"{hogehoge}\"", "at:tribute", "{hogehoge}")]
    [InlineData("at:tri_bute=\"{hogehoge}\"", "at:tri_bute", "{hogehoge}")]
    [InlineData("at-tri_bute=\"{hogehoge}\"", "at-tri_bute", "{hogehoge}")]
    [InlineData("at:tri-bute=\"{hogehoge}\"", "at:tri-bute", "{hogehoge}")]
    [InlineData("at:tri-but_e=\"{hogehoge}\"", "at:tri-but_e", "{hogehoge}")]
    [InlineData("attribute0=\"{hogehoge}\"", "attribute0", "{hogehoge}")]
    public void TestElementAttribute(string input, string expectedName, string expectedValue)
    {
        var result = XmlTokenParser.ElementAttributeForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attribute = result.Value;
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedName, attribute.Name.ToStringValue());

        Assert.NotNull(attribute.Value);
        TextSpan attribValue = (TextSpan)attribute.Value!;
        Assert.NotEmpty(attribValue.ToStringValue());
        Assert.Equal(expectedValue, attribValue.ToStringValue());
    }
}
