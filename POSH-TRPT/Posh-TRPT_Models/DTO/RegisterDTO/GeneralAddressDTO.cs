using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class GeneralAddressDTO
    {
        public Guid? Id { get; set; }
        public Guid? Country { get; set; }
        public Guid? State { get; set; }

        public Guid? City { get; set; }
        public string? Social_Security_Number { get; set; }
        public string? UserId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
