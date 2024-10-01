using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.PushNotificationDTO
{
    public class NotificationModelDTO
    {
        public string? DeviceId { get; set; }
        public bool IsAndroidDevice { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
