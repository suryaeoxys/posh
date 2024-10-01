using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.DriverDTO
{
    public class DriverDocumentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
       
        public IFormFile? ProfilePhoto{ get; set; }
       
        public IFormFile? DrivingLicenceDoc { get; set; }
      

        public IFormFile? VehicleRegistrationDoc { get; set; }
       

        public IFormFile? InsuarnceDoc { get; set; }
        [DefaultValue(null)]
       
        public IFormFile? PassportDoc { get; set; }
        public string? ProfilePhotoName { get; set; }
        public string? DrivingLicenceDocName { get; set; }

        public string? VehicleRegistrationDocName { get; set; }

        public string? InsuarnceDocName { get; set; }

        public string? PassportDocName { get; set; }
        public bool IsBackgroundVerificationChecked{ get; set; } = false;

        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string CreatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime? UpdatedDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UpdatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
