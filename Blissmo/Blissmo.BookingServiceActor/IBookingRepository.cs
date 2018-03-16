using Blissmo.BookingServiceActor.Interfaces.Model;
using System.Threading.Tasks;

namespace Blissmo.BookingServiceActor
{
    interface IBookingRepository
    {
        Task AddBooking(Booking booking);
    }
}
