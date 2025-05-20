using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

public class OnCDataTest
{
    [Theory]
    [InlineData("<![CDATA[]]>", "")]
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
    public void MatchOnCallback(string input, string expected)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnCDataCallback = (cdata, line, column) =>
                {
                    Assert.Equal(expected, cdata);
                }
            };
        SaxParser.Parse(input, handler);
    }
}
