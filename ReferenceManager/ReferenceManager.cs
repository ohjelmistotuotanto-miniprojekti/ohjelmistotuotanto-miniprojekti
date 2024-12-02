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

            var testReference = new ArticleReference
            {
                Author = "Allan Collins and John Seely Brown and Ann Holum",
                Title = "Cognitive apprenticeship: making thinking visible",
                Journal = "American Educator",
                Year = "1991",
                Volume = "6",
                Pages = "38--46"
            };
            references.Add(testReference);

            while (true)
            {
                _io.Write("\nChoose a command (type 'help' for available commands):");
                string command = _io.Read().Trim().ToLower();

                switch (command)
                {
                    case "add":
                        AddJournalArticle(references);
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

        /// <summary>
        /// Adds a new journal article to the BibTeX file.
        /// </summary>
        public void AddJournalArticle(List<Reference> references)
        {
            _io.Write("Authors: ");
            string author = _io.Read().Trim();
            _io.Write("Title: ");
            string Title = _io.Read().Trim();
            _io.Write("Journal: ");
            string Journal = _io.Read().Trim();
            _io.Write("Year: ");
            string Year = _io.Read().Trim();
            _io.Write("Volume: ");
            string Volume = _io.Read().Trim();
            _io.Write("Pages: ");
            string Pages = _io.Read().Trim();

            _io.Write("Do you want to add this article");
             string confirmation = _io.Read().Trim().ToLower();

            if (confirmation != "y")
            {
                _io.Write("Operation cancelled by the user.");
                return;
            }
            _io.Write("Adding journal article...");


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

            
        }

        /// <summary>
        /// Lists all references from the BibTeX file.
        /// </summary>
        private void ListReferences(List<Reference> references)
        {
            _io.Write("Listing references...");
            _io.Write(references.Count.ToString());
            // TODO: Implement logic to list references
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
