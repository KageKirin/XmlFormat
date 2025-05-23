using XmlFormat.SAX;
using XmlFormat.Test.Assets;

namespace SAX.Tokenizer.Test;

public class ResourceTokenizerValueTest
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
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.Declaration, "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.Comment, "<!-- main window -->"),
                    new TestHelper.TokenTypeAndValue(
                        XmlTokenizer.XmlToken.ElementStart,
                        "<Window \n        xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\nxmlns=\"https://github.com/avaloniaui\"\n        xmlns:vm=\"using:GitRise\"\n\n        mc:Ignorable=\"d\" d:DesignWidth=\"800\" d:DesignHeight=\"450\"\n\n        x:DataType=\"vm:MainWindowViewModel\"\n        x:Class=\"GitRise.MainWindow\"\n        xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n\n        Title=\"GitRise\"\n        Icon=\"avares://GitRise/Resources/GitRise.ico\"\n        xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n        >"
                    ),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.Comment, "<!-- ABC -->"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementStart, "<StackPanel>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEmpty, "<DataGrid AutoGenerateColumns=\"True\" ItemsSource=\"{CompiledBinding Commits}\" />"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEnd, "</StackPanel>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEmpty, "<EmptyElement />"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEmpty, "<Foobar Hoge=\"True\" />"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementStart, "<InlineContents>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.Content, "contents"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEnd, "</InlineContents>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEmpty, "<PocketMonsters Rhinocerox=\"Ground\" Elephallanx=\"Plant\" Alligatorade=\"Water\" Pandandelion=\"Plant\" Porcupinion=\"Metal\" Capybarista=\"Psycho\" />"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementStart, "<Code>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.CData, "<![CDATA[\n    for a in [1, 2, 3]:\n      print(a)\n  ]]>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEnd, "</Code>"),
                    new TestHelper.TokenTypeAndValue(
                        XmlTokenizer.XmlToken.Comment,
                        @"<!--
  Far out in the uncharted backwaters of the unfashionable end of the west-
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
It begins with a house.
  -->"
                    ),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementStart, "<LongText>"),
                    new TestHelper.TokenTypeAndValue(
                        XmlTokenizer.XmlToken.Content,
                        @"First, it is slightly cheaper; and secondly it has the words Don’t Panic
inscribed in large friendly letters on its cover.
But the story of this terrible, stupid Thursday, the story of its extraordinary
consequences, and the story of how these consequences are inextricably
intertwined with this remarkable book begins very simply.
It begins with a house.
  "
                    ),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEnd, "</LongText>"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.CData, "<![CDATA[<xml>]]>"),
                    new TestHelper.TokenTypeAndValue(
                        XmlTokenizer.XmlToken.CData,
                        @"<![CDATA[
  <xml>
  </xml>
  ]]>"
                    ),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.ElementEnd, "</Window>"),
                    new TestHelper.TokenTypeAndValue(
                        XmlTokenizer.XmlToken.Comment,
                        @"<!--
  <Xml>
  foobar
  </Xml>
-->"
                    ),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.Comment, "<!-- comment -->"),
                    new TestHelper.TokenTypeAndValue(XmlTokenizer.XmlToken.Comment, "<!--short-->"),
                ]
            )
        );
    }
}
