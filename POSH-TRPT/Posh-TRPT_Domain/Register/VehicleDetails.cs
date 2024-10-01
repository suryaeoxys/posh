using Microsoft.AspNetCore.Http;
using Posh_TRPT_Domain.Base;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    [Table("TblVehicleDetails")]
    public class VehicleDetail : AuditEntity<Guid>
    {
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public string? Vehicle_Identification_Number { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Color { get; set; }
        public string? Vehicle_Plate { get; set; }
        public Guid? RideCategoryId { get; set; }
        public DateTime? Inspection_Expiry_Date { get; set; }
       
    }
}
