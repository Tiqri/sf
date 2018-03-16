using Blissmo.NotificationService.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.NotificationService.Interface
{
    public interface INotificationEvents
    {
        void NotificationReceived(Notification notification);
    }
}
