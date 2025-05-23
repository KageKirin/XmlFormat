using XmlFormat.SAX;

public static class TestHelper
{
    public static bool Tokenize(string input, XmlTokenizer.XmlToken[] expectedTypes)
    {
        Assert.NotNull(input);
        Assert.NotEmpty(input);

        try
        {
            var tokens = XmlTokenizer.Instance.Tokenize(input);
            Assert.Equal(expectedTypes.Length, tokens.Count());
            for (int i = 0; i < expectedTypes.Length; i++)
            {
                var token = tokens.ElementAt(i);
                Assert.True(token.HasValue);
                Assert.Equal(expectedTypes[i], token.Kind);
                Console.WriteLine($"{token.Span.Position.Line}:{token.Span.Position.Column} {token.Kind} '{token.Span.ToStringValue()}'");
            }

            return expectedTypes.Length == tokens.Count();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"exception: {ex}");
            return false;
        }
    }

    public record struct TokenTypeAndValue(XmlTokenizer.XmlToken Token, string Value);

    public static bool Tokenize(string input, TokenTypeAndValue[] expectedTokens)
    {
        Assert.NotNull(input);
        Assert.NotEmpty(input);

        try
        {
            var tokens = XmlTokenizer.Instance.Tokenize(input);
            Assert.Equal(expectedTokens.Length, tokens.Count());
            for (int i = 0; i < expectedTokens.Length; i++)
            {
                var token = tokens.ElementAt(i);
                Assert.True(token.HasValue);
                Assert.Equal(expectedTokens[i].Token, token.Kind);
                Assert.Equal(expectedTokens[i].Value, token.Span.ToStringValue());
                Console.WriteLine($"{token.Span.Position.Line}:{token.Span.Position.Column} {token.Kind} '{token.Span.ToStringValue()}'");
            }

            return expectedTokens.Length == tokens.Count();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"exception: {ex}");
            return false;
        }
    }
}
