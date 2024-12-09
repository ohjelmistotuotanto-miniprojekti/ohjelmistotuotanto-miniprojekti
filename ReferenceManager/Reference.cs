using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceManager
{
    /// <summary>
    /// Abstract class for references.
    /// </summary>
    public abstract class Reference
    {
        public string Author { get; set; }
        public string Title { get; set; }

        private string _year;
        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                // Validation
            }
        }
        public string Key => GenerateKey();

        private string GenerateKey()
        {
            string lastName = Author.Contains(",")
                ? Author.Substring(0, Author.IndexOf(",")).Trim()
                : Author.Split(' ')[0];

            string firstLetterOfTitle = !string.IsNullOrEmpty(Title) ? Title[0].ToString() : "X";

            return $"{lastName}{Year}{firstLetterOfTitle}";
        }

        public abstract string ToBibtex();

        public bool ToBibtexFile()
        {
            try
            {
                string reference = ToBibtex();
                if (string.IsNullOrEmpty(reference))
                {
                    Console.WriteLine("BibTeX generation failed. Reference is invalid.");
                    return false;
                }

                using (var writer = new StreamWriter(Program.FilePath, true))
                {
                    writer.WriteLine(reference + "\n");
                    return true;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"File operation failed: {e.Message}");
                return false;
            }
        }
    }


    /// <summary>
    /// Class for journal article references.
    /// </summary>
    public class ArticleReference : Reference
    {
        public required string Journal { get; set; }
        public required string Volume { get; set; }
        public required string Pages { get; set; }

        public override string ToBibtex()
        {
            if (string.IsNullOrEmpty(Journal) || string.IsNullOrEmpty(Volume) || string.IsNullOrEmpty(Pages) ||
                string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Year))
            {
                return "";
            }

            return
            $"@article{{{Key},\n" +
            $"  author = {{{Author}}},\n" +
            $"  title = {{{Title}}},\n" +
            $"  journal = {{{Journal}}},\n" +
            $"  year = {{{Year}}},\n" +
            $"  volume = {{{Volume}}},\n" +
            $"  pages = {{{Pages}}}\n" +
            $"}}";
        }
    }


    /// <summary>
    /// Class for conference inproceedings references.
    /// </summary>
    public class InProceedingsReference : Reference
    {
        public required string BookTitle { get; set; }

        public override string ToBibtex()
        {
            if (string.IsNullOrEmpty(BookTitle) || string.IsNullOrEmpty(Author) ||
                string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Year))
            {
                return "";
            }

            return
            $"@inproceedings{{{Key},\n" +
            $"  author = {{{Author}}},\n" +
            $"  title = {{{Title}}},\n" +
            $"  booktitle = {{{BookTitle}}},\n" +
            $"  year = {{{Year}}}\n" +
            $"}}";
        }
    }

}
