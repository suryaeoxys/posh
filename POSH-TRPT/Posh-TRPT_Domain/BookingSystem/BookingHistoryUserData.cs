using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class BookingHistoryUserData
    {
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
        [JsonPropertyName("bookingId")]
        public Guid? BookingId { get; set; }
        [JsonPropertyName("sourceToDestinationTime")]
        public string? SourceToDestinationTime { get; set; } = string.Empty;
        [JsonPropertyName("sourceToDestinationDistance")]
        public string? SourceToDestinationDistance { get; set; } = string.Empty;
        [JsonPropertyName("email")]
        public string? Email { get; set; }
       
        [JsonPropertyName("categoryName")]
        public string? CategoryName { get; set; }
        [JsonPropertyName("promotion")]
        public decimal? Promotion { get; set; }

        [JsonPropertyName("url")]
        public string? URL { get; set; }
        [JsonPropertyName("stripeFee")]
        public decimal? StripeFee { get; set; }
        [JsonPropertyName("createdDateTime")]
        public string? CreatedDateTime { get; set; }
        [JsonPropertyName("bookingStatusName")]
        public string? BookingStatusName { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; } = string.Empty;
        [JsonPropertyName("price")]
        public string? Price { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("riderProfilePic")]
        public string? RiderProfilePic { get; set; }
        [JsonPropertyName("driverProfilePic")]
        public string? DriverProfilePic { get; set; }
        [JsonPropertyName("distance")]
        public string? Distance { get; set; }
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("sourceLat")]
        public double? SourceLat { get; set; }
        [JsonPropertyName("sourceLong")]
        public double? SourceLong { get; set; }
        [JsonPropertyName("sourceName")]
        public string? SourceName { get; set; }
        [JsonPropertyName("destinationLat")]
        public double? DestinationLat { get; set; }
        [JsonPropertyName("destinationLong")]
        public double? DestinationLong { get; set; }
        [JsonPropertyName("destinationName")]
        public string? DestinationName { get; set; }
        [JsonPropertyName("vehiclePlate")]
        public string? VehiclePlate { get; set; }
        [JsonPropertyName("vehicleColor")]
        public string? VehicleColor { get; set; }
        [JsonPropertyName("vehicleModel")]
        public string? VehicleModel { get; set; }
       
        [JsonPropertyName("angle")]
        public double? Angle { get; set; }
        [JsonPropertyName("minimumDistance")]
        public int? MinimumDistance { get; set; }
        [JsonPropertyName("riderName")]
        public string? RiderName { get; set; }
		[JsonPropertyName("driverName")]
		public string? DriverName { get; set; }
		[JsonPropertyName("riderId")]
        public string? RiderId { get; set; }
        [JsonPropertyName("riderDeviceId")]
        public string? RiderDeviceId { get; set; }
        [JsonPropertyName("driverId")]
        public string? DriverId { get; set; }
        [JsonPropertyName("driverDeviceId")]
        public string? DriverDeviceId { get; set; }
		[JsonPropertyName("driverLat")]
		public double? DriverLat { get; set; }
		[JsonPropertyName("driverLong")]
		public double? DriverLong { get; set; }
        [JsonPropertyName("serviceFee")]
        public double? ServiceFee { get; set; }
        [JsonPropertyName("reviewByDriver")]
        public string? ReviewByDriver { get; set; }
        [JsonPropertyName("reviewByRider")]
        public string? ReviewByRider { get; set; }
        [JsonPropertyName("tipMoney")]
        public double TipMoney { get; set; }

        [JsonPropertyName("cancellation")]
        public decimal CancellationCharge { get; set; }

        [JsonPropertyName("cancellationStripeFee")]
        public decimal CancellationStripeFee { get; set; }

    }
}
