using System;
using System.Collections.Generic;
using System.Text;

namespace Blissmo.SearchService.Interface.Model
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
