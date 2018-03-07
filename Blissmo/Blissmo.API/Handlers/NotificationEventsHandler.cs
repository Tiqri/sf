using Blissmo.API.Model;
using Blissmo.Helpers.MailProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Handlers
{
    public class NotificationEventsHandler
    {
        public static IEmailProvider IEmailProvider;
        public static ServiceContext ServiceContext;

        public static void NotificationReceived(string notificationJson)
        {
            //Use signalR to send the message to browser
            notificationJson = notificationJson.Replace("\"", "");
            var notification = JsonConvert.DeserializeObject<Notification>(notificationJson);
            IEmailProvider.SendEmailAsync(new Email {
                Subject = (notification.IsSuccess ? "The booking has successfully placed" : "The booking has been rejected") + "-" + ServiceContext.NodeContext.NodeName,
                Message = notification.Description,
                Recipients = new List<string> { "mfe@tiqri.com" }
            });
        }
    }
}
