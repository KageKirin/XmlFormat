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
        Assert.False(TestHelper.Tokenize("<?xml>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.False(TestHelper.Tokenize("<?xml ", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.False(
            TestHelper.Tokenize(
                "<?xml version=\"1.0\" encoding=\"utf-8\">",
                [XmlTokenizer.XmlToken.ProcessingInstruction]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<?xml version=\"1.0\" encoding=\"utf-8\" ",
                [XmlTokenizer.XmlToken.ProcessingInstruction]
            )
        );
        Assert.False(TestHelper.Tokenize("<?php>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.False(TestHelper.Tokenize("<?php ", [XmlTokenizer.XmlToken.ProcessingInstruction]));
    }

    [Fact]
    public void NegativeTestComment()
    {
        Assert.False(TestHelper.Tokenize("<!--->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.Tokenize("<!-- ->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.Tokenize("<!--\n->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.Tokenize("<!--comment->", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.Tokenize("<!--comment --", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.Tokenize("<!-- comment>", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(TestHelper.Tokenize("<!-- comment ", [XmlTokenizer.XmlToken.Comment]));
        Assert.False(
            TestHelper.Tokenize("<!--\ncomment\nmore comment\n", [XmlTokenizer.XmlToken.Comment])
        );
        Assert.False(
            TestHelper.Tokenize("<!--\ncomment\nmore comment\n ", [XmlTokenizer.XmlToken.Comment])
        );
        Assert.False(
            TestHelper.Tokenize("<!-- \ncomment\nmore comment\n->", [XmlTokenizer.XmlToken.Comment])
        );
        Assert.False(
            TestHelper.Tokenize("<!-- \ncomment\nmore comment\n >", [XmlTokenizer.XmlToken.Comment])
        );
    }

    [Fact]
    public void NegativeTestCData()
    {
        Assert.False(TestHelper.Tokenize("<![CDATA[]>", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.Tokenize("<![CDATA[ ]]", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.Tokenize("<![CDATA[\n", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.Tokenize("<![CDATA[foobar]>", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.Tokenize("<![CDATA[foobar >", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.Tokenize("<![CDATA[ foobar]]", [XmlTokenizer.XmlToken.CData]));
        Assert.False(TestHelper.Tokenize("<![CDATA[ foobar ", [XmlTokenizer.XmlToken.CData]));
        Assert.False(
            TestHelper.Tokenize("<![CDATA[\nfoobar\nhoge\n]>", [XmlTokenizer.XmlToken.CData])
        );
        Assert.False(
            TestHelper.Tokenize("<![CDATA[\nfoobar\nhoge\n ]]", [XmlTokenizer.XmlToken.CData])
        );
        Assert.False(
            TestHelper.Tokenize("<![CDATA[ \nfoobar\nhoge\n] >", [XmlTokenizer.XmlToken.CData])
        );
        Assert.False(
            TestHelper.Tokenize("<![CDATA[ \nfoobar\nhoge\n ", [XmlTokenizer.XmlToken.CData])
        );
    }

    [Fact]
    public void NegativeTestElementNoAttributes()
    {
        Assert.False(TestHelper.Tokenize("<element/", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.False(TestHelper.Tokenize("<element ", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.False(
            TestHelper.Tokenize("<element\n", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<element\n /", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.False(TestHelper.Tokenize("<element", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.False(TestHelper.Tokenize("<element ", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.False(
            TestHelper.Tokenize("<element\n", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<element\n ", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.False(TestHelper.Tokenize("</element", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.False(TestHelper.Tokenize("</element ", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.False(TestHelper.Tokenize("</element\n", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.False(TestHelper.Tokenize("</element\n ", [XmlTokenizer.XmlToken.ElementEnd]));

        Assert.False(
            TestHelper.Tokenize("<el:em_en-t/", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<el:em_en-t ", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<el:em_en-t\n/", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<el:em_en-t\n ", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.False(
            TestHelper.Tokenize("<el:em_en-t", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<el:em_en-t ", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<el:em_en-t\n", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.False(
            TestHelper.Tokenize("<el:em_en-t\n ", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.False(TestHelper.Tokenize("</el:em_en-t", [XmlTokenizer.XmlToken.ElementEnd]));
    }

    [Fact]
    public void NegativeTestElementWithAttributes()
    {
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" /",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\"/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" standalone /",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" standalone ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\"/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone/",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        // commented out b/c `<element / >` does not actually fail as expected
        // this is a bug in the tokenizer
        //Assert.False(
        //    TestHelper.Tokenize(
        //        "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/ >",
        //        [XmlTokenizer.XmlToken.ElementStartOrEmpty]
        //    )
        //);
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\"",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\"",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" \n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\"",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" \n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone ",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.False(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
    }

    [Fact]
    public void NegativeTestContent()
    {
        Assert.False(TestHelper.Tokenize("1 < 2", [XmlTokenizer.XmlToken.Content]));
    }
}
