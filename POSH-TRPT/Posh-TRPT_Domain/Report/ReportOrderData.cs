using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Report
{
    public class ReportOrderData
    {
        public Guid Id { get; set; }
        public TimeSpan PickUpTime { get; set; }
        public TimeSpan DropTime { get; set; }
        public DateTime Date { get; set; }
        public string? RideTotalPrice { get; set; }
        public string? RiderSourceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? Driver { get; set; }
        public string? Rider { get; set; }
        public string? Category { get; set; }
        public string? OrderStatus { get; set; }
        public decimal? TollFees { get; set; }
    }

    public class ReportData
    {
        public List<ReportOrderData>? ReportOrderData { get; set; }
        public string? PdfURL { get; set; }
        public string? ExeclURL { get; set; }
    }
}
