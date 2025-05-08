using System.Diagnostics;
using System.Text;
using CommandLine;
using TurboXml;
using XmlFormat;

namespace XmlFormat.Tool;

public class Program
{
    public class Options
    {
        [Option('i', "inline", Required = false, HelpText = "Process input file inline.")]
        public bool Inline { get; set; }

        [Value(0, MetaName = "input", HelpText = "Input file.")]
        public string? InputFile { get; set; }
    }

    public static void Main(string[] args)
    {
        CommandLine
            .Parser.Default.ParseArguments<Options>(args) //
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
    }

    static void RunOptions(Options options)
    {
        Console.WriteLine($"options.Inline: {options.Inline}");
        Console.WriteLine($"options.InputFile: {options.InputFile}");

        using var istream = OpenInputStreamOrStdIn(options);
        using var ostream = OpenOutputStreamOrStdOut(options);
        using var handler = new XmlReadHandlerBase(stream: ostream, encoding: Encoding.UTF8);
        XmlParser.Parse(istream, handler);
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
    }

    static Stream OpenInputStreamOrStdIn(Options options)
    {
        if (string.IsNullOrEmpty(options.InputFile))
        {
            if (options.Inline)
            {
                throw new InvalidOperationException("option -i/--inline requires an input file");
            }

            MemoryStream stream = new();
            Console.OpenStandardInput().CopyTo(stream);
            stream.Position = 0;
            return stream;
        }

        return File.Open(options.InputFile, FileMode.Open, FileAccess.Read);
    }

    static Stream OpenOutputStreamOrStdOut(Options options)
    {
        if (string.IsNullOrEmpty(options.InputFile))
        {
            return Console.OpenStandardOutput();
        }

        return File.Open(options.InputFile + ".tmp", FileMode.Create, FileAccess.Write);
    }
}
