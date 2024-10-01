using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class ConnectAccountReturnURL
	{
        public bool PayoutStatus { get; set; }
        public string? URL { get; set; }
    }
}
