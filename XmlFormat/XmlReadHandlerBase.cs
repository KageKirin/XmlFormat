using System;
using System.Text;
using XmlFormat.SAX;

namespace XmlFormat;

public class XmlReadHandlerBase : IXMLEventHandler, IDisposable
{
    protected readonly StreamWriter writer;

    public XmlReadHandlerBase(Stream stream, Encoding encoding)
        : this(new StreamWriter(stream, encoding, bufferSize: 4096, leaveOpen: true) { AutoFlush = true }) { }

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
    ) => writer.WriteLine($"Xml({line}:{column}): {version.ToString()} {encoding.ToString()} {standalone.ToString()}");

    public virtual void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column) =>
        writer.WriteLine($"PI({line}:{column}): {identifier.ToString()} {contents.ToString()}");

    public virtual void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementStart open({line}:{column}): {name.ToString()}");

    public virtual void OnElementStartClose(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementStart close({line}:{column}): {name.ToString()}");

    public virtual void OnElementEmptyOpen(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementEmpty open({line}:{column}): {name.ToString()}");

    public virtual void OnElementEmptyClose(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementEmpty close({line}:{column}): {name.ToString()}");

    //public virtual void OnEndTagEmpty() => writer.WriteLine($"EndTagEmpty");

    public virtual void OnElementEnd(ReadOnlySpan<char> name, int line, int column) =>
        writer.WriteLine($"ElementEnd({line}:{column}): {name.ToString()}");

    public virtual void OnAttribute(
        ReadOnlySpan<char> name,
        ReadOnlySpan<char> value,
        int nameLine,
        int nameColumn,
        int valueLine,
        int valueColumn
    ) => writer.WriteLine($"Attribute({nameLine}:{nameColumn})-({valueLine}:{valueColumn}): {name.ToString()}=\"{value.ToString()}\"");

    public virtual void OnText(ReadOnlySpan<char> text, int line, int column) =>
        writer.WriteLine($"Content({line}:{column}): {text.ToString()}");

    public virtual void OnComment(ReadOnlySpan<char> comment, int line, int column) =>
        writer.WriteLine($"Comment({line}:{column}): {comment.ToString()}");

    public virtual void OnCData(ReadOnlySpan<char> cdata, int line, int column) =>
        writer.WriteLine($"CDATA({line}:{column}): {cdata.ToString()}");

    public virtual void OnError(string message, int line, int column) => Console.Error.WriteLine($"ERROR({line}:{column}): {message}");

    #endregion
}
