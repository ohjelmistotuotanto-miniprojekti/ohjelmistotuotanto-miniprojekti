using System;

namespace ReferenceManager
{
    public class Program
    {
        public static string FilePath { get; set; } = "references.bib"; // Default BibTeX file

        public static void Main(string[] args)
        {
            // TODO: Initialize file path based on arguments
            Console.WriteLine($"Using BibTeX file: {FilePath}");

            while (true)
            {
                Console.WriteLine("\nChoose a command (type 'help' for available commands):");
                string command = Console.ReadLine()?.Trim().ToLower();

                switch (command)
                {
                    case "add":
                        AddJournalArticle();
                        break;
                    case "list":
                        ListReferences();
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    case "exit":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Unknown command. Type 'help' to see available commands.");
                        break;
                }
            }
        }

        static void AddJournalArticle()
        {
            // TODO: Implement logic to add a new journal article
        }

        static void ListReferences()
        {
            // TODO: Implement logic to list references
        }

        static void ShowHelp()
        {
            // TODO: Implement help command to show available options
        }

        static string GenerateBibTexKey(string authors, string title, string year)
        {
            // TODO: Implement logic to generate a BibTeX key
            return string.Empty;
        }

        static bool SaveBibTexEntry(string bibKey, string authors, string title, string journal, string year, string volume, string pages)
        {
            // TODO: Implement logic to save a BibTeX entry to file
            return false;
        }
    }
}
