using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class StripePrice
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		[JsonProperty("billing_scheme")]
		public string? BillingScheme { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("currency")]
		public string? Currency { get; set; }

		[JsonProperty("custom_unit_amount")]
		public object? CustomUnitAmount { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("lookup_key")]
		public object? LookupKey { get; set; }

		[JsonProperty("metadata")]
		public MetaDataRecord? Metadata { get; set; }

		[JsonProperty("nickname")]
		public object? Nickname { get; set; }

		[JsonProperty("product")]
		public string? Product { get; set; }

		[JsonProperty("recurring")]
		public Recurring? Recurring { get; set; }

		[JsonProperty("tax_behavior")]
		public string? TaxBehavior { get; set; }

		[JsonProperty("tiers_mode")]
		public object? TiersMode { get; set; }

		[JsonProperty("transform_quantity")]
		public object? TransformQuantity { get; set; }

		[JsonProperty("type")]
		public string? Type { get; set; }

		[JsonProperty("unit_amount")]
		public int UnitAmount { get; set; }

		[JsonProperty("unit_amount_decimal")]
		public string? UnitAmountDecimal { get; set; }
	}
	public class MetaDataRecord
		{
		}

		public class Recurring
		{
			[JsonProperty("aggregate_usage")]
			public object? AggregateUsage { get; set; }

			[JsonProperty("interval")]
			public string? Interval { get; set; }

			[JsonProperty("interval_count")]
			public int IntervalCount { get; set; }

			[JsonProperty("trial_period_days")]
			public object? TrialPeriodDays { get; set; }

			[JsonProperty("usage_type")]
			public string? UsageType { get; set; }
		}

		


	}

