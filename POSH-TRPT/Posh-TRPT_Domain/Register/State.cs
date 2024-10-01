using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{

    public class State : AuditEntity<Guid>
    {
        public string? Name { get; set; }
        public string? StateCode { get; set; }
        public Guid? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }
    }
}
