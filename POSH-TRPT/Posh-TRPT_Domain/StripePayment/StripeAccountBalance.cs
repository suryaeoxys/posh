using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
  
        public class Available
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("source_types")]
        public SourceTypes? SourceTypes { get; set; }
    }

    public class ConnectReserved
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }

    public class Pending
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("source_types")]
        public SourceTypes? SourceTypes { get; set; }
    }

    public class StripeAccountBalance
    {
        [JsonProperty("object")]
        public string? Object { get; set; }

        [JsonProperty("available")]
        public List<Available>? Available { get; set; }

        [JsonProperty("connect_reserved")]
        public List<ConnectReserved>? ConnectReserved { get; set; }

        [JsonProperty("livemode")]
        public bool Livemode { get; set; }

        [JsonProperty("pending")]
        public List<Pending>? Pending { get; set; }
    }

    public class SourceTypes
    {
        [JsonProperty("card")]
        public int Card { get; set; }
    }
}
    

