using Blissmo.API.Model;
using Blissmo.Helper.KeyVault;
using Blissmo.Helper.MessageBrokerProvider;
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
            //this._configuration["MessageBroker:EndPoint"] this._configuration["MessageBroker:ReservationResponseQueueName"]
            //var connection = new BrokerConnection { EndPoint = KeyVault.GetValue("AZURE_SERVICE_BUS_ENDPOINT"), QueueName = KeyVault.GetValue("RESERVATION_RESPONSE_QUEUE_NAME") };
            var connection = new BrokerConnection
            {
                EndPoint = KeyVault.GetValue("RABBITMQ_ENDPOINT"),
                Port = Convert.ToInt32(KeyVault.GetValue("RABBITMQ_PORT")),
                QueueName = KeyVault.GetValue("RESERVATION_RESPONSE_QUEUE_NAME"),
                UserName = "user",
                Password = "eXile1234567"
            };
            _messageBroker.ReceiveMessageAsync(
                connection,
                actionEvent: NotificationEventsHandler.NotificationReceived
            );
        }

        public void Deregister()
        {
            //throw new System.NotImplementedException();
            _messageBroker.Dispose();
        }
    }
}
