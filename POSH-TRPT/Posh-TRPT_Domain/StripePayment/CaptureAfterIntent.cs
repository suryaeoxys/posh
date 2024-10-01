using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{

	public class AmountDetail
	{
		[JsonProperty("tip")]
		public Tips? Tip { get; set; }
	}

	public class MetadataFound
	{
	}

	public class PaymentDataMethodOption
	{
	}

	public class CaptureAfterIntent
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
		public AmountDetail? AmountDetails { get; set; }

		[JsonProperty("amount_received")]
		public int AmountReceived { get; set; }

		[JsonProperty("application")]
		public object? Application { get; set; }

		[JsonProperty("application_fee_amount")]
		public object? ApplicationFeeAmount { get; set; }

		[JsonProperty("automatic_payment_methods")]
		public object? AutomaticPaymentMethods { get; set; }

		[JsonProperty("canceled_at")]
		public object?	CanceledAt { get; set; }

		[JsonProperty("cancellation_reason")]
		public object? CancellationReason { get; set; }

		[JsonProperty("capture_method")]
		public string?	 CaptureMethod { get; set; }

		[JsonProperty("client_secret")]
		public string? ClientSecret { get; set; }

		[JsonProperty("confirmation_method")]
		public string? ConfirmationMethod { get; set; }

		[JsonProperty("created")]
		public int Created { get; set; }

		[JsonProperty("currency")]
		public string? Currency { get; set; }

		[JsonProperty("customer")]
		public object? Customer { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("invoice")]
		public object? Invoice { get; set; }

		[JsonProperty("last_payment_error")]
		public object? LastPaymentError { get; set; }

		[JsonProperty("latest_charge")]
		public string? LatestCharge { get; set; }

		[JsonProperty("livemode")]
		public bool Livemode { get; set; }

		[JsonProperty("metadata")]
		public MetadataFound? Metadata { get; set; }

		[JsonProperty("next_action")]
		public object? NextAction { get; set; }

		[JsonProperty("on_behalf_of")]
		public object? OnBehalfOf { get; set; }

		[JsonProperty("payment_method")]
		public string? PaymentMethod { get; set; }

		[JsonProperty("payment_method_options")]
		public PaymentDataMethodOption? PaymentMethodOptions { get; set; }

		[JsonProperty("payment_method_types")]
		public List<string>? PaymentMethodTypes { get; set; }

		[JsonProperty("processing")]
		public object? Processing { get; set; }

		[JsonProperty("receipt_email")]
		public object? ReceiptEmail { get; set; }

		[JsonProperty("redaction")]
		public object? Redaction { get; set; }

		[JsonProperty("review")]
		public object? Review { get; set; }

		[JsonProperty("setup_future_usage")]
		public object? SetupFutureUsage { get; set; }

		[JsonProperty("shipping")]
		public object? Shipping { get; set; }

		[JsonProperty("statement_descriptor")]
		public object? StatementDescriptor { get; set; }

		[JsonProperty("statement_descriptor_suffix")]
		public object? StatementDescriptorSuffix { get; set; }

		[JsonProperty("status")]
		public string? Status { get; set; }

		[JsonProperty("transfer_data")]
		public object? TransferData { get; set; }

		[JsonProperty("transfer_group")]
		public object? TransferGroup { get; set; }
	}

	public class Tips
	{
	}
}
