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
        public bool isInt(string x)
        {
            return int.TryParse(x, out int value);
        }

        private string _author = "";
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
            }
        }

        private string _title = "";
        public string Title
        {
            get => _title;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Title is required.");
                }
                _title = value;
            }
        }

        private string _year = "";
        public string Year
        {
            get => _year;
            set
            {
                if (int.TryParse(value, out int year) && year >= 1 && year <= 9999)
                {
                    _year = value;
                }
                else
                {
                    throw new ArgumentException("Invalid year");
                }
            }
        }
        // Optionals
        public string? ReferenceKey { get; set; }
        private string _month = "";
        public string Month
        {
            get => _month;
            set
            {
                _month = value;
            }
        }

        public string Key => ReferenceKey is not null && !string.IsNullOrWhiteSpace(ReferenceKey) ? ReferenceKey : GenerateKey();

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
        private string _journal = "";
        public string Journal
        {
            get => _journal;
            set
            {
                _journal = value;
                // Validation
            }
        }

        private string _volume = "";
        public string Volume
        {
            get => _volume;
            set
            {
                if (!isInt(value))
                {
                    throw new ArgumentException("Volume must be a number");
                }
                _volume = value;

            }
        }

        private string _pages = "";
        public string Pages
        {
            get => _pages;
            set
            {
                //_pages = value;
                var input = value;
                if (!String.IsNullOrEmpty(value))
                {
                    var remove = new string[] { "--" };
                    foreach (var c in remove)
                    {
                        input = input.Replace(c, string.Empty);
                    }
                    if (!input.All(char.IsDigit))
                    {
                        throw new ArgumentNullException("You must eighter input a range of pages or a single page. Range must be separated by '--'. If there is no pages to input, leave this empty.");
                    }
                }
                else
                {
                    _pages = value;
                }
            }
        }
        private string _note = "";
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
            }
        }

        private string _doi = "";
        public string Doi
        {
            get => _doi;
            set
            {
                _doi = value;
            }
        }

        public override string ToBibtex()
        {
            if (string.IsNullOrEmpty(Journal) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Year))
            {
                return "";
            }

            return
            $"@article{{{Key},\n" +
            $"  author = {{{Author}}},\n" +
            $"  title = {{{Title}}},\n" +
            $"  journal = {{{Journal}}},\n" +
            $"  year = {{{Year}}},\n" +
            (string.IsNullOrEmpty(Month) ? "" : $"  month = {{{Month}}}\n") +
            (string.IsNullOrEmpty(Volume) ? "" : $"  volume = {{{Volume}}},\n") +
            (string.IsNullOrEmpty(Pages) ? "" : $"  pages = {{{Pages}}}\n") +
            (string.IsNullOrEmpty(Note) ? "" : $"  note = {{{Note}}}\n") +
            (string.IsNullOrEmpty(Doi) ? "" : $"  doi = {{{Doi}}}\n") +
            $"}}";
        }
    }


    /// <summary>
    /// Class for conference inproceedings references.
    /// </summary>
    public class InProceedingsReference : Reference
    {
        private string _bookTitle = "";
        public string BookTitle
        {
            get => _bookTitle;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("You must input a book title.");
                }
                _bookTitle = value;
            }
        }

        private string _volume = "";
        public string Volume
        {
            get => _volume;
            set
            {
                if (!isInt(value))
                {
                    throw new ArgumentException("Volume must be a number");
                }
                _volume = value;
            }
        }

        public string? Editor { get; set; }
        public string? Number { get; set; }
        public string? Series { get; set; }

        private string _pages = "";
        public string Pages
        {
            get => _pages;
            set
            {
                var input = value;
                if (!String.IsNullOrEmpty(value))
                {
                    var remove = new string[] { "--" };
                    foreach (var c in remove)
                    {
                        input = input.Replace(c, string.Empty);
                    }
                    if (!input.All(char.IsDigit))
                    {
                        throw new ArgumentNullException("You must eighter input a range of pages or a single page. Range must be separated by '--'. If there is no pages to input, leave this empty.");
                    }
                }
                else
                {
                    _pages = value;
                }
            }
        }
        public string? Address { get; set; }
        public string? Organization { get; set; }
        public string? Publisher { get; set; }
        public string? Note { get; set; }

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
            (string.IsNullOrEmpty(Editor) ? "" : $"  editor = {{{Editor}}},\n") +
            (string.IsNullOrEmpty(Volume) ? "" : $"  volume = {{{Volume}}},\n") +
            (string.IsNullOrEmpty(Series) ? "" : $"  series = {{{Series}}},\n") +
            (string.IsNullOrEmpty(Pages) ? "" : $"  pages = {{{Pages}}},\n") +
            (string.IsNullOrEmpty(Address) ? "" : $"  address = {{{Address}}},\n") +
            (string.IsNullOrEmpty(Month) ? "" : $"  month = {{{Month}}},\n") +
            (string.IsNullOrEmpty(Organization) ? "" : $"  organization = {{{Organization}}},\n") +
            (string.IsNullOrEmpty(Publisher) ? "" : $"  publisher = {{{Publisher}}},\n") +
            (string.IsNullOrEmpty(Note) ? "" : $"  note = {{{Note}}}\n") +
            $"}}";
        }
    }

}
