using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

    public bool ExecuteInstallXf()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Installing `xf`");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = "dotnet",
            Arguments = "tool install -g KageKirin.XmlFormat.Tool",
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
}
