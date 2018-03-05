using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helper.MessageBrokerProvider
{
    public interface IMessageBroker
    {
        Task SendMessageAsync<T>(BrokerConnection connection, T command) where T : class;

        Task ReceiveMessageAsync(BrokerConnection connection, Action<string> actionEvent);

        void Dispose();
    }
}
