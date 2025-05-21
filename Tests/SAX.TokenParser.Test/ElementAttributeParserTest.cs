using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ElementAttributeParserTest
{
    [Theory]
    [InlineData("attribute")]
    [InlineData("at-tribute")]
    [InlineData("at_tribute")]
    [InlineData("at:tribute")]
    [InlineData("at:tri_bute")]
    [InlineData("at-tri_bute")]
    [InlineData("at:tri-bute")]
    [InlineData("at:tri-but_e")]
    [InlineData("attribute0")]
    public void TestAttributeNoValue(string input)
    {
        var result = XmlTokenParser.ElementAttributeForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attribute = result.Value;
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(input, attribute.Name.ToStringValue());
        Assert.Null(attribute.Value);
    }

    [Theory]
    [InlineData("attribute=\"\"", "attribute")]
    [InlineData("at-tribute=\"\"", "at-tribute")]
    [InlineData("at_tribute=\"\"", "at_tribute")]
    [InlineData("at:tribute=\"\"", "at:tribute")]
    [InlineData("at:tri_bute=\"\"", "at:tri_bute")]
    [InlineData("at-tri_bute=\"\"", "at-tri_bute")]
    [InlineData("at:tri-bute=\"\"", "at:tri-bute")]
    [InlineData("at:tri-but_e=\"\"", "at:tri-but_e")]
    [InlineData("attribute0=\"\"", "attribute0")]
    public void TestAttributeEmptyValue(string input, string expected)
    {
        var result = XmlTokenParser.ElementAttributeForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attribute = result.Value;
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expected, attribute.Name.ToStringValue());

        Assert.NotNull(attribute.Value);
        TextSpan attribValue = (TextSpan)attribute.Value!;
        Assert.Empty(attribValue.ToStringValue());
    }

    [Theory]
    [InlineData("attribute=\"hogehoge\"", "attribute", "hogehoge")]
    [InlineData("at-tribute=\"hogehoge\"", "at-tribute", "hogehoge")]
    [InlineData("at_tribute=\"hogehoge\"", "at_tribute", "hogehoge")]
    [InlineData("at:tribute=\"hogehoge\"", "at:tribute", "hogehoge")]
    [InlineData("at:tri_bute=\"hogehoge\"", "at:tri_bute", "hogehoge")]
    [InlineData("at-tri_bute=\"hogehoge\"", "at-tri_bute", "hogehoge")]
    [InlineData("at:tri-bute=\"hogehoge\"", "at:tri-bute", "hogehoge")]
    [InlineData("at:tri-but_e=\"hogehoge\"", "at:tri-but_e", "hogehoge")]
    [InlineData("attribute0=\"hogehoge\"", "attribute0", "hogehoge")]
    [InlineData("attribute=\"{hogehoge}\"", "attribute", "{hogehoge}")]
    [InlineData("at-tribute=\"{hogehoge}\"", "at-tribute", "{hogehoge}")]
    [InlineData("at_tribute=\"{hogehoge}\"", "at_tribute", "{hogehoge}")]
    [InlineData("at:tribute=\"{hogehoge}\"", "at:tribute", "{hogehoge}")]
    [InlineData("at:tri_bute=\"{hogehoge}\"", "at:tri_bute", "{hogehoge}")]
    [InlineData("at-tri_bute=\"{hogehoge}\"", "at-tri_bute", "{hogehoge}")]
    [InlineData("at:tri-bute=\"{hogehoge}\"", "at:tri-bute", "{hogehoge}")]
    [InlineData("at:tri-but_e=\"{hogehoge}\"", "at:tri-but_e", "{hogehoge}")]
    [InlineData("attribute0=\"{hogehoge}\"", "attribute0", "{hogehoge}")]
    public void TestAttribute(string input, string expectedName, string expectedValue)
    {
        var result = XmlTokenParser.ElementAttributeForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attribute = result.Value;
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedName, attribute.Name.ToStringValue());

        Assert.NotNull(attribute.Value);
        TextSpan attribValue = (TextSpan)attribute.Value!;
        Assert.NotEmpty(attribValue.ToStringValue());
        Assert.Equal(expectedValue, attribValue.ToStringValue());
    }

    #region multi attribute tests on real data

    [Theory]
    [InlineData("aaa=\"bbb\" ccc=\"ddd\">", "aaa", "bbb", "ccc", "ddd")]
    [InlineData("attribute=\"hogehoge\" at-tribute=\"hogehoge\">", "attribute", "hogehoge", "at-tribute", "hogehoge")]
    [InlineData(" aaa=\"bbb\" ccc=\"ddd\">", "aaa", "bbb", "ccc", "ddd")]
    [InlineData("  attribute=\"hogehoge\" at-tribute=\"hogehoge\">", "attribute", "hogehoge", "at-tribute", "hogehoge")]
    [InlineData("\tattribute=\"hogehoge\" at-tribute=\"hogehoge\">", "attribute", "hogehoge", "at-tribute", "hogehoge")]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008"
    )]
    public void Test2Attributes(string input, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1)
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(2, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml"
    )]
    public void Test3Attributes(string input, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1, string expectedAttribute2, string expectedValue2)
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(3, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006"
    )]
    public void Test4Attributes(string input, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1, string expectedAttribute2, string expectedValue2, string expectedAttribute3, string expectedValue3)
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(4, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise"
    )]
    public void Test5Attributes(string input, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1, string expectedAttribute2, string expectedValue2, string expectedAttribute3, string expectedValue3, string expectedAttribute4, string expectedValue4)
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(5, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450"
    )]
    public void Test6Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(6, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450",
        "d:DesignWidth",
        "800"
    )]
    public void Test7Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5,
        string expectedAttribute6,
        string expectedValue6
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(7, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450",
        "d:DesignWidth",
        "800",
        "mc:Ignorable",
        "d"
    )]
    public void Test8Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5,
        string expectedAttribute6,
        string expectedValue6,
        string expectedAttribute7,
        string expectedValue7
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(8, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
          x:Class=""GitRise.MainWindow""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450",
        "d:DesignWidth",
        "800",
        "mc:Ignorable",
        "d",
        "x:Class",
        "GitRise.MainWindow"
    )]
    public void Test9Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5,
        string expectedAttribute6,
        string expectedValue6,
        string expectedAttribute7,
        string expectedValue7,
        string expectedAttribute8,
        string expectedValue8
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(9, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
          x:Class=""GitRise.MainWindow""
          x:DataType=""vm:MainWindowViewModel""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450",
        "d:DesignWidth",
        "800",
        "mc:Ignorable",
        "d",
        "x:Class",
        "GitRise.MainWindow",
        "x:DataType",
        "vm:MainWindowViewModel"
    )]
    public void Test10Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5,
        string expectedAttribute6,
        string expectedValue6,
        string expectedAttribute7,
        string expectedValue7,
        string expectedAttribute8,
        string expectedValue8,
        string expectedAttribute9,
        string expectedValue9
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(10, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());

        var attribute9 = attributes[9];
        Assert.NotEmpty(attribute9.Name.ToStringValue());
        Assert.Equal(expectedAttribute9, attribute9.Name.ToStringValue());
        Assert.NotNull(attribute9.Value);
        Assert.NotEmpty(((TextSpan)attribute9.Value!).ToStringValue());
        Assert.Equal(expectedValue9, ((TextSpan)attribute9.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
          x:Class=""GitRise.MainWindow""
          x:DataType=""vm:MainWindowViewModel""
          Icon=""avares://GitRise/Resources/GitRise.ico""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450",
        "d:DesignWidth",
        "800",
        "mc:Ignorable",
        "d",
        "x:Class",
        "GitRise.MainWindow",
        "x:DataType",
        "vm:MainWindowViewModel",
        "Icon",
        "avares://GitRise/Resources/GitRise.ico"
    )]
    public void Test11Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5,
        string expectedAttribute6,
        string expectedValue6,
        string expectedAttribute7,
        string expectedValue7,
        string expectedAttribute8,
        string expectedValue8,
        string expectedAttribute9,
        string expectedValue9,
        string expectedAttribute10,
        string expectedValue10
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(11, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());

        var attribute9 = attributes[9];
        Assert.NotEmpty(attribute9.Name.ToStringValue());
        Assert.Equal(expectedAttribute9, attribute9.Name.ToStringValue());
        Assert.NotNull(attribute9.Value);
        Assert.NotEmpty(((TextSpan)attribute9.Value!).ToStringValue());
        Assert.Equal(expectedValue9, ((TextSpan)attribute9.Value!).ToStringValue());

        var attribute10 = attributes[10];
        Assert.NotEmpty(attribute10.Name.ToStringValue());
        Assert.Equal(expectedAttribute10, attribute10.Name.ToStringValue());
        Assert.NotNull(attribute10.Value);
        Assert.NotEmpty(((TextSpan)attribute10.Value!).ToStringValue());
        Assert.Equal(expectedValue10, ((TextSpan)attribute10.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
          x:Class=""GitRise.MainWindow""
          x:DataType=""vm:MainWindowViewModel""
          Icon=""avares://GitRise/Resources/GitRise.ico""
          Title=""GitRise""
        ",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006",
        "xmlns:vm",
        "using:GitRise",
        "d:DesignHeight",
        "450",
        "d:DesignWidth",
        "800",
        "mc:Ignorable",
        "d",
        "x:Class",
        "GitRise.MainWindow",
        "x:DataType",
        "vm:MainWindowViewModel",
        "Icon",
        "avares://GitRise/Resources/GitRise.ico",
        "Title",
        "GitRise"
    )]
    public void Test12Attributes(
        string input,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4,
        string expectedAttribute5,
        string expectedValue5,
        string expectedAttribute6,
        string expectedValue6,
        string expectedAttribute7,
        string expectedValue7,
        string expectedAttribute8,
        string expectedValue8,
        string expectedAttribute9,
        string expectedValue9,
        string expectedAttribute10,
        string expectedValue10,
        string expectedAttribute11,
        string expectedValue11
    )
    {
        var result = XmlTokenParser.ManyElementAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var attributes = result.Value;
        Assert.NotEmpty(attributes);
        Assert.Equal(12, attributes.Length);

        var attribute0 = attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());

        var attribute9 = attributes[9];
        Assert.NotEmpty(attribute9.Name.ToStringValue());
        Assert.Equal(expectedAttribute9, attribute9.Name.ToStringValue());
        Assert.NotNull(attribute9.Value);
        Assert.NotEmpty(((TextSpan)attribute9.Value!).ToStringValue());
        Assert.Equal(expectedValue9, ((TextSpan)attribute9.Value!).ToStringValue());

        var attribute10 = attributes[10];
        Assert.NotEmpty(attribute10.Name.ToStringValue());
        Assert.Equal(expectedAttribute10, attribute10.Name.ToStringValue());
        Assert.NotNull(attribute10.Value);
        Assert.NotEmpty(((TextSpan)attribute10.Value!).ToStringValue());
        Assert.Equal(expectedValue10, ((TextSpan)attribute10.Value!).ToStringValue());

        var attribute11 = attributes[11];
        Assert.NotEmpty(attribute11.Name.ToStringValue());
        Assert.Equal(expectedAttribute11, attribute11.Name.ToStringValue());
        Assert.NotNull(attribute11.Value);
        Assert.NotEmpty(((TextSpan)attribute11.Value!).ToStringValue());
        Assert.Equal(expectedValue11, ((TextSpan)attribute11.Value!).ToStringValue());
    }

    #endregion
}
