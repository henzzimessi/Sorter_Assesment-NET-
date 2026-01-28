// Created by Ian, 01/28/2026

using NameSorter.Models;

namespace NameSorter.Interfaces;

/// <summary>
/// Contract for parsing name strings into Name objects.
/// </summary>
public interface INameParser
{
    Name Parse(string fullName);
    bool TryParse(string fullName, out Name? name);
}
