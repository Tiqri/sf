using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Blissmo.BookingServiceActor.Interfaces;
using Blissmo.BookingServiceActor.Interfaces.Model;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using Blissmo.BookingServiceActor.MessageBrokerProvider;

namespace Blissmo.BookingServiceActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class BookingServiceActor : Actor, IBookingServiceActor
    {
        private IMessageBroker _bookingMessageBroker;
        private IBookingRepository _bookingRepository;

        /// <summary>
        /// Initializes a new instance of BookingServiceActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param> -> , IMessageBroker messageBroker, IBookingRepository bookingRepository
        public BookingServiceActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
            this._bookingMessageBroker = new ServiceBusMessageBroker();
            this._bookingRepository = new BookingRepository(this.StateManager);
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            this._bookingMessageBroker = new ServiceBusMessageBroker();

            return this.StateManager.TryAddStateAsync("count", 0);
        }

        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <returns></returns>
        async Task<int> IBookingServiceActor.GetCountAsync(CancellationToken cancellationToken)
        {
            return await this.StateManager.GetStateAsync<int>("count", cancellationToken);
        }

        /// <summary>
        /// Add booking actor method.
        /// Send a message to theater azure function via service bus. Just for *DEMO*
        /// Save the booking object in state
        /// </summary>
        /// <param name="booking">booking object</param>
        /// <returns></returns>
        async Task IBookingServiceActor.AddBooking(Booking booking, CancellationToken cancellationToken)
        {
            await this._bookingMessageBroker.SendMessageAsync(booking);
            await this._bookingRepository.AddBooking(booking);
        }
    }
}
