using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class RideBookingStatus
    {
        public string? IsAccepted { get; set; }
        public string? RiderId { get; set; }
        public string? LocalTime { get; set; }
    }
}
