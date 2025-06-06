using System;
using System.Text;
using XmlFormat;
using XmlFormat.Test.Assets;

namespace SAX.Formatter.Test;

public class ResourceFormatterTest
{
    static FormattingOptions formattingOptions = new(120, "  ", 1, 2);

    [Theory]
    [InlineData("XmlFormat.Test.Assets.idtest.xml")]
    public void IdentityTest_idtest_xml(string resource)
    {
        var resourceContents = EmbeddedAssets.GetEmbeddedResourceString(resource);
        Assert.NotNull(resourceContents);
        Assert.NotEmpty(resourceContents);

        var formatted = XmlFormat.XmlFormat.Format(resourceContents, formattingOptions);
        Assert.NotNull(formatted);
        Assert.NotEmpty(resourceContents);
        Assert.Equal(resourceContents, formatted);
    }

    [Theory]
    [InlineData("XmlFormat.Test.Assets.idtest.xml")]
    public void IdentityTestStream_idtest_xml(string resource)
    {
        Encoding encoding = new UTF8Encoding(true);
        using Stream? xmlStream = EmbeddedAssets.GetEmbeddedResourceStream(resource);
        Assert.NotNull(xmlStream);

        string readStream(Stream stream)
        {
            StreamReader xmlReader = new(stream, encoding);
            return xmlReader.ReadToEnd();
        }
        string expected = readStream(xmlStream);
        xmlStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream outStream = new();
        XmlFormat.XmlFormat.Format(inputStream: xmlStream, outputStream: outStream, options: formattingOptions);
        outStream.Flush();
        Assert.True(outStream.Length > 0);

        using MemoryStream readableOutStream = new(outStream.ToArray());
        var formatted = readStream(readableOutStream);
        Assert.NotNull(expected);
        Assert.NotEmpty(expected);
        Assert.NotNull(formatted);
        Assert.NotEmpty(formatted);
        Assert.Equal(expected, formatted);
    }
}
