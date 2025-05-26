using System;
using System.Text;
using TurboXml;

namespace XmlFormat;

public static class XmlFormat
{
    internal static readonly Encoding encoding = new UTF8Encoding(false);

    public static void Format(Stream inputStream, Stream outputStream, FormattingOptions options)
    {
        var handler = new FormattingXmlReadHandler(stream: outputStream, encoding: encoding, options: options);
        XmlParser.Parse(inputStream, handler, options: new XmlParserOptions(Encoding: encoding));
    }

    public static string Format(string xml, FormattingOptions options)
    {
        using MemoryStream xmlStream = new(encoding.GetBytes(xml));
        using MemoryStream outStream = new();
        Format(inputStream: xmlStream, outputStream: outStream, options: options);
        outStream.Flush();
        return encoding.GetString(outStream.ToArray());
    }
}
