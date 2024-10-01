using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class StripeDriverPayment
	{
		public string? LocationFrom { get; set; }
		public string? LocationTo { get; set; }
		public int Rate { get; set; }

		public string? driveraccountNo { get; set; }
	}
}
