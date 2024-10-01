﻿using Microsoft.AspNetCore.Http;
using Posh_TRPT_Domain.Base;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{

    public class DriverDocuments : IDeleteEntity, IAuditEntity
    {
        [Key]
        public Guid Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? DrivingLicenceDocName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [NotMapped]
        public IFormFile? DrivingLicenceDoc { get; set; }
        [NotMapped]
        public IFormFile? VehicleRegistrationDoc { get; set; }
        [NotMapped]
        public IFormFile? InsuarnceDoc { get; set; }
        [DefaultValue(null)]
        [NotMapped]
        public IFormFile? PassportDoc { get; set; }
        [NotMapped]
        public IFormFile? ProfilePhoto { get; set; }
        [NotMapped]
        public string? ProfilePhotoName { get; set; }
        [NotMapped]
        public IFormFile? VehicleInspectionDoc { get; set; }
        public bool IsDocUploadCompleted { get; set; } = false;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? InsuarnceDocName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? VehicleRegistrationDocName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? VehicleInspectionDocName { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? PassportDocName { get; set; }
        [ForeignKey("ApplicationUser")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual ApplicationUser ApplicationUser { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? CreatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime? UpdatedDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? UpdatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public bool IsBackgroundVerificationChecked { get; set; } = false;
    }
}
