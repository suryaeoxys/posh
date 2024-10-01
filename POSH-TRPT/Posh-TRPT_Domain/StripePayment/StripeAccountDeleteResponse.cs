using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class StripeAccountDeleteResponse
	{
		
			[JsonProperty("id")]
			public string? CustomerId { get; set; }

			[JsonProperty("object")]
			public string? Object { get; set; }

			[JsonProperty("deleted")]
			public bool Deleted { get; set; }
		
	}
}
