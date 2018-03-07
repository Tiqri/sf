using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Blissmo.SearchService.Interfaces.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class Movie
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string tmsId { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        public string title { get; set; }

        //[IsSearchable]
        public string longDescription { get; set; }

        //[IsSearchable]
        public string shortDescription { get; set; }
    }
}
