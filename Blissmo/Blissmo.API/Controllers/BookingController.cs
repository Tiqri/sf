using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blissmo.API.Model;
using Blissmo.BookingServiceActor.Interfaces;
using Blissmo.BookingServiceActor.Interfaces.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Blissmo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Booking")]
    public class BookingController : Controller
    {
        private static Uri _serviceUri = new Uri($"{ FabricRuntime.GetActivationContext().ApplicationName }/BookingServiceActorService");

        private IBookingServiceActor GetBookingServiceActor(ref Guid bookingId)
        {
            bookingId = bookingId == Guid.Empty ? Guid.NewGuid() : bookingId;
            return ActorProxy.Create<IBookingServiceActor>(new ActorId(bookingId), _serviceUri);
        }

        // POST api/Booking
        [HttpPost]
        public async Task Post([FromBody]ApiBooking booking)
        {
            var bookingId = booking.Id;
            IBookingServiceActor bookingserviceActor = GetBookingServiceActor(ref bookingId);
            booking.Id = bookingId;

            var mappedObject = AutoMapper.Mapper.Map<ApiBooking, Booking>(booking);
            await bookingserviceActor.AddBooking(mappedObject, CancellationToken.None);
        }
    }
}