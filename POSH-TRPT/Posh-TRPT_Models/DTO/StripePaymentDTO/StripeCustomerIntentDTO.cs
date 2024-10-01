using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.StripePaymentDTO
{
	public class StripeCustomerIntentDTO
	{
        public Guid Id { get; set; }
        public string? UserId { get; set; }
		public string? StripeCustomerIntentId { get; set; }
		public string? Object { get; set; }
		public int Amount { get; set; }
		public int AmountCapturable { get; set; }
		public int AmountReceived { get; set; }
		public string? Application { get; set; }
		public string? ApplicationFeeAmount { get; set; }
		public bool AutomaticPaymentMethods_Enable { get; set; }
		public string? CanceledAt { get; set; }
		public string? CancellationReason { get; set; }
		public string? CaptureMethod { get; set; }
		public string? ClientSecret { get; set; }
		public string? ConfirmationMethod { get; set; }
		public string? Currency { get; set; }
		public string? CustomerId { get; set; }
		public string? Description { get; set; }
		public string? Invoice { get; set; }
		public string? LastPaymentError { get; set; }
		public string? LatestCharge { get; set; }
		public bool Livemode { get; set; }
		public string? Metadata { get; set; }
		public string? NextAction { get; set; }
		public string? OnBehalfOf { get; set; }
		public string? PaymentMethod { get; set; }
		public string? PaymentMethodOptions_Card { get; set; }
		public string? PaymentMethodOptions_Link { get; set; }
		public string? PaymentMethodTypes1 { get; set; }
		public string? PaymentMethodTypes2 { get; set; }
		public string? Processing { get; set; }
		public string? ReceiptEmail { get; set; }
		public string? Review { get; set; }
		public string? SetupFutureUsage { get; set; }
		public string? Shipping { get; set; }
		public string? Source { get; set; }
		public string? StatementDescriptor { get; set; }
		public string? StatementDescriptorSuffix { get; set; }
		public string? Status { get; set; }
		public string? TransferData { get; set; }
		public string? TransferGroup { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
