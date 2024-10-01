using Newtonsoft.Json;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.StripePaymentDTO
{
	public class StripeCustomerPaymentIntentDTO
	{
		[JsonProperty("id")]
		public string? CustomerPaymentIntentId { get; set; }
		[JsonProperty("amount")]
		public decimal? Amount { get; set; }
		[JsonProperty("amount_received")]
		public decimal? Amount_Received { get; set; }
		[JsonProperty("canceled_at")]
		public string? Canceled_At { get; set; }
		[JsonProperty("cancellation_reason")]
		public string? Cancellation_Reason { get; set; }
		[JsonProperty("client_secret")]
		public string? Client_Secret { get; set; }
		public string? EphemeralKey_Secret { get; set; }
		[JsonProperty("created")]
		public long Created { get; set; }
		[JsonProperty("description")]
		public string? Description { get; set; }
		[JsonProperty("latest_charge")]
		public string? Latest_Charge { get; set; }
		[JsonProperty("livemode")]
		public bool Livemode { get; set; }
		[JsonProperty("currency")]
		public string? Currency { get; set; }
		[JsonProperty("customer")]
		public string? Customer { get; set; }
		[JsonProperty("payment_method")]
		public string? Payment_Method { get; set; }
		[JsonProperty("status")]
		public string? Status { get; set; }
	}
}
