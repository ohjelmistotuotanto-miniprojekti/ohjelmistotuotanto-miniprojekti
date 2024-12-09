using System;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class with the specified IO handler.
        /// </summary>
        /// <param name="io">An instance of ConsoleIO for handling input and output.</param>
        public Program(ConsoleIO io)
        {
            _io = io;
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
            _io.Write("Select article type:");
            _io.Write("1. Article");
            _io.Write("2. Inproceedings");

            string choice = _io.Read().Trim();

            switch (choice)
            {
                case "1":
                    AddJournalArticle(references);
                    break;
                case "2":
                    AddInProceedings(references);
                    break;
                default:
                    _io.Write("Invalid choice. Returning to main menu.");
                    break;
            }
        }

        /// <summary>
        /// Adds a new journal article to the BibTeX file.
        /// </summary>
        public void AddJournalArticle(List<Reference> references)
        {
            var articleReference = new ArticleReference();

            _io.Write("Authors: ");
            string author = _io.Read().Trim();
            articleReference.Author = author;

            while (true)
            {
                _io.Write("Title: ");
                string Title = _io.Read().Trim();
                try
                {
                    articleReference.Title = Title;
                    break;
                }
                catch (Exception ex)
                {
                    _io.Write(ex.Message);
                }
            }

            _io.Write("Journal: ");
            string Journal = _io.Read().Trim();
            articleReference.Journal = Journal;

            _io.Write("Year: ");
            string Year = _io.Read().Trim();
            articleReference.Year = Year;

            _io.Write("Volume: ");
            string Volume = _io.Read().Trim();
            articleReference.Year = Volume;

            _io.Write("Pages: ");
            string Pages = _io.Read().Trim();
            articleReference.Year = Pages;

            _io.Write("Do you want to add this article (y/n)?");
            string confirmation = _io.Read().Trim().ToLower();

            if (confirmation != "y")
            {
                _io.Write("Operation cancelled by the user.");
                return;
            }
            _io.Write("Adding journal article...");

            /*
            var newArticleReference = new ArticleReference
            {
                Author = author,
                Title = Title,
                Journal = Journal,
                Year = Year,
                Volume = Volume,
                Pages = Pages,
            };
            references.Add(newArticleReference);

            if (newArticleReference.ToBibtexFile())
            {
                _io.Write("Journal article added successfully.");
            }
            else
            {
                _io.Write("Failed to add reference to BibTeX file.");
            }*/

        }

        public void AddInProceedings(List<Reference> references)
        {
            _io.Write("Adding an inproceedings article...");

            var InproceedingsReference = new InProceedingsReference();

            _io.Write("Authors: ");
            string author = _io.Read().Trim();
            InproceedingsReference.Author = author;

            string title;
            while (true)
            {
                _io.Write("Title: ");
                title = _io.Read().Trim();
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

            _io.Write("Year: ");
            string year = _io.Read().Trim();
            InproceedingsReference.Year = year;

            string bookTitle;
            while (true)
            {
                _io.Write("Book Title: ");
                bookTitle = _io.Read().Trim();
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

            _io.Write("Do you want to add this inproceedings article (y/n)?");
            string confirmation = _io.Read().Trim().ToLower();

            if (confirmation != "y")
            {
                _io.Write("Operation cancelled by the user.");
                return;
            }

            var newInProceedings = new InProceedingsReference
            {
                Author = author,
                Title = title,
                BookTitle = bookTitle,
                Year = year,
            };
            references.Add(newInProceedings);

            if (newInProceedings.ToBibtexFile())
            {
                _io.Write("Inproceedings article added successfully.");
            }
            else
            {
                _io.Write("Failed to add inproceedings article to BibTeX file.");
            }
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
                    string line;
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

        /// <summary>
        /// Displays a help message showing all available commands.
        /// </summary>
        private void ShowHelp()
        {
            _io.Write("Available commands: add, list, help, exit");
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
            var program = new Program(io);
            program.Run();
        }
    }
}
