using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverRegister
    {
        public string? Name { get; set; }


        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Mobile { get; set; }
        public DateTime? DOB { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string DeviceId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? Platform { get; set; }

    }
}
