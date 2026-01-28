# Name Sorter

A .NET 8 console application that sorts a list of names by last name, then by given names.

## Requirements

- .NET 8 SDK or later

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run --project src/NameSorter -- ./unsorted-names-list.txt
```

Or after publishing:

```bash
name-sorter ./unsorted-names-list.txt
```

### Input Format

The input file should contain one name per line. Each name must have:

- At least 1 given name (first name)
- Up to 3 given names (first name + middle names)
- Exactly 1 last name (the last word on each line)

Example input (`unsorted-names-list.txt`):

```
Janet Parsons
Vaughn Lewis
Adonis Julius Archer
Shelby Nathan Yoder
Marin Alvarez
```

### Output

The application will:

1. Print the sorted names to the console
2. Create a file called `sorted-names-list.txt` in the current directory

Names are sorted:

1. First by last name (alphabetically, case-insensitive)
2. Then by given names (alphabetically, case-insensitive)

## Running Tests

```bash
dotnet test
```

## Project Structure

```
NameSorter/
├── src/NameSorter/
│   ├── Models/
│   │   └── Name.cs                    # Name entity with validation
│   ├── Interfaces/
│   │   ├── INameParser.cs             # Contract for parsing names
│   │   ├── INameReader.cs             # Contract for reading names
│   │   ├── INameWriter.cs             # Contract for writing names
│   │   └── INameSorter.cs             # Contract for sorting names
│   ├── Services/
│   │   ├── NameParser.cs              # Parses string to Name objects
│   │   ├── FileNameReader.cs          # Reads names from a file
│   │   ├── FileNameWriter.cs          # Writes names to a file
│   │   ├── ConsoleNameWriter.cs       # Writes names to console
│   │   ├── LastNameFirstSorter.cs     # Sorts by last name, then given names
│   │   └── NameSorterApplication.cs   # Orchestrates the workflow
│   └── Program.cs                     # Entry point with dependency composition
├── tests/NameSorter.Tests/
│   ├── NameTests.cs
│   ├── NameParserTests.cs
│   ├── LastNameFirstSorterTests.cs
│   ├── FileNameReaderTests.cs
│   ├── FileNameWriterTests.cs
│   ├── ConsoleNameWriterTests.cs
│   └── NameSorterApplicationTests.cs
└── README.md
```

## Design Principles

This solution follows **SOLID** principles:

### Single Responsibility Principle (SRP)

Each class has one job:

- `Name` - Represents a person's name
- `NameParser` - Parses strings into Name objects
- `FileNameReader` - Reads names from files
- `FileNameWriter` - Writes names to files
- `ConsoleNameWriter` - Writes names to console
- `LastNameFirstSorter` - Sorts names by last name, then given names
- `NameSorterApplication` - Orchestrates the workflow

### Open/Closed Principle (OCP)

The system is open for extension but closed for modification:

- New sorters can be added by implementing `INameSorter`
- New readers can be added by implementing `INameReader`
- New writers can be added by implementing `INameWriter`

### Liskov Substitution Principle (LSP)

All implementations are interchangeable with their interfaces without affecting correctness.

### Interface Segregation Principle (ISP)

Interfaces are small and focused:

- `INameReader` - Only reading
- `INameWriter` - Only writing
- `INameSorter` - Only sorting
- `INameParser` - Only parsing

### Dependency Inversion Principle (DIP)

High-level modules depend on abstractions:

- `NameSorterApplication` depends on interfaces, not concrete implementations
- Dependencies are injected through the constructor

## Author

Ian - Created for the Dye & Durham coding assessment.
