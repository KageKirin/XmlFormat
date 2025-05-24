using System;
using System.Text;
using XmlFormat;
using XmlFormat.Test.Assets;

namespace SAX.Formatter.Test;

public class ResourceFormatterTest
{
    static FormattingOptions formattingOptions = new(120, "  ", 1);

    [Theory]
    [InlineData("XmlFormat.Test.Assets.idtest.xml")]
    public void IdentityTest_idtest_xml(string resource)
    {
        var resourceContents = EmbeddedAssets.GetEmbeddedResourceString(resource);
        Assert.NotNull(resourceContents);
        Assert.NotEmpty(resourceContents);

        var formatted = XmlFormat.XmlFormat.Format(resourceContents, formattingOptions);
        Assert.NotNull(formatted);
        Assert.NotEmpty(resourceContents);
        Assert.Equal(resourceContents, formatted);
    }
}
