// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Models;

namespace NameSorter.Services;

/// <summary>
/// Sorts names by last name, then by given names.
/// </summary>
public class LastNameFirstSorter : INameSorter
{
    public IEnumerable<Name> Sort(IEnumerable<Name> names)
    {
        if (names == null)
        {
            throw new ArgumentNullException(nameof(names));
        }

        return names
            .OrderBy(name => name.LastName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(name => GetGivenNamesForSorting(name), StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string GetGivenNamesForSorting(Name name)
    {
        return string.Join(" ", name.GivenNames);
    }
}
