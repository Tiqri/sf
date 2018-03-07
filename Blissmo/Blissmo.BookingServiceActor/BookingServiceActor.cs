using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Blissmo.BookingServiceActor.Interfaces;
using Blissmo.BookingServiceActor.Interfaces.Model;
using Blissmo.Helpers.MessageBrokerProvider;
using System.Configuration;
using Blissmo.Helpers.KeyVault;
using System;

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
        private readonly string _endPoint = KeyVault.GetValue("AZURE_SERVICE_BUS_ENDPOINT");
        private readonly string _queueName = KeyVault.GetValue("RESERVATION_QUEUE_NAME");

        /// <summary>
        /// Initializes a new instance of BookingServiceActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param> -> , IMessageBroker messageBroker, IBookingRepository bookingRepository
        public BookingServiceActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            this._bookingMessageBroker = new RabbitMQ(); // new ServiceBusMessageBroker();
            this._bookingRepository = new BookingRepository(this.StateManager);

            return base.OnActivateAsync();
        }

        /// <summary>
        /// Add booking actor method.
        /// Send a message to theater azure function via service bus. Just for *DEMO*
        /// Save the booking object in state
        /// </summary>
        /// <param name="booking">booking object</param>
        /// <returns></returns>
        public async Task AddBooking(Booking booking, CancellationToken cancellationToken)
        {
            var connection = new BrokerConnection
            {
                EndPoint = KeyVault.GetValue("RABBITMQ_ENDPOINT"),
                Port = Convert.ToInt32(KeyVault.GetValue("RABBITMQ_PORT")),
                QueueName = _queueName,
                UserName = "user",
                Password = "eXile1234567"
            };

            try
            {
                await this._bookingMessageBroker.SendMessageAsync(
                     connection,
                     booking);
                await this._bookingRepository.AddBooking(booking);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
