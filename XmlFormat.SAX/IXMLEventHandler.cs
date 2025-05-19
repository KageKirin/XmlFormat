using System;

namespace XmlFormat.SAX;

///<summary>
/// SAX event handler interface
///</summary>
public interface IXMLEventHandler
{
    void OnXmlDeclaration(ReadOnlySpan<char> version, ReadOnlySpan<char> encoding, ReadOnlySpan<char> standalone);

    void OnElementStart(ReadOnlySpan<char> name);
    void OnElementEnd(ReadOnlySpan<char> name);
    void OnElementEmpty(ReadOnlySpan<char> name);
    void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value);

    void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents);
    void OnCData(ReadOnlySpan<char> cdata);
    void OnComment(ReadOnlySpan<char> comment);
    void OnContent(ReadOnlySpan<char> text);

    void OnError(ReadOnlySpan<char> message);
}
