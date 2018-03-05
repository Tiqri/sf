using System;
using System.Collections.Generic;
using System.Text;

namespace Blissmo.Helper.MessageBrokerProvider
{
    public class BrokerConnection
    {
        public string EndPoint { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ExchangeName { get; set; }

        public string QueueName { get; set; }
    }
}
