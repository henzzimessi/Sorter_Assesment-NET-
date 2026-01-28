using NameSorter.Interfaces;
using NameSorter.Models;
using NameSorter.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for the NameParser class.
/// </summary>
public class NameParserTests
{
    private readonly INameParser _parser;

    public NameParserTests()
    {
        _parser = new NameParser();
    }

    [Fact]
    public void Parse_WithTwoPartName_ReturnsCorrectName()
    {
        // Arrange
        var fullName = "Janet Parsons";

        // Act
        var result = _parser.Parse(fullName);

        // Assert
        Assert.Single(result.GivenNames);
        Assert.Equal("Janet", result.GivenNames[0]);
        Assert.Equal("Parsons", result.LastName);
    }

    [Fact]
    public void Parse_WithThreePartName_ReturnsCorrectName()
    {
        // Arrange
        var fullName = "Adonis Julius Archer";

        // Act
        var result = _parser.Parse(fullName);

        // Assert
        Assert.Equal(2, result.GivenNames.Count);
        Assert.Equal("Adonis", result.GivenNames[0]);
        Assert.Equal("Julius", result.GivenNames[1]);
        Assert.Equal("Archer", result.LastName);
    }

    [Fact]
    public void Parse_WithFourPartName_ReturnsCorrectName()
    {
        // Arrange
        var fullName = "Hunter Uriah Mathew Clarke";

        // Act
        var result = _parser.Parse(fullName);

        // Assert
        Assert.Equal(3, result.GivenNames.Count);
        Assert.Equal("Hunter", result.GivenNames[0]);
        Assert.Equal("Uriah", result.GivenNames[1]);
        Assert.Equal("Mathew", result.GivenNames[2]);
        Assert.Equal("Clarke", result.LastName);
    }

    [Fact]
    public void Parse_WithExtraSpaces_HandlesCorrectly()
    {
        // Arrange
        var fullName = "  John    Michael   Smith  ";

        // Act
        var result = _parser.Parse(fullName);

        // Assert
        Assert.Equal(2, result.GivenNames.Count);
        Assert.Equal("John", result.GivenNames[0]);
        Assert.Equal("Michael", result.GivenNames[1]);
        Assert.Equal("Smith", result.LastName);
    }

    [Fact]
    public void Parse_WithSingleWord_ThrowsArgumentException()
    {
        // Arrange
        var fullName = "Smith";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(fullName));
        Assert.Contains("at least one given name", exception.Message);
    }

    [Fact]
    public void Parse_WithFivePartName_ThrowsArgumentException()
    {
        // Arrange - 4 given names + 1 last name
        var fullName = "John Michael James Robert Smith";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(fullName));
        Assert.Contains("between 1 and 3 given names", exception.Message);
    }

    [Fact]
    public void Parse_WithNullInput_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _parser.Parse(null!));
    }

    [Fact]
    public void Parse_WithEmptyString_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _parser.Parse(""));
    }

    [Fact]
    public void Parse_WithWhitespaceOnly_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _parser.Parse("   "));
    }

    [Fact]
    public void TryParse_WithValidName_ReturnsTrueAndName()
    {
        // Arrange
        var fullName = "John Smith";

        // Act
        var result = _parser.TryParse(fullName, out var name);

        // Assert
        Assert.True(result);
        Assert.NotNull(name);
        Assert.Equal("John Smith", name!.FullName);
    }

    [Fact]
    public void TryParse_WithInvalidName_ReturnsFalseAndNull()
    {
        // Arrange
        var fullName = "Smith"; // Missing given name

        // Act
        var result = _parser.TryParse(fullName, out var name);

        // Assert
        Assert.False(result);
        Assert.Null(name);
    }

    [Theory]
    [InlineData("Janet Parsons", "Janet", "Parsons")]
    [InlineData("Vaughn Lewis", "Vaughn", "Lewis")]
    [InlineData("Marin Alvarez", "Marin", "Alvarez")]
    [InlineData("London Lindsey", "London", "Lindsey")]
    [InlineData("Leo Gardner", "Leo", "Gardner")]
    [InlineData("Mikayla Lopez", "Mikayla", "Lopez")]
    public void Parse_WithTwoPartNames_FromSampleData(string fullName, string expectedFirstName, string expectedLastName)
    {
        // Act
        var result = _parser.Parse(fullName);

        // Assert
        Assert.Equal(expectedFirstName, result.GivenNames[0]);
        Assert.Equal(expectedLastName, result.LastName);
    }

    [Theory]
    [InlineData("Adonis Julius Archer", new[] { "Adonis", "Julius" }, "Archer")]
    [InlineData("Shelby Nathan Yoder", new[] { "Shelby", "Nathan" }, "Yoder")]
    [InlineData("Beau Tristan Bentley", new[] { "Beau", "Tristan" }, "Bentley")]
    [InlineData("Frankie Conner Ritter", new[] { "Frankie", "Conner" }, "Ritter")]
    public void Parse_WithThreePartNames_FromSampleData(string fullName, string[] expectedGivenNames, string expectedLastName)
    {
        // Act
        var result = _parser.Parse(fullName);

        // Assert
        Assert.Equal(expectedGivenNames, result.GivenNames);
        Assert.Equal(expectedLastName, result.LastName);
    }
}
