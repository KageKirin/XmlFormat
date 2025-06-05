using XmlFormat.SAX;

public static class TestHelper
{
    public static bool Tokenize(string input, XmlTokenizer.XmlToken[] expectedTypes)
    {
        Assert.NotNull(input);
        Assert.NotEmpty(input);

        Assert.NotRaisedAny(
            _ => { },
            _ => { },
            () =>
            {
                XmlTokenizer.Instance.Tokenize(input);
            }
        );

        try
        {
            var tokens = XmlTokenizer.Instance.Tokenize(input);
            Assert.Equal(expectedTypes.Length, tokens.Count());
            for (int i = 0; i < expectedTypes.Length; i++)
            {
                var token = tokens.ElementAt(i);
                Assert.True(token.HasValue);
                Console.WriteLine($"{token.Span.Position.Line}:{token.Span.Position.Column} {token.Kind} '{token.Span.ToStringValue()}'");
                if (expectedTypes[i] != token.Kind)
                    Console.WriteLine($"expected: {expectedTypes[i]}, got {token.Kind})");
                Assert.Equal(expectedTypes[i], token.Kind);
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

        Assert.NotRaisedAny(
            _ => { },
            _ => { },
            () =>
            {
                XmlTokenizer.Instance.Tokenize(input);
            }
        );

        try
        {
            var tokens = XmlTokenizer.Instance.Tokenize(input);
            Assert.Equal(expectedTokens.Length, tokens.Count());
            for (int i = 0; i < expectedTokens.Length; i++)
            {
                var token = tokens.ElementAt(i);
                Assert.True(token.HasValue);
                Console.WriteLine($"{token.Span.Position.Line}:{token.Span.Position.Column} {token.Kind} '{token.Span.ToStringValue()}'");
                if (expectedTokens[i].Token != token.Kind || expectedTokens[i].Value != token.Span.ToStringValue().Trim(' '))
                {
                    Console.WriteLine($"expected: {expectedTokens[i].Token}, got {token.Kind}");
                    Console.WriteLine($"expected: '{expectedTokens[i].Value}', got '{token.Span.ToStringValue()}'");
                }
                Assert.Equal(expectedTokens[i].Token, token.Kind);
                Assert.Equal(expectedTokens[i].Value, token.Span.ToStringValue().Trim(' '));
            }

            return expectedTokens.Length == tokens.Count();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"exception: {ex}");
            return false;
        }
    }

    public static bool FailTokenize(string input, XmlTokenizer.XmlToken[] expectedTypes)
    {
        Assert.NotNull(input);
        Assert.NotEmpty(input);

        Assert.ThrowsAny<Superpower.ParseException>(() =>
        {
            XmlTokenizer.Instance.Tokenize(input);
        });

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

    public static bool FailTokenize(string input, TokenTypeAndValue[] expectedTokens)
    {
        Assert.NotNull(input);
        Assert.NotEmpty(input);

        Assert.ThrowsAny<Superpower.ParseException>(() =>
        {
            XmlTokenizer.Instance.Tokenize(input);
        });

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
