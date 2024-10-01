using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class NearestDriverRequest
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? CategoryId { get; set; }
        public string? StateId { get; set; }
    }
}
