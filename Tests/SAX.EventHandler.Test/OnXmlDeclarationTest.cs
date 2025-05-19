using Superpower.Parsers;
using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

public class OnXmlDeclarationTest
{
    [Theory]
    [InlineData("<?xml?>")]
    [InlineData("<?xml ?>")]
    public void MatchOnCallbackEmptyHeader(string input)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnXmlDeclarationCallback = (version, encoding, standalone) =>
                {
                    Assert.NotNull(version);
                    Assert.Empty(version);

                    Assert.NotNull(encoding);
                    Assert.Empty(encoding);

                    Assert.NotNull(standalone);
                    Assert.Empty(standalone);
                }
            };
        SaxParser.Parse(input, handler);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0""?>", "1.0")]
    [InlineData(@"<?xml version=""1.0"" ?>", "1.0")]
    public void MatchOnCallbackWithVersion(string input, string expected)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnXmlDeclarationCallback = (version, encoding, standalone) =>
                {
                    Assert.NotNull(version);
                    Assert.NotEmpty(version);
                    Assert.Equal(expected, version);

                    Assert.NotNull(encoding);
                    Assert.Empty(encoding);

                    Assert.NotNull(standalone);
                    Assert.Empty(standalone);
                }
            };
        SaxParser.Parse(input, handler);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8""?>", "1.0", "utf-8")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" ?>", "1.0", "utf-8")]
    public void MatchOnCallbackWithVersionAndEncoding(string input, string expectedVersion, string expectedEncoding)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnXmlDeclarationCallback = (version, encoding, standalone) =>
                {
                    Assert.NotNull(version);
                    Assert.NotEmpty(version);
                    Assert.Equal(expectedVersion, version);

                    Assert.NotNull(encoding);
                    Assert.NotEmpty(encoding);
                    Assert.Equal(expectedEncoding, encoding);

                    Assert.NotNull(standalone);
                    Assert.Empty(standalone);
                }
            };
        SaxParser.Parse(input, handler);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true""?>", "1.0", "utf-8", "true")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>", "1.0", "utf-8", "true")]
    public void MatchOnCallbackWithVersionEncodingAndStandalone(string input, string expectedVersion, string expectedEncoding, string expectedStandalone)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnXmlDeclarationCallback = (version, encoding, standalone) =>
                {
                    Assert.NotNull(version);
                    Assert.NotEmpty(version);
                    Assert.Equal(expectedVersion, version);

                    Assert.NotNull(encoding);
                    Assert.NotEmpty(encoding);
                    Assert.Equal(expectedEncoding, encoding);

                    Assert.NotNull(standalone);
                    Assert.NotEmpty(standalone);
                    Assert.Equal(expectedStandalone, standalone);
                }
            };
        SaxParser.Parse(input, handler);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone?>", "1.0", "utf-8")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone ?>", "1.0", "utf-8")]
    public void MatchOnCallbackWithVersionEncodingAndEmptyStandalone(string input, string expectedVersion, string expectedEncoding)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnXmlDeclarationCallback = (version, encoding, standalone) =>
                {
                    Assert.NotNull(version);
                    Assert.NotEmpty(version);
                    Assert.Equal(expectedVersion, version);

                    Assert.NotNull(encoding);
                    Assert.NotEmpty(encoding);
                    Assert.Equal(expectedEncoding, encoding);

                    Assert.NotNull(standalone);
                    Assert.Empty(standalone);
                }
            };
        SaxParser.Parse(input, handler);
    }
}
