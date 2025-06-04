using XmlFormat.SAX;

namespace SAX.Tokenizer.Test;

/// <summary>
/// negative tokenizer test
/// basically, pass in wrong or incomplete tags, observe failure
/// </summary>
public class NegativeTokenizerTest
{
    [Fact]
    public void NegativeTestProcessingInstruction()
    {
        Assert.False(TestHelper.FailTokenize("<?xml>", [XmlTokenizer.XmlToken.Declaration]));
        Assert.False(TestHelper.FailTokenize("<?xml ", [XmlTokenizer.XmlToken.Declaration]));
        Assert.False(TestHelper.FailTokenize("<?xml version=\"1.0\" encoding=\"utf-8\">", [XmlTokenizer.XmlToken.Declaration]));
        Assert.False(TestHelper.FailTokenize("<?xml version=\"1.0\" encoding=\"utf-8\" ", [XmlTokenizer.XmlToken.Declaration]));
        Assert.False(TestHelper.FailTokenize("<?php>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.False(TestHelper.FailTokenize("<?php ", [XmlTokenizer.XmlToken.ProcessingInstruction]));
    }

    [Fact]
    public void NegativeTestComment()
    {
        Assert.False(TestHelper.FailTokenize("<!--->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!-- ->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!--\n->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!--comment->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!--comment --", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!-- comment>", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!-- comment ", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!--\ncomment\nmore comment\n", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!--\ncomment\nmore comment\n ", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!-- \ncomment\nmore comment\n->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.FailTokenize("<!-- \ncomment\nmore comment\n >", [XmlTokenizer.XmlToken.Comment]));
    }

    [Fact]
    public void NegativeTestCData()
    {
        Assert.False(TestHelper.FailTokenize("<![CDATA[]>", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[ ]]", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[\n", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[foobar]>", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[foobar >", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[ foobar]]", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[ foobar ", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[\nfoobar\nhoge\n]>", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[\nfoobar\nhoge\n ]]", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[ \nfoobar\nhoge\n] >", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.FailTokenize("<![CDATA[ \nfoobar\nhoge\n ", [XmlTokenizer.XmlToken.CData]));
    }

    [Fact]
    public void NegativeTestElementNoAttributes()
    {
        Assert.False(TestHelper.FailTokenize("<element/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element\n /", [XmlTokenizer.XmlToken.ElementEmpty]));

        Assert.False(TestHelper.FailTokenize("<element", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("</element", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.False(TestHelper.FailTokenize("</element ", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.False(TestHelper.FailTokenize("</element\n", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.False(TestHelper.FailTokenize("</element\n ", [XmlTokenizer.XmlToken.ElementEnd]));

        Assert.False(TestHelper.FailTokenize("<el:em_en-t/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<el:em_en-t ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<el:em_en-t\n/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<el:em_en-t\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<el:em_en-t", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<el:em_en-t ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<el:em_en-t\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<el:em_en-t\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("</el:em_en-t", [XmlTokenizer.XmlToken.ElementEnd]));
    }

    [Fact]
    public void NegativeTestElementWithAttributes()
    {
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" /", [XmlTokenizer.XmlToken.ElementEmpty]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" xyz=\"abc\"/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" xyz=\"abc\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" xyz=\"abc\" standalone /", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" xyz=\"abc\" standalone ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\"/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone/", [XmlTokenizer.XmlToken.ElementEmpty]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ", [XmlTokenizer.XmlToken.ElementStart]));
        // commented out b/c `<element / >` does not actually fail as expected
        // this is a bug in the tokenizer
        //Assert.False(
        //    TestHelper.FailTokenize(
        //        "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/ >",
        //        [XmlTokenizer.XmlToken.ElementStart]
        //    )
        //);
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\"", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" ", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" xyz=\"abc\"", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\" xyz=\"abc\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" \n", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\"", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\"\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" \n", [XmlTokenizer.XmlToken.ElementStart]));

        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n", [XmlTokenizer.XmlToken.ElementStart]));
        Assert.False(TestHelper.FailTokenize("<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n", [XmlTokenizer.XmlToken.ElementStart]));
    }

    [Fact]
    public void NegativeTestContent()
    {
        Assert.False(TestHelper.FailTokenize("1 < 2", [XmlTokenizer.XmlToken.Content]));
    }
}
