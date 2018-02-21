using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.BookingServiceActor.Interfaces.Model
{
    [Serializable]
    public class Booking
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public Movie Movie { get; set; }

        public Theater Theater { get; set; }

        public ShowTime ShowTime { get; set; }

        public int NumberOfTickets { get; set; }        
    }
}
