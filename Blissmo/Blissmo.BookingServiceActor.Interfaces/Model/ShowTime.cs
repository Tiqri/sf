using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.BookingServiceActor.Interfaces.Model
{
    [Serializable]
    public class ShowTime
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
