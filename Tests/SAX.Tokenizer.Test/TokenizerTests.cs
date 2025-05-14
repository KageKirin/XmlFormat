using XmlFormat.SAX;

namespace SAX.Tokenizer.Test;

public class TokenizerTest
{
    [Theory]
    [InlineData("<?xml?>", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?xml ?>", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?xml version=\"1.0\" encoding=\"utf-8\"?>", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", XmlTokenizer.XmlToken.Declaration)]
    [InlineData("<?php?>", XmlTokenizer.XmlToken.ProcessingInstruction)]
    [InlineData("<?php ?>", XmlTokenizer.XmlToken.ProcessingInstruction)]
    [InlineData(@"<?php print(""hello world"");?>", XmlTokenizer.XmlToken.ProcessingInstruction)]
    [InlineData(@"<?php print(""hello world""); ?>", XmlTokenizer.XmlToken.ProcessingInstruction)]
    public void TestProcessingInstruction(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken]));
    }

    [Theory]
    [InlineData("<!---->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- -->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--\n-->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--comment-->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--comment -->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- comment-->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- comment -->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--\ncomment\nmore comment\n-->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!--\ncomment\nmore comment\n -->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- \ncomment\nmore comment\n-->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- \ncomment\nmore comment\n -->", XmlTokenizer.XmlToken.Comment)]
    public void TestComment(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken]));
    }

    [Theory]
    [InlineData("<![CDATA[]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ ]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[\n]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[foobar]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[foobar ]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ foobar]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ foobar ]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[\nfoobar\nhoge\n]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[\nfoobar\nhoge\n ]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ \nfoobar\nhoge\n]]>", XmlTokenizer.XmlToken.CData)]
    [InlineData("<![CDATA[ \nfoobar\nhoge\n ]]>", XmlTokenizer.XmlToken.CData)]
    public void TestCData(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken]));
    }

    [Theory]
    [InlineData("<element/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element\n/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element\n />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element\n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element\n >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("</element>", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("</element >", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("</element\n>", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("</element\n >", XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<el:em_en-t/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<el:em_en-t />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<el:em_en-t\n/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<el:em_en-t\n />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<el:em_en-t>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<el:em_en-t >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<el:em_en-t\n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<el:em_en-t\n >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("</el:em_en-t>", XmlTokenizer.XmlToken.ElementEnd)]
    public void TestElementNoAttributes(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken]));
    }

    [Theory]
    [InlineData("<element foobar=\"hogehoge\"/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\"/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\"/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" standalone/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" standalone />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\"/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n />", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\">", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\" >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\">", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\">", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\" xyz=\"abc\" >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" \n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\">", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" \n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone >", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n>", XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n>", XmlTokenizer.XmlToken.ElementStart)]
    public void TestElementWithAttributes(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken]));
    }

    [Theory]
    [InlineData("The quick brown fox jumped over the lazy dog.", XmlTokenizer.XmlToken.Content)]
    public void TestContent(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken]));
    }
}
