using XmlFormat.SAX;

namespace SAX.Tokenizer.Test;

public class TokenizerValueTest
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
    public void TestSingleTokenContent(string input, XmlTokenizer.XmlToken expectedToken)
    {
        Assert.True(TestHelper.Tokenize(input, [new TestHelper.TokenTypeAndValue(expectedToken, input.Trim())]));
    }

    [Theory]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  ",
        XmlTokenizer.XmlToken.Comment,
        "<!-- main window -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>"
    )]
    public void Test2Elements(string input, XmlTokenizer.XmlToken expectedToken1, string expectedValue1, XmlTokenizer.XmlToken expectedToken2, string expectedValue2)
    {
        Assert.True(TestHelper.Tokenize(input, [new TestHelper.TokenTypeAndValue(expectedToken1, expectedValue1), new TestHelper.TokenTypeAndValue(expectedToken2, expectedValue2)]));
    }

    [Theory]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  ",
        XmlTokenizer.XmlToken.Comment,
        "<!-- main window -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>",
        XmlTokenizer.XmlToken.Comment,
        "<!-- ABC -->"
    )]
    public void Test3Elements(string input, XmlTokenizer.XmlToken expectedToken1, string expectedValue1, XmlTokenizer.XmlToken expectedToken2, string expectedValue2, XmlTokenizer.XmlToken expectedToken3, string expectedValue3)
    {
        Assert.True(TestHelper.Tokenize(input, [new TestHelper.TokenTypeAndValue(expectedToken1, expectedValue1), new TestHelper.TokenTypeAndValue(expectedToken2, expectedValue2), new TestHelper.TokenTypeAndValue(expectedToken3, expectedValue3)]));
    }

    [Theory]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel>",
        XmlTokenizer.XmlToken.Comment,
        "<!-- main window -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>",
        XmlTokenizer.XmlToken.Comment,
        "<!-- ABC -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<StackPanel>"
    )]
    public void Test4Elements(string input, XmlTokenizer.XmlToken expectedToken1, string expectedValue1, XmlTokenizer.XmlToken expectedToken2, string expectedValue2, XmlTokenizer.XmlToken expectedToken3, string expectedValue3, XmlTokenizer.XmlToken expectedToken4, string expectedValue4)
    {
        Assert.True(
            TestHelper.Tokenize(
                input,
                [new TestHelper.TokenTypeAndValue(expectedToken1, expectedValue1), new TestHelper.TokenTypeAndValue(expectedToken2, expectedValue2), new TestHelper.TokenTypeAndValue(expectedToken3, expectedValue3), new TestHelper.TokenTypeAndValue(expectedToken4, expectedValue4)]
            )
        );
    }

    [Theory]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel><DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />\n  ",
        XmlTokenizer.XmlToken.Comment,
        "<!-- main window -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>",
        XmlTokenizer.XmlToken.Comment,
        "<!-- ABC -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<StackPanel>",
        XmlTokenizer.XmlToken.ElementEmpty,
        "<DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />"
    )]
    public void Test5Elements(
        string input,
        XmlTokenizer.XmlToken expectedToken1,
        string expectedValue1,
        XmlTokenizer.XmlToken expectedToken2,
        string expectedValue2,
        XmlTokenizer.XmlToken expectedToken3,
        string expectedValue3,
        XmlTokenizer.XmlToken expectedToken4,
        string expectedValue4,
        XmlTokenizer.XmlToken expectedToken5,
        string expectedValue5
    )
    {
        Assert.True(
            TestHelper.Tokenize(
                input,
                [
                    new TestHelper.TokenTypeAndValue(expectedToken1, expectedValue1),
                    new TestHelper.TokenTypeAndValue(expectedToken2, expectedValue2),
                    new TestHelper.TokenTypeAndValue(expectedToken3, expectedValue3),
                    new TestHelper.TokenTypeAndValue(expectedToken4, expectedValue4),
                    new TestHelper.TokenTypeAndValue(expectedToken5, expectedValue5)
                ]
            )
        );
    }

    [Theory]
    [InlineData(
        "    <!-- main window -->\n<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>\n  <!-- ABC -->\n  <StackPanel><DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />\n  </StackPanel>\n",
        XmlTokenizer.XmlToken.Comment,
        "<!-- main window -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<Window\n  xmlns=\"https://github.com/avaloniaui\"\n  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n  xmlns:vm=\"using:GitRise\"\n  d:DesignHeight=\"450\"\n  d:DesignWidth=\"800\"\n  mc:Ignorable=\"d\"\n  x:Class=\"GitRise.MainWindow\"\n  x:DataType=\"vm:MainWindowViewModel\"\n  Icon=\"avares://GitRise/Resources/GitRise.ico\"\n  Title=\"GitRise\"\n>",
        XmlTokenizer.XmlToken.Comment,
        "<!-- ABC -->",
        XmlTokenizer.XmlToken.ElementStart,
        "<StackPanel>",
        XmlTokenizer.XmlToken.ElementEmpty,
        "<DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />",
        XmlTokenizer.XmlToken.ElementEnd,
        "</StackPanel>"
    )]
    public void Test6Elements(
        string input,
        XmlTokenizer.XmlToken expectedToken1,
        string expectedValue1,
        XmlTokenizer.XmlToken expectedToken2,
        string expectedValue2,
        XmlTokenizer.XmlToken expectedToken3,
        string expectedValue3,
        XmlTokenizer.XmlToken expectedToken4,
        string expectedValue4,
        XmlTokenizer.XmlToken expectedToken5,
        string expectedValue5,
        XmlTokenizer.XmlToken expectedToken6,
        string expectedValue6
    )
    {
        Assert.True(
            TestHelper.Tokenize(
                input,
                [
                    new TestHelper.TokenTypeAndValue(expectedToken1, expectedValue1),
                    new TestHelper.TokenTypeAndValue(expectedToken2, expectedValue2),
                    new TestHelper.TokenTypeAndValue(expectedToken3, expectedValue3),
                    new TestHelper.TokenTypeAndValue(expectedToken4, expectedValue4),
                    new TestHelper.TokenTypeAndValue(expectedToken5, expectedValue5),
                    new TestHelper.TokenTypeAndValue(expectedToken6, expectedValue6)
                ]
            )
        );
    }
}
