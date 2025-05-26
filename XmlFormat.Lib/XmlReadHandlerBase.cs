using System;
using System.Text;
using TurboXml;

namespace XmlFormat;

public class XmlReadHandlerBase : IXmlReadHandler, IDisposable
{
    protected readonly StreamWriter writer;

    public XmlReadHandlerBase(Stream stream, Encoding encoding)
        : this(new StreamWriter(stream, encoding, leaveOpen: true) { AutoFlush = true, }) { }

    public XmlReadHandlerBase(StreamWriter streamWriter)
    {
        this.writer = streamWriter;
    }

    #region IDisposable implementation

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        writer.Dispose();
    }

    #endregion

    #region  IXmlReadHandler implementation

    public virtual void OnXmlDeclaration(
        ReadOnlySpan<char> version,
        ReadOnlySpan<char> encoding,
        ReadOnlySpan<char> standalone,
        int line,
        int column
    ) => writer.WriteLine($"Xml({line + 1}:{column + 1}): {version} {encoding} {standalone}");

    public virtual void OnBeginTag(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"BeginTag({line + 1}:{column + 1}): {name}");

    public virtual void OnEndTagEmpty() => writer.WriteLine($"EndTagEmpty");

    public virtual void OnEndTag(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"EndTag({line + 1}:{column + 1}): {name}");

    public virtual void OnAttribute(
        ReadOnlySpan<char> name,
        ReadOnlySpan<char> value,
        int nameLine,
        int nameColumn,
        int valueLine,
        int valueColumn
    ) =>
        writer.WriteLine(
            $"Attribute({nameLine + 1}:{nameColumn + 1})-({valueLine + 1}:{valueColumn + 1}): {name}=\"{value}\""
        );

    public virtual void OnText(ReadOnlySpan<char> text, int line, int column) =>
        writer.WriteLine($"Content({line + 1}:{column + 1}): {text}");

    public virtual void OnComment(ReadOnlySpan<char> comment, int line, int column) =>
        writer.WriteLine($"Comment({line + 1}:{column + 1}): {comment}");

    public virtual void OnCData(ReadOnlySpan<char> cdata, int line, int column) =>
        writer.WriteLine($"CDATA({line + 1}:{column + 1}): {cdata}");

    public virtual void OnError(string message, int line, int column) =>
        Console.Error.WriteLine($"ERROR({line + 1}:{column + 1}): {message}");

    #endregion
}
