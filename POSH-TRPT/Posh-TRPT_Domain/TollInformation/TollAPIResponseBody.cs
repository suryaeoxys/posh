using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.TollInformation
{
    public class TollAPIResponseBody
    {
        public List<Route> routes { get; set; }
    }
    public class EstimatedPrice
    {
        public string currencyCode { get; set; }
        public string units { get; set; }
    }

    public class Leg
    {
        public TravelAdvisory travelAdvisory { get; set; }
    }

    public class Route
    {
        public List<Leg> legs { get; set; }
        public int distanceMeters { get; set; }
        public string duration { get; set; }
        public TravelAdvisory travelAdvisory { get; set; }
    }

    public class TollInfo
    {
        public List<EstimatedPrice> estimatedPrice { get; set; }
    }

    public class TravelAdvisory
    {
        public TollInfo tollInfo { get; set; }
    }
}
