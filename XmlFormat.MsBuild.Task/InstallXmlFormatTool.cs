using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Diagnostics;

namespace XmlFormat.MsBuild.Task;
public class InstallXmlFormatTool : Microsoft.Build.Utilities.Task
{
    private bool _Success = true;

    public virtual bool Success
    {
        get { return _Success; }
        set { _Success = value; }
    }

    public override bool Execute()
    {
        Log.LogMessage(MessageImportance.High, "Formatting: Installing `xf`");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
            FileName = "dotnet",
            Arguments = "tool install -g KageKirin.XmlFormat.Tool",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

        process.WaitForExit();
        return Success;
    }
}
