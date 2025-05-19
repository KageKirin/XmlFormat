using System;
using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

///<summary>
/// Logs SAX events to command line
///</summary>
public class DelegateXMLEventHandler : IXMLEventHandler
{
    public Action<ReadOnlySpan<char>, ReadOnlySpan<char>, ReadOnlySpan<char>> OnXmlDeclarationCallback = (_, __, ___) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnElementStartCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnElementEndCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnElementEmptyCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, ReadOnlySpan<char>> OnAttributeCallback = (_, __) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>, ReadOnlySpan<char>> OnProcessingInstructionCallback = (_, __) =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnCDataCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnCommentCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnContentCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<ReadOnlySpan<char>> OnErrorCallback = _ =>
    {
        Assert.False(true);
    };

    public void OnXmlDeclaration(ReadOnlySpan<char> version, ReadOnlySpan<char> encoding, ReadOnlySpan<char> standalone) => OnXmlDeclarationCallback(version, encoding, standalone);

    public void OnElementStart(ReadOnlySpan<char> name) => OnElementStartCallback(name);

    public void OnElementEnd(ReadOnlySpan<char> name) => OnElementEndCallback(name);

    public void OnElementEmpty(ReadOnlySpan<char> name) => OnElementEmptyCallback(name);

    public void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value) => OnAttributeCallback(name, value);

    public void OnProcessingInstruction(ReadOnlySpan<char> name, ReadOnlySpan<char> value) => OnProcessingInstructionCallback(name, value);

    public void OnCData(ReadOnlySpan<char> cdata) => OnCDataCallback(cdata);

    public void OnComment(ReadOnlySpan<char> comment) => OnCommentCallback(comment);

    public void OnContent(ReadOnlySpan<char> text) => OnContentCallback(text);

    public void OnError(ReadOnlySpan<char> message) => OnErrorCallback(message);
}
