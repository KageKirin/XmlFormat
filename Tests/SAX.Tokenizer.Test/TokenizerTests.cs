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
    [InlineData("The quick brown fox jumped over the lazy dog.", XmlTokenizer.XmlToken.Content)]
    public void TestSingleToken(string input, XmlTokenizer.XmlToken expectedToken)
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
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Content
    )]
    [InlineData("<aaa/><bbb/><ccc/>", XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEmpty)]
    [InlineData("<aaa><bbb/></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.ElementEmpty, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa>hello world</aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.Content, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa><!-- hello world --></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.Comment, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData("<aaa><![CDATA[ hello world ]]></aaa>", XmlTokenizer.XmlToken.ElementStart, XmlTokenizer.XmlToken.CData, XmlTokenizer.XmlToken.ElementEnd)]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  ",
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content
    )]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel>",
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart
    )]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel><DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />\n  ",
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.ElementEmpty,
        XmlTokenizer.XmlToken.Content
    )]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel><DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />\n  </StackPanel>\n",
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.Comment,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementStart,
        XmlTokenizer.XmlToken.ElementEmpty,
        XmlTokenizer.XmlToken.Content,
        XmlTokenizer.XmlToken.ElementEnd,
        XmlTokenizer.XmlToken.Content
    )]
    public void TestManyTokens(string input, params XmlTokenizer.XmlToken[] tokens)
    {
        Assert.True(TestHelper.Tokenize(input, tokens));
    }
}
