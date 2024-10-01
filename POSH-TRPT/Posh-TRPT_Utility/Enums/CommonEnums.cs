using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Utility.Enums
{
    public static class CommonEnums
    {

        public enum BookingStatus
        {
            UNASSIGNED = 0,
            NOTIFIED_DRIVER = 1,
            ACCEPTED=2,
            STARTED=3,
            COMPLETED=4,
            DECLINED=5,
            CANCELLED=6
        };
    }
}
