using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class BookingHistoryData
    {
        public string? BookingId { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedDateTime { get; set; }
        public string? BookingStatusName { get; set; }
        public decimal? Price { get; set; }
        public string? RiderId { get; set; }
        public string? SourceName { get; set; }
        public string? DestinationName { get; set; }
        public string? StatusType { get; set; }
        public decimal ServiceFee { get; set; }
        public string? ReviewByDriver { get; set; }
        public string? ReviewByRider { get; set; }
        public double TipMoney { get; set; }
    }
}

