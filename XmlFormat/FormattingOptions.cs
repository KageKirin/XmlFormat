namespace XmlFormat;

/// <summary>
/// Defines formatting options for the XML formatter.
/// </summary>
/// <param name="LineLength">The maximum number of characters per line.</param>
/// <param name="Tabs">The whitespace character(s) to use for indentation.</param>
/// <param name="TabsRepeat">The number of indentation characters to use per level.</param>
/// <param name="MaxEmptyLines">The maximum number of consecutive empty lines.</param>
/// <param name="MaxWordsPerLine">The maximum words in an element's text to keep it on a single line.</param>
public record class FormattingOptions(
    int LineLength = 120,
    string Tabs = " ",
    int TabsRepeat = 1,
    int MaxEmptyLines = 2,
    int MaxWordsPerLine = 4
);
