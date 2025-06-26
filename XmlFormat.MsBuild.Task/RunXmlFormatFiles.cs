using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace XmlFormat.MsBuild.Task;

public class RunXmlFormatFiles : Microsoft.Build.Utilities.Task
{
    public bool UseLocalConfig { get; set; } = false;

    public virtual int LineLength { get; set; } = 0;

    public virtual string Tabs { get; set; } = string.Empty;

    public virtual int TabsRepeat { get; set; } = 0;

    public virtual int MaxEmptyLines { get; set; } = 0;

    [Required]
    public virtual Microsoft.Build.Framework.ITaskItem[] Files { get; set; } = [];

    public virtual bool Success { get; set; } = true;

    private FormattingOptions ReadOptions()
    {
        Dictionary<string, string?> memoryOptions = [];

        if (LineLength > 0)
        {
            memoryOptions["lineLength"] = LineLength.ToString();
        }

        if (!string.IsNullOrEmpty(Tabs))
        {
            memoryOptions["tabs"] = Tabs;
        }

        if (TabsRepeat > 0)
        {
            memoryOptions["tabsRepeat"] = TabsRepeat.ToString();
        }

        if (MaxEmptyLines > 0)
        {
            memoryOptions["maxEmptyLines"] = MaxEmptyLines.ToString();
        }

        ConfigurationBuilder configBuilder = new();
        configBuilder.AddInMemoryCollection(memoryOptions);

        if (UseLocalConfig)
        {
            configBuilder
                .AddTomlFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xmlformat.toml"), optional: false, reloadOnChange: true)
                .AddTomlFile(Path.Combine(Environment.CurrentDirectory, ".xmlformat"), optional: true, reloadOnChange: true);
        }

        IConfiguration config = configBuilder.Build();

        FormattingOptions formattingOptions = new();
        config.Bind(formattingOptions);

        return formattingOptions;
    }

    public override bool Execute()
    {
        FormattingOptions formattingOptions = ReadOptions();
        Log.LogMessage("Formatting with options: {}", formattingOptions);

        foreach (string fileName in Files.Select(item => item.ItemSpec))
        {
            if (string.IsNullOrEmpty(fileName))
                continue;

            if (!File.Exists(fileName))
            {
                Log.LogError($"{fileName} does not exist.");
                Success = false;
                continue;
            }

            var xml = File.ReadAllText(fileName);
            if (string.IsNullOrWhiteSpace(xml))
            {
                Log.LogWarning($"{fileName} is empty.");
                continue;
            }

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

        return Success;
    }
}
