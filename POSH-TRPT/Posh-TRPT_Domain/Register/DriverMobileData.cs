using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverMobileData
    {
        public string? Id { get; set; }
        public string? MobileNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? DeviceId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
