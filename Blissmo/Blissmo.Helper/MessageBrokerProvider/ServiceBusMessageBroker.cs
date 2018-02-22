using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helper.MessageBrokerProvider
{
    public class ServiceBusMessageBroker : IMessageBroker
    {
        public async Task ReceiveMessageAsync(string connectionEndPoint, string queueName, Action<string> actionEvent)
        {
            var client = QueueClient.CreateFromConnectionString(connectionEndPoint, queueName);

            client.OnMessage(message =>
            {
                var value = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8).ReadToEnd();
                actionEvent(value);

                Console.WriteLine(String.Format("Message body: {0}", value));
                Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
            });
        }

        public async Task SendMessageAsync<T>(string connectionEndPoint, string queueName, T messageObject) where T : class
        {
            var jsonContent = JsonConvert.SerializeObject(messageObject);

            var client = QueueClient.CreateFromConnectionString(connectionEndPoint, queueName);
            var message = new BrokeredMessage(jsonContent);

            await client.SendAsync(message);
        }
    }
}
