using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.NotificationService.MessageBrokerProvider
{
    interface IMessageBroker
    {
        Task ReceiveMessageAsync();
    }
}
