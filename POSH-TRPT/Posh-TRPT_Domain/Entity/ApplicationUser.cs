using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class ApplicationUser:IdentityUser,IDeleteEntity,IAuditEntity
    {

        public Guid? StatusId { get; set; }

        public string? Comment { get; set; }
        public string? Name { get; set; }
        [NotMapped]
        public string? Password { get; set; }

        [NotMapped]
        public string? ConfirmPassword { get; set; }

        public Guid BookingStatusId { get; set; }
        public bool Payouts_Enabled { get; set; } = false;
        public string? StripeCustomerId { get; set; }
        public string? StripeConnectedAccountId { get; set; }
        public double? Angle { get; set; } = 0;
        public DateTime? DOB { get; set; }
        
        public string? ProfilePhoto { get; set; }
        public bool IsTwilioVerified { get; set; }
        public bool IsLoggedIn { get; set; }

        public string? Platform { get; set; }
        public string? DeviceId { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsDocumentVerified { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public DateTime? CreatedDate { get ; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get ; set ; }
        public DateTime? UpdatedDate { get; set ; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? LocalTimeZone { get; set; }


    }
}
