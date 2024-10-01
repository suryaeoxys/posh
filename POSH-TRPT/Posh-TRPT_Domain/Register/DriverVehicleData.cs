using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverVehicleData
    {
        public Guid? Id { get; set; }
        public string? Vehicle_Identification_Number { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public Guid? RideCategoryId { get; set; }
        public string? Year { get; set; }
        public string? Color { get; set; }
        public string? Vehicle_Plate { get; set; }
        public string? Inspection_Expiry_Date { get; set; }
    }
}
