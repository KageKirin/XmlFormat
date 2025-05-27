using System;

namespace XmlFormat.SAX;

///<summary>
/// Logs SAX events to command line
///</summary>
public class LoggingXMLEventHandler : IXMLEventHandler
{
    public virtual void OnXmlDeclaration(
        ReadOnlySpan<char> version,
        ReadOnlySpan<char> encoding,
        ReadOnlySpan<char> standalone,
        int line,
        int column
    ) => Console.WriteLine($"XML Declaration {version.ToString()}, {encoding.ToString()}, {standalone.ToString()}, {line}:{column}");

    public virtual void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column) =>
        Console.WriteLine($"XML Element Start (open) `{name.ToString()}`, {line}:{column}");

    public virtual void OnElementStartClose(ReadOnlySpan<char> name, int line, int column) =>
        Console.WriteLine($"XML Element Start (close) `{name.ToString()}`, {line}:{column}");

    public virtual void OnElementEnd(ReadOnlySpan<char> name, int line, int column) =>
        Console.WriteLine($"XML Element End `{name.ToString()}`, {line}:{column}");

    public virtual void OnElementEmptyOpen(ReadOnlySpan<char> name, int line, int column) =>
        Console.WriteLine($"XML Element Empty (open) `{name.ToString()}`, {line}:{column}");

    public virtual void OnElementEmptyClose(ReadOnlySpan<char> name, int line, int column) =>
        Console.WriteLine($"XML Element Empty (close) `{name.ToString()}`, {line}:{column}");

    public virtual void OnAttribute(
        ReadOnlySpan<char> name,
        ReadOnlySpan<char> value,
        int nameLine,
        int nameColumn,
        int valueLine,
        int valueColumn
    ) => Console.WriteLine($"XML Attribute `{name.ToString()}`:{value.ToString()}, {nameLine}:{nameColumn}, {valueLine}:{valueColumn}");

    public virtual void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column) =>
        Console.WriteLine($"XML processing instruction `{identifier.ToString()}`: `{contents.ToString()}`, {line}:{column}");

    public virtual void OnCData(ReadOnlySpan<char> cdata, int line, int column) =>
        Console.WriteLine($"XML CData `{cdata.ToString()}`, {line}:{column}");

    public virtual void OnComment(ReadOnlySpan<char> comment, int line, int column) =>
        Console.WriteLine($"XML Comment `{comment.ToString()}`, {line}:{column}");

    public virtual void OnText(ReadOnlySpan<char> text, int line, int column) =>
        Console.WriteLine($"XML Content {text.ToString()}, {line}:{column}");

    public virtual void OnError(string message, int line, int column) => Console.Error.WriteLine($"XML Error: {message} {line}:{column}");
}
