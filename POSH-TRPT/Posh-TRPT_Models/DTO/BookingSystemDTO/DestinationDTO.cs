using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.BookingSystemDTO
{
    public class DestinationDTO
    {
        public string? DestinationName { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
