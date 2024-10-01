using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Models.DTO.BookingSystemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO
{
    public class DistanceCalculateDTO
    {
        public SourceDTO? Source { get; set; }
        public DestinationDTO? Destination { get; set; }
        public Guid StateId { get; set; }
        public double CashBackPrice { get; set; }
        public bool IsWalletApplied { get; set; }
        public string? StatusType { get; set; }
        public int MinimumDistance { get; set; }
        public Guid CategoryId { get; set; }
        public RiderDetailDTO? riderDetail { get; set; }
        public DriverDetailDTO? driverDetail { get; set; }
        //public decimal Price { get; set; }
        public string? LocalTime { get; set; }
    }

    public class RiderDetailDTO
    {
        public string? Id { get; set; }
        public string? DeviceId { get; set; }
    }

    public class DriverDetailDTO
    {
        public string? Id { get; set; }
        public string? DeviceId { get; set; }
    }
}
