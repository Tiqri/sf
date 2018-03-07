using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helpers.MessageBrokerProvider
{
    public class ServiceBusMessageBroker : IMessageBroker
    {

        public async Task ReceiveMessageAsync(BrokerConnection connection, Action<string> actionEvent)
        {
            var client = QueueClient.CreateFromConnectionString(connection.EndPoint, connection.QueueName);

            client.OnMessage(message =>
            {
                var value = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8).ReadToEnd();
                actionEvent(value);

                Console.WriteLine(String.Format("Message body: {0}", value));
                Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
            });
        }

        public async Task SendMessageAsync<T>(BrokerConnection connection, T command) where T : class
        {
            var jsonContent = JsonConvert.SerializeObject(command);

            var client = QueueClient.CreateFromConnectionString(connection.EndPoint, connection.QueueName);
            var message = new BrokeredMessage(jsonContent);

            await client.SendAsync(message);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
