using Superpower.Model;

namespace XmlFormat.SAX;

public static class TextSpanExtensions
{
    public static TextSpan Trim(this TextSpan span, string chars) => span.Trim(chars, chars);

    public static TextSpan Trim(this TextSpan span, string left, string right) =>
        new(
            span.Source!,
            new Position(span.Position.Absolute + left.Length, span.Position.Line, span.Position.Column + left.Length),
            span.Length - left.Length - right.Length
        );
}
