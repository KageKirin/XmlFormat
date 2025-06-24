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
        return Success;
    }
}
