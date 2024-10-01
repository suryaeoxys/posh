using Newtonsoft.Json;
using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
	public class StripeCustomersSetupIntent:AuditEntity<Guid>
	{
		[ForeignKey("StripeCustomers")]
		public Guid? StripeCustomerRecordId { get; set; }
		public virtual StripeCustomers? StripeCustomers { get; set; }
        public string? CustomerId { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }		
		public virtual ApplicationUser? ApplicationUser { get; set; }
		public bool Livemode { get; set; }
		public string? EphemeralKey { get; set; }
		public string? CustomerPaymentIntentId { get; set; }
		public string? Client_Secret { get; set; }
		public string? EphemeralKey_Secret { get; set; }
		public string? Description { get; set; }
		public string? Latest_Charge { get; set; }
		public string? Payment_Method { get; set; }
		public string? Status { get; set; }
		public string? Usage { get; set; }

	}
}
