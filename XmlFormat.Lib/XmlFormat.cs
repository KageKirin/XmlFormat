using System;
using System.Text;
using XmlFormat.SAX;

namespace XmlFormat;

public static class XmlFormat
{
    internal static readonly Encoding encoding = new UTF8Encoding(false);

    public static void Format(Stream inputStream, Stream outputStream, FormattingOptions options)
    {
        using StreamReader reader = new(inputStream);
        using var handler = new FormattingXmlReadHandler(stream: outputStream, encoding: Encoding.UTF8, options: options);

        SaxParser.Parse(reader.ReadToEnd(), handler);
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
