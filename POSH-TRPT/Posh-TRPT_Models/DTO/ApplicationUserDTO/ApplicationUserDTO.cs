using Microsoft.AspNetCore.Http;
using Posh_TRPT_Models.DTO.RegisterDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.ApplicationUserDTO
{
    public class ApplicationUserDTO
    {
        public string? Country { get; set; }
        public string? State { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }
        public string? Address { get; set; }


        public IFormFile? DrivingLicencePhoto { get; set; }

        public string? DrivingLicenceNumber { get; set; }

        public IFormFile? InsuarnceDocPhoto { get; set; }

        public string? InsuarnceNumber { get; set; }

        public IFormFile? PassportPhoto { get; set; }
        public DateTime? DOB { get; set; }
        public string? PassportNumber { get; set; }
        public string? Name { get; set; }

        public string? Id { get; set; }
        public string? Platform { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
        public string? DeviceId { get; set; }
        public string? UserName { get; set; }
        public string? Mobile { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public DateTime CreatedDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string CreatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UpdatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime? UpdatedDate { get; set; }
    }
}
