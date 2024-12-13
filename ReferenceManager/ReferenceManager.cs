using System;
using System.ComponentModel.Design;

using FluentAssertions.Equivalency;

using Newtonsoft.Json;

namespace ReferenceManager
{
    /// <summary>
    /// Main application class for managing BibTeX references.
    /// Handles user interaction and command processing.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The file path for the BibTeX file where references are stored.
        /// </summary>
        public static string FilePath { get; set; } = "references.bib";

        private readonly ConsoleIO _io;
        private readonly IReferenceLoader _referenceLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class with the specified IO handler.
        /// </summary>
        /// <param name="io">An instance of ConsoleIO for handling input and output.</param>
        public Program(ConsoleIO io, IReferenceLoader referenceLoader)
        {
            _io = io;
            _referenceLoader = referenceLoader;
        }

        /// <summary>
        /// Main loop of the program. Handles user commands and processes them.
        /// </summary>
        public void Run()
        {
            _io.Write($"Using BibTeX file: {FilePath}");

            List<Reference> references = new List<Reference>();

            while (true)
            {
                _io.Write("\nChoose a command (type 'help' for available commands):");
                string command = _io.Read().Trim().ToLower();

                switch (command)
                {
                    case "add":
                        AddReference(references);
                        break;
                    case "list":
                        ListReferences(references);
                        break;
                    case "filter":
                        FilterReferences();
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    case "exit":
                        _io.Write("Exiting the application. Goodbye!");
                        return;
                    default:
                        _io.Write("Unknown command. Type 'help' to see available commands.");
                        break;
                }
            }
        }

        public void AddReference(List<Reference> references)
        {
            _io.Write("Select article type (enter '1' or 'article' for article, '2' or 'inproceedings' for inproceedings):");
            string choice = _io.Read().Trim();

            switch (choice)
            {
                case "1":
                case "article":
                    AddJournalArticle(references);
                    break;
                case "2":
                case "inproceedings":
                    AddInProceedings(references);
                    break;
                default:
                    _io.Write("Invalid choice. Returning to main menu.");
                    break;
            }
        }

        /// <summary>
        /// Adds a mandatory field to the user input.
        /// </summary>
        public string GiveUserInputFromMandatoryField(string field)
        {
            string? input;
            do
            {
                _io.Write(field + "*: ");
                input = _io.Read();

                if (string.IsNullOrWhiteSpace(input))
                {
                    _io.Write(field + " cannot be empty. Please enter again.");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }

        /// <summary>
        /// Adds a new journal article to the BibTeX file.
        /// </summary>
        public void AddJournalArticle(List<Reference> references)
        {
            var newArticleReference = new ArticleReference();
            _io.Write("Mandatory fields are followed by *");
            string author = GetAuthors();
            newArticleReference.Author = author;
            string title = GiveUserInputFromMandatoryField("Title");
            newArticleReference.Title = title;
            string journal = GiveUserInputFromMandatoryField("Journal");
            newArticleReference.Journal = journal;

            while (true)
            {
                string year = GiveUserInputFromMandatoryField("Year");
                try
                {
                    newArticleReference.Year = year;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            _io.Write("Month: ");
            string month = _io.Read().Trim();

            while (true)
            {
                _io.Write("Volume: ");
                string volume = _io.Read().Trim();

                if (string.IsNullOrEmpty(volume))
                {
                    break;
                }

                try
                {
                    newArticleReference.Volume = volume;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            while (true)
            {
                _io.Write("Pages: ");
                string pages = _io.Read().Trim();
                try
                {
                    newArticleReference.Pages = pages;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            if (!string.IsNullOrEmpty(month))
            {
                newArticleReference.Month = month;
            }

            _io.Write("Doi: ");
            string doi = _io.Read().Trim();
            _io.Write("Note: ");
            string note = _io.Read().Trim();
            _io.Write("Key: ");
            string key = _io.Read().Trim();

            _io.Write("Do you want to add this article (y/n)?");
            string confirmation = _io.Read().Trim().ToLower();

            if (confirmation != "y")
            {
                _io.Write("Operation cancelled by the user.");
                return;
            }
            _io.Write("Adding journal article...");


            references.Add(newArticleReference);

            if (newArticleReference.ToBibtexFile())
            {
                _io.Write("Journal article added successfully.");
            }
            else
            {
                _io.Write("Failed to add reference to BibTeX file.");
            }
        }


        public void AddInProceedings(List<Reference> references)
        {
            _io.Write("Adding an inproceedings article...");
            _io.Write("Mandatory fields are followed by *");
            var InproceedingsReference = new InProceedingsReference();

            string author = GetAuthors();
            InproceedingsReference.Author = author;

            while (true)
            {
                string title = GiveUserInputFromMandatoryField("Title");
                try
                {
                    InproceedingsReference.Title = title;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            while (true)
            {
                string bookTitle = GiveUserInputFromMandatoryField("Book Title");
                try
                {
                    InproceedingsReference.BookTitle = bookTitle;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            while (true)
            {
                string year = GiveUserInputFromMandatoryField("Year");
                try
                {
                    InproceedingsReference.Year = year;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            _io.Write("Month: ");
            string month = _io.Read().Trim();

            if (!string.IsNullOrEmpty(month))
            {
                InproceedingsReference.Month = month;
            }

            _io.Write("Editor: ");
            string editor = _io.Read().Trim();

            if (!string.IsNullOrEmpty(editor))
            {
                InproceedingsReference.Editor = editor;
            }

            while (true)
            {
                _io.Write("Volume: ");
                string volume = _io.Read().Trim();

                if (string.IsNullOrEmpty(volume))
                {
                    break;
                }

                try
                {
                    InproceedingsReference.Volume = volume;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            _io.Write("Series: ");
            string series = _io.Read().Trim();
            if (!string.IsNullOrEmpty(series))
            {
                InproceedingsReference.Series = series;
            }

            while (true)
            {
                _io.Write("Pages: ");
                string pages = _io.Read().Trim();
                try
                {
                    InproceedingsReference.Pages = pages;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            _io.Write("Address: ");
            string address = _io.Read().Trim();
            if (!string.IsNullOrEmpty(address))
            {
                InproceedingsReference.Address = address;
            }

            _io.Write("Organization: ");
            string organization = _io.Read().Trim();
            if (!string.IsNullOrEmpty(organization))
            {
                InproceedingsReference.Organization = organization;
            }

            _io.Write("Publisher: ");
            string publisher = _io.Read().Trim();
            if (!string.IsNullOrEmpty(publisher))
            {
                InproceedingsReference.Publisher = publisher;
            }

            _io.Write("Note: ");
            string note = _io.Read().Trim();
            if (!string.IsNullOrEmpty(note))
            {
                InproceedingsReference.Note = note;
            }

            _io.Write("Key: ");
            string key = _io.Read().Trim();

            _io.Write("Do you want to add this inproceedings article (y/n)?");
            string confirmation = _io.Read().Trim().ToLower();

            if (confirmation != "y")
            {
                _io.Write("Operation cancelled by the user.");
                return;
            }

            references.Add(InproceedingsReference);

            if (InproceedingsReference.ToBibtexFile())
            {
                _io.Write("Inproceedings article added successfully.");
            }
            else
            {
                _io.Write("Failed to add inproceedings article to BibTeX file.");
            }
        }


        /// <summary>
        /// Collects authors from the user one author at time
        /// </summary>
        /// <returns> A string of authors</returns>
        public string GetAuthors()
        {
            List<string> authors = new List<string>();

            while (true)
            {
                _io.Write(authors.Count == 0
                    ? "Enter author name (at least one author is required):"
                    : "Enter another author name (or press Enter to finish):");
                string? author = _io.Read();

                if (!string.IsNullOrWhiteSpace(author))
                {
                    authors.Add(author.Trim());
                }
                else if (authors.Count > 0)
                {
                    break; // Exit if the user presses Enter after adding at least one author
                }
                else
                {
                    _io.Write("At least one author is required. Please add an author.");
                }
            }

            authors.Sort();
            return string.Join(", ", authors);
        }


        /// <summary>
        /// Lists all references from the BibTeX file.
        /// </summary>
        /// <summary>
        /// Lists all references, combining in-memory references and file content.
        /// </summary>
        public void ListReferences(List<Reference> references)
        {
            _io.Write("Listing all references:");

            // Read references from file
            if (File.Exists(FilePath))
            {
                _io.Write("\nReferences from file:");
                using (var reader = new StreamReader(FilePath))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        _io.Write(line);
                    }
                }
            }
            else
            {
                _io.Write("\nNo file found. Add references to create the file.");
            }
        }


        public List<Reference> LoadReferencesFromFile()
        {
            var references = new List<Reference>();

            if (!File.Exists(Program.FilePath))
            {
                Console.WriteLine("References file not found.");
                return references;
            }

            try
            {
                // Read all lines from the BibTeX file
                string[] lines = File.ReadAllLines(Program.FilePath);

                // Example parsing logic (you'll need to adapt this based on your BibTeX format)
                Reference? currentReference = null;

                foreach (string line in lines)
                {
                    if (line.StartsWith("@article") || line.StartsWith("@inproceedings"))
                    {
                        if (currentReference != null)
                        {
                            references.Add(currentReference);
                        }

                        // Determine reference type
                        currentReference = line.StartsWith("@article")
                            ? new ArticleReference()
                            : new InProceedingsReference();
                    }
                    else if (line.Contains("=") && currentReference != null)
                    {
                        // Parse key-value pairs (e.g., author = {John Doe})
                        string[] parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim().ToLower();
                            string value = parts[1].Trim().Trim('{', '}', ',');

                            // Assign values to the appropriate property
                            switch (key)
                            {
                                case "author":
                                    currentReference.Author = value;
                                    break;
                                case "title":
                                    currentReference.Title = value;
                                    break;
                                case "year":
                                    currentReference.Year = value;
                                    break;
                                case "journal" when currentReference is ArticleReference article:
                                    article.Journal = value;
                                    break;
                                case "booktitle" when currentReference is InProceedingsReference inProc:
                                    inProc.BookTitle = value;
                                    break;
                                    // Handle other fields as needed
                            }
                        }
                    }
                }

                // Add the last reference if not null
                if (currentReference != null)
                {
                    references.Add(currentReference);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading references from file: {ex.Message}");
            }

            return references;
        }


        /// <summary>
        /// Filters the list of references by a given criterion and value.
        /// </summary>
        /// <param name="references">The list of references to filter.</param>
        /// <remarks>
        /// Supported filter criteria are "author", "journal", "year", and "title".
        /// The filter is case-insensitive.
        /// </remarks>
        public void FilterReferences()
        {
            // Load references from the file
            List<Reference> references = LoadReferencesFromFile();

            if (references.Count == 0)
            {
                Console.WriteLine("No references found in the file.");
                return;
            }

            bool unknownCriteria = true;

            // Ask the user to select filter criteria
            Console.WriteLine("Select filter criteria (e.g., 'author year', 'title', 'author journal'):");
            Console.WriteLine("If you want to filter exactly, use '\"' (e.g., \"John\" for John and John for john Doe, Johnnes and ...)");
            Console.WriteLine("Available criteria: author, journal, year, title");
            string? selectedCriteria = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(selectedCriteria))
            {
                Console.WriteLine("No criteria selected. Displaying all references.");
                ListReferences(references);
                return;
            }

            // Parse selected criteria
            var criteriaList = selectedCriteria.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct();

            // Initialize filter values
            string? authorFilter = null;
            string? journalFilter = null;
            string? yearFilter = null;
            string? titleFilter = null;

            // Prompt the user for each selected criterion
            foreach (var criterion in criteriaList)
            {
                switch (criterion)
                {
                    case "author":
                        Console.WriteLine("Enter author (or leave blank to skip): ");
                        authorFilter = Console.ReadLine()?.Trim();
                        break;
                    case "journal":
                        Console.WriteLine("Enter journal (or leave blank to skip): ");
                        journalFilter = Console.ReadLine()?.Trim();
                        break;
                    case "year":
                        Console.WriteLine("Enter year (or leave blank to skip): ");
                        yearFilter = Console.ReadLine()?.Trim();
                        break;
                    case "title":
                        Console.WriteLine("Enter title (or leave blank to skip): ");
                        titleFilter = Console.ReadLine()?.Trim();
                        break;
                    default:
                        Console.WriteLine($"Unknown criterion: {criterion}");
                        break;
                }
            }



            // Split author filter into individual authors
            var authorFilters = string.IsNullOrEmpty(authorFilter)
                ? new string[] { }
                : authorFilter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToArray();

            // Apply filters to references
            var filteredReferences = references
                .Where(r =>
                    // Author filter
                    (string.IsNullOrEmpty(authorFilter) ||
                    (r.Author != null && authorFilters.All(a =>
                        (a.StartsWith("\"") && a.EndsWith("\"") && r.Author.Split(',').Any(writer => writer.Trim().Equals(a.Trim('"'), StringComparison.OrdinalIgnoreCase))) ||
                        (!a.StartsWith("\"") && !a.EndsWith("\"") && r.Author.Split(',').Any(writer => writer.Trim().Contains(a, StringComparison.OrdinalIgnoreCase)))
                    )))

                    // Journal filter
                    && (string.IsNullOrEmpty(journalFilter) ||
                        (r is ArticleReference article && article.Journal != null && (
                            (journalFilter.StartsWith("\"") && journalFilter.EndsWith("\"") && article.Journal.Equals(journalFilter.Trim('"'), StringComparison.OrdinalIgnoreCase)) ||
                            (!journalFilter.StartsWith("\"") && !journalFilter.EndsWith("\"") && article.Journal.Contains(journalFilter, StringComparison.OrdinalIgnoreCase))
                        )))

                    // Year filter
                    && (string.IsNullOrEmpty(yearFilter) || (r.Year != null && r.Year.Equals(yearFilter.Trim('"'), StringComparison.OrdinalIgnoreCase)))

                    // Title filter
                    && (string.IsNullOrEmpty(titleFilter) ||
                        (r.Title != null && (
                            (!titleFilter.StartsWith("\"") && !titleFilter.EndsWith("\"") && r.Title.Contains(titleFilter, StringComparison.OrdinalIgnoreCase)) ||
                            (titleFilter.StartsWith("\"") && titleFilter.EndsWith("\"") && r.Title.Equals(titleFilter.Trim('"'), StringComparison.OrdinalIgnoreCase))
                        )))
                )
                .ToList();

            // Display results
            if (filteredReferences.Count == 0)
            {
                Console.WriteLine("No references match the given criteria.");
            }
            else
            {
                Console.WriteLine($"Filtered references (matching criteria):");
                foreach (var reference in filteredReferences)
                {
                    Console.WriteLine(reference.ToBibtex());
                }
            }
        }


        /// <summary>
        /// Displays a help message showing all available commands.
        /// </summary>
        private void ShowHelp()
        {
            _io.Write("Available commands:");
            _io.Write("  add - Add a new reference");
            _io.Write("  list - List all references");
            _io.Write("  filter - Filter references by author, journal, year, or title");
            _io.Write("  help - Show available commands");
            _io.Write("  exit - Exit the application");
        }
    }


    /// <summary>
    /// Handles basic input and output for console-based applications.
    /// Provides methods to write to the console and read user input.
    /// </summary>
    public class ConsoleIO
    {
        /// <summary>
        /// Writes a message to the console.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        public virtual void Write(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        /// Reads input from the console.
        /// </summary>
        /// <returns>The user's input as a string.</returns>
        public virtual string Read()
        {
            return Console.ReadLine() ?? string.Empty;
        }
    }

    /// <summary>
    /// The entry point of the application.
    /// </summary>
    public static class EntryPoint
    {
        /// <summary>
        /// The main method where the program starts execution.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            var io = new ConsoleIO();
            var referenceLoader = new FileReferenceLoader(Program.FilePath);
            var program = new Program(io, referenceLoader);
            program.Run();
        }
    }
}
