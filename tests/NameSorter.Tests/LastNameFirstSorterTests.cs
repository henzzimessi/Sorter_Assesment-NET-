using NameSorter.Interfaces;
using NameSorter.Models;
using NameSorter.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for the LastNameFirstSorter class.
/// </summary>
public class LastNameFirstSorterTests
{
    private readonly INameSorter _sorter;

    public LastNameFirstSorterTests()
    {
        _sorter = new LastNameFirstSorter();
    }

    [Fact]
    public void Sort_WithEmptyCollection_ReturnsEmptyCollection()
    {
        // Arrange
        var names = Enumerable.Empty<Name>();

        // Act
        var result = _sorter.Sort(names);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Sort_WithSingleName_ReturnsSameName()
    {
        // Arrange
        var names = new[] { new Name(new[] { "John" }, "Smith") };

        // Act
        var result = _sorter.Sort(names).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("John Smith", result[0].FullName);
    }

    [Fact]
    public void Sort_ByLastName_SortsAlphabetically()
    {
        // Arrange
        var names = new[]
        {
            new Name(new[] { "John" }, "Zebra"),
            new Name(new[] { "Jane" }, "Alpha"),
            new Name(new[] { "Bob" }, "Middle")
        };

        // Act
        var result = _sorter.Sort(names).ToList();

        // Assert
        Assert.Equal("Jane Alpha", result[0].FullName);
        Assert.Equal("Bob Middle", result[1].FullName);
        Assert.Equal("John Zebra", result[2].FullName);
    }

    [Fact]
    public void Sort_WithSameLastName_SortsByGivenNames()
    {
        // Arrange
        var names = new[]
        {
            new Name(new[] { "Zoe" }, "Smith"),
            new Name(new[] { "Alice" }, "Smith"),
            new Name(new[] { "Mike" }, "Smith")
        };

        // Act
        var result = _sorter.Sort(names).ToList();

        // Assert
        Assert.Equal("Alice Smith", result[0].FullName);
        Assert.Equal("Mike Smith", result[1].FullName);
        Assert.Equal("Zoe Smith", result[2].FullName);
    }

    [Fact]
    public void Sort_WithSameLastNameAndMultipleGivenNames_SortsByAllGivenNames()
    {
        // Arrange
        var names = new[]
        {
            new Name(new[] { "John", "Zebra" }, "Smith"),
            new Name(new[] { "John", "Alpha" }, "Smith"),
            new Name(new[] { "John", "Middle" }, "Smith")
        };

        // Act
        var result = _sorter.Sort(names).ToList();

        // Assert
        Assert.Equal("John Alpha Smith", result[0].FullName);
        Assert.Equal("John Middle Smith", result[1].FullName);
        Assert.Equal("John Zebra Smith", result[2].FullName);
    }

    [Fact]
    public void Sort_IsCaseInsensitive()
    {
        // Arrange
        var names = new[]
        {
            new Name(new[] { "john" }, "SMITH"),
            new Name(new[] { "JANE" }, "smith"),
            new Name(new[] { "Bob" }, "Smith")
        };

        // Act
        var result = _sorter.Sort(names).ToList();

        // Assert
        // All have same last name (case insensitive), so sort by first name
        Assert.Equal("Bob Smith", result[0].FullName);
        Assert.Equal("JANE smith", result[1].FullName);
        Assert.Equal("john SMITH", result[2].FullName);
    }

    [Fact]
    public void Sort_WithSampleData_ReturnsCorrectOrder()
    {
        // Arrange - Using the exact sample data from the requirements
        var parser = new NameParser();
        var names = new[]
        {
            "Janet Parsons",
            "Vaughn Lewis",
            "Adonis Julius Archer",
            "Shelby Nathan Yoder",
            "Marin Alvarez",
            "London Lindsey",
            "Beau Tristan Bentley",
            "Leo Gardner",
            "Hunter Uriah Mathew Clarke",
            "Mikayla Lopez",
            "Frankie Conner Ritter"
        }.Select(parser.Parse);

        var expectedOrder = new[]
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

        // Act
        var result = _sorter.Sort(names).Select(n => n.FullName).ToList();

        // Assert
        Assert.Equal(expectedOrder, result);
    }

    [Fact]
    public void Sort_WithNullCollection_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _sorter.Sort(null!));
    }

    [Fact]
    public void Sort_DoesNotModifyOriginalCollection()
    {
        // Arrange
        var originalNames = new List<Name>
        {
            new Name(new[] { "John" }, "Zebra"),
            new Name(new[] { "Jane" }, "Alpha")
        };
        var originalFirst = originalNames[0].FullName;

        // Act
        var result = _sorter.Sort(originalNames);

        // Assert - Original collection should be unchanged
        Assert.Equal(originalFirst, originalNames[0].FullName);
    }

    [Fact]
    public void Sort_ReturnsNewCollection()
    {
        // Arrange
        var names = new List<Name>
        {
            new Name(new[] { "John" }, "Smith")
        };

        // Act
        var result = _sorter.Sort(names);

        // Assert - Should be a different reference
        Assert.NotSame(names, result);
    }
}
