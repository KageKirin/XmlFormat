using System;
using System.Text;
using XmlFormat;

namespace SAX.Formatter.Test;

public class QuantifierTest
{
    [Theory]
    [InlineData(" ", 1)]
    [InlineData("\t", 1)]
    [InlineData("  ", 2)]
    [InlineData("\t\t", 2)]
    [InlineData("\t ", 2)]
    [InlineData(" \t ", 3)]
    [InlineData(" foobar", 1)]
    [InlineData("\tfoobar", 1)]
    [InlineData("  foobar", 2)]
    [InlineData("\t\tfoobar", 2)]
    [InlineData("\t foobar", 2)]
    [InlineData(" \t foobar", 3)]
    [InlineData("foobar ", 1)]
    [InlineData("foobar\t", 1)]
    [InlineData("foobar  ", 2)]
    [InlineData("foobar\t\t", 2)]
    [InlineData("foobar\t ", 2)]
    [InlineData("foobar \t ", 3)]
    [InlineData(" foobar ", 2)]
    [InlineData("\tfoobar\t", 2)]
    [InlineData("\tfoobar ", 2)]
    [InlineData(" \tfoobar ", 3)]
    [InlineData(" foobar\t ", 3)]
    [InlineData(" foo bar ", 3)]
    [InlineData("\tfoo bar\t", 3)]
    [InlineData("\tfoo bar ", 3)]
    [InlineData(" \tfoo bar ", 4)]
    [InlineData(" foo bar\t ", 4)]
    public void CountWhitespace(string input_, int expectedCount)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        var count = input.Count(Char.IsWhiteSpace);
        Assert.Equal(expectedCount, count);
    }

    [Theory]
    [InlineData(" ", 1)]
    [InlineData("\t", 1)]
    [InlineData("  ", 2)]
    [InlineData("\t\t", 2)]
    [InlineData("\t ", 2)]
    [InlineData(" \t ", 3)]
    [InlineData(" foobar", 1)]
    [InlineData("\tfoobar", 1)]
    [InlineData("  foobar", 2)]
    [InlineData("\t\tfoobar", 2)]
    [InlineData("\t foobar", 2)]
    [InlineData(" \t foobar", 3)]
    [InlineData("foobar ", 0)]
    [InlineData("foobar\t", 0)]
    [InlineData("foobar  ", 0)]
    [InlineData("foobar\t\t", 0)]
    [InlineData("foobar\t ", 0)]
    [InlineData("foobar \t ", 0)]
    [InlineData(" foobar ", 1)]
    [InlineData("\tfoobar\t", 1)]
    [InlineData("\tfoobar ", 1)]
    [InlineData(" \tfoobar ", 2)]
    [InlineData(" foobar\t ", 1)]
    [InlineData(" foo bar ", 1)]
    [InlineData("\tfoo bar\t", 1)]
    [InlineData("\tfoo bar ", 1)]
    [InlineData(" \tfoo bar ", 2)]
    [InlineData(" foo bar\t ", 1)]
    public void CountLeadingWhitespace(string input_, int expectedCount)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        var count = input.CountStart(Char.IsWhiteSpace);
        Assert.Equal(expectedCount, count);
    }

    [Theory]
    [InlineData(" ", 1)]
    [InlineData("\t", 1)]
    [InlineData("  ", 2)]
    [InlineData("\t\t", 2)]
    [InlineData("\t ", 2)]
    [InlineData(" \t ", 3)]
    [InlineData(" foobar", 0)]
    [InlineData("\tfoobar", 0)]
    [InlineData("  foobar", 0)]
    [InlineData("\t\tfoobar", 0)]
    [InlineData("\t foobar", 0)]
    [InlineData(" \t foobar", 0)]
    [InlineData("foobar ", 1)]
    [InlineData("foobar\t", 1)]
    [InlineData("foobar  ", 2)]
    [InlineData("foobar\t\t", 2)]
    [InlineData("foobar\t ", 2)]
    [InlineData("foobar \t ", 3)]
    [InlineData(" foobar ", 1)]
    [InlineData("\tfoobar\t", 1)]
    [InlineData("\tfoobar ", 1)]
    [InlineData(" \tfoobar ", 1)]
    [InlineData(" foobar\t ", 2)]
    [InlineData(" foo bar ", 1)]
    [InlineData("\tfoo bar\t", 1)]
    [InlineData("\tfoo bar ", 1)]
    [InlineData(" \tfoo bar ", 1)]
    [InlineData(" foo bar\t ", 2)]
    public void CountEndingWhitespace(string input_, int expectedCount)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        var count = input.CountEnd(Char.IsWhiteSpace);
        Assert.Equal(expectedCount, count);
    }

    [Theory]
    [InlineData(" ", -1)]
    [InlineData("\t", -1)]
    [InlineData("  ", -1)]
    [InlineData("\t\t", -1)]
    [InlineData("\t ", -1)]
    [InlineData(" \t ", -1)]
    [InlineData(" foobar", 1)]
    [InlineData("\tfoobar", 1)]
    [InlineData("  foobar", 2)]
    [InlineData("\t\tfoobar", 2)]
    [InlineData("\t foobar", 2)]
    [InlineData(" \t foobar", 3)]
    [InlineData("foobar ", 0)]
    [InlineData("foobar\t", 0)]
    [InlineData("foobar  ", 0)]
    [InlineData("foobar\t\t", 0)]
    [InlineData("foobar\t ", 0)]
    [InlineData("foobar \t ", 0)]
    [InlineData(" foobar ", 1)]
    [InlineData("\tfoobar\t", 1)]
    [InlineData("\tfoobar ", 1)]
    [InlineData(" \tfoobar ", 2)]
    [InlineData(" foobar\t ", 1)]
    [InlineData(" foo bar ", 1)]
    [InlineData("\tfoo bar\t", 1)]
    [InlineData("\tfoo bar ", 1)]
    [InlineData(" \tfoo bar ", 2)]
    [InlineData(" foo bar\t ", 1)]
    public void IndexOfNonWhitespace(string input_, int expectedCount)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        var count = input.IndexOf(c => !Char.IsWhiteSpace(c));
        Assert.Equal(expectedCount, count);
    }
}
