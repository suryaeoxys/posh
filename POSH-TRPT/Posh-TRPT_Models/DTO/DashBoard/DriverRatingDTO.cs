using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.DashBoard
{
    public class DriverRatingDTO
    {
        public double Rating { get; set; }
        public string? DriverName { get; set; }
        public string? ProfilePhoto { get; set; }
    }
}
