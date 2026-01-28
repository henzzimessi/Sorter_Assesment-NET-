// Created by Ian, 01/28/2026

namespace NameSorter.Models;

/// <summary>
/// Represents a person's name with given names and a last name.
/// </summary>
public class Name
{
    public IReadOnlyList<string> GivenNames { get; }
    public string LastName { get; }
    public string FullName { get; }

    public Name(IEnumerable<string> givenNames, string lastName)
    {
        var givenNamesList = givenNames?.ToList() ?? new List<string>();

        ValidateGivenNames(givenNamesList);
        ValidateLastName(lastName);

        GivenNames = givenNamesList.AsReadOnly();
        LastName = lastName;
        FullName = BuildFullName(givenNamesList, lastName);
    }

    private static void ValidateGivenNames(List<string> givenNames)
    {
        if (givenNames.Count < 1 || givenNames.Count > 3)
        {
            throw new ArgumentException(
                "A name must have at least 1 given name and may have up to 3 given names.",
                nameof(givenNames));
        }

        if (givenNames.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException(
                "Given names cannot be null or whitespace.",
                nameof(givenNames));
        }
    }

    private static void ValidateLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException(
                "Last name cannot be null or whitespace.",
                nameof(lastName));
        }
    }

    private static string BuildFullName(List<string> givenNames, string lastName)
    {
        return string.Join(" ", givenNames.Concat(new[] { lastName }));
    }

    public override string ToString() => FullName;

    public override bool Equals(object? obj)
    {
        if (obj is not Name other)
            return false;

        return FullName.Equals(other.FullName, StringComparison.Ordinal);
    }

    public override int GetHashCode() => FullName.GetHashCode();
}
