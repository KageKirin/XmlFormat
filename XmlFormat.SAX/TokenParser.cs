using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Superpower;
using Superpower.Display;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace XmlFormat.SAX;

public static class XmlTokenParser
{
    public static TextParser<TextSpan> Trim(string left, string right) =>
        (TextSpan input) =>
        {
            TextSpan trimmed = input.Trim(left, right);
            TextSpan remainder =
                new(
                    trimmed.Source!,
                    new Position(
                        trimmed.Position.Absolute + trimmed.Length + right.Length,
                        trimmed.Position.Line,
                        trimmed.Position.Column + trimmed.Length + right.Length
                    ),
                    0
                );
            return Result.Value(trimmed, input, remainder);
        };

    public static TextParser<TextSpan> TrimCData { get; } = Trim("<![CDATA[", "]]>");
    public static TextParser<TextSpan> TrimComment { get; } = Trim("<!--", "-->");

    public static TextParser<char> XmlChar { get; } =
        Character.LetterOrDigit.Or(Character.EqualTo(':')).Or(Character.EqualTo('_')).Or(Character.EqualTo('-'));

    public static TextParser<TextSpan> XmlChars { get; } =
        Span.WithAll(ch => !Char.IsWhiteSpace(ch) && (Char.IsLetterOrDigit(ch) || ch == '_' || ch == '-' || ch == ':'));

    public static TextParser<TextSpan> QuotedStringWithQuotes { get; } =
        from lq in Span.EqualTo("\"")
        from qt in Span.Except("\"").Optional()
        from rq in Span.EqualTo("\"")
        select new TextSpan(
            lq.Source!,
            new Position(lq.Position.Absolute, lq.Position.Line, lq.Position.Column),
            2 + (qt?.Length ?? 0) //rq.Position.Absolute - lq.Position.Absolute + 1
        );

    public static TextParser<TextSpan> QuotedString { get; } =
        from qs in QuotedStringWithQuotes
        select qs.Trim("\"", "\"");

    internal static TextParser<TextSpan> ElementIdentifier { get; } =
        from identifier in Character.EqualTo('<').IgnoreThen(XmlChars)
        select identifier;

    public static TextParser<TextSpan> ElementIdentifierForUnitTestsOnly
    {
        get => ElementIdentifier;
    }

    public record struct Attribute(TextSpan Name, TextSpan? Value = default);

    internal static TextParser<Attribute> ElementAttribute { get; } =
        from identifier in XmlChars
        from value in Span.EqualTo("=").IgnoreThen(QuotedString).Optional()
        select new Attribute(identifier, value);

    public static TextParser<Attribute> ElementAttributeForUnitTestsOnly
    {
        get => ElementAttribute;
    }

    public static TextParser<Attribute[]> ManyElementAttributesForUnitTestsOnly { get; } = ElementAttribute.Many();

    public record struct Element(TextSpan Identifier, Attribute[] Attributes);

    public static TextParser<Element> ElementAndAttributesForUnitTestsOnly { get; } =
        from identifier in ElementIdentifier
        from attributes in Character.WhiteSpace.Many().IgnoreThen(ElementAttribute.Many())
        select new Element(identifier, attributes);

    public static TextParser<Element> ElementStart { get; } =
        from identifier in ElementIdentifier
        from attributes in Character.WhiteSpace.Many().IgnoreThen(ElementAttribute.Many())
        from closing in Character.WhiteSpace.Many().IgnoreThen(Character.EqualTo('>'))
        select new Element(identifier, attributes);
}
