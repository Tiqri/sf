using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helper.MessageBrokerProvider
{
    public interface IMessageBroker
    {
        Task SendMessageAsync<T>(string connectionEndPoint, string queueName, T messageObject) where T : class;

        Task ReceiveMessageAsync(string connectionEndPoint, string queueName, Action<string> actionEvent);
    }
}
