using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class CDataParserTest
{
    [Theory]
    [InlineData("<![CDATA[]]>")]
    public void TestEmptyCData(string input)
    {
        var result = XmlTokenParser.TrimCData(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var cdata = result.Value;
        Assert.Empty(cdata.ToStringValue());
    }

    [Theory]
    [InlineData("<![CDATA[ ]]>", " ")]
    [InlineData("<![CDATA[\n]]>", "\n")]
    [InlineData("<![CDATA[foobar]]>", "foobar")]
    [InlineData("<![CDATA[foobar ]]>", "foobar ")]
    [InlineData("<![CDATA[ foobar]]>", " foobar")]
    [InlineData("<![CDATA[ foobar ]]>", " foobar ")]
    [InlineData("<![CDATA[\nfoobar\nhoge\n]]>", "\nfoobar\nhoge\n")]
    [InlineData("<![CDATA[\nfoobar\nhoge\n ]]>", "\nfoobar\nhoge\n ")]
    [InlineData("<![CDATA[ \nfoobar\nhoge\n]]>", " \nfoobar\nhoge\n")]
    [InlineData("<![CDATA[ \nfoobar\nhoge\n ]]>", " \nfoobar\nhoge\n ")]
    public void TestCData(string input, string expected)
    {
        var result = XmlTokenParser.TrimCData(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var cdata = result.Value;
        Assert.NotEmpty(cdata.ToStringValue());
        Assert.True(cdata.EqualsValue(expected));
    }
}
