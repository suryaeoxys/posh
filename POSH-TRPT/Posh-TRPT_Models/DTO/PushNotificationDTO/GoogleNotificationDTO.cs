using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.PushNotificationDTO
{
    public class GoogleNotificationDTO
    {
        public class DataPayload
        {
           
            public string? Title { get; set; }
            public string? Body { get; set; }
        }
        public string Priority { get; set; } = "high";
        public DataPayload? Data { get; set; }
        public DataPayload? Notification { get; set; }
    }
}
