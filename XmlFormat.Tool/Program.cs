using System.Diagnostics;
using System.Text;
using CommandLine;
using Microsoft.Extensions.Configuration;
using XmlFormat;

namespace XmlFormat.Tool;

public class Program
{
    public class Options
    {
        [Option('i', "inline", Required = false, HelpText = "Process input files inline.")]
        public bool Inline { get; set; } = false;

        [Option('p', "profile", Required = false, HelpText = "Specify the XML formatting profile to use instead of the file extension.")]
        public string? Profile { get; set; } = default;

        [Option(
            'f',
            "format",
            Required = false,
            Separator = ';',
            HelpText = "Specify formatting options to override the configuration. Use ';' as separator."
        )]
        public IEnumerable<string>? FormattingOptions { get; set; } = default;

        [Value(0, MetaName = "inputs", HelpText = "Input files.")]
        public IEnumerable<string>? InputFiles { get; set; } = default;
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
        IConfiguration config = new ConfigurationBuilder()
            .AddTomlFile(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "xmlformat.toml"), optional: false, reloadOnChange: true)
            .AddTomlFile(Path.Join(Environment.CurrentDirectory, ".xmlformat"), optional: true, reloadOnChange: true)
            .AddCommandLine(options.FormattingOptions?.ToArray() ?? [])
            .Build();

        Console.WriteLine($"options.Inline: {options.Inline}");
        Console.WriteLine($"options.InputFiles: {options.InputFiles}");

        FormattingOptions formattingOptions = new(240, " ", 4, 2);
        config.Bind(formattingOptions);
        Console.WriteLine($"formattingOptions: {formattingOptions}");

        var inputFiles = options.InputFiles ?? [""];
        if (inputFiles?.Count() == 0)
            inputFiles = [""];

        foreach (var inputFile in inputFiles!)
        {
            FormattingOptions actualFormattingOptions = formattingOptions with { };
            string? profile = options.Profile ?? Path.GetExtension(inputFile)?.Trim('.');
            Console.WriteLine($"profile: {profile}");

            if (!string.IsNullOrEmpty(profile))
            {
                var configSection = config.GetSection(profile);
                configSection.Bind(actualFormattingOptions);
            }
            Console.WriteLine($"actual formattingOptions: {actualFormattingOptions}");

            using (Stream istream = OpenInputStreamOrStdIn(inputFile, options.Inline))
            using (Stream ostream = OpenOutputStreamOrStdOut(inputFile, options.Inline))
            {
                XmlFormat.Format(istream, ostream, options: actualFormattingOptions);
            }

            if (options.Inline)
            {
                File.Copy(inputFile + ".tmp", inputFile, overwrite: true);
                File.Delete(inputFile + ".tmp");
            }
        }
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        //handle errors
    }

    static Stream OpenInputStreamOrStdIn(string? inputFile, bool inline)
    {
        if (string.IsNullOrEmpty(inputFile))
        {
            if (inline)
            {
                throw new InvalidOperationException("option -i/--inline requires at least one input file.");
            }

            MemoryStream stream = new();
            Console.OpenStandardInput().CopyTo(stream);
            stream.Position = 0;
            return stream;
        }

        return File.Open(inputFile, FileMode.Open, FileAccess.Read);
    }

    static Stream OpenOutputStreamOrStdOut(string? inputFile, bool inline)
    {
        if (string.IsNullOrEmpty(inputFile) || !inline)
        {
            return Console.OpenStandardOutput();
        }

        return File.Open(inputFile + ".tmp", FileMode.Create, FileAccess.Write);
    }
}
