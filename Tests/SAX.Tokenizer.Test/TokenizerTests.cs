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
    [InlineData("<!-- <comment>\n<more comment>\n -->", XmlTokenizer.XmlToken.Comment)]
    [InlineData("<!-- <comment/>\n<more comment/>\n -->", XmlTokenizer.XmlToken.Comment)]
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
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone/>", XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<element foobar=\"hogehoge\"   xyz=\"abc\" standalone />", XmlTokenizer.XmlToken.ElementEmpty)]
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
    [InlineData(
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>",
        XmlTokenizer.XmlToken.ElementStart
    )]
    [InlineData(
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n/>",
        XmlTokenizer.XmlToken.ElementEmpty
    )]
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

    [Theory]
    [InlineData("<aaa/><bbb/>", XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<aaa><bbb/>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<aaa><bbb>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.ElementStart)]
    [InlineData("<aaa></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa>Hello World", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.Content)]
    [InlineData("<aaa><!-- Hello World -->", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.Comment)]
    [InlineData("<aaa><![CDATA[ Hello World ]]>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.CData)]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  ",
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.ElementStart
    )]
    public void Test2Elements(string input, XmlTokenizer.XmlToken expectedToken1, XmlTokenizer.XmlToken expectedToken2)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken1, expectedToken2]));
    }

    [Theory]
    [InlineData("<aaa/><bbb/><ccc/>", XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<aaa><bbb/></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa>hello world</aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.Content, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa><!-- hello world --></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.Comment, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa><![CDATA[ hello world ]]></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.CData, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  ",
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Comment
    )]
    public void Test3Elements(string input, XmlTokenizer.XmlToken expectedToken1, XmlTokenizer.XmlToken expectedToken2, XmlTokenizer.XmlToken expectedToken3)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken1, expectedToken2, expectedToken3]));
    }

    [Theory]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel>",
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.ElementStart
    )]
    public void Test4Elements(string input, XmlTokenizer.XmlToken expectedToken1, XmlTokenizer.XmlToken expectedToken2, XmlTokenizer.XmlToken expectedToken3, XmlTokenizer.XmlToken expectedToken4)
    {
        Assert.True(TestHelper.Tokenize(input, [expectedToken1, expectedToken2, expectedToken3, expectedToken4]));
    }
}
