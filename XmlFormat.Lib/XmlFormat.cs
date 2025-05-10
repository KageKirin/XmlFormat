using System;
using System.Text;
using TurboXml;

namespace XmlFormat;

public static class XmlFormat
{
    public static void Format(Stream inputStream, Stream outputStream)
    {
        using var handler = new FormattingXmlReadHandler(
            stream: outputStream,
            encoding: Encoding.UTF8
        );
        XmlParser.Parse(inputStream, handler);
    }

    public static string Format(string xml)
    {
        using Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml) ?? new byte[0]);
        using Stream outStream = new MemoryStream();
        Format(inputStream: xmlStream, outputStream: outStream);
        return outStream.ToString() ?? "";
    }
}
