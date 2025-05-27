using System;
using System.Text;
using XmlFormat.SAX;

namespace XmlFormat;

public class XmlReadHandlerBase : IXMLEventHandler, IDisposable
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
    ) => writer.WriteLine($"Xml({line}:{column}): {version} {encoding} {standalone}");

    public virtual void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column) =>
        writer.WriteLine($"PI({line}:{column}): {identifier} {contents}");

    public virtual void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementStart open({line}:{column}): {name}");

    public virtual void OnElementStartClose(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementStart close({line}:{column}): {name}");

    public virtual void OnElementEmpty(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementEmpty({line}:{column}): {name}");

    //public virtual void OnEndTagEmpty() => writer.WriteLine($"EndTagEmpty");

    public virtual void OnElementEnd(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementEnd({line}:{column}): {name}");

    public virtual void OnAttribute(
        ReadOnlySpan<char> name,
        ReadOnlySpan<char> value,
        int nameLine,
        int nameColumn,
        int valueLine,
        int valueColumn
    ) => writer.WriteLine($"Attribute({nameLine}:{nameColumn})-({valueLine}:{valueColumn}): {name}=\"{value}\"");

    public virtual void OnText(ReadOnlySpan<char> text, int line, int column) => writer.WriteLine($"Content({line}:{column}): {text}");

    public virtual void OnComment(ReadOnlySpan<char> comment, int line, int column) =>
        writer.WriteLine($"Comment({line}:{column}): {comment}");

    public virtual void OnCData(ReadOnlySpan<char> cdata, int line, int column) => writer.WriteLine($"CDATA({line}:{column}): {cdata}");

    public virtual void OnError(string message, int line, int column) => Console.Error.WriteLine($"ERROR({line}:{column}): {message}");

    #endregion
}
