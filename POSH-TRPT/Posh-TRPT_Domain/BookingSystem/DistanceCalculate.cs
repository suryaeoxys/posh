using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class DistanceCalculate
    {
        public Source? Source { get; set; }
        public Destination? Destination { get; set; }
        public Guid StateId { get; set; }
        public string? StatusType { get; set; }
        public int MinimumDistance { get; set; }
        public Guid CategoryId { get; set; }
        public double CashBackPrice { get; set; }
        public bool IsWalletApplied { get; set; }
        public RiderDetail? riderDetail { get; set; }
        public DriverDetail? driverDetail { get; set; }
        public string? LocalTime { get; set; }


    }


    public class RiderDetail
    {
        public string? Id { get; set; }
        public string? DeviceId { get; set; }
    }

    public class DriverDetail
    {
        public string? Id { get; set; }
        public string? DeviceId { get; set; }
    }
}
