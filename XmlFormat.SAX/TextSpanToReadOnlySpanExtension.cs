using System;
using Superpower.Model;

namespace XmlFormat.SAX;

public static class TextSpanToReadOnlySpanExtension
{
    public static ReadOnlySpan<char> ToReadOnlySpan(this TextSpan span) => span.Source!.AsSpan().Slice(span.Position.Absolute, span.Length);
}
