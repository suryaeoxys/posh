using Posh_TRPT_Domain.BookingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.DashBoard
{
    public class MonthlyCompletedCanceledRides
    {
        public string? Date { get; set; }
        public string? Status { get; set; }
        public int? Count { get; set; }
    }
}
