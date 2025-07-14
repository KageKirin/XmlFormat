using System;
using System.Linq;
using System.Text;
using XmlFormat;

namespace SAX.Formatter.Test;

public class TokenizerTest
{
    [Theory]
    [InlineData(" ", 0)]
    [InlineData("\t", 0)]
    [InlineData("  ", 0)]
    [InlineData("\t\t", 0)]
    [InlineData("\t ", 0)]
    [InlineData(" \t ", 0)]
    [InlineData(" foobar", 1)]
    [InlineData("\tfoobar", 1)]
    [InlineData("  foobar", 1)]
    [InlineData("\t\tfoobar", 1)]
    [InlineData("\t foobar", 1)]
    [InlineData(" \t foobar", 1)]
    [InlineData("foobar ", 1)]
    [InlineData("foobar\t", 1)]
    [InlineData("foobar  ", 1)]
    [InlineData("foobar\t\t", 1)]
    [InlineData("foobar\t ", 1)]
    [InlineData("foobar \t ", 1)]
    [InlineData(" foobar ", 1)]
    [InlineData("\tfoobar\t", 1)]
    [InlineData("\tfoobar ", 1)]
    [InlineData(" \tfoobar ", 1)]
    [InlineData(" foobar\t ", 1)]
    [InlineData(" foo bar ", 2)]
    [InlineData("\tfoo bar\t", 2)]
    [InlineData("\tfoo bar ", 2)]
    [InlineData(" \tfoo bar ", 2)]
    [InlineData(" foo bar\t ", 2)]
    public void CountTokens(string input_, int expectedCount)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        int count = 0;
        var tokens = input.Tokenize(Char.IsWhiteSpace);
        while (tokens.MoveNext())
        {
            Console.WriteLine($"token[{count}]: {tokens.Current}");
            count++;
        }
        Assert.Equal(expectedCount, count);
    }

    [Theory]
    [InlineData(" foobar", "foobar")]
    [InlineData("\tfoobar", "foobar")]
    [InlineData("  foobar", "foobar")]
    [InlineData("\t\tfoobar", "foobar")]
    [InlineData("\t foobar", "foobar")]
    [InlineData(" \t foobar", "foobar")]
    [InlineData("foobar ", "foobar")]
    [InlineData("foobar\t", "foobar")]
    [InlineData("foobar  ", "foobar")]
    [InlineData("foobar\t\t", "foobar")]
    [InlineData("foobar\t ", "foobar")]
    [InlineData("foobar \t ", "foobar")]
    [InlineData(" foobar ", "foobar")]
    [InlineData("\tfoobar\t", "foobar")]
    [InlineData("\tfoobar ", "foobar")]
    [InlineData(" \tfoobar ", "foobar")]
    [InlineData(" foobar\t ", "foobar")]
    [InlineData(" foo bar ", "foo")]
    [InlineData("\tfoo bar\t", "foo")]
    [InlineData("\tfoo bar ", "foo")]
    [InlineData(" \tfoo bar ", "foo")]
    [InlineData(" foo bar\t ", "foo")]
    public void CompareFirstToken(string input_, string expected_)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        ReadOnlySpan<char> expected = expected_.AsSpan();

        var tokens = input.Tokenize(Char.IsWhiteSpace);
        bool hasToken = tokens.MoveNext();
        Assert.True(hasToken);
        Assert.Equal(expected, tokens.Current);
    }

    [Theory]
    [InlineData(" foo bar ", "foo", "bar")]
    [InlineData("\tfoo bar\t", "foo", "bar")]
    [InlineData("\tfoo bar ", "foo", "bar")]
    [InlineData(" \tfoo bar ", "foo", "bar")]
    [InlineData(" foo bar\t ", "foo", "bar")]
    public void CompareTwoTokens(string input_, string expected0_, string expected1_)
    {
        ReadOnlySpan<char> input = input_.AsSpan();
        ReadOnlySpan<char> expected0 = expected0_.AsSpan();
        ReadOnlySpan<char> expected1 = expected1_.AsSpan();

        var tokens = input.Tokenize(Char.IsWhiteSpace);
        bool hasToken = tokens.MoveNext();
        Assert.True(hasToken);
        Assert.Equal(expected0, tokens.Current);
        hasToken = tokens.MoveNext();
        Assert.True(hasToken);
        Assert.Equal(expected1, tokens.Current);
        hasToken = tokens.MoveNext();
        Assert.False(hasToken);
    }
}
