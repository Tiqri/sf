using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.BookingServiceActor.Interfaces.Model
{
    [Serializable]
    public class Movie
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string LongDescription { get; set; }

        public string ShortDescription { get; set; }
    }
}
