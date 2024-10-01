using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.StripePaymentDTO
{
	public class StripeDriverPaymentTransferDetailsDTO
	{
        public Guid? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? RiderId { get; set; }
        public string? RiderCustomerId { get; set; }
        public DateTime? UpdatedDate { get; set; }
		public string? UpdatedBy { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string? UserId { get; set; }
		public string? TransferId { get; set; }
		public string? Object { get; set; }

        public string? DriverId { get; set; }
        public double Amount { get; set; }


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


		public string? TransferGroup { get; set; }
		public string? Reversals_Object { get; set; }


		public bool Reversals_HasMore { get; set; }


		public int Reversals_TotalCount { get; set; }


		public string? Reversals_Url { get; set; }
	}
}
