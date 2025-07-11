using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;
using Microsoft.Toolkit.HighPerformance;

namespace XmlFormat;

/// <summary>
/// An <see cref="XmlReadHandlerBase"/> implementation that formats and writes XML content to a stream.
/// </summary>
public class FormattingXmlReadHandler : XmlReadHandlerBase
{
    /// <summary>
    /// Gets the formatting options used by this handler.
    /// </summary>
    public FormattingOptions Options { get; private set; }

    /// <summary>
    /// Represents an XML attribute with a name and a value.
    /// </summary>
    /// <param name="Name">The name of the attribute.</param>
    /// <param name="Value">The value of the attribute.</param>
    public readonly record struct Attribute(string Name, string Value)
    {
        const string xmlns = "xmlns";
        const string colon = ":";
        const string http = "http";

        /// <summary>
        /// Compares two attributes to determine their sort order.
        /// </summary>
        /// <remarks>
        /// The comparison logic is as follows:
        /// 1. The 'xmlns' attribute comes first.
        /// 2. Namespace declarations ('xmlns:...') come next, sorted by their URI value.
        /// 3. Attributes with namespace prefixes ('ns:name') come after, sorted by name.
        /// 4. All other attributes are last, sorted by name.
        /// </remarks>
        /// <param name="lhv">The left-hand side attribute to compare.</param>
        /// <param name="rhv">The right-hand side attribute to compare.</param>
        /// <returns>An integer that indicates the relative order of the attributes being compared.</returns>
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
    private bool unhandledNewLineAfterElementStart = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormattingXmlReadHandler"/> class with a stream, encoding, and formatting options.
    /// </summary>
    /// <param name="stream">The stream to write the formatted XML to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="options">The options for formatting the XML.</param>
    public FormattingXmlReadHandler(Stream stream, Encoding encoding, FormattingOptions options)
        : this(new StreamWriter(stream, encoding) { AutoFlush = true }, options) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormattingXmlReadHandler"/> class with a stream writer and formatting options.
    /// </summary>
    /// <param name="streamWriter">The <see cref="StreamWriter"/> used to write the formatted XML.</param>
    /// <param name="options">The options for formatting the XML.</param>
    public FormattingXmlReadHandler(StreamWriter streamWriter, FormattingOptions options)
        : base(streamWriter)
    {
        this.Options = options;
        this.textWriter = new IndentedTextWriter(writer, tabString: Options.Tabs.Repeat(Options.TabsRepeat));
    }

    /// <summary>
    /// Releases the resources used by the handler.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(true);
    }

    #region IXmlReadHandler overrides
    /// <summary>
    /// Handles the XML declaration.
    /// </summary>
    /// <param name="version">The XML version.</param>
    /// <param name="encoding">The encoding declaration.</param>
    /// <param name="standalone">The standalone status declaration.</param>
    /// <param name="line">The line number where the declaration appears.</param>
    /// <param name="column">The column number where the declaration appears.</param>
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

    /// <summary>
    /// Handles a processing instruction.
    /// </summary>
    /// <param name="identifier">The processing instruction identifier.</param>
    /// <param name="contents">The content of the processing instruction.</param>
    /// <param name="line">The line number where the instruction appears.</param>
    /// <param name="column">The column number where the instruction appears.</param>
    public override void OnProcessingInstruction(ReadOnlySpan<char> identifier, ReadOnlySpan<char> contents, int line, int column)
    {
        textWriter.WriteLine(@$"<?{identifier.ToString()}{contents.ToString()}?>");
        textWriter.Flush();
    }

    /// <summary>
    /// Handles the opening of an element tag.
    /// </summary>
    /// <param name="name">The name of the element.</param>
    /// <param name="line">The line number where the element starts.</param>
    /// <param name="column">The column number where the element starts.</param>
    public virtual void OnElementOpen(ReadOnlySpan<char> name, int line, int column)
    {
        HandleNewLineAfterElementStart();

        textWriter.Write($"<{name.ToString()}");
        textWriter.Flush();
        textWriter.Indent++;
    }

    /// <summary>
    /// Handles the start of an opening element tag.
    /// </summary>
    /// <param name="name">The name of the element.</param>
    /// <param name="line">The line number where the element starts.</param>
    /// <param name="column">The column number where the element starts.</param>
    public override void OnElementStartOpen(ReadOnlySpan<char> name, int line, int column)
    {
        OnElementOpen(name, line, column);
    }

    /// <summary>
    /// Writes an element attribute, potentially across multiple lines.
    /// </summary>
    /// <param name="multiline">A value indicating whether the attribute should be written on a new line.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="value">The value of the attribute.</param>
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

    /// <summary>
    /// Handles the closing of an element's start tag.
    /// </summary>
    /// <param name="name">The name of the element.</param>
    /// <param name="line">The line number where the start tag closes.</param>
    /// <param name="column">The column number where the start tag closes.</param>
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
        textWriter.Write(">");
        textWriter.Flush();
        textWriter.Indent++;
        unhandledNewLineAfterElementStart = true;
    }

    /// <summary>
    /// Handles the opening of an empty element tag.
    /// </summary>
    /// <param name="name">The name of the empty element.</param>
    /// <param name="line">The line number where the empty element starts.</param>
    /// <param name="column">The column number where the empty element starts.</param>
    public override void OnElementEmptyOpen(ReadOnlySpan<char> name, int line, int column)
    {
        OnElementOpen(name, line, column);
    }

    /// <summary>
    /// Handles the closing of an empty element tag.
    /// </summary>
    /// <param name="name">The name of the empty element.</param>
    /// <param name="line">The line number where the empty element closes.</param>
    /// <param name="column">The column number where the empty element closes.</param>
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

    /// <summary>
    /// Handles an element's end tag.
    /// </summary>
    /// <param name="name">The name of the element.</param>
    /// <param name="line">The line number where the end tag appears.</param>
    /// <param name="column">The column number where the end tag appears.</param>
    public override void OnElementEnd(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Indent--;
        textWriter.WriteLine($"</{name.ToString()}>");
        textWriter.Flush();
    }

    /// <summary>
    /// Handles an attribute.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="nameLine">The line number of the attribute name.</param>
    /// <param name="nameColumn">The column number of the attribute name.</param>
    /// <param name="valueLine">The line number of the attribute value.</param>
    /// <param name="valueColumn">The column number of the attribute value.</param>
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

    /// <summary>
    /// Handles text content within an element.
    /// </summary>
    /// <param name="text">The text content.</param>
    /// <param name="line">The line number where the text starts.</param>
    /// <param name="column">The column number where the text starts.</param>
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

        var extraTrimText = trimText.Trim();
        if (extraTrimText.None(x => Char.IsWhiteSpace(x)))
        {
            textWriter.Write(extraTrimText.ToString());
            unhandledNewLineAfterElementStart = false;
        }
        else
        {
            HandleNewLineAfterElementStart();

            foreach (var token in extraTrimText.Tokenize('\n'))
                textWriter.WriteLine(token.Trim().ToString());
        }

        // eat trailing newlines
        // note: due to textWriter.WriteLine() above, we have to discount 1 trailing newline
        for (int i = 0; i < Math.Min(trailingNewlines - 1, Options.MaxEmptyLines); i++)
            textWriter.WriteLine();

        textWriter.Flush();
    }

    /// <summary>
    /// Handles a comment.
    /// </summary>
    /// <param name="comment">The content of the comment.</param>
    /// <param name="line">The line number where the comment appears.</param>
    /// <param name="column">The column number where the comment appears.</param>
    public override void OnComment(ReadOnlySpan<char> comment, int line, int column)
    {
        HandleNewLineAfterElementStart();

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

    /// <summary>
    /// Handles a CDATA section.
    /// </summary>
    /// <param name="cdata">The content of the CDATA section.</param>
    /// <param name="line">The line number where the CDATA section appears.</param>
    /// <param name="column">The column number where the CDATA section appears.</param>
    public override void OnCData(ReadOnlySpan<char> cdata, int line, int column)
    {
        HandleNewLineAfterElementStart();

        textWriter.WriteLine("<![CDATA[");
        textWriter.WriteLineNoTabs($"{cdata.Trim('\n').TrimEnd().ToString()}");
        textWriter.WriteTabs();
        textWriter.WriteLine("]]>");
        textWriter.Flush();
    }

    #endregion

    private void HandleNewLineAfterElementStart()
    {
        if (unhandledNewLineAfterElementStart)
        {
            textWriter.WriteLine();
            unhandledNewLineAfterElementStart = false;
        }
    }
}

/// <summary>
/// Provides extension methods for collections of <see cref="FormattingXmlReadHandler.Attribute"/>.
/// </summary>
internal static class IEnumerableOfAttributesExtensions
{
    /// <summary>
    /// Calculates the total length of attributes if they were rendered on a single line.
    /// </summary>
    /// <param name="attributes">The collection of attributes.</param>
    /// <returns>The calculated total length.</returns>
    public static int SingleLineLength(this IEnumerable<FormattingXmlReadHandler.Attribute> attributes) =>
        attributes.Sum(k => k.Name.Length + k.Value.Length + 4); //< ' =""' are the 4 extra chars
}
