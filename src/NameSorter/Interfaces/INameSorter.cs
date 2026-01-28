// Created by Ian, 01/28/2026

using NameSorter.Models;

namespace NameSorter.Interfaces;

/// <summary>
/// Contract for sorting names.
/// </summary>
public interface INameSorter
{
    IEnumerable<Name> Sort(IEnumerable<Name> names);
}
