using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class BookingStatusResponse
    {
        public string? DriverId { get; set; }
        public string? BookingStatus { get; set; }
        public string? BookingStatusId { get; set; }
        public string? StatusType { get; set; }

    }
}
