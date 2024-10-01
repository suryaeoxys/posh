using Newtonsoft.Json;
using Posh_TRPT_Domain.BookingSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.PushNotification
{
    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonPropertyName("title")]
            public string? title { get; set; }
            [JsonPropertyName("body")]
            public string? body { get; set; }
            [JsonPropertyName("sound")]

            public string? sound { get; set; } = "default";
        }

        public class UserData
        {
            [JsonPropertyName("type")]
            public string? type { get; set; } = string.Empty;
            
            [JsonPropertyName("intentPaymentStatus")]
			public string? IntentPaymentStatus { get; set; } = string.Empty;

			[JsonPropertyName("userData")]
            public UserDataResponse? userData { get; set; }

        }

        
       
        public class UserDataResponse
        {
            [JsonPropertyName("price")]
            public string? price { get; set; }
            [JsonPropertyName("name")]
            public string? name { get; set; }
            [JsonPropertyName("riderProfilePic")]

            public string? RiderProfilePic { get; set; }
            [JsonPropertyName("sourceToDestinationTime")]
            public string? SourceToDestinationTime { get; set; } = string.Empty;
            [JsonPropertyName("sourceToDestinationDistance")]
            public string? SourceToDestinationDistance { get; set; } = string.Empty;
            [JsonPropertyName("driverProfilePic")]
            public string? DriverProfilePic { get; set; }
            [JsonPropertyName("distance")]
            public string? distance { get; set; }
            [JsonPropertyName("time")]
            public string? time { get; set; }
            [JsonPropertyName("id")]
            public Guid id { get; set; }
            [JsonPropertyName("rideCategoryName")]
            public string? RideCategoryName { get; set; }
            [JsonPropertyName("sourceLat")]
            public double? sourceLat { get; set; }
            [JsonPropertyName("sourceLong")]
            public double? sourceLong { get; set; }
            [JsonPropertyName("sourcePlaceName")]
            public string? sourcePlaceName { get; set; }
            [JsonPropertyName("destinationLat")]
            public double? destinationLat { get; set; }
            [JsonPropertyName("destinationLong")]
            public double? destinationLong { get; set; }
            [JsonPropertyName("destinationPlaceName")]
            public string? destinationPlaceName { get; set; }
            [JsonPropertyName("vehiclePlate")]
            public string? VehiclePlate { get; set; }
            [JsonPropertyName("vehicleColor")]
            public string? VehicleColor { get; set; }
            [JsonPropertyName("vehicleModel")]
            public string? VehicleModel { get; set; }
            [JsonPropertyName("riderDetail")]
            public RiderDetail? riderDetail { get; set; }
            [JsonPropertyName("driverDetail")]
            public DriverDetail? driverDetail { get; set; }
            [JsonPropertyName("angle")]
            public double? Angle { get; set; }
            [JsonPropertyName("minimumDistance")]
            public int? MinimumDistance { get; set; }
            [JsonPropertyName("riderName")]
            public string? RiderName { get; set; }
            [JsonPropertyName("serviceFee")]
            public decimal ServiceFee { get; set; }
            [JsonPropertyName("tollFees")]
            public decimal? TollFees { get; set; }

        }
        [JsonPropertyName("to")]
        public string? to { get; set; }
        [JsonPropertyName("priority")]
        public string Priority { get; set; } = "high";
        [JsonPropertyName("data")]
        public UserData? data { get; set; }
        [JsonPropertyName("notification")]
        public DataPayload? notification { get; set; }
    }

    public class RiderDetail
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
		[JsonPropertyName("ridermobile")]
		public string? RiderMobile { get; set; }
        [JsonPropertyName("drivermobile")]
        public string? DriverMobile { get; set; }
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        [JsonPropertyName("source")]

        public Source? source { get; set; }
        [JsonPropertyName("destination")]
        public Destination? destination { get; set; }


	}

    public class DriverDetail
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("ridermobile")]
        public string? RiderMobile { get; set; }
        [JsonPropertyName("drivermobile")]
		public string? DriverMobile { get; set; }
        [JsonPropertyName("rating")]
        public decimal Rating { get; set; }
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        [JsonPropertyName("source")]
        public Source? source { get; set; }
        [JsonPropertyName("destination")]
        public Destination? destination { get; set; }

	}    
    
}
