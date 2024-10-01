using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{


	public class Reversals
	{
		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("data")]
		public List<object>? Data { get; set; }

		[JsonProperty("has_more")]
		public bool HasMore { get; set; }

		[JsonProperty("total_count")]
		public int TotalCount { get; set; }

		[JsonProperty("url")]
		public string? Url { get; set; }
	}

	public class StripeTransfer
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("amount_reversed")]
		public int AmountReversed { get; set; }

		[JsonProperty("balance_transaction")]
		public string? BalanceTransaction { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("currency")]
		public string? Currency { get; set; }

		[JsonProperty("description")]
		public object? Description { get; set; }

		[JsonProperty("destination")]
		public string? Destination { get; set; }

		[JsonProperty("destination_payment")]
		public string? DestinationPayment { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public Metadata? Metadata { get; set; }

		[JsonProperty("reversals")]
		public Reversals? Reversals { get; set; }

		[JsonProperty("reversed")]
		public bool Reversed { get; set; }

		[JsonProperty("source_transaction")]
		public object? SourceTransaction { get; set; }

		[JsonProperty("source_type")]
		public string? SourceType { get; set; }

		[JsonProperty("transfer_group")]
		public string? TransferGroup { get; set; }
	}
	public class MetaData
	{
	}

	

}
	

