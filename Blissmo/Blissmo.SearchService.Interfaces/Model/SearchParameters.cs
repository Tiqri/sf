using System.Collections.Generic;

namespace Blissmo.SearchService.Interfaces.Model
{
    public class SearchParameters
    {
        public IList<string> Select { get; set; }

        public string Filter { get; set; }

        public IList<string> OrderBy { get; set; }

        public int? Top { get; set; }

        public string SearchTerm { get; set; } = "*";
    }
}
