using Superpower.Parsers;
using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

public class OnProcessingInstructionTest
{
    [Theory]
    //[InlineData("<?php?>", "php")]
    [InlineData("<?php ?>", "php")]
    //[InlineData("<?pi?>", "pi")]
    [InlineData("<?pi ?>", "pi")]
    public void MatchOnCallbackEmptyContents(string input, string expected)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnProcessingInstructionCallback = (identifier, contents) =>
                {
                    Assert.Equal(expected, identifier);
                    Assert.False(contents.IsEmpty);
                }
            };
        SaxParser.Parse(input, handler);
    }

    [Theory]
    [InlineData(@"<?php print(""hello world"");?>", "php", @"print(""hello world"");")]
    [InlineData(@"<?php print(""hello world""); ?>", "php", @"print(""hello world"");")]
    public void MatchOnCallback(string input, string expectedIdentifier, string expectedContents)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnProcessingInstructionCallback = (identifier, contents) =>
                {
                    Assert.Equal(expectedIdentifier, identifier);
                    Assert.False(contents.IsEmpty);
                    Assert.Equal(expectedContents, contents.Trim());
                }
            };
        SaxParser.Parse(input, handler);
    }
}
