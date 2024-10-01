using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class MasterRegister
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }

        public IFormFile? ProfilePhoto { get; set; }

        public IFormFile? DrivingLicenceDoc { get; set; }
            
        public IFormFile? VehicleInspectionDoc { get; set; }
        public IFormFile? VehicleRegistrationDoc { get; set; }
        public bool IsDocActive { get; set; }
        public bool IsAddressActive { get; set; }
        public bool IsVehicleActive { get; set; }
        public bool IsDocDeleted { get; set; } = false;
        public bool IsAddressDeleted { get; set; } = false;
        public bool IsVehicleDeleted { get; set; } = false;
        public IFormFile? InsuarnceDoc { get; set; }
        [DefaultValue(null)]

        public IFormFile? PassportDoc { get; set; }

        public bool IsBackgroundVerificationChecked { get; set; } = false;
        public Guid? Country { get; set; }
        public Guid? State { get; set; }

        public Guid? City { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }

        public string? CityName { get; set; }
        public string? Social_Security_Number { get; set; }
        public string? Vehicle_Identification_Number { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Color { get; set; }
        public DateTime? Inspection_Expiry_Date { get; set; }
        public string? Vehicle_Plate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
