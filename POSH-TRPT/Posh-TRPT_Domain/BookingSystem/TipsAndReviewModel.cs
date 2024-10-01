using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class TipsAndReviewModel
    {
        public double Tip { get; set; } = 0.0;
        public string? Review { get; set; }
        public Guid? BookingId { get; set; }
        public double Rating { get; set; }
    }
}
