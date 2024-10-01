using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class CustomerEphemeralKey
	{
		[JsonProperty("id")]
        public string? Id { get; set; }
		[JsonProperty("type")]
		public string? Type { get; set; }
		[JsonProperty("created")]
		public string? Created { get; set; }
		[JsonProperty("expires")]
		public string? Expires { get; set; }
		[JsonProperty("secret")]
		public string? Secret { get; set; }

    }
}
