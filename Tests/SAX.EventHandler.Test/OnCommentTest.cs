using XmlFormat.SAX;

namespace SAX.EventHandler.Test;

public class OnCommentTest
{
    [Theory]
    [InlineData("<!---->", "")]
    [InlineData("<!-- -->", " ")]
    [InlineData("<!--\n-->", "\n")]
    [InlineData("<!--comment-->", "comment")]
    [InlineData("<!--comment -->", "comment ")]
    [InlineData("<!-- comment-->", " comment")]
    [InlineData("<!-- comment -->", " comment ")]
    [InlineData("<!--\ncomment\nmore comment\n-->", "\ncomment\nmore comment\n")]
    [InlineData("<!--\ncomment\nmore comment\n -->", "\ncomment\nmore comment\n ")]
    [InlineData("<!-- \ncomment\nmore comment\n-->", " \ncomment\nmore comment\n")]
    [InlineData("<!-- \ncomment\nmore comment\n -->", " \ncomment\nmore comment\n ")]
    public void MatchOnCallback(string input, string expected)
    {
        DelegateXMLEventHandler handler =
            new()
            {
                OnCommentCallback = comment =>
                {
                    Assert.Equal(expected, comment);
                }
            };
        SaxParser.Parse(input, handler);
    }
}
