using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Model
{
    public class MessageBroker
    {
        public string EndPoint { get; set; }

        public string ReservationQueueName { get; set; }

        public string ReservationResponseQueueName { get; set; }
    }
}
