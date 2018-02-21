using Blissmo.BookingServiceActor.MessageBrokerProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.ServiceFabric;

namespace Blissmo.BookingServiceActor
{
    class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IMessageBroker>()
                .As<ServiceBusMessageBroker>()
                .SingleInstance();

            builder.RegisterType<IBookingRepository>()
                .As<BookingRepository>()
                .SingleInstance();
        }
    }
}
