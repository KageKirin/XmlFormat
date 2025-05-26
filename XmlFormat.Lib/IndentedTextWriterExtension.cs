using System.CodeDom.Compiler;

namespace XmlFormat;

public static class IndentedTextWriterExtension
{
    public static void WriteTabs(this IndentedTextWriter writer) => writer.Write("");
}
