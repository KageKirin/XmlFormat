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
    public virtual int LineLength { get; set; } = 0;

    public virtual string Tabs { get; set; } = string.Empty;

    public virtual int TabsRepeat { get; set; } = 0;

    public virtual int MaxEmptyLines { get; set; } = 0;

    [Required]
    public virtual Microsoft.Build.Framework.ITaskItem[] Files { get; set; } = [];

    public virtual bool Success { get; set; } = true;

    private FormattingOptions ReadOptions()
    {
        FormattingOptions formattingOptions = new();

        if (LineLength > 0)
        {
            formattingOptions = formattingOptions with { LineLength = LineLength };
        }

        if (!string.IsNullOrEmpty(Tabs))
        {
            formattingOptions = formattingOptions with { Tabs = Tabs };
        }

        if (TabsRepeat > 0)
        {
            formattingOptions = formattingOptions with { TabsRepeat = TabsRepeat };
        }

        if (MaxEmptyLines > 0)
        {
            formattingOptions = formattingOptions with { MaxEmptyLines = MaxEmptyLines };
        }

        return formattingOptions;
    }

    public override bool Execute()
    {
        FormattingOptions formattingOptions = ReadOptions();
        Log.LogMessage($"Formatting with options: {formattingOptions}");

        foreach (string fileName in Files.Select(item => item.ItemSpec))
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Log.LogError($"{fileName} does not exist.");
                Success = false;
            }
            else
            {
                var xml = File.ReadAllText(fileName);
                if (string.IsNullOrEmpty(xml))
                {
                    Log.LogWarning($"{fileName} is empty.");
                }
                else
                {
                    try
                    {
                        Log.LogMessage(importance: MessageImportance.High, $"Formatting: {fileName}");
                        File.WriteAllText(fileName, contents: XmlFormat.Format(xml, formattingOptions));
                    }
                    catch (Exception ex)
                    {
                        Log.LogErrorFromException(ex);
                        Success = false;
                        break;
                    }
                }
            }
        }

        return Success;
    }
}
