// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Models;

namespace NameSorter.Services;

/// <summary>
/// Parses full name strings into Name objects.
/// </summary>
public class NameParser : INameParser
{
    private const int MinimumGivenNames = 1;
    private const int MaximumGivenNames = 3;
    private const int MinimumNameParts = 2;

    public Name Parse(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Full name cannot be null or whitespace.", nameof(fullName));
        }

        var parts = SplitNameIntoParts(fullName);

        ValidateNamePartsCount(parts, fullName);

        var givenNames = ExtractGivenNames(parts);
        var lastName = ExtractLastName(parts);

        return new Name(givenNames, lastName);
    }

    public bool TryParse(string fullName, out Name? name)
    {
        name = null;

        try
        {
            name = Parse(fullName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string[] SplitNameIntoParts(string fullName)
    {
        return fullName
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Trim())
            .ToArray();
    }

    private static void ValidateNamePartsCount(string[] parts, string fullName)
    {
        if (parts.Length < MinimumNameParts)
        {
            throw new ArgumentException(
                $"Name must have at least one given name and a last name. Got: '{fullName}'",
                nameof(fullName));
        }

        var givenNamesCount = parts.Length - 1;

        if (givenNamesCount < MinimumGivenNames || givenNamesCount > MaximumGivenNames)
        {
            throw new ArgumentException(
                $"Name must have between {MinimumGivenNames} and {MaximumGivenNames} given names. Got {givenNamesCount} in: '{fullName}'",
                nameof(fullName));
        }
    }

    private static IEnumerable<string> ExtractGivenNames(string[] parts)
    {
        return parts.Take(parts.Length - 1);
    }

    private static string ExtractLastName(string[] parts)
    {
        return parts.Last();
    }
}
