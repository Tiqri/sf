using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.RecommendMoviesActor.Interfaces.Models
{
    [Serializable]
    public class Movie
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string LongDescription { get; set; }
        
        public string ShortDescription { get; set; }
    }
}
