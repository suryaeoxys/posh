using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class AvailableCategoryFare
    {
        public string? Name { get; set; }
        public decimal Miles { get; set; }
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public decimal BaseFare { get; set; }
        public decimal Cost_Per_Mile { get; set; }
        public decimal Cost_Per_Minute { get; set; }
        public decimal Maximum_Fare { get; set; }
        public decimal Minimum_Fare { get; set; }
        public decimal Service_Fee { get; set; }
        public decimal Price { get; set; }       
        public double DriverLat { get; set; }
        public double DriverLong { get; set; }
        public int DriverEstimationTime { get; set; }


    }
    public class PriceResponse
    {
        public List<AvailableCategoryFare>? Record { get; set; }
        public decimal? Promotion { get; set; } = 0.0m;
    }

    public class BookingNotificationRequesr
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DriverName { get; set; }
        public string? DeviceId { get; set; }
        public string? UserId { get; set; }
        public double? Angle { get; set; }
        public decimal Price { get; set; }
        public decimal ServiceFee { get; set; }
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Email { get; set; }
        public string? VehiclePlate { get; set; }
        public string? VehicleIdentificationNumber { get; set; }
        public string? Platform { get; set; }
        public double DriverLat { get; set; }
        public double DriverLong { get; set; }
        public int DriverEstimationTime { get; set; }
        public string? UserProfilePic { get; set; }
        public string? RideCategoryName { get; set; }
    }
}
