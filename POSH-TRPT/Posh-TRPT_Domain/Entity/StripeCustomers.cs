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
	public class StripeCustomers: AuditEntity<Guid>
	{

        public string? StripeCustomerId { get; set; }
        public string? Email { get; set; }

		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }
		[NotMapped]
		public virtual ApplicationUser? ApplicationUser { get; set; }
		public bool Livemode { get; set; }
        public string? Name { get; set; }
		public string? DefaultPaymentMethod { get; set; }
		public string? Address { get; set; }
		public int? Balance { get; set; }
		public string? Currency { get; set; }
		public string? DefaultSource { get; set; }
		public string? Description { get; set; }
		public string? Discount { get; set; }
		public string? InvoicePrefix { get; set; }
		public string? Phone { get; set; }
		public string? Shipping { get; set; }
		public string? EphemeralKey { get; set; }
        public decimal? Promotion { get; set; } = 0.0m;
        public string? EphemeralSecret { get; set; }
        public DateTime? EphemeralCreatedDate { get; set; }
		public DateTime? EphemeralExpiresDate { get; set; }

	}
}
