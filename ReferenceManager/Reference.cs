using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceManager
{
    /// <summary>
    /// Abstract class for references
    /// </summary>
    public abstract class Reference
    {
        public required string Key { get; set; }
        public required string Author { get; set; }
        public required string Title { get; set; }
        public required string Year { get; set; }

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
}
