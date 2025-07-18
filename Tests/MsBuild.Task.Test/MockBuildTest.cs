using Microsoft.Build.Framework;
using Moq;
using XmlFormat.MsBuild.Task;

[assembly: CaptureConsole(CaptureError = false, CaptureOut = false)]

namespace MsBuild.Task.Test;

public class MockBuildTest
{
    private Mock<IBuildEngine> buildEngine = new();
    private List<BuildErrorEventArgs> errors = [];

    public MockBuildTest()
    {
        buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));
    }

    [Fact]
    public void EmptyFiles()
    {
        //Arrange
        RunXmlFormatFiles xmlFormatFilesTask = new() { Files = [], BuildEngine = buildEngine.Object };

        //Act
        var success = xmlFormatFilesTask.Execute();

        //Assert
        Assert.True(success);
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("a.xml")]
    [InlineData("b.xml")]
    public void ValidFile(string file)
    {
        string filePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, file);
        Assert.True(Path.Exists(filePath), $"{filePath} doesn't exist.");
        Console.WriteLine($"filePath: {filePath}");

        //Arrange
        var item = new Mock<ITaskItem>();
        item.Setup(x => x.ItemSpec).Returns(filePath);
        item.Setup(x => x.GetMetadata("FullPath")).Returns(filePath);
        item.Setup(x => x.MetadataCount).Returns(1);
        item.Setup(x => x.MetadataNames).Returns(new string[] { "FullPath" });
        Console.WriteLine($"item.Object.ItemSpec: {item.Object.ItemSpec}");

        RunXmlFormatFiles xmlFormatFilesTask = new() { Files = [item.Object], BuildEngine = buildEngine.Object };

        //Act
        var success = xmlFormatFilesTask.Execute();

        //Assert
        Assert.True(success);
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("a.xml")]
    [InlineData("b.xml")]
    public void ValidFileWithFormat(string file)
    {
        string filePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, file);
        Assert.True(Path.Exists(filePath), $"{filePath} doesn't exist.");
        Console.WriteLine($"filePath: {filePath}");

        //Arrange
        var item = new Mock<ITaskItem>();
        item.Setup(x => x.ItemSpec).Returns(filePath);
        item.Setup(x => x.GetMetadata("FullPath")).Returns(filePath);
        item.Setup(x => x.MetadataCount).Returns(1);
        item.Setup(x => x.MetadataNames).Returns(new string[] { "FullPath" });
        Console.WriteLine($"item.Object.ItemSpec: {item.Object.ItemSpec}");

        RunXmlFormatFiles xmlFormatFilesTask = new()
        {
            Files = [item.Object],
            LineLength = 142,
            Tabs = " ",
            TabsRepeat = 3,
            MaxEmptyLines = 2,
            BuildEngine = buildEngine.Object,
        };

        //Act
        var success = xmlFormatFilesTask.Execute();

        //Assert
        Assert.True(success);
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("a.xml")]
    [InlineData("b.xml")]
    public void ValidFileWithConfig(string file)
    {
        string filePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, file);
        Assert.True(Path.Exists(filePath), $"{filePath} doesn't exist.");
        Console.WriteLine($"filePath: {filePath}");

        //Arrange
        var item = new Mock<ITaskItem>();
        item.Setup(x => x.ItemSpec).Returns(filePath);
        item.Setup(x => x.GetMetadata("FullPath")).Returns(filePath);
        item.Setup(x => x.MetadataCount).Returns(1);
        item.Setup(x => x.MetadataNames).Returns(new string[] { "FullPath" });
        Console.WriteLine($"item.Object.ItemSpec: {item.Object.ItemSpec}");

        RunXmlFormatFiles xmlFormatFilesTask = new()
        {
            UseLocalConfig = true,
            Files = [item.Object],
            LineLength = 142,
            Tabs = " ",
            TabsRepeat = 3,
            MaxEmptyLines = 2,
            BuildEngine = buildEngine.Object,
        };

        //Act
        var success = xmlFormatFilesTask.Execute();

        //Assert
        Assert.True(success);
        Assert.Empty(errors);
    }
}
