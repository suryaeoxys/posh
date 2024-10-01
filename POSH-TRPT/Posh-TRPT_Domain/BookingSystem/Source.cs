using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class Source
    {
        [JsonPropertyName("sourceName")]
        public string? SourceName { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]

        public double Longitude { get; set; }
    }
}
