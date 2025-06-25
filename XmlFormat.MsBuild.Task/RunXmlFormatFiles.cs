using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace XmlFormat.MsBuild.Task;

public class RunXmlFormatFiles : Microsoft.Build.Utilities.Task
{
    public virtual string AssemblyFile { get; set; } = string.Empty;

    public virtual int LineLength { get; set; } = 0;

    public virtual string Tabs { get; set; } = string.Empty;

    private int _TabsRepeat = 0;

    public virtual int TabsRepeat
    {
        get { return _TabsRepeat; }
        set { _TabsRepeat = value; }
    }

    private int _MaxEmptyLines = 0;

    public virtual int MaxEmptyLines
    {
        get { return _MaxEmptyLines; }
        set { _MaxEmptyLines = value; }
    }

    private Microsoft.Build.Framework.ITaskItem[] _Files = [];

    public virtual Microsoft.Build.Framework.ITaskItem[] Files
    {
        get { return _Files; }
        set { _Files = value; }
    }

    private bool _Success = true;

    public virtual bool Success
    {
        get { return _Success; }
        set { _Success = value; }
    }

    private int RunCommand(string command, string arguments)
    {
        Log.LogMessage(MessageImportance.High, "Formatting: `{} {}`", command, arguments);
        Process process = new()
        {
            StartInfo = new()
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

        process.WaitForExit();
        return process.ExitCode;
    }

    public override bool Execute()
    {
        int exitCode = RunCommand("dotnet", $"\"{AssemblyFile}\" --help");
        Success = exitCode == 0;
        if (!Success)
            return Success;

        exitCode = RunCommand("dotnet", $"\"{AssemblyFile}\" --version");
        Success = exitCode == 0;
        if (!Success)
            return Success;

        string formatParam = string.Empty;

        if (LineLength > 0)
        {
            formatParam = $"/LineLength={LineLength}";
        }

        if (!string.IsNullOrEmpty(Tabs))
        {
            formatParam += (string.IsNullOrEmpty(formatParam) ? "" : ";");
            formatParam += $"/Tabs={Tabs}";
        }

        if (TabsRepeat > 0)
        {
            formatParam += (string.IsNullOrEmpty(formatParam) ? "" : ";");
            formatParam += $"/TabsRepeat={TabsRepeat}";
        }

        if (MaxEmptyLines > 0)
        {
            formatParam += (string.IsNullOrEmpty(formatParam) ? "" : ";");
            formatParam += $"/MaxEmptyLines={MaxEmptyLines}";
        }

        if (!string.IsNullOrEmpty(formatParam))
        {
            formatParam = $"--format \"{formatParam}\"";
        }

        string files = string.Join(" ", Files.Select(f => f.ItemSpec));
        exitCode = RunCommand("dotnet", $"\"{AssemblyFile}\" --inline {formatParam} {files}");
        Success = exitCode == 0;

        return Success;
    }
}
