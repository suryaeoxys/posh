using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
	public class StripeDriverPaymentTransferDetails : AuditEntity<Guid>
	{
		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }	
		public virtual ApplicationUser? ApplicationUser { get; set; }
		public string? TransferId { get; set; }
		public string? Object { get; set; }

        public string? DriverId { get; set; }
        public int Amount { get; set; }


		public int AmountReversed { get; set; }


		public string? BalanceTransactionId { get; set; }

		public DateTime? Created { get; set; }

		public string? Currency { get; set; }


		public string? Description { get; set; }

		public string? DestinationAccountNo { get; set; }


		public string? DestinationPaymentId { get; set; }


		public bool Livemode { get; set; }


		public bool Reversed { get; set; }


		public string? SourceTransaction { get; set; }


		public string? SourceType { get; set; }

		public string? RiderId { get; set; }
		public string? RiderCustomerId { get; set; }
		public string? TransferGroup { get; set; }
		public string? Reversals_Object { get; set; }




		public bool Reversals_HasMore { get; set; }


		public int Reversals_TotalCount { get; set; }


		public string? Reversals_Url { get; set; }
	}
}
