// Created by Ian, 01/28/2026

using NameSorter.Models;

namespace NameSorter.Interfaces;

/// <summary>
/// Contract for reading names from a source.
/// </summary>
public interface INameReader
{
    Task<IEnumerable<Name>> ReadAsync();
}
