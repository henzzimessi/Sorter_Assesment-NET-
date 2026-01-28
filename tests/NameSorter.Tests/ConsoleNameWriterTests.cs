using NameSorter.Interfaces;
using NameSorter.Models;
using NameSorter.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for the ConsoleNameWriter class.
/// </summary>
public class ConsoleNameWriterTests
{
    [Fact]
    public async Task WriteAsync_WithNames_WritesEachNameOnSeparateLine()
    {
        // Arrange
        using var stringWriter = new StringWriter();
        var writer = new ConsoleNameWriter(stringWriter);
        var names = new[]
        {
            new Name(new[] { "John" }, "Smith"),
            new Name(new[] { "Jane" }, "Doe")
        };

        // Act
        await writer.WriteAsync(names);

        // Assert
        var output = stringWriter.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(2, lines.Length);
        Assert.Equal("John Smith", lines[0]);
        Assert.Equal("Jane Doe", lines[1]);
    }

    [Fact]
    public async Task WriteAsync_WithEmptyCollection_WritesNothing()
    {
        // Arrange
        using var stringWriter = new StringWriter();
        var writer = new ConsoleNameWriter(stringWriter);
        var names = Enumerable.Empty<Name>();

        // Act
        await writer.WriteAsync(names);

        // Assert
        Assert.Equal("", stringWriter.ToString());
    }

    [Fact]
    public async Task WriteAsync_WithNullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        using var stringWriter = new StringWriter();
        var writer = new ConsoleNameWriter(stringWriter);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => writer.WriteAsync(null!));
    }

    [Fact]
    public void Constructor_WithNullTextWriter_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ConsoleNameWriter(null!));
    }
}
