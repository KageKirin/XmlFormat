using System.Reflection;

namespace XmlFormat.Test.Assets;

public static class EmbeddedAssets
{
    internal static string AsString(this Stream stream)
    {
        using var source = new StreamReader(stream);
        var fileContent = source.ReadToEnd();
        return fileContent;
    }

    public static Stream? GetEmbeddedResourceStream(string resourceName)
    {
        var asm = Assembly.GetExecutingAssembly();
        Assert.NotNull(asm);
        if (asm is not null)
        {
            var resourceNames = asm.GetManifestResourceNames();

            Assert.Contains(resourceName, resourceNames);

            return asm.GetManifestResourceStream(resourceName);
        }

        return null;
    }

    public static string? GetEmbeddedResourceString(string resourceName) => GetEmbeddedResourceStream(resourceName)?.AsString();
}
