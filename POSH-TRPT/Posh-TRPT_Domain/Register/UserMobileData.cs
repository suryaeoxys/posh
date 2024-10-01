using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class UserMobileData
    {
        public string? Id { get; set; }
        public string? MobileNumber { get; set; }
        public string? LocalTimeZone { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? DeviceId { get; set; }
        public string? ProfilePic { get; set; }
        public decimal? Promotion { get; set; } = 0.0m;
    }
}
