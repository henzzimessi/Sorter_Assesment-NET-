using NameSorter.Interfaces;
using NameSorter.Models;
using NameSorter.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for the FileNameReader class.
/// </summary>
public class FileNameReaderTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly List<string> _createdFiles;

    public FileNameReaderTests()
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
    public async Task ReadAsync_WithValidFile_ReturnsNames()
    {
        // Arrange
        var content = "John Smith\nJane Doe";
        var filePath = await CreateTestFileAsync("valid.txt", content);
        var reader = new FileNameReader(filePath, new NameParser());

        // Act
        var result = (await reader.ReadAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("John Smith", result[0].FullName);
        Assert.Equal("Jane Doe", result[1].FullName);
    }

    [Fact]
    public async Task ReadAsync_WithEmptyLines_SkipsThem()
    {
        // Arrange
        var content = "John Smith\n\n\nJane Doe\n";
        var filePath = await CreateTestFileAsync("empty-lines.txt", content);
        var reader = new FileNameReader(filePath, new NameParser());

        // Act
        var result = (await reader.ReadAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ReadAsync_WithEmptyFile_ReturnsEmptyCollection()
    {
        // Arrange
        var filePath = await CreateTestFileAsync("empty.txt", "");
        var reader = new FileNameReader(filePath, new NameParser());

        // Act
        var result = await reader.ReadAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ReadAsync_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "nonexistent.txt");
        var reader = new FileNameReader(filePath, new NameParser());

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => reader.ReadAsync());
    }

    [Fact]
    public async Task ReadAsync_WithInvalidName_ThrowsInvalidOperationException()
    {
        // Arrange - Single word is invalid (no given name)
        var content = "Smith";
        var filePath = await CreateTestFileAsync("invalid.txt", content);
        var reader = new FileNameReader(filePath, new NameParser());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => reader.ReadAsync());
        Assert.Contains("line 1", exception.Message);
    }

    [Fact]
    public async Task ReadAsync_WithSampleData_ReturnsAllNames()
    {
        // Arrange - Using the exact sample data from the requirements
        var content = @"Janet Parsons
Vaughn Lewis
Adonis Julius Archer
Shelby Nathan Yoder
Marin Alvarez
London Lindsey
Beau Tristan Bentley
Leo Gardner
Hunter Uriah Mathew Clarke
Mikayla Lopez
Frankie Conner Ritter";

        var filePath = await CreateTestFileAsync("sample.txt", content);
        var reader = new FileNameReader(filePath, new NameParser());

        // Act
        var result = (await reader.ReadAsync()).ToList();

        // Assert
        Assert.Equal(11, result.Count);
        Assert.Equal("Janet Parsons", result[0].FullName);
        Assert.Equal("Hunter Uriah Mathew Clarke", result[8].FullName);
    }

    [Fact]
    public void Constructor_WithNullFilePath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FileNameReader(null!, new NameParser()));
    }

    [Fact]
    public void Constructor_WithNullParser_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FileNameReader("path.txt", null!));
    }

    private async Task<string> CreateTestFileAsync(string fileName, string content)
    {
        var filePath = Path.Combine(_testDirectory, fileName);
        await File.WriteAllTextAsync(filePath, content);
        _createdFiles.Add(filePath);
        return filePath;
    }
}
