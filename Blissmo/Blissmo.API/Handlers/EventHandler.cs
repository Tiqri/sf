using Blissmo.API.Model;
using Blissmo.Helper.MessageBrokerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
            _messageBroker.ReceiveMessageAsync(
                connectionEndPoint: "Endpoint=sb://blissmo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BT1A0IXPd6goWbJ4PSidrtMMIQXcKNh4TYJAJcVf364=", 
                queueName: "reservationresponse", 
                actionEvent: NotificationEventsHandler.NotificationReceived
            );
        }

        public void Deregister()
        {
            //throw new System.NotImplementedException();
        }
    }
}
