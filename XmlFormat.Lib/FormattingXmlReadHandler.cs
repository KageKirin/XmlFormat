using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;
using TurboXml;

namespace XmlFormat;

public class FormattingXmlReadHandler : XmlReadHandlerBase
{
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

    public FormattingXmlReadHandler(Stream stream, Encoding encoding)
        : this(new StreamWriter(stream, encoding) { AutoFlush = true, }) { }

    public FormattingXmlReadHandler(StreamWriter streamWriter)
        : base(streamWriter)
    {
        this.textWriter = new IndentedTextWriter(writer, tabString: "".PadLeft(2)); //TODO param
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
        textWriter.Write(@$"<?xml version=""{version}"" encoding=""{encoding}""");

        if (!standalone.IsEmpty)
        {
            textWriter.Write(@$" standalone=""{standalone}""");
        }

        textWriter.WriteLine(" ?>");
        textWriter.WriteLine("");
    }

    public override void OnBeginTag(ReadOnlySpan<char> name, int line, int column)
    {
        if (requireClosingPreviousElementTag)
            OnBeginTagClose();

        requireClosingPreviousElementTag = true;
        textWriter.Write($"<{name}");
    }

    public virtual void OnBeginTagClose()
    {
        if (currentAttributes != null)
        {
            currentAttributes.Sort(Attribute.Compare);
            if (currentAttributes.SingleLineLength() > 80) //TODO param
            {
                textWriter.WriteLine("");
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
            textWriter.WriteLine(">");
            requireClosingPreviousElementTag = false;
        }

        textWriter.Indent++;
    }

    public override void OnEndTagEmpty()
    {
        requireClosingPreviousElementTag = false;
        bool multiLineAttributes = currentAttributes?.SingleLineLength() > 80; // TODO param
        OnBeginTagClose();
        textWriter.Indent--;

        textWriter.WriteLine($"{(multiLineAttributes ? "" : " ")}/>");
    }

    public override void OnEndTag(ReadOnlySpan<char> name, int line, int column)
    {
        textWriter.Indent--;
        textWriter.WriteLine($"</{name}>");
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
        if (requireClosingPreviousElementTag)
            OnBeginTagClose();

        var trimText = text.ToString().Trim();
        if (!string.IsNullOrEmpty(trimText))
            writer.WriteLine(trimText);
        if (text.ToString().Split('\n').Length > 2) //< 1 empty line = 2x \n, 2 empty lines = 3x \n
            writer.WriteLine("");
    }

    public override void OnComment(ReadOnlySpan<char> comment, int line, int column)
    {
        if (requireClosingPreviousElementTag)
            OnBeginTagClose();

        if (comment.Length < 80) // TODO param
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
    }

    public override void OnCData(ReadOnlySpan<char> cdata, int line, int column)
    {
        if (requireClosingPreviousElementTag)
            OnBeginTagClose();

        textWriter.Indent++;
        textWriter.Write("<![CDATA[");
        textWriter.WriteLine(cdata);
        textWriter.WriteLine("]]>");
        textWriter.Indent--;
    }

    #endregion
}

internal static class IEnumerableOfAttributesExtensions
{
    public static int SingleLineLength(
        this IEnumerable<FormattingXmlReadHandler.Attribute> attributes
    ) => attributes.Sum(k => k.Name.Length + k.Value.Length + 4); //< ' =""' are the 4 extra chars
}
