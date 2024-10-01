using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.ExpireInspection
{
    public class DueInspectionsDriversData
    {
        public string? DriverName {  get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Inspection_Expiry_Date { get; set; }
        public string? InspectionNote { get; set; }
    }
}
