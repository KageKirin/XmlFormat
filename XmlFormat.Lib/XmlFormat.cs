using System;
using System.Text;
using XmlFormat.SAX;

namespace XmlFormat;

public static class XmlFormat
{
    public static void Format(Stream inputStream, Stream outputStream, FormattingOptions options)
    {
        using StreamReader reader = new(inputStream);
        using var handler = new FormattingXmlReadHandler(stream: outputStream, encoding: Encoding.UTF8, options: options);

        SaxParser.Parse(reader.ReadToEnd(), handler);
    }

    public static string Format(string xml, FormattingOptions options)
    {
        using Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml) ?? new byte[0]);
        using Stream outStream = new MemoryStream();
        Format(inputStream: xmlStream, outputStream: outStream, options: options);
        return outStream.ToString() ?? "";
    }
}
