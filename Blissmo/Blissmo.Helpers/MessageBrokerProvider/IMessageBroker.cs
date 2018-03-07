using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helpers.MessageBrokerProvider
{
    public interface IMessageBroker
    {
        Task SendMessageAsync<T>(BrokerConnection connection, T command) where T : class;

        Task ReceiveMessageAsync(BrokerConnection connection, Action<string> actionEvent);

        void Dispose();
    }
}
