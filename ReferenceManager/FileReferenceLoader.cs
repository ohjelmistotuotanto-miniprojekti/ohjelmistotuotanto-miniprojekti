using System;
using System.Collections.Generic;
using System.IO;

namespace ReferenceManager
{
    /// <summary>
    /// Loads references from a file.
    /// </summary>
    public class FileReferenceLoader : IReferenceLoader
    {
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReferenceLoader"/> class.
        /// </summary>
        /// <param name="filePath">The path to the BibTeX file.</param>
        public FileReferenceLoader(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Loads references from the BibTeX file.
        /// </summary>
        /// <returns>A list of references.</returns>
        public List<Reference> LoadReferences()
        {
            var references = new List<Reference>();

            if (!File.Exists(_filePath))
            {
                Console.WriteLine("References file not found.");
                return references;
            }

            try
            {
                string[] lines = File.ReadAllLines(_filePath);

                Reference? currentReference = null;
                foreach (string line in lines)
                {
                    if (line.StartsWith("@article") || line.StartsWith("@inproceedings"))
                    {
                        if (currentReference != null)
                        {
                            references.Add(currentReference);
                        }

                        currentReference = line.StartsWith("@article")
                            ? new ArticleReference()
                            : new InProceedingsReference();
                    }
                    else if (line.Contains("=") && currentReference != null)
                    {
                        string[] parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim().ToLower();
                            string value = parts[1].Trim().Trim('{', '}', ',');

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
                            }
                        }
                    }
                }

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
    }
}
