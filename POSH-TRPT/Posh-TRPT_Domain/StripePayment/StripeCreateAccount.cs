using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class StripeCreateAccount
	{
        public string? AccountType { get; set; }
		public string? Country { get; set; }
        public string? Email { get; set; }
		public bool Capabilities_card_payments_requested { get; set; }
        public bool Capabilities_transfers_requested { get; set; }
        public string? UserId  { get; set; }
    }
}
