using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.StripePaymentDTO
{
		public class StripeSetupIntentDTO
	{
		[JsonProperty("id")]
		public string? CustomerPaymentIntentId { get; set; }

		[JsonProperty("client_secret")]
		public string? Client_Secret { get; set; }
		public string? EphemeralKey_Secret { get; set; }
		[JsonProperty("created")]
		public long Created { get; set; }
		[JsonProperty("description")]
		public string? Description { get; set; }
		[JsonProperty("latest_attempt")]
		public string? Latest_Charge { get; set; }
		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("customer")]
		public string? Customer { get; set; }
		[JsonProperty("payment_method")]
		public string? Payment_Method { get; set; }
		[JsonProperty("status")]
		public string? Status { get; set; }
		
		[JsonProperty("usage")]
		public string? Usage { get; set; }


	}
}
