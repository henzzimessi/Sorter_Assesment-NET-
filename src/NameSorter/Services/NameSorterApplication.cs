// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Models;

namespace NameSorter.Services;

/// <summary>
/// Orchestrates the name sorting workflow.
/// </summary>
public class NameSorterApplication
{
    private readonly INameReader _reader;
    private readonly INameSorter _sorter;
    private readonly IEnumerable<INameWriter> _writers;

    public NameSorterApplication(
        INameReader reader,
        INameSorter sorter,
        IEnumerable<INameWriter> writers)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _sorter = sorter ?? throw new ArgumentNullException(nameof(sorter));
        _writers = writers ?? throw new ArgumentNullException(nameof(writers));
    }

    public async Task RunAsync()
    {
        var names = await ReadNamesAsync();
        var sortedNames = SortNames(names);
        await WriteNamesAsync(sortedNames);
    }

    private async Task<IEnumerable<Name>> ReadNamesAsync()
    {
        return await _reader.ReadAsync();
    }

    private IEnumerable<Name> SortNames(IEnumerable<Name> names)
    {
        return _sorter.Sort(names);
    }

    private async Task WriteNamesAsync(IEnumerable<Name> names)
    {
        var namesList = names.ToList();

        foreach (var writer in _writers)
        {
            await writer.WriteAsync(namesList);
        }
    }
}
