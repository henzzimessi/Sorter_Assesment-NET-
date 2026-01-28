// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Models;

namespace NameSorter.Services;

/// <summary>
/// Writes names to a text file.
/// </summary>
public class FileNameWriter : INameWriter
{
    private readonly string _filePath;

    public FileNameWriter(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public async Task WriteAsync(IEnumerable<Name> names)
    {
        if (names == null)
        {
            throw new ArgumentNullException(nameof(names));
        }

        var lines = names.Select(name => name.FullName);
        await File.WriteAllLinesAsync(_filePath, lines);
    }
}
