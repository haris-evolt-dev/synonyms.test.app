using System.Collections.Generic;

namespace Synonyms.Test.App.Models
{
    public class WordDetails
    {
        public string Name { get; set; }
        public string Definitions { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
    }
}
