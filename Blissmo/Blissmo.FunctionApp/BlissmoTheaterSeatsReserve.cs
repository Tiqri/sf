using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace Blissmo.FunctionApp
{
    public static class BlissmoTheaterSeatsReserve
    {
        [FunctionName("BlissmoTheaterSeatsReserve")]
        [return: ServiceBus("reservationresponse", Connection = "blissmo_RootManageSharedAccessKey_SERVICEBUS")]
        public static void Run([ServiceBusTrigger("blissmotheaterseatsreserve", AccessRights.Manage, Connection = "Endpoint=sb://blissmo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BT1A0IXPd6goWbJ4PSidrtMMIQXcKNh4TYJAJcVf364=")]BrokeredMessage myQueueItem, TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem.GetBody<string>()}");
        }
    }
}
