using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.PushNotification
{
    public interface IPushNotificationRepository
    {
        Task<bool> SendNotification(NotificationModel notificationModel);
    }
}
