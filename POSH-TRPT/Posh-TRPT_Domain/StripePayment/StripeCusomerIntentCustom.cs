using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	
	  public class AmountDetails
	{
		[JsonProperty("tip")]
		public Tip? Tip { get; set; }
	}

	public class AutomaticPaymentMethods
	{
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }
	}

	public class CardRecord
	{
		[JsonProperty("installments")]
		public object? Installments { get; set; }

		[JsonProperty("mandate_options")]
		public object? MandateOptions { get; set; }

		[JsonProperty("network")]
		public object? Network { get; set; }

		[JsonProperty("request_three_d_secure")]
		public string? RequestThreeDSecure { get; set; }
	}

	public class Link
	{
		[JsonProperty("persistent_token")]
		public string? PersistentToken { get; set; }
	}

	public class MatadataRecord
	{
	}

	public class PaymentsMethodOptions
	{
		[JsonProperty("card")]
		public CardRecord? Card { get; set; }

		[JsonProperty("link")]
		public Link? Link { get; set; }
	}

	public class StripeCustomerIntentCustom
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("object")]
		public string? Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("amount_capturable")]
		public int AmountCapturable { get; set; }

		[JsonProperty("amount_details")]
		public AmountDetails? AmountDetails { get; set; }

		[JsonProperty("amount_received")]
		public int AmountReceived { get; set; }

		[JsonProperty("application")]
		public string? Application { get; set; }

		[JsonProperty("application_fee_amount")]
		public string? ApplicationFeeAmount { get; set; }

		[JsonProperty("automatic_payment_methods")]
		public AutomaticPaymentMethods? AutomaticPaymentMethods { get; set; }

		[JsonProperty("canceled_at")]
		public string? CanceledAt { get; set; }

		[JsonProperty("cancellation_reason")]
		public string? CancellationReason { get; set; }

		[JsonProperty("capture_method")]
		public string? CaptureMethod { get; set; }

		[JsonProperty("client_secret")]
		public string? ClientSecret { get; set; }

		[JsonProperty("confirmation_method")]
		public string? ConfirmationMethod { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("currency")]
		public string? Currency { get; set; }

		[JsonProperty("customer")]
		public string? Customer { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("invoice")]
		public string? Invoice { get; set; }

		[JsonProperty("last_payment_error")]
		public string? LastPaymentError { get; set; }

		[JsonProperty("latest_charge")]
		public string? LatestCharge { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public MatadataRecord? Metadata { get; set; }

		[JsonProperty("next_action")]
		public string? NextAction { get; set; }

		[JsonProperty("on_behalf_of")]
		public string? OnBehalfOf { get; set; }

		[JsonProperty("payment_method")]
		public string? PaymentMethod { get; set; }

		[JsonProperty("payment_method_options")]
		public PaymentsMethodOptions? PaymentMethodOptions { get; set; }

		[JsonProperty("payment_method_types")]
		public List<string>? PaymentMethodTypes { get; set; }

		[JsonProperty("processing")]
		public string? Processing { get; set; }

		[JsonProperty("receipt_email")]
		public string? ReceiptEmail { get; set; }

		[JsonProperty("review")]
		public string? Review { get; set; }

		[JsonProperty("setup_future_usage")]
		public string? SetupFutureUsage { get; set; }

		[JsonProperty("shipping")]
		public string? Shipping { get; set; }

		[JsonProperty("source")]
		public string? Source { get; set; }

		[JsonProperty("statement_descriptor")]
		public string? StatementDescriptor { get; set; }

		[JsonProperty("statement_descriptor_suffix")]
		public string? StatementDescriptorSuffix { get; set; }

		[JsonProperty("status")]
		public string? Status { get; set; }

		[JsonProperty("transfer_data")]
		public string? TransferData { get; set; }

		[JsonProperty("transfer_group")]
		public string? TransferGroup { get; set; }
	}

	public class Tip
	{
	}
}
