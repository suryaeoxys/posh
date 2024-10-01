using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Posh_TRPT_Domain.Base;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class RegisterInfo : IDeleteEntity, IAuditEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public RegisterInfo()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
        }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CityId { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        [NotMapped]
        public string? Password { get; set; }
        [NotMapped]
        public string? ConfirmPassword { get; set; }
        public string? RoleType { get; set; }
        public DateTime? DOB { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
        public string? Platform { get; set; }
        public string? DeviceId { get; set; }
        public IFormFile? DrivingLicencePhoto { get; set; }
        public string? MobileNumber { get; set; }
        public string? DrivingLicenceNumber { get; set; }

        public IFormFile? InsuarnceDocPhoto { get; set; }

        public string? InsuarnceNumber { get; set; }

        public IFormFile? PassportPhoto { get; set; }

        public string? PassportNumber { get; set; }
        public bool IsDeleted { get; set ; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get ; set; }
        public DateTime? UpdatedDate { get ; set ; }
        public string? UpdatedBy { get; set ; }
    }
}
