// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Models;

namespace NameSorter.Services;

/// <summary>
/// Reads names from a text file.
/// </summary>
public class FileNameReader : INameReader
{
    private readonly string _filePath;
    private readonly INameParser _nameParser;

    public FileNameReader(string filePath, INameParser nameParser)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        _nameParser = nameParser ?? throw new ArgumentNullException(nameof(nameParser));
    }

    public async Task<IEnumerable<Name>> ReadAsync()
    {
        ValidateFileExists();

        var lines = await File.ReadAllLinesAsync(_filePath);
        return ParseNames(lines);
    }

    private void ValidateFileExists()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException(
                $"The input file was not found: {_filePath}",
                _filePath);
        }
    }

    private IEnumerable<Name> ParseNames(string[] lines)
    {
        var names = new List<Name>();
        var lineNumber = 0;

        foreach (var line in lines)
        {
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                var name = _nameParser.Parse(line);
                names.Add(name);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to parse name on line {lineNumber}: '{line}'. {ex.Message}",
                    ex);
            }
        }

        return names;
    }
}
