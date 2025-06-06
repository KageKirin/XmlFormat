using XmlFormat.SAX;

namespace SAX.Tokenizer.Negative.Test;

/// <summary>
/// negative tokenizer test
/// basically, pass in wrong or incomplete tags, observe failure
/// </summary>
public class NegativeTokenizerTest
{
    [Theory]
    [InlineData("<?xml>", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?xml ", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?xml version=\"1.0\" encoding=\"utf-8\">", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?xml version=\"1.0\" encoding=\"utf-8\" ", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?php>", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?php ", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<!--->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- ->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--\n->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--comment->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--comment --", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- comment>", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- comment ", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--\ncomment\nmore comment\n", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--\ncomment\nmore comment\n ", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- \ncomment\nmore comment\n->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- \ncomment\nmore comment\n >", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<![CDATA[]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ ]]", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[\n", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[foobar]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[foobar >", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ foobar]]", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ foobar ", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[\nfoobar\nhoge\n]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[\nfoobar\nhoge\n ]]", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ \nfoobar\nhoge\n] >", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ \nfoobar\nhoge\n ", XmlTokenizer.XmlToken.CData)]
    [InlineData("<element/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element\n", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element\n /", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element\n ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("</element", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("</element ", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("</element\n", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("</element\n ", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<el:em_en-t/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<el:em_en-t ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<el:em_en-t\n/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<el:em_en-t\n ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<el:em_en-t", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<el:em_en-t\n", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("</el:em_en-t", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<element foobar=\"hogehoge\"/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" /", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\"/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" standalone /", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" standalone ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\"/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone/", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\"", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\"", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" \n", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\"", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("1 < 2", XmlTokenizer.XmlToken.Content)]
    public void NegativeTest(string input, XmlTokenizer.XmlToken expectedType)
    {
        Assert.False(TestHelper.FailTokenize(input, [expectedType]));
    }
}
