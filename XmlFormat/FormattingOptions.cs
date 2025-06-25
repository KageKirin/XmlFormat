namespace XmlFormat;

public record class FormattingOptions(int LineLength = 120, string Tabs = " ", int TabsRepeat = 1, int MaxEmptyLines = 1);
