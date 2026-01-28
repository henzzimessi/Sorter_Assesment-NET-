// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Models;

namespace NameSorter.Services;

/// <summary>
/// Writes names to the console.
/// </summary>
public class ConsoleNameWriter : INameWriter
{
    private readonly TextWriter _output;

    public ConsoleNameWriter() : this(Console.Out) { }

    public ConsoleNameWriter(TextWriter output)
    {
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    public Task WriteAsync(IEnumerable<Name> names)
    {
        if (names == null)
        {
            throw new ArgumentNullException(nameof(names));
        }

        foreach (var name in names)
        {
            _output.WriteLine(name.FullName);
        }

        return Task.CompletedTask;
    }
}
