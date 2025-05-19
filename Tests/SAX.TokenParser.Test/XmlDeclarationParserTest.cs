using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class XmlDeclarationParserTest
{
    [Theory]
    [InlineData("<?xml?>")]
    [InlineData("<?xml ?>")]
    public void TestEmptyDeclaration(string input)
    {
        var result = XmlTokenParser.Declaration(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var declaration = result.Value;
        Assert.Null(declaration.Version);
        Assert.Null(declaration.Encoding);
        Assert.Null(declaration.Standalone);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0""?>", "1.0")]
    [InlineData(@"<?xml version=""1.0"" ?>", "1.0")]
    public void TestDeclarationVersion(string input, string expectedVersion)
    {
        var result = XmlTokenParser.Declaration(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var declaration = result.Value;
        Assert.NotNull(declaration.Version);
        TextSpan version = (TextSpan)declaration.Version!;
        Assert.NotEmpty(version.ToStringValue());
        Assert.Equal(expectedVersion, version.ToStringValue());

        Assert.Null(declaration.Encoding);
        Assert.Null(declaration.Standalone);
    }

    [Theory]
    [InlineData(@"<?xml encoding=""utf-8""?>", "utf-8")]
    [InlineData(@"<?xml encoding=""utf-8"" ?>", "utf-8")]
    public void TestDeclarationEncoding(string input, string expectedEncoding)
    {
        var result = XmlTokenParser.Declaration(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var declaration = result.Value;
        Assert.NotNull(declaration.Encoding);
        TextSpan encoding = (TextSpan)declaration.Encoding!;
        Assert.NotEmpty(encoding.ToStringValue());
        Assert.Equal(expectedEncoding, encoding.ToStringValue());

        Assert.Null(declaration.Version);
        Assert.Null(declaration.Standalone);
    }

    [Theory]
    [InlineData(@"<?xml standalone=""true""?>", "true")]
    [InlineData(@"<?xml standalone=""true"" ?>", "true")]
    public void TestDeclarationStandalone(string input, string expectedStandalone)
    {
        var result = XmlTokenParser.Declaration(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var declaration = result.Value;
        Assert.NotNull(declaration.Standalone);
        TextSpan standalone = (TextSpan)declaration.Standalone!;
        Assert.NotEmpty(standalone.ToStringValue());
        Assert.Equal(expectedStandalone, standalone.ToStringValue());

        Assert.Null(declaration.Version);
        Assert.Null(declaration.Encoding);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8""?>", "1.0", "utf-8")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" ?>", "1.0", "utf-8")]
    public void TestDeclarationVersionEncoding(string input, string expectedVersion, string expectedEncoding)
    {
        var result = XmlTokenParser.Declaration(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var declaration = result.Value;
        Assert.NotNull(declaration.Version);
        TextSpan version = (TextSpan)declaration.Version!;
        Assert.NotEmpty(version.ToStringValue());
        Assert.Equal(expectedVersion, version.ToStringValue());

        Assert.NotNull(declaration.Encoding);
        TextSpan encoding = (TextSpan)declaration.Encoding!;
        Assert.NotEmpty(encoding.ToStringValue());
        Assert.Equal(expectedEncoding, encoding.ToStringValue());

        Assert.Null(declaration.Standalone);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true""?>", "1.0", "utf-8", "true")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>", "1.0", "utf-8", "true")]
    public void TestDeclarationVersionEncodingStandalone(string input, string expectedVersion, string expectedEncoding, string expectedStandalone)
    {
        var result = XmlTokenParser.Declaration(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var declaration = result.Value;
        Assert.NotNull(declaration.Version);
        TextSpan version = (TextSpan)declaration.Version!;
        Assert.NotEmpty(version.ToStringValue());
        Assert.Equal(expectedVersion, version.ToStringValue());

        Assert.NotNull(declaration.Encoding);
        TextSpan encoding = (TextSpan)declaration.Encoding!;
        Assert.NotEmpty(encoding.ToStringValue());
        Assert.Equal(expectedEncoding, encoding.ToStringValue());

        Assert.NotNull(declaration.Standalone);
        TextSpan standalone = (TextSpan)declaration.Standalone!;
        Assert.NotEmpty(standalone.ToStringValue());
        Assert.Equal(expectedStandalone, standalone.ToStringValue());
    }
}
