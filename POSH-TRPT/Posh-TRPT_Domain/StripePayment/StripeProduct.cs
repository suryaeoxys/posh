using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class Matadata
	{
	}

	public class StripeProduct
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; } = 0;

		[JsonProperty("default_price")]
		public string? DefaultPrice { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("images")]
		public List<string>? Images { get; set; }

		[JsonProperty("features")]
		public List<string>? Features { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public Matadata? Metadata { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("package_dimensions")]
		public string? PackageDimensions { get; set; }

		[JsonProperty("shippable")]
		public string? Shippable { get; set; }

		[JsonProperty("statement_descriptor")]
		public string? StatementDescriptor { get; set; }

		[JsonProperty("tax_code")]
		public string? TaxCode { get; set; }

		[JsonProperty("unit_label")]
		public string? UnitLabel { get; set; }

		[JsonProperty("updated")]
		public int Updated { get; set; }

		[JsonProperty("url")]
		public string? Url { get; set; }
	}

}
