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
                Assert.True(tokens.ElementAt(i).HasValue);
                Assert.Equal(expectedTypes[i], tokens.ElementAt(i).Kind);
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
