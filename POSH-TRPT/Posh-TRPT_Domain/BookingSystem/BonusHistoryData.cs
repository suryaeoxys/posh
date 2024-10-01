using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class BonusHistoryData
    {
        public decimal? TotalCashback { get; set; } = 0.0m;
        public decimal? ReedemableCashback { get; set; } = 0.0m;
        public List<decimal?>? CashbackHistory { get; set; }
    }
}
