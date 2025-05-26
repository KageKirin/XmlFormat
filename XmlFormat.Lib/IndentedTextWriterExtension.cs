using System.CodeDom.Compiler;

namespace XmlFormat;

public static class IndentedTextWriterExtension
{
    public static void WriteTabs(this IndentedTextWriter writer) => writer.Write("");

    public static void WriteLine(this IndentedTextWriter writer) => writer.WriteLineNoTabs("");

    public static void WriteLineNoTabs(this IndentedTextWriter writer) => writer.WriteLineNoTabs("");
}
