using System;
using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

///<summary>
/// Logs SAX events to command line
///</summary>
public class DelegateXMLEventHandler : IXMLEventHandler
{
    public Action<ReadOnlySpan<char>, ReadOnlySpan<char>, ReadOnlySpan<char>, int, int> OnXmlDeclarationCallback = (_, __, ___, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnElementStartOpenCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnElementStartCloseCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnElementEndCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnElementEmptyCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, ReadOnlySpan<char>, int, int, int, int> OnAttributeCallback = (_, __, nameLine, nameColumn, valueLine, valueColumn) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, ReadOnlySpan<char>, int, int> OnProcessingInstructionCallback = (_, __, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnCDataCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnCommentCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, int, int> OnTextCallback = (_, line, column) =>
    {
        Assert.False(true);
    };
    public Action<string, int, int> OnErrorCallback = (_, line, column) =>
    {
        Assert.False(true);
    };

    public void OnXmlDeclaration(ReadOnlySpan<char> version, ReadOnlySpan<char> encoding, ReadOnlySpan<char> standalone, int line, int column) => OnXmlDeclarationCallback(version, encoding, standalone, line, column);

    public void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column) => OnElementStartOpenCallback(name, line, column);

    public void OnElementStartClose(ReadOnlySpan<char> name, int line, int column) => OnElementStartCloseCallback(name, line, column);

    public void OnElementEnd(ReadOnlySpan<char> name, int line, int column) => OnElementEndCallback(name, line, column);

    public void OnElementEmpty(ReadOnlySpan<char> name, int line, int column) => OnElementEmptyCallback(name, line, column);

    public void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value, int nameLine, int nameColumn, int valueLine, int valueColumn) => OnAttributeCallback(name, value, nameLine, nameColumn, valueLine, valueColumn);

    public void OnProcessingInstruction(ReadOnlySpan<char> name, ReadOnlySpan<char> value, int line, int column) => OnProcessingInstructionCallback(name, value, line, column);

    public void OnCData(ReadOnlySpan<char> cdata, int line, int column) => OnCDataCallback(cdata, line, column);

    public void OnComment(ReadOnlySpan<char> comment, int line, int column) => OnCommentCallback(comment, line, column);

    public void OnText(ReadOnlySpan<char> text, int line, int column) => OnTextCallback(text, line, column);

    public void OnError(string message, int line, int column) => OnErrorCallback(message, line, column);
}
