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
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.ElementEmpty,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.CData,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.ElementStart,
                    XmlTokenizer.XmlToken.Content,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.CData,
                    XmlTokenizer.XmlToken.CData,
                    XmlTokenizer.XmlToken.ElementEnd,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Comment,
                    XmlTokenizer.XmlToken.Comment,
                ]
            )
        );
    }
}
