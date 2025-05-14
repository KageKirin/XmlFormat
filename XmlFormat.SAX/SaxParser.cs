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
                        tempDeclaration.Value.Version?.ToStringValue() ?? string.Empty,
                        tempDeclaration.Value.Encoding?.ToStringValue() ?? string.Empty,
                        tempDeclaration.Value.Standalone?.ToStringValue() ?? string.Empty
                    );
                    break;

                case XmlTokenizer.XmlToken.ProcessingInstruction:
                    var tempPI = XmlTokenParser.ProcessingInstruction(token.Span);
                    handler.OnProcessingInstruction(
                        tempPI.Value.Identifier.ToStringValue(),
                        tempPI.Value.Contents?.ToStringValue() ?? string.Empty
                    );
                    break;

                case XmlTokenizer.XmlToken.Comment:
                    var tempComment = XmlTokenParser.TrimComment(token.Span);
                    handler.OnComment(tempComment.Value.ToStringValue());
                    break;

                case XmlTokenizer.XmlToken.CData:
                    var tempCData = XmlTokenParser.TrimCData(token.Span);
                    handler.OnCData(tempCData.Value.ToStringValue());
                    break;

                case XmlTokenizer.XmlToken.ElementStart:
                    var tempElementStart = XmlTokenParser.ElementStart(token.Span);
                    handler.OnElementStart(tempElementStart.Value.Identifier.ToStringValue());
                    foreach (var attribute in tempElementStart.Value.Attributes)
                    {
                        handler.OnAttribute(attribute.Name.ToStringValue(), attribute.Value?.ToStringValue());
                    }
                    break;

                case XmlTokenizer.XmlToken.ElementEmpty:
                    var tempElementEmpty = XmlTokenParser.ElementEmpty(token.Span);
                    handler.OnElementEmpty(tempElementEmpty.Value.Identifier.ToStringValue());
                    foreach (var attribute in tempElementEmpty.Value.Attributes)
                    {
                        handler.OnAttribute(attribute.Name.ToStringValue(), attribute.Value?.ToStringValue());
                    }
                    break;

                case XmlTokenizer.XmlToken.ElementEnd:
                    var tempElementEnd = XmlTokenParser.ElementEnd(token.Span);
                    handler.OnElementEnd(tempElementEnd.Value.ToStringValue());
                    break;

                case XmlTokenizer.XmlToken.Content:
                    handler.OnContent(token.Span.ToStringValue());
                    break;

                default:
                    handler.OnError(token.Span.ToStringValue());
                    break;
            }
        }
    }
}
