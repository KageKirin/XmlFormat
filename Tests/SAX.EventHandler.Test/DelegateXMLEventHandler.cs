using System;
using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

///<summary>
/// Logs SAX events to command line
///</summary>
public class DelegateXMLEventHandler : IXMLEventHandler
{
    public Action<string, string, string> OnXmlDeclarationCallback = (_, __, ___) =>
    {
        Assert.False(true);
    };
    public Action<string> OnElementStartCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<string> OnElementEndCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<string> OnElementEmptyCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<string, string?> OnAttributeCallback = (_, __) =>
    {
        Assert.False(true);
    };
    public Action<string, string> OnProcessingInstructionCallback = (_, __) =>
    {
        Assert.False(true);
    };
    public Action<string> OnCDataCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<string> OnCommentCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<string> OnContentCallback = _ =>
    {
        Assert.False(true);
    };
    public Action<string> OnErrorCallback = _ =>
    {
        Assert.False(true);
    };

    public void OnXmlDeclaration(string version, string encoding, string standalone) => OnXmlDeclarationCallback(version, encoding, standalone);

    public void OnElementStart(string name) => OnElementStartCallback(name);

    public void OnElementEnd(string name) => OnElementEndCallback(name);

    public void OnElementEmpty(string name) => OnElementEmptyCallback(name);

    public void OnAttribute(string name, string? value) => OnAttributeCallback(name, value);

    public void OnProcessingInstruction(string name, string value) => OnProcessingInstructionCallback(name, value);

    public void OnCData(string cdata) => OnCDataCallback(cdata);

    public void OnComment(string comment) => OnCommentCallback(comment);

    public void OnContent(string text) => OnContentCallback(text);

    public void OnError(string message) => OnErrorCallback(message);
}
