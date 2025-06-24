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

    public override bool Execute()
    {
        int exitCode = RunCommand("dotnet", "tool install -g KageKirin.XmlFormat.Tool");
        Success = exitCode == 0;
        if (!Success)
            return Success;

        // wait a few moments to finalize install
        // this avoids the subsequent commands not finding `xf`
        Thread.Sleep(5000); //< time in ms

        exitCode = RunCommand("xf", "--help");
        Success = exitCode == 0;
        if (!Success)
            return Success;

        exitCode = RunCommand("xf", "--version");
        Success = exitCode == 0;
        if (!Success)
            return Success;
        Success = RunCommand("xf", "--version") == 0;
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
        RunCommand("xf", $"--inline --format \"{formatParam}\" {files}");
        Success = exitCode == 0;

        return Success;
    }
}
