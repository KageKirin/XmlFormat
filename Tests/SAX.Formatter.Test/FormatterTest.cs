using System;
using System.Text;
using XmlFormat;

namespace SAX.Formatter.Test;

public class FormatterTest
{
    [Theory]
    [InlineData("<?xml?>", "<?xml ?>")]
    [InlineData("<?xml ?>", "<?xml ?>")]
    [InlineData(@"<?xml version=""1.0""?>", @"<?xml version=""1.0"" ?>")]
    [InlineData(@"<?xml version=""1.0"" ?>", @"<?xml version=""1.0"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8""?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" ?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true""?>", @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>", @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone ?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData("<?php ?>", "<?php ?>")]
    [InlineData("<?pi ?>", "<?pi ?>")]
    [InlineData("<element> ", "<element>")]
    [InlineData("<element > ", "<element>")]
    [InlineData("<element/> ", "<element />")]
    [InlineData("<element /> ", "<element />")]
    [InlineData("<element attribute=\"1\"/> ", "<element attribute=\"1\" />")]
    [InlineData("<element>test</element> ", "<element>\ntest\n</element>")]
    [InlineData("<element attribute=\"1\">test</element> ", "<element attribute=\"1\">\ntest\n</element>")]
    public void IdentityTest(string input, string expected)
    {
        var formatted = XmlFormat.XmlFormat.Format(input, new FormattingOptions(80, "", 1));
        Assert.NotNull(formatted);

        if (string.IsNullOrEmpty(expected))
        {
            Assert.Empty(formatted);
        }
        else
        {
            Assert.NotEmpty(formatted);
        }
        Assert.Equal(expected, formatted.Trim());
    }

    [Theory]
    [InlineData("<?xml?>", "<?xml ?>")]
    [InlineData("<?xml ?>", "<?xml ?>")]
    [InlineData(@"<?xml version=""1.0""?>", @"<?xml version=""1.0"" ?>")]
    [InlineData(@"<?xml version=""1.0"" ?>", @"<?xml version=""1.0"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8""?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" ?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true""?>", @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>", @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone ?>", @"<?xml version=""1.0"" encoding=""utf-8"" ?>")]
    [InlineData("<?php ?>", "<?php ?>")]
    [InlineData("<?pi ?>", "<?pi ?>")]
    [InlineData("<element> ", "<element>")]
    [InlineData("<element > ", "<element>")]
    [InlineData("<element/> ", "<element />")]
    [InlineData("<element /> ", "<element />")]
    [InlineData("<element attribute=\"1\"/> ", "<element attribute=\"1\" />")]
    [InlineData("<element>test</element> ", "<element>\ntest\n</element>")]
    [InlineData("<element attribute=\"1\">test</element> ", "<element attribute=\"1\">\ntest\n</element>")]
    public void IdentityTestStream(string input, string expected)
    {
        Encoding encoding = new UTF8Encoding(true);
        MemoryStream xmlStream = new(encoding.GetBytes(input));
        MemoryStream outStream = new();
        XmlFormat.XmlFormat.Format(inputStream: xmlStream, outputStream: outStream, options: new FormattingOptions(80, "", 1));
        outStream.Flush();
        var formatted = encoding.GetString(outStream.ToArray());
        Assert.NotNull(formatted);

        if (string.IsNullOrEmpty(expected))
        {
            Assert.Empty(formatted);
        }
        else
        {
            Assert.NotEmpty(formatted);
        }
        Assert.Equal(expected, formatted.Trim());
    }
}
