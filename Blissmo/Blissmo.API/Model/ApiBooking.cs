using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Model
{
    public class ApiBooking
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public ApiMovie Movie { get; set; }

        public ApiTheater Theater { get; set; }

        public ApiShowTime ShowTime { get; set; }

        public int NumberOfTickets { get; set; }
    }
}
