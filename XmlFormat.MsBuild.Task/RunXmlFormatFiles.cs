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
    private int _LineLength;

    public virtual int LineLength
    {
        get { return _LineLength; }
        set { _LineLength = value; }
    }

    private string _Tabs;

    public virtual string Tabs
    {
        get { return _Tabs; }
        set { _Tabs = value; }
    }

    private int _TabsRepeat;

    public virtual int TabsRepeat
    {
        get { return _TabsRepeat; }
        set { _TabsRepeat = value; }
    }

    private int _MaxEmptyLines;

    public virtual int MaxEmptyLines
    {
        get { return _MaxEmptyLines; }
        set { _MaxEmptyLines = value; }
    }

    private Microsoft.Build.Framework.ITaskItem[] _Files;

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

    public bool ExecuteInstallXf()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Installing `xf`");
        Success = RunCommand("dotnet", "tool install -g KageKirin.XmlFormat.Tool") == 0;
        return Success;
    }

    public bool ExecuteXfHelp()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Checking `xf` help");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = "xf",
            Arguments = "--help",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

        process.WaitForExit();
        Success = process.ExitCode == 0;
        return Success;
    }

    public bool ExecuteXfVersion()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Checking `xf` version");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = "xf",
            Arguments = "--version",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

        process.WaitForExit();
        Success = process.ExitCode == 0;
        return Success;
    }

    public bool ExecuteXf()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Running `xf`");

        string formatParam = string.Empty;

        if (LineLength > 0)
        {
            formatParam = $"/LineLength={LineLength}";
        }

        if (Tabs is not null)
        {
            formatParam += (string.IsNullOrEmpty(formatParam) ? "" : ";");
            formatParam += $"/Tabs='{Tabs}'";
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

        string files = string.Join(" ", Files.Select(f => f.ItemSpec));
        string arguments = $"--inline --format \"{formatParam}\" {files}";
        Log.LogMessage(MessageImportance.High, "Formatting: Running `xf {arguments}`");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = "xf",
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

        process.WaitForExit();
        Success = process.ExitCode == 0;
        return Success;
    }

    public override bool Execute()
    {
        return ExecuteInstallXf() //
            && ExecuteXfHelp()
            && ExecuteXfVersion()
            && ExecuteXf();
    }
}
