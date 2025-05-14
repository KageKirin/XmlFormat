using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Superpower;
using Superpower.Display;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace XmlFormat.SAX;

public static class XmlTokenizer
{
    public enum XmlToken
    {
        /// <summary>
        /// XML declaration, i.e. <?xml ... ?>
        /// </summary>
        [Token(Example = "<?xml ... ?>")]
        Declaration,

        /// <summary>
        /// processing instruction, e.g. <?php ... ?>
        /// </summary>
        [Token(Example = "<?pi ... ?>")]
        ProcessingInstruction,

        /// <summary>
        /// regular comment
        /// possibly multiline
        /// </summary>
        [Token(Example = "<!-- ... -->")]
        Comment,

        /// <summary>
        /// CData content
        /// possibly multiline
        /// </summary>
        [Token(Example = "<![CDATA[ ... ]]>")]
        CData,

        /// <summary>
        /// XML opening <element>
        /// </summary>
        [Token(Example = "<element ... >")]
        ElementStart,

        /// <summary>
        /// XML closing </element>
        /// </summary>
        [Token(Example = "</element>")]
        ElementEnd,

        /// <summary>
        /// content text
        /// basically everything not between <>
        /// </summary>
        Content,
    }

    /// <summary>
    /// token parser for XML Declaration
    /// </summary>
    static TextParser<Unit> XmlDeclaration { get; } =
        from open in Span.EqualTo("<?xml").Try()
        from rest in Span.Except("?>").Many().Value(Unit.Value).Try()
        from close in Span.EqualTo("?>").Try()
        select Unit.Value;

    /// <summary>
    /// token parser for Processing Instructions
    /// </summary>
    static TextParser<Unit> XmlProcessingInstruction { get; } =
        from open in Span.EqualTo("<?").Try()
        from identifier in Character.LetterOrDigit.AtLeastOnce().Value(Unit.Value).Try()
        from rest in Span.Except("?>").Many().Value(Unit.Value).Try()
        from close in Span.EqualTo("?>").Try()
        select Unit.Value;

    /// <summary>
    /// token parser for Comments
    /// </summary>
    static TextParser<Unit> XmlComment { get; } =
        from open in Span.EqualTo("<!--").Try()
        from comment in Span.Except("-->").Many().Value(Unit.Value).Try()
        from close in Span.EqualTo("-->").Try()
        select Unit.Value;

    /// <summary>
    /// token parser for CData elements
    /// </summary>
    static TextParser<Unit> XmlCData { get; } =
        from open in Span.EqualTo("<![CDATA[").Try()
        from cdata in Span.Except("]]>").Many().Value(Unit.Value).Try()
        from close in Span.EqualTo("]]>").Try()
        select Unit.Value;

    /// <summary>
    /// token parser for opening <element> and empty <element/>
    /// </summary>
    static TextParser<Unit> XmlElementStartOrEmpty { get; } =
        from open in Character.EqualTo('<').Try()
        from identifier in Character.LetterOrDigit.AtLeastOnce().Value(Unit.Value).Try()
        from rest in Character.Except('>').Many().Value(Unit.Value).Try()
        from close in Character.EqualTo('>')
        select Unit.Value;

    /// <summary>
    /// token parser for closing </element>
    /// </summary>
    static TextParser<Unit> XmlElementEnd { get; } =
        from open in Span.EqualTo("</").Try()
        from identifier in Character.LetterOrDigit.AtLeastOnce().Value(Unit.Value).Try()
        from rest in Character.Except('>').Many().Value(Unit.Value).Try()
        from close in Character.EqualTo('>')
        select Unit.Value;

    /// <summary>
    /// token parser for content
    /// </summary>
    static TextParser<Unit> XmlContent { get; } =
        from content in Character.ExceptIn(['<', '>']).Many().Value(Unit.Value).Try()
        select Unit.Value;

    /// <summary>
    /// the actual tokenizer instance
    /// usage: `Instance.Tokenize(string);`
    /// </summary>
    public static Tokenizer<XmlToken> Instance { get; } =
        new TokenizerBuilder<XmlToken>()
            .Ignore(Span.WhiteSpace)
            .Match(XmlDeclaration, XmlToken.Declaration)
            .Match(XmlProcessingInstruction, XmlToken.ProcessingInstruction)
            .Match(XmlComment, XmlToken.Comment)
            .Match(XmlCData, XmlToken.CData)
            .Match(XmlElementEnd, XmlToken.ElementEnd)
            .Match(XmlElementStartOrEmpty, XmlToken.ElementStart)
            .Match(XmlContent, XmlToken.Content)
            .Build();
}
