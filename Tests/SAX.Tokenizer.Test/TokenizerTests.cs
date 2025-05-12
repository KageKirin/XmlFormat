using XmlFormat.SAX;

namespace SAX.Tokenizer.Test;

public class TokenizerTest
{
    [Fact]
    public void TestProcessingInstruction()
    {
        Assert.True(TestHelper.Tokenize("<?xml?>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.True(TestHelper.Tokenize("<?xml ?>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.True(
            TestHelper.Tokenize(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                [XmlTokenizer.XmlToken.ProcessingInstruction]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
                [XmlTokenizer.XmlToken.ProcessingInstruction]
            )
        );
        Assert.True(TestHelper.Tokenize("<?php?>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
        Assert.True(TestHelper.Tokenize("<?php ?>", [XmlTokenizer.XmlToken.ProcessingInstruction]));
    }

    [Fact]
    public void TestComment()
    {
        Assert.True(TestHelper.Tokenize("<!---->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(TestHelper.Tokenize("<!-- -->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(TestHelper.Tokenize("<!--\n-->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(TestHelper.Tokenize("<!--comment-->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(TestHelper.Tokenize("<!--comment -->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(TestHelper.Tokenize("<!-- comment-->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(TestHelper.Tokenize("<!-- comment -->", [XmlTokenizer.XmlToken.Comment]));
        Assert.True(
            TestHelper.Tokenize("<!--\ncomment\nmore comment\n-->", [XmlTokenizer.XmlToken.Comment])
        );
        Assert.True(
            TestHelper.Tokenize(
                "<!--\ncomment\nmore comment\n -->",
                [XmlTokenizer.XmlToken.Comment]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<!-- \ncomment\nmore comment\n-->",
                [XmlTokenizer.XmlToken.Comment]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<!-- \ncomment\nmore comment\n -->",
                [XmlTokenizer.XmlToken.Comment]
            )
        );
    }

    [Fact]
    public void TestCData()
    {
        Assert.True(TestHelper.Tokenize("<![CDATA[]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(TestHelper.Tokenize("<![CDATA[ ]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(TestHelper.Tokenize("<![CDATA[\n]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(TestHelper.Tokenize("<![CDATA[foobar]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(TestHelper.Tokenize("<![CDATA[foobar ]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(TestHelper.Tokenize("<![CDATA[ foobar]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(TestHelper.Tokenize("<![CDATA[ foobar ]]>", [XmlTokenizer.XmlToken.CData]));
        Assert.True(
            TestHelper.Tokenize("<![CDATA[\nfoobar\nhoge\n]]>", [XmlTokenizer.XmlToken.CData])
        );
        Assert.True(
            TestHelper.Tokenize("<![CDATA[\nfoobar\nhoge\n ]]>", [XmlTokenizer.XmlToken.CData])
        );
        Assert.True(
            TestHelper.Tokenize("<![CDATA[ \nfoobar\nhoge\n]]>", [XmlTokenizer.XmlToken.CData])
        );
        Assert.True(
            TestHelper.Tokenize("<![CDATA[ \nfoobar\nhoge\n ]]>", [XmlTokenizer.XmlToken.CData])
        );
    }

    [Fact]
    public void TestElementNoAttributes()
    {
        Assert.True(TestHelper.Tokenize("<element/>", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.True(
            TestHelper.Tokenize("<element />", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<element\n/>", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<element\n />", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.True(TestHelper.Tokenize("<element>", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.True(TestHelper.Tokenize("<element >", [XmlTokenizer.XmlToken.ElementStartOrEmpty]));
        Assert.True(
            TestHelper.Tokenize("<element\n>", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<element\n >", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.True(TestHelper.Tokenize("</element>", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.True(TestHelper.Tokenize("</element >", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.True(TestHelper.Tokenize("</element\n>", [XmlTokenizer.XmlToken.ElementEnd]));
        Assert.True(TestHelper.Tokenize("</element\n >", [XmlTokenizer.XmlToken.ElementEnd]));

        Assert.True(
            TestHelper.Tokenize("<el:em_en-t/>", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<el:em_en-t />", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<el:em_en-t\n/>", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<el:em_en-t\n />", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.True(
            TestHelper.Tokenize("<el:em_en-t>", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<el:em_en-t >", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<el:em_en-t\n>", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );
        Assert.True(
            TestHelper.Tokenize("<el:em_en-t\n >", [XmlTokenizer.XmlToken.ElementStartOrEmpty])
        );

        Assert.True(TestHelper.Tokenize("</el:em_en-t>", [XmlTokenizer.XmlToken.ElementEnd]));
    }

    [Fact]
    public void TestElementWithAttributes()
    {
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\"/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\"/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" standalone/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" standalone />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\"/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n/>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n />",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\">",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" >",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\">",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" >",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\">",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\" xyz=\"abc\" >",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" \n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone >",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\">",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" >",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\"\n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" \n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );

        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"hogehoge\"   xyz=\"abc\" standalone >",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone\n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
        Assert.True(
            TestHelper.Tokenize(
                "<element foobar=\"{hogehoge}\" xyz=\"abc\" standalone \n>",
                [XmlTokenizer.XmlToken.ElementStartOrEmpty]
            )
        );
    }

    [Fact]
    public void TestContent()
    {
        Assert.True(
            TestHelper.Tokenize(
                "The quick brown fox jumped over the lazy dog.",
                [XmlTokenizer.XmlToken.Content]
            )
        );
    }
}
