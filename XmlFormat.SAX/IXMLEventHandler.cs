using System;

namespace XmlFormat.SAX;

///<summary>
/// SAX event handler interface
///</summary>
public interface IXMLEventHandler
{
    void OnXmlDeclaration(string version, string encoding, string standalone);

    void OnElementStart(string name);
    void OnElementEnd(string name);
    void OnElementEmpty(string name);
    void OnAttribute(string name, string? value);

    void OnProcessingInstruction(string identifier, string contents);
    void OnCData(string cdata);
    void OnComment(string comment);
    void OnContent(string text);

    void OnError(string message);
}
