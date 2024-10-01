using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class VehicleDetailDTO
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Vehicle_Identification_Number { get; set; }
        public string? Make { get; set; }
        public string?  Model { get; set; }
        public string? Year { get; set; }
        public string? Color { get; set; }
        public bool? IsRegsDone { get; set; }
        public string? Vehicle_Plate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Inspection_Expiry_Date { get; set; }

    }
}
