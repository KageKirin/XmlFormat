using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ElementAndAttributesParserTest
{
    [Theory]
    [InlineData("<element", "element")]
    [InlineData("<el-ement", "el-ement")]
    [InlineData("<el_ement", "el_ement")]
    [InlineData("<el:ement", "el:ement")]
    [InlineData("<el:em_ent", "el:em_ent")]
    [InlineData("<el-em_ent", "el-em_ent")]
    [InlineData("<el:em-ent", "el:em-ent")]
    [InlineData("<el:em-en_t", "el:em-en_t")]
    [InlineData("<element1", "element1")]
    [InlineData("<element ", "element")]
    [InlineData("<el-ement ", "el-ement")]
    [InlineData("<el_ement ", "el_ement")]
    [InlineData("<el:ement ", "el:ement")]
    [InlineData("<el:em_ent ", "el:em_ent")]
    [InlineData("<el-em_ent ", "el-em_ent")]
    [InlineData("<el:em-ent ", "el:em-ent")]
    [InlineData("<el:em-en_t ", "el:em-en_t")]
    [InlineData("<element1 ", "element1")]
    [InlineData("<element\n ", "element")]
    [InlineData("<el-ement\n ", "el-ement")]
    [InlineData("<el_ement\n ", "el_ement")]
    [InlineData("<el:ement\n ", "el:ement")]
    [InlineData("<el:em_ent\n ", "el:em_ent")]
    [InlineData("<el-em_ent\n ", "el-em_ent")]
    [InlineData("<el:em-ent\n ", "el:em-ent")]
    [InlineData("<el:em-en_t\n ", "el:em-en_t")]
    [InlineData("<element1\n ", "element1")]
    public void TestElementAndNoAttributes(string input, string expected)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expected, element.Identifier.ToStringValue());
        Assert.Empty(element.Attributes);
    }

    [Theory]
    [InlineData("<element attribute", "element", "attribute")]
    [InlineData("<el-ement at-tribute", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0", "element1", "attribute0")]
    [InlineData("<element attribute ", "element", "attribute")]
    [InlineData("<el-ement at-tribute ", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute ", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute ", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute ", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute ", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute ", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e ", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0 ", "element1", "attribute0")]
    [InlineData("<element\nattribute \n", "element", "attribute")]
    [InlineData("<el-ement\nat-tribute \n", "el-ement", "at-tribute")]
    [InlineData("<el_ement\nat_tribute \n", "el_ement", "at_tribute")]
    [InlineData("<el:ement\nat:tribute \n", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent\nat:tri_bute \n", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent\nat-tri_bute \n", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent\nat:tri-bute \n", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t\nat:tri-but_e \n", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1\nattribute0 \n", "element1", "attribute0")]
    public void TestElementAndAttributeNoValue(string input, string expectedElement, string expectedAttribute)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Single(element.Attributes);
        var attribute = element.Attributes[0];
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedAttribute, attribute.Name.ToStringValue());
        Assert.Null(attribute.Value);
    }

    [Theory]
    [InlineData("<element attribute=\"\"", "element", "attribute")]
    [InlineData("<el-ement at-tribute=\"\"", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute=\"\"", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute=\"\"", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute=\"\"", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute=\"\"", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute=\"\"", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e=\"\"", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0=\"\"", "element1", "attribute0")]
    [InlineData("<element attribute=\"\" ", "element", "attribute")]
    [InlineData("<el-ement at-tribute=\"\" ", "el-ement", "at-tribute")]
    [InlineData("<el_ement at_tribute=\"\" ", "el_ement", "at_tribute")]
    [InlineData("<el:ement at:tribute=\"\" ", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent at:tri_bute=\"\" ", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent at-tri_bute=\"\" ", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent at:tri-bute=\"\" ", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t at:tri-but_e=\"\" ", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1 attribute0=\"\" ", "element1", "attribute0")]
    [InlineData("<element\nattribute=\"\"\n", "element", "attribute")]
    [InlineData("<el-ement\nat-tribute=\"\"\n", "el-ement", "at-tribute")]
    [InlineData("<el_ement\nat_tribute=\"\"\n", "el_ement", "at_tribute")]
    [InlineData("<el:ement\nat:tribute=\"\"\n", "el:ement", "at:tribute")]
    [InlineData("<el:em_ent\nat:tri_bute=\"\"\n", "el:em_ent", "at:tri_bute")]
    [InlineData("<el-em_ent\nat-tri_bute=\"\"\n", "el-em_ent", "at-tri_bute")]
    [InlineData("<el:em-ent\nat:tri-bute=\"\"\n", "el:em-ent", "at:tri-bute")]
    [InlineData("<el:em-en_t\nat:tri-but_e=\"\"\n", "el:em-en_t", "at:tri-but_e")]
    [InlineData("<element1\nattribute0=\"\"\n", "element1", "attribute0")]
    public void TestElementAndAttributeEmptyValue(string input, string expectedElement, string expectedAttribute)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Single(element.Attributes);
        var attribute = element.Attributes[0];
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedAttribute, attribute.Name.ToStringValue());
        Assert.NotNull(attribute.Value);
        Assert.Empty(((TextSpan)attribute.Value!).ToStringValue());
    }

    [Theory]
    [InlineData("<element attribute=\"hogehoge\"", "element", "attribute", "hogehoge")]
    [InlineData("<el-ement at-tribute=\"hogehoge\"", "el-ement", "at-tribute", "hogehoge")]
    [InlineData("<el_ement at_tribute=\"hogehoge\"", "el_ement", "at_tribute", "hogehoge")]
    [InlineData("<el:ement at:tribute=\"hogehoge\"", "el:ement", "at:tribute", "hogehoge")]
    [InlineData("<el:em_ent at:tri_bute=\"hogehoge\"", "el:em_ent", "at:tri_bute", "hogehoge")]
    [InlineData("<el-em_ent at-tri_bute=\"hogehoge\"", "el-em_ent", "at-tri_bute", "hogehoge")]
    [InlineData("<el:em-ent at:tri-bute=\"hogehoge\"", "el:em-ent", "at:tri-bute", "hogehoge")]
    [InlineData("<el:em-en_t at:tri-but_e=\"hogehoge\"", "el:em-en_t", "at:tri-but_e", "hogehoge")]
    [InlineData("<element1 attribute0=\"hogehoge\"", "element1", "attribute0", "hogehoge")]
    [InlineData("<element attribute=\"{hogehoge}\"", "element", "attribute", "{hogehoge}")]
    [InlineData("<el-ement at-tribute=\"{hogehoge}\"", "el-ement", "at-tribute", "{hogehoge}")]
    [InlineData("<el_ement at_tribute=\"{hogehoge}\"", "el_ement", "at_tribute", "{hogehoge}")]
    [InlineData("<el:ement at:tribute=\"{hogehoge}\"", "el:ement", "at:tribute", "{hogehoge}")]
    [InlineData("<el:em_ent at:tri_bute=\"{hogehoge}\"", "el:em_ent", "at:tri_bute", "{hogehoge}")]
    [InlineData("<el-em_ent at-tri_bute=\"{hogehoge}\"", "el-em_ent", "at-tri_bute", "{hogehoge}")]
    [InlineData("<el:em-ent at:tri-bute=\"{hogehoge}\"", "el:em-ent", "at:tri-bute", "{hogehoge}")]
    [InlineData("<el:em-en_t at:tri-but_e=\"{hogehoge}\"", "el:em-en_t", "at:tri-but_e", "{hogehoge}")]
    [InlineData("<element1 attribute0=\"{hogehoge}\"", "element1", "attribute0", "{hogehoge}")]
    public void TestElementAndOneAttribute(string input, string expectedElement, string expectedAttribute, string expectedValue)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Single(element.Attributes);
        var attribute = element.Attributes[0];
        Assert.NotEmpty(attribute.Name.ToStringValue());
        Assert.Equal(expectedAttribute, attribute.Name.ToStringValue());
        Assert.NotNull(attribute.Value);
        Assert.NotEmpty(((TextSpan)attribute.Value!).ToStringValue());
        Assert.Equal(expectedValue, ((TextSpan)attribute.Value!).ToStringValue());
    }

    #region multi attribute tests on real data

    [Theory]
    [InlineData("<element aaa=\"bbb\" ccc=\"ddd\">", "element", "aaa", "bbb", "ccc", "ddd")]
    [InlineData("<element attribute=\"hogehoge\" at-tribute=\"hogehoge\">", "element", "attribute", "hogehoge", "at-tribute", "hogehoge")]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
        ",
        "Window",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008"
    )]
    public void TestElement2Attributes(string input, string expectedElement, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(2, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        ",
        "Window",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml"
    )]
    public void TestElement3Attributes(string input, string expectedElement, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1, string expectedAttribute2, string expectedValue2)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(3, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        ",
        "Window",
        "xmlns",
        "https://github.com/avaloniaui",
        "xmlns:d",
        "http://schemas.microsoft.com/expression/blend/2008",
        "xmlns:x",
        "http://schemas.microsoft.com/winfx/2006/xaml",
        "xmlns:mc",
        "http://schemas.openxmlformats.org/markup-compatibility/2006"
    )]
    public void TestElement4Attributes(string input, string expectedElement, string expectedAttribute0, string expectedValue0, string expectedAttribute1, string expectedValue1, string expectedAttribute2, string expectedValue2, string expectedAttribute3, string expectedValue3)
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(4, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
        ",
        "Window",
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
    public void TestElement5Attributes(
        string input,
        string expectedElement,
        string expectedAttribute0,
        string expectedValue0,
        string expectedAttribute1,
        string expectedValue1,
        string expectedAttribute2,
        string expectedValue2,
        string expectedAttribute3,
        string expectedValue3,
        string expectedAttribute4,
        string expectedValue4
    )
    {
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(5, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
        ",
        "Window",
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
    public void TestElement6Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(6, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
        ",
        "Window",
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
    public void TestElement7Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(7, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = element.Attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
        ",
        "Window",
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
    public void TestElement8Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(8, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = element.Attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = element.Attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
          xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
          xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
          xmlns:vm=""using:GitRise""
          d:DesignHeight=""450""
          d:DesignWidth=""800""
          mc:Ignorable=""d""
          x:Class=""GitRise.MainWindow""
        ",
        "Window",
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
    public void TestElement9Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(9, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = element.Attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = element.Attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = element.Attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
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
        "Window",
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
    public void TestElement10Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(10, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = element.Attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = element.Attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = element.Attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());

        var attribute9 = element.Attributes[9];
        Assert.NotEmpty(attribute9.Name.ToStringValue());
        Assert.Equal(expectedAttribute9, attribute9.Name.ToStringValue());
        Assert.NotNull(attribute9.Value);
        Assert.NotEmpty(((TextSpan)attribute9.Value!).ToStringValue());
        Assert.Equal(expectedValue9, ((TextSpan)attribute9.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
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
        "Window",
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
    public void TestElement11Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(11, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = element.Attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = element.Attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = element.Attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());

        var attribute9 = element.Attributes[9];
        Assert.NotEmpty(attribute9.Name.ToStringValue());
        Assert.Equal(expectedAttribute9, attribute9.Name.ToStringValue());
        Assert.NotNull(attribute9.Value);
        Assert.NotEmpty(((TextSpan)attribute9.Value!).ToStringValue());
        Assert.Equal(expectedValue9, ((TextSpan)attribute9.Value!).ToStringValue());

        var attribute10 = element.Attributes[10];
        Assert.NotEmpty(attribute10.Name.ToStringValue());
        Assert.Equal(expectedAttribute10, attribute10.Name.ToStringValue());
        Assert.NotNull(attribute10.Value);
        Assert.NotEmpty(((TextSpan)attribute10.Value!).ToStringValue());
        Assert.Equal(expectedValue10, ((TextSpan)attribute10.Value!).ToStringValue());
    }

    [Theory]
    [InlineData(
        @"<Window
          xmlns=""https://github.com/avaloniaui""
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
        "Window",
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
    public void TestElement12Attributes(
        string input,
        string expectedElement,
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
        var result = XmlTokenParser.ElementAndAttributesForUnitTestsOnly(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");

        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var element = result.Value;
        Assert.NotEmpty(element.Identifier.ToStringValue());
        Assert.Equal(expectedElement, element.Identifier.ToStringValue());

        Assert.NotEmpty(element.Attributes);
        Assert.Equal(12, element.Attributes.Length);

        var attribute0 = element.Attributes[0];
        Assert.NotEmpty(attribute0.Name.ToStringValue());
        Assert.Equal(expectedAttribute0, attribute0.Name.ToStringValue());
        Assert.NotNull(attribute0.Value);
        Assert.NotEmpty(((TextSpan)attribute0.Value!).ToStringValue());
        Assert.Equal(expectedValue0, ((TextSpan)attribute0.Value!).ToStringValue());

        var attribute1 = element.Attributes[1];
        Assert.NotEmpty(attribute1.Name.ToStringValue());
        Assert.Equal(expectedAttribute1, attribute1.Name.ToStringValue());
        Assert.NotNull(attribute1.Value);
        Assert.NotEmpty(((TextSpan)attribute1.Value!).ToStringValue());
        Assert.Equal(expectedValue1, ((TextSpan)attribute1.Value!).ToStringValue());

        var attribute2 = element.Attributes[2];
        Assert.NotEmpty(attribute2.Name.ToStringValue());
        Assert.Equal(expectedAttribute2, attribute2.Name.ToStringValue());
        Assert.NotNull(attribute2.Value);
        Assert.NotEmpty(((TextSpan)attribute2.Value!).ToStringValue());
        Assert.Equal(expectedValue2, ((TextSpan)attribute2.Value!).ToStringValue());

        var attribute3 = element.Attributes[3];
        Assert.NotEmpty(attribute3.Name.ToStringValue());
        Assert.Equal(expectedAttribute3, attribute3.Name.ToStringValue());
        Assert.NotNull(attribute3.Value);
        Assert.NotEmpty(((TextSpan)attribute3.Value!).ToStringValue());
        Assert.Equal(expectedValue3, ((TextSpan)attribute3.Value!).ToStringValue());

        var attribute4 = element.Attributes[4];
        Assert.NotEmpty(attribute4.Name.ToStringValue());
        Assert.Equal(expectedAttribute4, attribute4.Name.ToStringValue());
        Assert.NotNull(attribute4.Value);
        Assert.NotEmpty(((TextSpan)attribute4.Value!).ToStringValue());
        Assert.Equal(expectedValue4, ((TextSpan)attribute4.Value!).ToStringValue());

        var attribute5 = element.Attributes[5];
        Assert.NotEmpty(attribute5.Name.ToStringValue());
        Assert.Equal(expectedAttribute5, attribute5.Name.ToStringValue());
        Assert.NotNull(attribute5.Value);
        Assert.NotEmpty(((TextSpan)attribute5.Value!).ToStringValue());
        Assert.Equal(expectedValue5, ((TextSpan)attribute5.Value!).ToStringValue());

        var attribute6 = element.Attributes[6];
        Assert.NotEmpty(attribute6.Name.ToStringValue());
        Assert.Equal(expectedAttribute6, attribute6.Name.ToStringValue());
        Assert.NotNull(attribute6.Value);
        Assert.NotEmpty(((TextSpan)attribute6.Value!).ToStringValue());
        Assert.Equal(expectedValue6, ((TextSpan)attribute6.Value!).ToStringValue());

        var attribute7 = element.Attributes[7];
        Assert.NotEmpty(attribute7.Name.ToStringValue());
        Assert.Equal(expectedAttribute7, attribute7.Name.ToStringValue());
        Assert.NotNull(attribute7.Value);
        Assert.NotEmpty(((TextSpan)attribute7.Value!).ToStringValue());
        Assert.Equal(expectedValue7, ((TextSpan)attribute7.Value!).ToStringValue());

        var attribute8 = element.Attributes[8];
        Assert.NotEmpty(attribute8.Name.ToStringValue());
        Assert.Equal(expectedAttribute8, attribute8.Name.ToStringValue());
        Assert.NotNull(attribute8.Value);
        Assert.NotEmpty(((TextSpan)attribute8.Value!).ToStringValue());
        Assert.Equal(expectedValue8, ((TextSpan)attribute8.Value!).ToStringValue());

        var attribute9 = element.Attributes[9];
        Assert.NotEmpty(attribute9.Name.ToStringValue());
        Assert.Equal(expectedAttribute9, attribute9.Name.ToStringValue());
        Assert.NotNull(attribute9.Value);
        Assert.NotEmpty(((TextSpan)attribute9.Value!).ToStringValue());
        Assert.Equal(expectedValue9, ((TextSpan)attribute9.Value!).ToStringValue());

        var attribute10 = element.Attributes[10];
        Assert.NotEmpty(attribute10.Name.ToStringValue());
        Assert.Equal(expectedAttribute10, attribute10.Name.ToStringValue());
        Assert.NotNull(attribute10.Value);
        Assert.NotEmpty(((TextSpan)attribute10.Value!).ToStringValue());
        Assert.Equal(expectedValue10, ((TextSpan)attribute10.Value!).ToStringValue());

        var attribute11 = element.Attributes[11];
        Assert.NotEmpty(attribute11.Name.ToStringValue());
        Assert.Equal(expectedAttribute11, attribute11.Name.ToStringValue());
        Assert.NotNull(attribute11.Value);
        Assert.NotEmpty(((TextSpan)attribute11.Value!).ToStringValue());
        Assert.Equal(expectedValue11, ((TextSpan)attribute11.Value!).ToStringValue());
    }

    #endregion
}
