using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions.Equivalency;

namespace ReferenceManager
{
    /// <summary>
    /// Abstract class for references
    /// </summary>
    public abstract class Reference
    {
        public required string Author { get; set; }
        public required string Title { get; set; }
        public required string Year { get; set; }
        public string Key => GenerateKey();

        private string GenerateKey()
        {
            int index = Author.IndexOf(" ");
            string firstname = Author.Substring(0, index);
            string key = firstname + Year + Title[0];
            return key;
        }

        /// <summary>
        /// Method for generating BibTeX
        /// </summary>
        /// <returns>BibTeX as a string</returns>
        public abstract string ToBibtex();
        public abstract bool ToBibtexFile();
    }

    /// <summary>
    /// Class for journal article references
    /// </summary>
    public class ArticleReference : Reference
    {
        public required string Journal { get; set; }
        public required string Volume { get; set; }
        public required string Pages { get; set; }

        public override string ToBibtex()
        {
            if (string.IsNullOrEmpty(Journal) || string.IsNullOrEmpty(Volume) || string.IsNullOrEmpty(Pages) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Year))
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

        public override bool ToBibtexFile()
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Program.FilePath), true))
        {
            string reference = ToBibtex();
            if(!string.IsNullOrEmpty(reference)) 
            {
                outputFile.Write(reference + "\n" + "\n");
                return true;
            }
            else{
                return false;
            }
        }
        }
    }
}
