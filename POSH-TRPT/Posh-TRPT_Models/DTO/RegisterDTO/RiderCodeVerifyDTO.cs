using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class RiderCodeVerifyDTO
    {
        public string? MobileNumber { get; set; }
        public string? HashCode { get; set; }
        public string? Otp { get; set; }
        public string? CountryCode { get; set; }
        public string? DeviceId { get; set; }
    }
}
