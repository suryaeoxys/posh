using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.StripePaymentDTO
{
	public class StripeCustomerDTO
	{
		[JsonPropertyName("id")]
		public string? Id{ get; set; }
		[JsonPropertyName("userId")]
		public string? UserId { get; set; }
		[JsonPropertyName("stripeCustomerId")]
		public string? StripeCustomerId { get; set; }

		[JsonPropertyName("email")]
		public string? Email { get; set; }

		[JsonPropertyName("Livemode")]
		public bool Livemode { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("emhemeralKeySecret")]
		public string? EmhemeralKeySecret { get; set; }

	}
}
