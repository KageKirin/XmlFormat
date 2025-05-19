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

    public static TextParser<char> XmlChar { get; } =
        Character.LetterOrDigit.Or(Character.EqualTo(':')).Or(Character.EqualTo('_')).Or(Character.EqualTo('-'));

    public static TextParser<TextSpan> XmlChars { get; } =
        Span.WithAll(ch => !Char.IsWhiteSpace(ch) && (Char.IsLetterOrDigit(ch) || ch == '_' || ch == '-' || ch == ':'));
}
