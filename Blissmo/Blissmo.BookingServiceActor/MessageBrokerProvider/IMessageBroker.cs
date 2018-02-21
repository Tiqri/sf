using Blissmo.BookingServiceActor.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.BookingServiceActor.MessageBrokerProvider
{
    public interface IMessageBroker
    {
        Task SendMessageAsync(Booking booking);
    }
}
