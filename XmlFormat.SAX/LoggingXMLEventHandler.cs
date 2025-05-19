using System;

namespace XmlFormat.SAX;

///<summary>
/// Logs SAX events to command line
///</summary>
public class LoggingXMLEventHandler : IXMLEventHandler
{
    public virtual void OnXmlDeclaration(ReadOnlySpan<char> version, ReadOnlySpan<char> encoding, ReadOnlySpan<char> standalone) =>
        Console.WriteLine($"XML Declaration {version.ToString()}, {encoding.ToString()}, {standalone.ToString()}");

    public virtual void OnElementStart(ReadOnlySpan<char> name) => Console.WriteLine($"XML Element Start `{name.ToString()}`");

    public virtual void OnElementEnd(ReadOnlySpan<char> name) => Console.WriteLine($"XML Element End `{name.ToString()}`");

    public virtual void OnElementEmpty(ReadOnlySpan<char> name) => Console.WriteLine($"XML Element Empty `{name.ToString()}`");

    public virtual void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value) =>
        Console.WriteLine($"XML Attribute `{name.ToString()}`:{value.ToString()}");

    public virtual void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents) =>
        Console.WriteLine($"XML processing instruction `{identifier.ToString()}`: `{contents.ToString()}`");

    public virtual void OnCData(ReadOnlySpan<char> cdata) => Console.WriteLine($"XML CData `{cdata.ToString()}`");

    public virtual void OnComment(ReadOnlySpan<char> comment) => Console.WriteLine($"XML Comment `{comment.ToString()}`");

    public virtual void OnContent(ReadOnlySpan<char> text) => Console.WriteLine($"XML Content {text.ToString()}");

    public virtual void OnError(ReadOnlySpan<char> message) => Console.Error.WriteLine($"XML Error: {message.ToString()}");
}
