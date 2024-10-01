using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverUserData
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? DOB { get; set; }
        public string? DriverName { get; set;}
        public string? ProfilePhoto { get; set; }
        public string? DocStatus { get; set; }
        public string? Comment { get; set; }
        public string? Inspection_Expiry_Date { get; set; }
    }
}
