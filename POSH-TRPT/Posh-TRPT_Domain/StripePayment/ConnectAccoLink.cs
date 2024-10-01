using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class ConnectAccoLink
	{
        public string? ConnectAcctId { get; set; }
		public string? AccountType { get; set; }
		public string? RefreshUrl { get; set; }
		public string? ReturnUrl { get; set; }
        public string? UserId { get; set; }
    }
}
