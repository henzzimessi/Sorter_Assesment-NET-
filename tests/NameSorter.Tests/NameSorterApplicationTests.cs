using NameSorter.Interfaces;
using NameSorter.Models;
using NameSorter.Services;

namespace NameSorter.Tests;

/// <summary>
/// Integration tests for the NameSorterApplication class.
/// Tests the full workflow from reading to sorting to writing.
/// </summary>
public class NameSorterApplicationTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly List<string> _createdFiles;

    public NameSorterApplicationTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "NameSorterTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _createdFiles = new List<string>();
    }

    public void Dispose()
    {
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
    public async Task RunAsync_WithSampleData_ProducesCorrectOutput()
    {
        // Arrange - Create input file with sample data
        var inputContent = @"Janet Parsons
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

        var inputPath = await CreateTestFileAsync("input.txt", inputContent);
        var outputPath = CreateTestFilePath("output.txt");

        using var consoleOutput = new StringWriter();

        var parser = new NameParser();
        var reader = new FileNameReader(inputPath, parser);
        var sorter = new LastNameFirstSorter();
        var writers = new INameWriter[]
        {
            new ConsoleNameWriter(consoleOutput),
            new FileNameWriter(outputPath)
        };

        var application = new NameSorterApplication(reader, sorter, writers);

        // Act
        await application.RunAsync();

        // Assert - Check file output
        var expectedOutput = new[]
        {
            "Marin Alvarez",
            "Adonis Julius Archer",
            "Beau Tristan Bentley",
            "Hunter Uriah Mathew Clarke",
            "Leo Gardner",
            "Vaughn Lewis",
            "London Lindsey",
            "Mikayla Lopez",
            "Janet Parsons",
            "Frankie Conner Ritter",
            "Shelby Nathan Yoder"
        };

        var fileOutput = await File.ReadAllLinesAsync(outputPath);
        Assert.Equal(expectedOutput, fileOutput);

        // Assert - Check console output
        var consoleLines = consoleOutput.ToString()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(expectedOutput, consoleLines);
    }

    [Fact]
    public async Task RunAsync_WithEmptyInput_ProducesEmptyOutput()
    {
        // Arrange
        var inputPath = await CreateTestFileAsync("empty.txt", "");
        var outputPath = CreateTestFilePath("output.txt");

        using var consoleOutput = new StringWriter();

        var parser = new NameParser();
        var reader = new FileNameReader(inputPath, parser);
        var sorter = new LastNameFirstSorter();
        var writers = new INameWriter[]
        {
            new ConsoleNameWriter(consoleOutput),
            new FileNameWriter(outputPath)
        };

        var application = new NameSorterApplication(reader, sorter, writers);

        // Act
        await application.RunAsync();

        // Assert
        var fileOutput = await File.ReadAllTextAsync(outputPath);
        Assert.Empty(fileOutput);
        Assert.Equal("", consoleOutput.ToString());
    }

    [Fact]
    public void Constructor_WithNullReader_ThrowsArgumentNullException()
    {
        // Arrange
        var sorter = new LastNameFirstSorter();
        var writers = new INameWriter[] { new ConsoleNameWriter() };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new NameSorterApplication(null!, sorter, writers));
    }

    [Fact]
    public void Constructor_WithNullSorter_ThrowsArgumentNullException()
    {
        // Arrange
        var parser = new NameParser();
        var reader = new FileNameReader("path.txt", parser);
        var writers = new INameWriter[] { new ConsoleNameWriter() };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new NameSorterApplication(reader, null!, writers));
    }

    [Fact]
    public void Constructor_WithNullWriters_ThrowsArgumentNullException()
    {
        // Arrange
        var parser = new NameParser();
        var reader = new FileNameReader("path.txt", parser);
        var sorter = new LastNameFirstSorter();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new NameSorterApplication(reader, sorter, null!));
    }

    private async Task<string> CreateTestFileAsync(string fileName, string content)
    {
        var filePath = Path.Combine(_testDirectory, fileName);
        await File.WriteAllTextAsync(filePath, content);
        _createdFiles.Add(filePath);
        return filePath;
    }

    private string CreateTestFilePath(string fileName)
    {
        var filePath = Path.Combine(_testDirectory, fileName);
        _createdFiles.Add(filePath);
        return filePath;
    }
}
