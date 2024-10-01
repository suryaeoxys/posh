using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{


	public class CustomerPaymentMethodAvaliable
	{
		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("url")]
		public string? Url { get; set; }

		[JsonProperty("has_more")]
		public bool HasMore { get; set; }
		public bool IsPaymentMethodAvailable { get; set; }

		[JsonProperty("data")]
		public List<Data>? Data { get; set; }
	}
	public class Address
	{
		[JsonProperty("city")]
		public string? City { get; set; }

		[JsonProperty("country")]
		public string? Country { get; set; }

		[JsonProperty("line1")]
		public string? Line1 { get; set; }

		[JsonProperty("line2")]
		public string? Line2 { get; set; }

		[JsonProperty("postal_code")]
		public string? PostalCode { get; set; }

		[JsonProperty("state")]
		public string? State { get; set; }
	}

	public class BillingDetails
	{
		[JsonProperty("address")]
		public Address? Address { get; set; }

		[JsonProperty("email")]
		public string? Email { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("phone")]
		public string? Phone { get; set; }
	}

	public class Card
	{
		[JsonProperty("brand")]
		public string? Brand { get; set; }

		[JsonProperty("checks")]
		public Checks? Checks { get; set; }

		[JsonProperty("country")]
		public string? Country { get; set; }

		[JsonProperty("exp_month")]
		public int ExpMonth { get; set; }

		[JsonProperty("exp_year")]
		public int ExpYear { get; set; }

		[JsonProperty("fingerprint")]
		public string? Fingerprint { get; set; }

		[JsonProperty("funding")]
		public string? Funding { get; set; }

		[JsonProperty("generated_from")]
		public string? GeneratedFrom { get; set; }

		[JsonProperty("last4")]
		public string? Last4 { get; set; }

		[JsonProperty("networks")]
		public Networks? Networks { get; set; }

		[JsonProperty("three_d_secure_usage")]
		public ThreeDSecureUsage? ThreeDSecureUsage { get; set; }

		[JsonProperty("wallet")]
		public string? Wallet { get; set; }
	}

	public class Checks
	{
		[JsonProperty("address_line1_check")]
		public string? AddressLine1Check { get; set; }

		[JsonProperty("address_postal_code_check")]
		public string? AddressPostalCodeCheck { get; set; }

		[JsonProperty("cvc_check")]
		public string? CvcCheck { get; set; }
	}

	public class Data
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("billing_details")]
		public BillingDetails? BillingDetails { get; set; }

		[JsonProperty("card")]
		public Card? Card { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("customer")]
		public string? Customer { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public Metadata? Metadata { get; set; }

		[JsonProperty("redaction")]
		public string? Redaction { get; set; }

		[JsonProperty("type")]
		public string? Type { get; set; }
	}

	public class Metadata
	{
	}

	public class Networks
	{
		[JsonProperty("available")]
		public List<string>? Available { get; set; }

		[JsonProperty("preferred")]
		public string? Preferred { get; set; }
	}



	public class ThreeDSecureUsage
	{
		[JsonProperty("supported")]
		public bool Supported { get; set; }
	}


}
