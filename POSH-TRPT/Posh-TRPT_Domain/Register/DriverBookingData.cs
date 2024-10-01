using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverBookingData
    {
        public Guid? Id { get; set; }
        public string? RiderDeviceId { get; set; }
        public string? DriverDeviceId { get; set; }
        public string? Email { get; set; }
        public string? RiderMobile { get; set; }
		public string? DriverMobile { get; set; }
		public string? DriverName { get; set; }
        public string? VehicleIdentificationNumber { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? RiderId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CategoryId { get; set; }
        public int? MinimumDistance { get; set; }
        public string? VehiclePlate { get; set; }
        public string? VehicleColor { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? RideCategoryName { get; set; }
        public string? Platform { get; set; }
        public string? RiderSourceName { get; set; }
        public double? RiderLat { get; set; }
        public double? RiderLong { get; set; }
        public string? Price { get; set; }
        public string? Distance { get; set; }
        public string? RiderName { get; set; }
        public string? Time { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? DriverId { get; set; }
        public double RiderDestinationLat { get; set; }
        public double RiderDestinationLong { get; set; }
        public double? DriverLat { get; set; }
        public double? DriverLong { get; set; }
        public double? Angle { get; set; }
        public string? StatusType { get; set; }
        public Guid? BookingStatusId { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal? StripeFees { get; set; } = 0.0m;


    }
}
