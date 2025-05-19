using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class CommentParserTest
{
    [Theory]
    [InlineData("<!---->")]
    public void TestEmptyComment(string input)
    {
        var result = XmlTokenParser.TrimComment(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var comment = result.Value;
        Assert.Empty(comment.ToStringValue());
    }

    [Theory]
    [InlineData("<!-- -->", " ")]
    [InlineData("<!--\n-->", "\n")]
    [InlineData("<!--comment-->", "comment")]
    [InlineData("<!--comment -->", "comment ")]
    [InlineData("<!-- comment-->", " comment")]
    [InlineData("<!-- comment -->", " comment ")]
    [InlineData("<!--\ncomment\nmore comment\n-->", "\ncomment\nmore comment\n")]
    [InlineData("<!--\ncomment\nmore comment\n -->", "\ncomment\nmore comment\n ")]
    [InlineData("<!-- \ncomment\nmore comment\n-->", " \ncomment\nmore comment\n")]
    [InlineData("<!-- \ncomment\nmore comment\n -->", " \ncomment\nmore comment\n ")]
    public void TestComment(string input, string expected)
    {
        var result = XmlTokenParser.TrimComment(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var comment = result.Value;
        Assert.NotEmpty(comment.ToStringValue());
        Assert.True(comment.EqualsValue(expected));
    }
}
