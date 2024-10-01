using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class CustomerSetupIntentResponse
	{
		public string? Client_Secret { get; set; }
		public string? EphemeralKey_Secret { get; set; }
		public string? CustomerId { get; set; }
	}
}
