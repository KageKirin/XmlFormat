using System.Diagnostics.CodeAnalysis;
using Superpower.Parsers;
using XmlFormat.SAX;
using XmlFormat.Test.Assets;

[assembly: SuppressMessage("xUnit", "xUnit1013", Justification = "Test class is correctly implementing an interface.")]

namespace SAX.EventHandler.Test;

public class ResourceTest : IXMLEventHandler
{
    [Theory]
    [InlineData("XmlFormat.Test.Assets.test.xml")]
    public void TestResource_test_xml(string resource)
    {
        var resourceContents = EmbeddedAssets.GetEmbeddedResourceString(resource);
        Assert.NotNull(resourceContents);
        Assert.NotEmpty(resourceContents);

        SaxParser.Parse(resourceContents, this);
    }

    public void OnError(string message, int line, int column)
    {
        Assert.Fail($"{message} at {line}:{column}");
    }

    public void OnXmlDeclaration(ReadOnlySpan<char> version, ReadOnlySpan<char> encoding, ReadOnlySpan<char> standalone, int line, int column)
    {
        Assert.False(version.IsEmpty);
        Assert.False(encoding.IsEmpty);
        Assert.True(standalone.IsEmpty);

        Assert.Equal("1.0", version);
        Assert.Equal("utf-8", encoding);
    }

    private static string[] startingElements = ["Window", "StackPanel", "InlineContents", "Code", "LongText",];
    private IEnumerator<string> startingElementEnumerator = ((IEnumerable<string>)startingElements).GetEnumerator();

    public void OnElementStart(ReadOnlySpan<char> name, int line, int column)
    {
        startingElementEnumerator.MoveNext();
        Assert.Equal(startingElementEnumerator.Current, name);
    }

    private static string[] endingElements = ["StackPanel", "InlineContents", "Code", "LongText", "Window",];
    private IEnumerator<string> endingElementEnumerator = ((IEnumerable<string>)endingElements).GetEnumerator();

    public void OnElementEnd(ReadOnlySpan<char> name, int line, int column)
    {
        endingElementEnumerator.MoveNext();
        Assert.Equal(endingElementEnumerator.Current, name);
    }

    private static string[] emptyElements = ["DataGrid", "EmptyElement", "Foobar", "PocketMonsters",];
    private IEnumerator<string> emptyElementEnumerator = ((IEnumerable<string>)emptyElements).GetEnumerator();

    public void OnElementEmpty(ReadOnlySpan<char> name, int line, int column)
    {
        emptyElementEnumerator.MoveNext();
        Assert.Equal(emptyElementEnumerator.Current, name);
    }

    public void OnAttribute(ReadOnlySpan<char> name, ReadOnlySpan<char> value, int nameLine, int nameColumn, int valueLine, int valueColumn) { }

    public void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column)
    {
        Assert.Fail($"encountered processing instruction {identifier} => `{contents}` at {line}:{column}");
    }

    private static string[] cdataBlocks =
    [
        @"for a in [1, 2, 3]:
      print(a)",
        "<xml>",
        @"<xml>
  </xml>",
    ];
    private IEnumerator<string> cdataBlockEnumerator = ((IEnumerable<string>)cdataBlocks).GetEnumerator();

    public void OnCData(ReadOnlySpan<char> cdata, int line, int column)
    {
        cdataBlockEnumerator.MoveNext();
        Assert.Equal(cdataBlockEnumerator.Current.Trim(), cdata.Trim());
    }

    private static string[] commentBlocks =
    [
        "main window",
        "ABC",
        @"Far out in the uncharted backwaters of the unfashionable end of the west-
ern spiral arm of the Galaxy lies a small unregarded yellow sun. Orbiting
this at a distance of roughly ninety-two million miles is an utterly insignifi-
cant little blue green planet whose ape-descended life forms are so amazingly
primitive that they still think digital watches are a pretty neat idea.
This planet has – or rather had – a problem, which was this: most of the
people on it were unhappy for pretty much of the time. Many solutions were
suggested for this problem, but most of these were largely concerned with the
movements of small green pieces of paper, which is odd because on the whole
it wasn’t the small green pieces of paper that were unhappy.
And so the problem remained; lots of the people were mean, and most of
them were miserable, even the ones with digital watches.
Many were increasingly of the opinion that they’d all made a big mistake
in coming down from the trees in the first place. And some said that even the
trees had been a bad move, and that no one should ever have left the oceans.
And then, one Thursday, nearly two thousand years after one man had
been nailed to a tree for saying how great it would be to be nice to people
for a change, one girl sitting on her own in a small cafe in Rickmansworth
suddenly realized what it was that had been going wrong all this time, and she
finally knew how the world could be made a good and happy place. This time
it was right, it would work, and no one would have to get nailed to anything.
Sadly, however, before she could get to a phone to tell anyone about it, a
terribly stupid catastrophe occurred, and the idea was lost forever.
This is not her story.
But it is the story of that terrible stupid catastrophe and some of its
consequences.
It is also the story of a book, a book called The Hitchhiker’s Guide to the
Galaxy – not an Earth book, never published on Earth, and until the terrible
catastrophe occurred, never seen or heard of by any Earthman.
Nevertheless, a wholly remarkable book.
In fact it was probably the most remarkable book ever to come out of the
great publishing houses of Ursa Minor – of which no Earthman had ever heard
either.
Not only is it a wholly remarkable book, it is also a highly successful one
– more popular than the Celestial Home Care Omnibus, better selling than
Fifty More Things to do in Zero Gravity, and more controversial than Oolon
Colluphid’s trilogy of philosophical blockbusters Where God Went Wrong,
Some More of God’s Greatest Mistakes and Who is this God Person Anyway?
In many of the more relaxed civilizations on the Outer Eastern Rim of the
Galaxy, the Hitchhiker’s Guide has already supplanted the great Encyclopedia
Galactica as the standard repository of all knowledge and wisdom, for though
it has many omissions and contains much that is apocryphal, or at least wildly
inaccurate, it scores over the older, more pedestrian work in two important
respects.
First, it is slightly cheaper; and secondly it has the words Don’t Panic
inscribed in large friendly letters on its cover.
But the story of this terrible, stupid Thursday, the story of its extraordinary
consequences, and the story of how these consequences are inextricably
intertwined with this remarkable book begins very simply.
It begins with a house.",
        @"<Xml>
  foobar
  </Xml>",
        "comment",
        "short",
    ];
    private IEnumerator<string> commentBlockEnumerator = ((IEnumerable<string>)commentBlocks).GetEnumerator();

    public void OnComment(ReadOnlySpan<char> comment, int line, int column)
    {
        commentBlockEnumerator.MoveNext();
        Assert.Equal(commentBlockEnumerator.Current.Trim(), comment.Trim());
    }

    private static string[] textBlocks =
    [
        "contents",
        @"First, it is slightly cheaper; and secondly it has the words Don’t Panic
inscribed in large friendly letters on its cover.
But the story of this terrible, stupid Thursday, the story of its extraordinary
consequences, and the story of how these consequences are inextricably
intertwined with this remarkable book begins very simply.
It begins with a house.",
    ];
    private IEnumerator<string> textBlockEnumerator = ((IEnumerable<string>)textBlocks).GetEnumerator();

    public void OnText(ReadOnlySpan<char> text, int line, int column)
    {
        textBlockEnumerator.MoveNext();
        Assert.Equal(textBlockEnumerator.Current.Trim(), text.Trim());
    }
}
