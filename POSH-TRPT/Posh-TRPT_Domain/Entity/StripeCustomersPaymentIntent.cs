using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
	public class StripeCustomersPaymentIntent : AuditEntity<Guid>
	{
		public string? CustomerPaymentIntentId { get; set; }
		[ForeignKey("StripeCustomers")]
		public Guid? StripeCustomerRecordId { get; set; }
		[NotMapped]
		public virtual StripeCustomers? StripeCustomers { get; set; }
		public decimal? Amount { get; set; }
		[NotMapped]
		public string? EphemeralKey_Secret { get; set; }
		public decimal? Amount_Received { get; set; }
		public string? Canceled_At { get; set; }
		public string? Cancellation_Reason { get; set; }
		public string? Client_Secret { get; set; }
		public DateTime? Created { get; set; }
		public string? Description { get; set; }
		public string? Latest_Charge { get; set; }
		public bool Livemode { get; set; }
		public string? Currency { get; set; }
		public string? Customer { get; set; }
		public string? Payment_Method { get; set; }
		public string? Status { get; set; }
	}
}
