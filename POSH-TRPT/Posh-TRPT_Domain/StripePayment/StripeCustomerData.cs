using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class InvoiceSettings
	{
		[JsonProperty("custom_fields")]
		public string? CustomFields { get; set; }

		[JsonProperty("default_payment_method")]
		public string? DefaultPaymentMethod { get; set; }

		[JsonProperty("footer")]
		public string? Footer { get; set; }

		[JsonProperty("rendering_options")]
		public string? RenderingOptions { get; set; }
	}

	public class MetadataCustomer
	{
	}

	public class StripeCustomerData
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("address")]
		public string? Address { get; set; }

		[JsonProperty("balance")]
		public int Balance { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("currency")]
		public string? Currency { get; set; }

		[JsonProperty("default_source")]
		public string? DefaultSource { get; set; }

		[JsonProperty("delinquent")]
		public bool Delinquent { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("discount")]
		public string? Discount { get; set; }

		[JsonProperty("email")]
		public string? Email { get; set; }

		[JsonProperty("invoice_prefix")]
		public string? InvoicePrefix { get; set; }

		[JsonProperty("invoice_settings")]
		public InvoiceSettings? InvoiceSettings { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public MetadataCustomer? Metadata { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("next_invoice_sequence")]
		public int NextInvoiceSequence { get; set; }

		[JsonProperty("phone")]
		public string? Phone { get; set; }

		[JsonProperty("preferred_locales")]
		public List<object>? PreferredLocales { get; set; }

		[JsonProperty("shipping")]
		public string? Shipping { get; set; }

		[JsonProperty("tax_exempt")]
		public string? TaxExempt { get; set; }

		[JsonProperty("test_clock")]
		public string? TestClock { get; set; }
	}

}
