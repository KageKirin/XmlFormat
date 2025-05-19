using Superpower.Model;

namespace XmlFormat.SAX;

public static class SaxParser
{
    public static void Parse(string xml, IXMLEventHandler handler)
    {
        foreach (var token in XmlTokenizer.Instance.Tokenize(xml))
        {
            switch (token.Kind)
            {
                case XmlTokenizer.XmlToken.Declaration:
                    var tempDeclaration = XmlTokenParser.Declaration(token.Span);
                    handler.OnXmlDeclaration(
                        tempDeclaration.Value.Version == null
                            ? ReadOnlySpan<char>.Empty
                            : ((TextSpan)tempDeclaration.Value.Version!).ToReadOnlySpan(),
                        tempDeclaration.Value.Encoding == null
                            ? ReadOnlySpan<char>.Empty
                            : ((TextSpan)tempDeclaration.Value.Encoding!).ToReadOnlySpan(),
                        tempDeclaration.Value.Standalone == null
                            ? ReadOnlySpan<char>.Empty
                            : ((TextSpan)tempDeclaration.Value.Standalone!).ToReadOnlySpan()
                    );
                    break;

                case XmlTokenizer.XmlToken.ProcessingInstruction:
                    var tempPI = XmlTokenParser.ProcessingInstruction(token.Span);
                    handler.OnProcessingInstruction(
                        tempPI.Value.Identifier.ToReadOnlySpan(),
                        tempPI.Value.Contents == null ? ReadOnlySpan<char>.Empty : ((TextSpan)tempPI.Value.Contents!).ToReadOnlySpan()
                    );
                    break;

                case XmlTokenizer.XmlToken.Comment:
                    var tempComment = XmlTokenParser.TrimComment(token.Span);
                    handler.OnComment(tempComment.Value.ToReadOnlySpan());
                    break;

                case XmlTokenizer.XmlToken.CData:
                    var tempCData = XmlTokenParser.TrimCData(token.Span);
                    handler.OnCData(tempCData.Value.ToReadOnlySpan());
                    break;

                case XmlTokenizer.XmlToken.ElementStart:
                    var tempElementStart = XmlTokenParser.ElementStart(token.Span);
                    handler.OnElementStart(tempElementStart.Value.Identifier.ToReadOnlySpan());
                    foreach (var attribute in tempElementStart.Value.Attributes)
                    {
                        handler.OnAttribute(
                            attribute.Name.ToReadOnlySpan(),
                            attribute.Value == null ? ReadOnlySpan<char>.Empty : ((TextSpan)attribute.Value!).ToReadOnlySpan()
                        );
                    }
                    break;

                case XmlTokenizer.XmlToken.ElementEmpty:
                    var tempElementEmpty = XmlTokenParser.ElementEmpty(token.Span);
                    handler.OnElementEmpty(tempElementEmpty.Value.Identifier.ToReadOnlySpan());
                    foreach (var attribute in tempElementEmpty.Value.Attributes)
                    {
                        handler.OnAttribute(
                            attribute.Name.ToReadOnlySpan(),
                            attribute.Value == null ? ReadOnlySpan<char>.Empty : ((TextSpan)attribute.Value!).ToReadOnlySpan()
                        );
                    }
                    break;

                case XmlTokenizer.XmlToken.ElementEnd:
                    var tempElementEnd = XmlTokenParser.ElementEnd(token.Span);
                    handler.OnElementEnd(tempElementEnd.Value.ToReadOnlySpan());
                    break;

                case XmlTokenizer.XmlToken.Content:
                    handler.OnContent(token.Span.ToReadOnlySpan());
                    break;

                default:
                    handler.OnError(token.Span.ToReadOnlySpan());
                    break;
            }
        }
    }
}
