using System;
using System.Threading.Tasks;
using Blissmo.BookingServiceActor.Interfaces.Model;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data.Collections;

namespace Blissmo.BookingServiceActor
{
    class BookingRepository : IBookingRepository
    {
        private readonly IActorStateManager _stateManager;

        public BookingRepository(IActorStateManager stateManager)
        {
            this._stateManager = stateManager;
        }

        public async Task AddBooking(Booking booking)
        {
            await this._stateManager.AddOrUpdateStateAsync("booking", booking, (id, value) => booking);
        }
    }
}
