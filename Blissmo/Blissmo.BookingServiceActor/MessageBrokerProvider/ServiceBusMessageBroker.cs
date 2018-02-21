using System.Configuration;
using System.Threading.Tasks;
using Blissmo.BookingServiceActor.Interfaces.Model;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Blissmo.BookingServiceActor.MessageBrokerProvider
{
    public class ServiceBusMessageBroker : IMessageBroker
    {
        private readonly string _endPoint = ConfigurationManager.AppSettings["ServiceBusEndPoint"];
        private readonly string _queueName = ConfigurationManager.AppSettings["ServiceBusQueueName"];

        public async Task SendMessageAsync(Booking booking)
        {
            var bookingJson = JsonConvert.SerializeObject(booking);

            var client = QueueClient.CreateFromConnectionString(_endPoint, _queueName);
            var message = new BrokeredMessage(bookingJson);

            await client.SendAsync(message);
        }
    }
}
