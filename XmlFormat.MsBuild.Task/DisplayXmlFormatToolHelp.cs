using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;

namespace XmlFormat.MsBuild.Task;
public class DisplayXmlFormatToolHelp : Microsoft.Build.Utilities.Task
{
    private bool _Success = true;

    public virtual bool Success
    {
        get { return _Success; }
        set { _Success = value; }
    }

    public override bool Execute()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Checking `xf` help");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = "xf",
            Arguments = "--help",
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
