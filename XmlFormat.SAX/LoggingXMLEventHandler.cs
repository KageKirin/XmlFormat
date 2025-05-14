using System;

namespace XmlFormat.SAX;

///<summary>
/// Logs SAX events to command line
///</summary>
public class LoggingXMLEventHandler : IXMLEventHandler
{
    public virtual void OnXmlDeclaration(string version, string encoding, string standalone) =>
        Console.WriteLine($"XML Declaration {version}, {encoding}, {standalone}");

    public virtual void OnElementStart(string name) => Console.WriteLine($"XML Element Start `{name}`");

    public virtual void OnElementEnd(string name) => Console.WriteLine($"XML Element End `{name}`");

    public virtual void OnElementEmpty(string name) => Console.WriteLine($"XML Element Empty `{name}`");

    public virtual void OnAttribute(string name, string? value) => Console.WriteLine($"XML Attribute `{name}`:{value}");

    public virtual void OnProcessingInstruction(string identifier, string contents) =>
        Console.WriteLine($"XML processing instruction `{identifier}`: `{contents}`");

    public virtual void OnCData(string cdata) => Console.WriteLine($"XML CData `{cdata}`");

    public virtual void OnComment(string comment) => Console.WriteLine($"XML Comment `{comment}`");

    public virtual void OnContent(string text) => Console.WriteLine($"XML Content {text}");

    public virtual void OnError(string message) => Console.Error.WriteLine($"XML Error: {message}");
}
