using Superpower.Model;
using XmlFormat.SAX;

namespace SAX.TokenParser.Test;

public class ProcessingInstructionParserTest
{
    [Theory]
    [InlineData("<?xml?>", "xml")]
    [InlineData("<?xml ?>", "xml")]
    [InlineData("<?php?>", "php")]
    [InlineData("<?php ?>", "php")]
    [InlineData("<?pi?>", "pi")]
    [InlineData("<?pi ?>", "pi")]
    public void EmptyProcessingInstructionMatch(string input, string expected)
    {
        var result = XmlTokenParser.ProcessingInstruction(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var instruction = result.Value;
        Assert.NotEmpty(instruction.Identifier.ToStringValue());
        Assert.Equal(expected, instruction.Identifier.ToStringValue());

        Assert.NotNull(instruction.Contents);
        //Assert.Empty(instruction.Contents);
    }

    [Theory]
    [InlineData(@"<?xml version=""1.0""?>", "xml", @"version=""1.0""")]
    [InlineData(@"<?xml version=""1.0"" ?>", "xml", @"version=""1.0""")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8""?>", "xml", @"version=""1.0"" encoding=""utf-8""")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" ?>", "xml", @"version=""1.0"" encoding=""utf-8""")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true""?>", "xml", @"version=""1.0"" encoding=""utf-8"" standalone=""true""")]
    [InlineData(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""true"" ?>", "xml", @"version=""1.0"" encoding=""utf-8"" standalone=""true""")]
    [InlineData(@"<?php print(""hello world"");?>", "php", @"print(""hello world"");")]
    [InlineData(@"<?php print(""hello world""); ?>", "php", @"print(""hello world"");")]
    public void ProcessingInstructionMatch(string input, string expectedIdentifier, string expectedContents)
    {
        var result = XmlTokenParser.ProcessingInstruction(new TextSpan(input));
        Console.WriteLine($"parsing: `{input}`\nresult: {result}");
        Assert.Null(result.ErrorMessage);
        Assert.True(result.HasValue);

        var instruction = result.Value;
        Assert.NotEmpty(instruction.Identifier.ToStringValue());
        Assert.Equal(expectedIdentifier, instruction.Identifier.ToStringValue());
        Assert.NotNull(instruction.Contents);
        TextSpan contents = (TextSpan)instruction.Contents!;
        Assert.Equal(expectedContents, contents.ToStringValue().Trim());
    }
}
