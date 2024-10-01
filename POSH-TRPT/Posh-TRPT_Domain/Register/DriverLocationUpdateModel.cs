using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverLocationUpdateModel
    {
        public string? DeviceId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Angle { get; set; }
    }
}
