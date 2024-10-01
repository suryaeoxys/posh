using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class AutomaticTaxx
	{
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		[JsonProperty("liability")]
		public string? Liability { get; set; }

		[JsonProperty("status")]
		public string? Status { get; set; }
	}

	public class CustomTextt
	{
		[JsonProperty("shipping_address")]
		public string? ShippingAddress { get; set; }

		[JsonProperty("submit")]
		public string? Submit { get; set; }
	}

	public class InvoiceCreations
	{
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		[JsonProperty("invoice_data")]
		public InvoiceDataa? InvoiceData { get; set; }
	}

	public class InvoiceDataa
	{
		[JsonProperty("account_tax_ids")]
		public string? AccountTaxIds { get; set; }

		[JsonProperty("custom_fields")]
		public string? CustomFields { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("footer")]
		public string? Footer { get; set; }

		[JsonProperty("issuer")]
		public string? Issuer { get; set; }

		[JsonProperty("metadata")]
		public Metadata? Metadata { get; set; }

		[JsonProperty("rendering_options")]
		public string? RenderingOptions { get; set; }
	}

	public class Meetadata
	{
	}

	public class PaymentMethodOption
	{
	}

	public class PhoneNumCollection
	{
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }
	}

	public class StripeSessionData
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("after_expiration")]
		public string? AfterExpiration { get; set; }

		[JsonProperty("allow_promotion_codes")]
		public string? AllowPromotionCodes { get; set; }

		[JsonProperty("amount_subtotal")]
		public int AmountSubtotal { get; set; }

		[JsonProperty("amount_total")]
		public int AmountTotal { get; set; }

		[JsonProperty("automatic_tax")]
		public AutomaticTaxx? AutomaticTax { get; set; }

		[JsonProperty("billing_address_collection")]
		public string? BillingAddressCollection { get; set; }

		[JsonProperty("cancel_url")]
		public string? CancelUrl { get; set; }

		[JsonProperty("client_reference_id")]
		public string? ClientReferenceId { get; set; }

		[JsonProperty("consent")]
		public string? Consent { get; set; }

		[JsonProperty("consent_collection")]
		public object? ConsentCollection { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("currency")]
		public string? Currency { get; set; }

		[JsonProperty("custom_fields")]
		public List<object>? CustomFields { get; set; }

		[JsonProperty("custom_text")]
		public CustomTextt? CustomText { get; set; }

		[JsonProperty("customer")]
		public string? Customer { get; set; }

		[JsonProperty("customer_creation")]
		public string? CustomerCreation { get; set; }

		[JsonProperty("customer_details")]
		public object? CustomerDetails { get; set; }

		[JsonProperty("customer_email")]
		public string? CustomerEmail { get; set; }

		[JsonProperty("expires_at")]
		public int ExpiresAt { get; set; }

		[JsonProperty("invoice")]
		public string? Invoice { get; set; }

		[JsonProperty("invoice_creation")]
		public InvoiceCreations? InvoiceCreation { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("locale")]
		public string? Locale { get; set; }

		[JsonProperty("metadata")]
		public Metadata? Metadata { get; set; }

		[JsonProperty("mode")]
		public string? Mode { get; set; }

		[JsonProperty("payment_intent")]
		public string? PaymentIntent { get; set; }

		[JsonProperty("payment_link")]
		public string? PaymentLink { get; set; }

		[JsonProperty("payment_method_collection")]
		public string? PaymentMethodCollection { get; set; }

		[JsonProperty("payment_method_options")]
		public PaymentMethodOption? PaymentMethodOptions { get; set; }

		[JsonProperty("payment_method_types")]
		public List<string>? PaymentMethodTypes { get; set; }

		[JsonProperty("payment_status")]
		public string? PaymentStatus { get; set; }

		[JsonProperty("phone_number_collection")]
		public PhoneNumCollection? PhoneNumCollection { get; set; }

		[JsonProperty("recovered_from")]
		public string? RecoveredFrom { get; set; }

		[JsonProperty("setup_intent")]
		public string? SetupIntent { get; set; }

		[JsonProperty("shipping_address_collection")]
		public string? ShippingAddressCollection { get; set; }

		[JsonProperty("shipping_cost")]
		public string? ShippingCost { get; set; }

		[JsonProperty("shipping_details")]
		public string?  ShippingDetails { get; set; }

		[JsonProperty("shipping_options")]
		public List<string>? ShippingOptions { get; set; }

		[JsonProperty("status")]
		public string? Status { get; set; }

		[JsonProperty("submit_type")]
		public string? SubmitType { get; set; }

		[JsonProperty("subscription")]
		public string? Subscription { get; set; }

		[JsonProperty("success_url")]
		public string? SuccessUrl { get; set; }

		[JsonProperty("total_details")]
		public TotalDetailsInfo? TotalDetails { get; set; }

		[JsonProperty("url")]
		public string? Url { get; set; }
	}

	public class TotalDetailsInfo
	{
		[JsonProperty("amount_discount")]
		public int AmountDiscount { get; set; }

		[JsonProperty("amount_shipping")]
		public int AmountShipping { get; set; }

		[JsonProperty("amount_tax")]
		public int AmountTax { get; set; }
	}

}
