using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class BookingStatusResponse
    {
        public string? DriverId { get; set; }
        public string? BookingStatus { get; set; }
        public string? BookingStatusId { get; set; }
		public string? PaymentToDriverStatus { get; set; }
		public string? StatusType { get; set; }
		public bool PayoutStatus { get; set; } = false;
		public string? URL { get; set; }

	}
}
