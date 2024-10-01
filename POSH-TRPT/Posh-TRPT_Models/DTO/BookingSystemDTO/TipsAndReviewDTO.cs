using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.BookingSystemDTO
{
    public class TipsAndReviewDTO
    {
        public Guid? Id { get; set; }
        public Guid? BookingDetailsId { get; set; }
        public double TipMoney { get; set; }

        public double? TipPaid { get; set; }

        public double? StripeProcessFees { get; set; }

        public string? ReviewByRider { get; set; }
        public string? ReviewByDriver { get; set; }
        public string? DriverId { get; set; }
        public string? RiderId { get; set; }
        public double RatingByRider { get; set; }
        public double RatingByDriver { get; set; }
        public string? TipPaymentStatus { get; set; }
        public string? TransferId { get; set; }
        public string? BalanceTransactionId { get; set; }
        public string? DestinationAccountNo { get; set; }
        public string? DestinationPaymentId { get; set; }
        public string? RiderCustomerId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
