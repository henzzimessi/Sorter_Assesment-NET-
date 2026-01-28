// Created by Ian, 01/28/2026

using NameSorter.Interfaces;
using NameSorter.Services;

namespace NameSorter;

public class Program
{
    private const string OutputFileName = "sorted-names-list.txt";
    private const int ExitCodeSuccess = 0;
    private const int ExitCodeError = 1;

    public static async Task<int> Main(string[] args)
    {
        try
        {
            ValidateArguments(args);

            var inputFilePath = args[0];
            var outputFilePath = GetOutputFilePath();

            var application = BuildApplication(inputFilePath, outputFilePath);
            await application.RunAsync();

            return ExitCodeSuccess;
        }
        catch (Exception ex)
        {
            WriteError(ex.Message);
            return ExitCodeError;
        }
    }

    private static void ValidateArguments(string[] args)
    {
        if (args.Length == 0)
        {
            throw new ArgumentException(
                "Usage: name-sorter <input-file-path>\n" +
                "Example: name-sorter ./unsorted-names-list.txt");
        }
    }

    private static string GetOutputFilePath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), OutputFileName);
    }

    private static NameSorterApplication BuildApplication(string inputFilePath, string outputFilePath)
    {
        INameParser nameParser = new NameParser();
        INameReader nameReader = new FileNameReader(inputFilePath, nameParser);
        INameSorter nameSorter = new LastNameFirstSorter();

        var nameWriters = new List<INameWriter>
        {
            new ConsoleNameWriter(),
            new FileNameWriter(outputFilePath)
        };

        return new NameSorterApplication(nameReader, nameSorter, nameWriters);
    }

    private static void WriteError(string message)
    {
        Console.Error.WriteLine($"Error: {message}");
    }
}
