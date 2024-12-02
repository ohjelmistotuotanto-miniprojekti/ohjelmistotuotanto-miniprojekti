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
        private string _key;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = GenerateKey();
            }
        }
        public required string Author { get; set; }
        public required string Title { get; set; }
        public required string Year { get; set; }

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
            return "BibTeX";
        }
    }
}
