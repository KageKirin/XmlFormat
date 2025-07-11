using System.CodeDom.Compiler;

namespace XmlFormat;

public static class IndentedTextWriterExtension
{
    public static void WriteTabs(this IndentedTextWriter writer) => writer.Write("");

    public static void WriteNoTabs(this IndentedTextWriter writer, string s)
    {
        var indent = writer.Indent;
        writer.Indent = 0;
        writer.Write(s);
        writer.Indent = indent;
    }

    public static void WriteLine(this IndentedTextWriter writer) => writer.WriteLine("");

    public static void WriteLineNoTabs(this IndentedTextWriter writer) => writer.WriteLineNoTabs("");
}
