using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class QuotedStringParserTest
{
    [Theory]
    [InlineData("\"\"")]
    [InlineData("\"1234\"")]
    [InlineData("\"hogehoge\"")]
    [InlineData("\"{hogehoge}\"")]
    [InlineData("\"{hogehoge;}\"")]
    [InlineData("\"Supercalifragilisticexpialidocious\"")]
    [InlineData("\"Supercalifragilisticexpialidocious.\"")]
    [InlineData("\"The quick brown fox jumped over the lazy dog.\"")]
    public void TestQuotedStringWithQuotes(string input)
    {
        var result = XmlTokenParser.QuotedStringWithQuotes(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var chars = result.Value;
        Assert.NotEmpty(chars.ToStringValue());
        Assert.True(chars.EqualsValue(input));
    }

    [Theory]
    [InlineData("\"\"")]
    public void TestEmptyQuotedString(string input)
    {
        var result = XmlTokenParser.QuotedString(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var chars = result.Value;
        Assert.Empty(chars.ToStringValue());
        Assert.Equal(input.Trim('"'), chars.ToStringValue());
    }

    [Theory]
    [InlineData("\"1234\"")]
    [InlineData("\"hogehoge\"")]
    [InlineData("\"{hogehoge}\"")]
    [InlineData("\"{hogehoge;}\"")]
    [InlineData("\"Supercalifragilisticexpialidocious\"")]
    [InlineData("\"Supercalifragilisticexpialidocious.\"")]
    [InlineData("\"The quick brown fox jumped over the lazy dog.\"")]
    public void TestQuotedString(string input)
    {
        var result = XmlTokenParser.QuotedString(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var chars = result.Value;
        Assert.NotEmpty(chars.ToStringValue());
        Assert.Equal(input.Trim('"'), chars.ToStringValue());
    }
}
