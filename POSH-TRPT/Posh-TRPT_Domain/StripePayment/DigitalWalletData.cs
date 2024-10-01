using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
    public class DigitalWalletData
    {
        public Guid? Id { get; set; }
        public string? UserId { get; set; }
        public Guid? BookingId { get; set; }
        public decimal? Balance { get; set; }
        public bool IsApplied { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
