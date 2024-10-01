using Microsoft.AspNetCore.Http;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverDocumentsData
    {
        public Guid? Id { get; set; }

        public string? DrivingLicenceDocName { get; set; }

        public string? InsuarnceDocName { get; set; }

        public string? VehicleRegistrationDocName { get; set; }

        public string? PassportDocName { get; set; }
        public string? VehicleInspectionDocName { get; set; }

    }
}
