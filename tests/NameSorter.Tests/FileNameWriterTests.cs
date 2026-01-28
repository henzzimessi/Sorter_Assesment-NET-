using NameSorter.Interfaces;
using NameSorter.Models;
using NameSorter.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for the FileNameWriter class.
/// </summary>
public class FileNameWriterTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly List<string> _createdFiles;

    public FileNameWriterTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "NameSorterTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _createdFiles = new List<string>();
    }

    public void Dispose()
    {
        // Cleanup test files
        foreach (var file in _createdFiles)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Fact]
    public async Task WriteAsync_WithNames_WritesCorrectContent()
    {
        // Arrange
        var filePath = CreateTestFilePath("output.txt");
        var writer = new FileNameWriter(filePath);
        var names = new[]
        {
            new Name(new[] { "John" }, "Smith"),
            new Name(new[] { "Jane" }, "Doe")
        };

        // Act
        await writer.WriteAsync(names);

        // Assert
        Assert.True(File.Exists(filePath));
        var lines = await File.ReadAllLinesAsync(filePath);
        Assert.Equal(2, lines.Length);
        Assert.Equal("John Smith", lines[0]);
        Assert.Equal("Jane Doe", lines[1]);
    }

    [Fact]
    public async Task WriteAsync_WithEmptyCollection_CreatesEmptyFile()
    {
        // Arrange
        var filePath = CreateTestFilePath("empty.txt");
        var writer = new FileNameWriter(filePath);
        var names = Enumerable.Empty<Name>();

        // Act
        await writer.WriteAsync(names);

        // Assert
        Assert.True(File.Exists(filePath));
        var content = await File.ReadAllTextAsync(filePath);
        Assert.Equal("", content);
    }

    [Fact]
    public async Task WriteAsync_OverwritesExistingFile()
    {
        // Arrange
        var filePath = CreateTestFilePath("existing.txt");
        await File.WriteAllTextAsync(filePath, "Old content");
        var writer = new FileNameWriter(filePath);
        var names = new[] { new Name(new[] { "New" }, "Name") };

        // Act
        await writer.WriteAsync(names);

        // Assert
        var content = await File.ReadAllTextAsync(filePath);
        Assert.DoesNotContain("Old content", content);
        Assert.Contains("New Name", content);
    }

    [Fact]
    public async Task WriteAsync_WithNullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        var filePath = CreateTestFilePath("null.txt");
        var writer = new FileNameWriter(filePath);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => writer.WriteAsync(null!));
    }

    [Fact]
    public void Constructor_WithNullFilePath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FileNameWriter(null!));
    }

    private string CreateTestFilePath(string fileName)
    {
        var filePath = Path.Combine(_testDirectory, fileName);
        _createdFiles.Add(filePath);
        return filePath;
    }
}
