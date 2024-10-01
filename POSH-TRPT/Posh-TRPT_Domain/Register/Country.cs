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
    public class Country : AuditEntity<Guid>
    {
      
        public string? Name { get; set; }
        public string? CountryCode { get; set; }

    }
}
