using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class TipsReviews : AuditEntity<Guid>
    {
        public Guid? BookingDetailsId { get; set; }
        public double TipMoney { get; set; } = 0.0;
        public double? TipPaid { get; set; } = 0.0;

        public double? StripeProcessFees { get; set; } = 0.0;
        public string? ReviewByRider { get; set; }
        public double RatingByRider { get; set; }
        public double RatingByDriver { get; set; }
        public string? ReviewByDriver{ get; set; }
        public string? DriverId { get; set; }
        public string? TipPaymentStatus { get; set; }
        public string? RiderId { get; set; }
        public string? TransferId { get; set; }
        public string? BalanceTransactionId { get; set; }
        public string? DestinationAccountNo { get; set; }
        public string? DestinationPaymentId { get; set; }
        public string? RiderCustomerId { get; set; }
    }
}
