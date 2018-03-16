using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Blissmo.NotificationService.MessageBrokerProvider
{
    class ServiceBusMessageBroker : IMessageBroker
    {
        private readonly string _endPoint = ConfigurationManager.AppSettings["ServiceBusEndPoint"];
        private readonly string _queueName = ConfigurationManager.AppSettings["ServiceBusQueueName"];

        public async Task ReceiveMessageAsync()
        {
            var client = QueueClient.CreateFromConnectionString(_endPoint, _queueName);

            client.OnMessage(message =>
            {
                var value = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8).ReadToEnd();

                Console.WriteLine(String.Format("Message body: {0}", value));
                Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
            });
        }
    }
}
