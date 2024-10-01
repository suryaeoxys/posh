using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.DashBoard
{
    public class ReportOrderData
    {
        public Guid Id { get; set; }
        public TimeSpan? PickUpTime { get; set; }
        public TimeSpan? DropTime { get; set; }
        public DateTime? Date { get; set; }
        public string? RideTotalPrice { get; set; }
        public string? RiderSourceName { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? Driver { get; set; }
        public string? Rider { get; set; }
        public string? Category { get; set; }
        public string? OrderStatus { get; set; }
        public decimal? TollFees { get; set; }
        public string? NewDate { get; set; }
        public string? DropOffTime { get; set; }
        public string? PickTime { get; set; }
    }

    public class ReportOrderDataDTO
    {
        public List<ReportOrderData>? reportOrderData { get; set; }
        public string? pdfURL { get; set; }
        public string? execlURL { get; set; }
    }
}
