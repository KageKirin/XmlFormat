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
}
