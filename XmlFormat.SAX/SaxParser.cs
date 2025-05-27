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
                        tempDeclaration.Value.Version == null ? [] : ((TextSpan)tempDeclaration.Value.Version!).ToReadOnlySpan(),
                        tempDeclaration.Value.Encoding == null ? [] : ((TextSpan)tempDeclaration.Value.Encoding!).ToReadOnlySpan(),
                        tempDeclaration.Value.Standalone == null ? [] : ((TextSpan)tempDeclaration.Value.Standalone!).ToReadOnlySpan(),
                        token.Span.Position.Line,
                        token.Span.Position.Column
                    );
                    break;

                case XmlTokenizer.XmlToken.ProcessingInstruction:
                    var tempPI = XmlTokenParser.ProcessingInstruction(token.Span);
                    handler.OnProcessingInstruction(
                        tempPI.Value.Identifier.ToReadOnlySpan(),
                        tempPI.Value.Contents == null ? [] : ((TextSpan)tempPI.Value.Contents!).ToReadOnlySpan(),
                        token.Span.Position.Line,
                        token.Span.Position.Column
                    );
                    break;

                case XmlTokenizer.XmlToken.Comment:
                    var tempComment = XmlTokenParser.TrimComment(token.Span);
                    handler.OnComment(tempComment.Value.ToReadOnlySpan(), token.Span.Position.Line, token.Span.Position.Column);
                    break;

                case XmlTokenizer.XmlToken.CData:
                    var tempCData = XmlTokenParser.TrimCData(token.Span);
                    handler.OnCData(tempCData.Value.ToReadOnlySpan(), token.Span.Position.Line, token.Span.Position.Column);
                    break;

                case XmlTokenizer.XmlToken.ElementStart:
                    var tempElementStart = XmlTokenParser.ElementStart(token.Span);
                    handler.OnElementStartOpen(
                        tempElementStart.Value.Identifier.ToReadOnlySpan(),
                        token.Span.Position.Line,
                        token.Span.Position.Column
                    );
                    foreach (var attribute in tempElementStart.Value.Attributes)
                    {
                        handler.OnAttribute(
                            attribute.Name.ToReadOnlySpan(),
                            attribute.Value == null ? [] : ((TextSpan)attribute.Value!).ToReadOnlySpan(),
                            attribute.Name.Position.Line,
                            attribute.Name.Position.Column,
                            attribute.Value == null ? -1 : ((TextSpan)attribute.Value!).Position.Line,
                            attribute.Value == null ? -1 : ((TextSpan)attribute.Value!).Position.Column
                        );
                    }
                    handler.OnElementStartClose(
                        tempElementStart.Value.Identifier.ToReadOnlySpan(),
                        token.Span.Position.Line,
                        token.Span.Position.Column + token.Span.Length - 1
                    );
                    break;

                case XmlTokenizer.XmlToken.ElementEmpty:
                    var tempElementEmpty = XmlTokenParser.ElementEmpty(token.Span);
                    handler.OnElementEmptyOpen(
                        tempElementEmpty.Value.Identifier.ToReadOnlySpan(),
                        token.Span.Position.Line,
                        token.Span.Position.Column
                    );
                    foreach (var attribute in tempElementEmpty.Value.Attributes)
                    {
                        handler.OnAttribute(
                            attribute.Name.ToReadOnlySpan(),
                            attribute.Value == null ? [] : ((TextSpan)attribute.Value!).ToReadOnlySpan(),
                            attribute.Name.Position.Line,
                            attribute.Name.Position.Column,
                            attribute.Value == null ? -1 : ((TextSpan)attribute.Value!).Position.Line,
                            attribute.Value == null ? -1 : ((TextSpan)attribute.Value!).Position.Column
                        );
                    }
                    handler.OnElementEmptyClose(
                        tempElementEmpty.Value.Identifier.ToReadOnlySpan(),
                        token.Span.Position.Line,
                        token.Span.Position.Column + token.Span.Length - 1
                    );
                    break;

                case XmlTokenizer.XmlToken.ElementEnd:
                    var tempElementEnd = XmlTokenParser.ElementEnd(token.Span);
                    handler.OnElementEnd(tempElementEnd.Value.ToReadOnlySpan(), token.Span.Position.Line, token.Span.Position.Column);
                    break;

                case XmlTokenizer.XmlToken.Content:
                    handler.OnText(token.Span.ToReadOnlySpan(), token.Span.Position.Line, token.Span.Position.Column);
                    break;

                default:
                    handler.OnError(token.ToString(), token.Span.Position.Line, token.Span.Position.Column);
                    break;
            }
        }
    }
}
