using XmlFormat.SAX;
using XmlFormat.Test.Assets;

namespace SAX.Tokenizer.Test;

public class ResourceTokenizerTest
{
    [Theory]
    [InlineData("XmlFormat.Test.Assets.test.xml")]
    public void TestResource_test_xml(string resource)
    {
        var resourceContents = EmbeddedAssets.GetEmbeddedResourceString(resource);
        Assert.NotNull(resourceContents);
        Assert.NotEmpty(resourceContents);
        Assert.True(
            TestHelper.Tokenize(
                resourceContents,
                [
                    XmlTokenizer.XmlToken.Declaration,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.CData,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.CData,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.CData,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Content
                ]
            )
        );
    }
}
