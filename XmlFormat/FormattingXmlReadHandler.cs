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
            textWriter.WriteNoTabs(@$" version=""{version.ToString()}""");
        }

        if (!encoding.IsEmpty)
        {
            textWriter.WriteNoTabs(@$" encoding=""{encoding.ToString()}""");
        }

        if (!standalone.IsEmpty)
        {
            textWriter.WriteNoTabs(@$" standalone=""{standalone.ToString()}""");
        }

        textWriter.WriteLineNoTabs(" ?>");
        textWriter.Flush();
    }

    public override void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column)
    {
        textWriter.WriteLine(@$"<?{identifier.ToString()}{contents.ToString()}?>");
        textWriter.Flush();
    }

    public virtual void OnElementOpen(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Write($"<{name.ToString()}");
        textWriter.Flush();
        textWriter.Indent++;
    }

    public override void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column)
    {
        OnElementOpen(name, line, column);
    }

    public virtual void OnWriteElementAttribute(bool multiline, ReadOnlySpan<char> name, ReadOnlySpan<char> value)
    {
        if (multiline)
        {
            if (value.IsEmpty)
                textWriter.WriteLine(@$"{name.ToString()}");
            else
                textWriter.WriteLine(@$"{name.ToString()}=""{value.ToString()}""");
        }
        else
        {
            if (value.IsEmpty)
                textWriter.Write(@$" {name.ToString()}");
            else
                textWriter.Write(@$" {name.ToString()}=""{value.ToString()}""");
        }
    }

    public override void OnElementStartClose(ReadOnlySpan<char> name, int line, int column)
    {
        if (currentAttributes != null)
        {
            bool multiline = currentAttributes.SingleLineLength() > Options.LineLength;

            // newline after element start open
            if (multiline)
                textWriter.WriteLine();

            currentAttributes.Sort(Attribute.Compare);
            foreach (var attribute in currentAttributes)
            {
                OnWriteElementAttribute(multiline: multiline, name: attribute.Name.AsSpan(), value: attribute.Value.AsSpan());
            }

            currentAttributes = null;
        }

        textWriter.Indent--;
        textWriter.WriteLine(">");
        textWriter.Flush();
        textWriter.Indent++;
    }

    public override void OnElementEmptyOpen(ReadOnlySpan<char> name, int line, int column)
    {
        OnElementOpen(name, line, column);
    }

    public override void OnElementEmptyClose(ReadOnlySpan<char> name, int line, int column)
    {
        bool multiline = false;
        if (currentAttributes != null)
        {
            multiline = currentAttributes.SingleLineLength() > Options.LineLength;
            // newline after element empty open
            if (multiline)
                textWriter.WriteLine();

            currentAttributes.Sort(Attribute.Compare);
            foreach (var attribute in currentAttributes)
            {
                OnWriteElementAttribute(multiline: multiline, name: attribute.Name.AsSpan(), value: attribute.Value.AsSpan());
            }

            currentAttributes = null;
        }

        textWriter.Indent--;
        textWriter.WriteLine($"{(multiline ? "" : " ")}/>");
        textWriter.Flush();
    }

    public override void OnElementEnd(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Indent--;
        textWriter.WriteLine($"</{name.ToString()}>");
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
        var trimText = text.Trim(c => char.IsWhiteSpace(c) && c != '\n');

        if (trimText.IsEmpty)
            return;

        int totalNewlines = trimText.Count(c => c == '\n');
        int leadingNewlines = trimText.CountStart(c => c == '\n');
        int trailingNewlines = trimText.CountEnd(c => c == '\n');

        // eat newlines: text is just newlines, and reduce any amount of newlines to max 2
        if (trimText.IsWhiteSpace())
        {
            for (int i = 0; i < Math.Min(totalNewlines - 1, Options.MaxEmptyLines); i++)
                textWriter.WriteLineNoTabs();

            return;
        }

        // eat leading newlines
        for (int i = 0; i < Math.Min(leadingNewlines - 1, Options.MaxEmptyLines); i++)
            textWriter.WriteLineNoTabs();

        textWriter.WriteLine(trimText.Trim().ToString());

        // eat trailing newlines
        // note: due to textWriter.WriteLine() above, we have to discount 1 trailing newline
        for (int i = 0; i < Math.Min(trailingNewlines - 1, Options.MaxEmptyLines); i++)
            textWriter.WriteLine();

        textWriter.Flush();
    }

    public override void OnComment(ReadOnlySpan<char> comment, int line, int column)
    {
        if (comment.Length < Options.LineLength)
        {
            textWriter.WriteLine($"<!-- {comment.Trim().ToString()} -->");
        }
        else
        {
            textWriter.WriteLine("<!--");
            textWriter.Indent++;
            textWriter.WriteLine(comment.Trim().ToString());
            textWriter.Indent--;
            textWriter.WriteLine("-->");
        }
        textWriter.Flush();
    }

    public override void OnCData(ReadOnlySpan<char> cdata, int line, int column)
    {
        textWriter.WriteLine("<![CDATA[");
        textWriter.WriteLineNoTabs($"{cdata.Trim('\n').TrimEnd().ToString()}");
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
