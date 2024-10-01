using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
	public class StripeDriverPaymentInfo
	{

		public int Rate { get; set; } = 0;
		public string? DriverAccountNo { get; set; }
        public string? RiderId { get; set; }
        public string? RiderCustomerId { get; set; }
    }
}
