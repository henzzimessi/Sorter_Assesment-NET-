// Created by Ian, 01/28/2026

using NameSorter.Models;

namespace NameSorter.Interfaces;

/// <summary>
/// Contract for writing names to a destination.
/// </summary>
public interface INameWriter
{
    Task WriteAsync(IEnumerable<Name> names);
}
