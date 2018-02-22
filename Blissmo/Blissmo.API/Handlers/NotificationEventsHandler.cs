using Blissmo.API.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Handlers
{
    public class NotificationEventsHandler
    {
        public static void NotificationReceived(string notificationJson)
        {
            //Use signalR to send the message to browser
            var notification = JsonConvert.DeserializeObject<Notification>(notificationJson);
        }
    }
}
