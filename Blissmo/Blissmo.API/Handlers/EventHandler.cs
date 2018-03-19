using Blissmo.API.Model;
using Blissmo.Helpers.KeyVault;
using Blissmo.Helpers.MessageBrokerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;

namespace Blissmo.API.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IConfiguration _configuration;

        public EventHandler(IConfiguration configuration, IMessageBroker messageBroker)
        {
            this._configuration = configuration;
            this._messageBroker = messageBroker;
        }

        public void Register()
        {
            var connection = new BrokerConnection
            {
                EndPoint = KeyVault.GetValue("RABBITMQ_ENDPOINT"),
                Port = Convert.ToInt32(KeyVault.GetValue("RABBITMQ_PORT")),
                QueueName = KeyVault.GetValue("RESERVATION_RESPONSE_QUEUENAME"),
                UserName = KeyVault.GetValue("RABBITMQ_USERNAME"),
                Password = KeyVault.GetValue("RABBITMQ_PASSWORD"),
            };

            _messageBroker.ReceiveMessageAsync(
                connection,
                actionEvent: NotificationEventsHandler.NotificationReceived
            );
        }

        public void Deregister()
        {
            _messageBroker.Dispose();
        }
    }
}
