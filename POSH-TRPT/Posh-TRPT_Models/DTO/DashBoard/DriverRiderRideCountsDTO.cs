using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.DashBoard
{
    public class DriverRiderRideCountsDTO
    {
        [JsonPropertyName("totalDrivers")]
        public int? TotalDrivers { get; set; }
        [JsonPropertyName("totalRiders")]
        public int? TotalRiders { get; set; }
        [JsonPropertyName("runningRides")]
        public int? RunningRides { get; set; }
        [JsonPropertyName("totalCompleteRides")]
        public int? TotalCompleteRides { get; set; }
    }
}
