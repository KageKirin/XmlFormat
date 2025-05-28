using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;

namespace XmlFormat;

public class FormattingXmlReadHandler : XmlReadHandlerBase
{
    public FormattingOptions Options { get; private set; }

    public readonly record struct Attribute(string Name, string Value)
    {
        const string xmlns = "xmlns";
        const string colon = ":";
        const string http = "http";

        public static int Compare(Attribute lhv, Attribute rhv)
        {
            // 'xmlns' must come first
            if (lhv.Name == xmlns)
                return -1;
            if (rhv.Name == xmlns)
                return 1;

            if (lhv.Name.StartsWith(xmlns) && rhv.Name.StartsWith(xmlns))
            {
                if (lhv.Value.StartsWith(http) && rhv.Value.StartsWith(http))
                    return lhv.Value.CompareTo(rhv.Value);

                if (lhv.Value.StartsWith(http))
                    return -1;
                if (rhv.Value.StartsWith(http))
                    return 2;

                return lhv.Name.CompareTo(rhv.Name);
            }

            if (lhv.Name.StartsWith(xmlns))
                return -1;
            if (rhv.Name.StartsWith(xmlns))
                return 1;

            // namespaces 'ns:name' must come second
            if (lhv.Name.Contains(colon) && rhv.Name.Contains(colon))
                return lhv.Name.CompareTo(rhv.Name);

            if (lhv.Name.Contains(colon))
                return -1;
            if (rhv.Name.Contains(colon))
                return 1;

            return lhv.Name.CompareTo(rhv.Name);
        }
    }

    private readonly IndentedTextWriter textWriter;

    private bool requireClosingPreviousElementTag = false;
    private List<Attribute>? currentAttributes = default;

    public FormattingXmlReadHandler(Stream stream, Encoding encoding, FormattingOptions options)
        : this(new StreamWriter(stream, encoding) { AutoFlush = true, }, options) { }

    public FormattingXmlReadHandler(StreamWriter streamWriter, FormattingOptions options)
        : base(streamWriter)
    {
        this.Options = options;
        this.textWriter = new IndentedTextWriter(writer, tabString: Options.Tabs.Repeat(Options.TabsRepeat));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(true);
    }

    #region IXmlReadHandler overrides
    public override void OnXmlDeclaration(
        ReadOnlySpan<char> version,
        ReadOnlySpan<char> encoding,
        ReadOnlySpan<char> standalone,
        int line,
        int column
    )
    {
        textWriter.WriteNoTabs(@$"<?xml");

        if (!version.IsEmpty)
        {
            textWriter.WriteNoTabs(@$" version=""{version}""");
        }

        if (!encoding.IsEmpty)
        {
            textWriter.WriteNoTabs(@$" encoding=""{encoding}""");
        }

        if (!standalone.IsEmpty)
        {
            textWriter.WriteNoTabs(@$" standalone=""{standalone}""");
        }

        textWriter.WriteLineNoTabs(" ?>");
        textWriter.Flush();
    }

    public override void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column)
    {
        textWriter.WriteLine(@$"<?{identifier}{contents}?>");
        textWriter.Flush();
    }

    public virtual void OnElementOpen(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Write($"<{name}");
        textWriter.Flush();
        textWriter.Indent++;
    }

    public override void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Write($"<{name}");
        textWriter.Flush();
        textWriter.Indent++;
    }

    public override void OnElementStartClose(ReadOnlySpan<char> name, int line, int column)
    {
        bool inline = false;
        if (currentAttributes != null)
        {
            currentAttributes.Sort(Attribute.Compare);
            if (currentAttributes.SingleLineLength() > Options.LineLength)
            {
                textWriter.WriteLine();
                textWriter.Indent++;
                foreach (var attribute in currentAttributes)
                {
                    textWriter.WriteLine(@$"{attribute.Name}=""{attribute.Value}""");
                }
                textWriter.Indent--;
            }
            else
            {
                foreach (var attribute in currentAttributes)
                {
                    textWriter.Write(@$" {attribute.Name}=""{attribute.Value}""");
                }
            }
            currentAttributes = null;
        }

        if (requireClosingPreviousElementTag)
        {
            if (inline)
                textWriter.Write(">");
            else
                textWriter.WriteLine(">");
            requireClosingPreviousElementTag = false;
        }

        textWriter.Indent++;
        textWriter.Flush();
    }

    public override void OnElementEmptyOpen(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Write($"<{name}");
        textWriter.Flush();
        textWriter.Indent++;
    }

    public override void OnElementEmptyClose(ReadOnlySpan<char> name, int line, int column)
    {
        requireClosingPreviousElementTag = false;
        bool multiLineAttributes = currentAttributes?.SingleLineLength() > Options.LineLength;
        OnElementStartClose();
        textWriter.Indent--;

        textWriter.WriteLine($"{(multiLineAttributes ? "" : " ")}/>");
        textWriter.Flush();
    }

    public override void OnElementEnd(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Indent--;
        textWriter.WriteLine($"</{name}>");
        textWriter.Flush();
    }

    public override void OnAttribute(
        ReadOnlySpan<char> name,
        ReadOnlySpan<char> value,
        int nameLine,
        int nameColumn,
        int valueLine,
        int valueColumn
    )
    {
        currentAttributes ??= [];
        currentAttributes.Add(new(name.ToString(), value.ToString()));
    }

    public override void OnText(ReadOnlySpan<char> text, int line, int column)
    {
        bool inlineContents = text.ToString().Split('\n').Length <= 2; //< 1 empty line = 2x \n, 2 empty lines = 3x \n
        bool addExtraLine = text.ToString().Split('\n').Length > 2; //< 1 empty line = 2x \n, 2 empty lines = 3x \n

        if (requireClosingPreviousElementTag)
            OnElementStartClose(inlineContents);

        var trimText = text.ToString().Trim();
        if (!string.IsNullOrEmpty(trimText))
        {
            if (inlineContents)
                textWriter.Write(trimText);
            else
                textWriter.WriteLine(trimText);
        }

        if (addExtraLine)
            writer.WriteLine("");
    }

    public override void OnComment(ReadOnlySpan<char> comment, int line, int column)
    {
        if (requireClosingPreviousElementTag)
            OnElementStartClose();

        if (comment.Length < Options.LineLength)
        {
            textWriter.WriteLine($"<!-- {comment.Trim()} -->");
        }
        else
        {
            textWriter.WriteLine("<!--");
            textWriter.Indent++;
            textWriter.WriteLine(comment.Trim());
            textWriter.Indent--;
            textWriter.WriteLine("-->");
        }
        textWriter.Flush();
    }

    public override void OnCData(ReadOnlySpan<char> cdata, int line, int column)
    {
        if (requireClosingPreviousElementTag)
            OnElementStartClose();

        textWriter.WriteLine("<![CDATA[");
        textWriter.WriteLineNoTabs(cdata.ToString().Trim('\n').TrimEnd());
        textWriter.WriteTabs();
        textWriter.WriteLine("]]>");
        textWriter.Flush();
    }

    #endregion
}

internal static class IEnumerableOfAttributesExtensions
{
    public static int SingleLineLength(this IEnumerable<FormattingXmlReadHandler.Attribute> attributes) =>
        attributes.Sum(k => k.Name.Length + k.Value.Length + 4); //< ' =""' are the 4 extra chars
}
