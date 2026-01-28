using NameSorter.Models;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for the Name class.
/// </summary>
public class NameTests
{
    [Fact]
    public void Constructor_WithOneGivenName_CreatesValidName()
    {
        // Arrange
        var givenNames = new[] { "John" };
        var lastName = "Smith";

        // Act
        var name = new Name(givenNames, lastName);

        // Assert
        Assert.Single(name.GivenNames);
        Assert.Equal("John", name.GivenNames[0]);
        Assert.Equal("Smith", name.LastName);
        Assert.Equal("John Smith", name.FullName);
    }

    [Fact]
    public void Constructor_WithTwoGivenNames_CreatesValidName()
    {
        // Arrange
        var givenNames = new[] { "John", "Michael" };
        var lastName = "Smith";

        // Act
        var name = new Name(givenNames, lastName);

        // Assert
        Assert.Equal(2, name.GivenNames.Count);
        Assert.Equal("John", name.GivenNames[0]);
        Assert.Equal("Michael", name.GivenNames[1]);
        Assert.Equal("Smith", name.LastName);
        Assert.Equal("John Michael Smith", name.FullName);
    }

    [Fact]
    public void Constructor_WithThreeGivenNames_CreatesValidName()
    {
        // Arrange
        var givenNames = new[] { "John", "Michael", "James" };
        var lastName = "Smith";

        // Act
        var name = new Name(givenNames, lastName);

        // Assert
        Assert.Equal(3, name.GivenNames.Count);
        Assert.Equal("John Michael James Smith", name.FullName);
    }

    [Fact]
    public void Constructor_WithNoGivenNames_ThrowsArgumentException()
    {
        // Arrange
        var givenNames = Array.Empty<string>();
        var lastName = "Smith";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Name(givenNames, lastName));
        Assert.Contains("at least 1 given name", exception.Message);
    }

    [Fact]
    public void Constructor_WithMoreThanThreeGivenNames_ThrowsArgumentException()
    {
        // Arrange
        var givenNames = new[] { "John", "Michael", "James", "Robert" };
        var lastName = "Smith";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Name(givenNames, lastName));
        Assert.Contains("up to 3 given names", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullLastName_ThrowsArgumentException()
    {
        // Arrange
        var givenNames = new[] { "John" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Name(givenNames, null!));
    }

    [Fact]
    public void Constructor_WithEmptyLastName_ThrowsArgumentException()
    {
        // Arrange
        var givenNames = new[] { "John" };
        var lastName = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Name(givenNames, lastName));
    }

    [Fact]
    public void Constructor_WithWhitespaceLastName_ThrowsArgumentException()
    {
        // Arrange
        var givenNames = new[] { "John" };
        var lastName = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Name(givenNames, lastName));
    }

    [Fact]
    public void Constructor_WithWhitespaceGivenName_ThrowsArgumentException()
    {
        // Arrange
        var givenNames = new[] { "John", "   " };
        var lastName = "Smith";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Name(givenNames, lastName));
    }

    [Fact]
    public void ToString_ReturnsFullName()
    {
        // Arrange
        var name = new Name(new[] { "John", "Michael" }, "Smith");

        // Act
        var result = name.ToString();

        // Assert
        Assert.Equal("John Michael Smith", result);
    }

    [Fact]
    public void Equals_WithSameFullName_ReturnsTrue()
    {
        // Arrange
        var name1 = new Name(new[] { "John" }, "Smith");
        var name2 = new Name(new[] { "John" }, "Smith");

        // Act & Assert
        Assert.True(name1.Equals(name2));
    }

    [Fact]
    public void Equals_WithDifferentFullName_ReturnsFalse()
    {
        // Arrange
        var name1 = new Name(new[] { "John" }, "Smith");
        var name2 = new Name(new[] { "Jane" }, "Smith");

        // Act & Assert
        Assert.False(name1.Equals(name2));
    }

    [Fact]
    public void GetHashCode_ForEqualNames_ReturnsSameValue()
    {
        // Arrange
        var name1 = new Name(new[] { "John" }, "Smith");
        var name2 = new Name(new[] { "John" }, "Smith");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void GivenNames_IsReadOnly()
    {
        // Arrange
        var givenNames = new List<string> { "John", "Michael" };
        var name = new Name(givenNames, "Smith");

        // Act - Try to modify the original list
        givenNames.Add("James");

        // Assert - Name should not be affected
        Assert.Equal(2, name.GivenNames.Count);
    }
}
