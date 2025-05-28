using System;

namespace XmlFormat.SAX;

///<summary>
/// SAX event handler interface
///</summary>
public interface IXMLEventHandler
{
    void OnXmlDeclaration(ReadOnlySpan<char> version, ReadOnlySpan<char> encoding, ReadOnlySpan<char> standalone, int line, int column);

    void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column);
    void OnElementStartClose(ReadOnlySpan<char> name, int line, int column);
    void OnElementEnd(ReadOnlySpan<char> name, int line, int column);
    void OnElementEmptyOpen(ReadOnlySpan<char> name, int line, int column);
    void OnElementEmptyClose(ReadOnlySpan<char> name, int line, int column);
    void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value, int nameLine, int nameColumn, int valueLine, int valueColumn);

    void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column);
    void OnCData(ReadOnlySpan<char> cdata, int line, int column);
    void OnComment(ReadOnlySpan<char> comment, int line, int column);
    void OnText(ReadOnlySpan<char> text, int line, int column);

    void OnError(string message, int line, int column);
}
