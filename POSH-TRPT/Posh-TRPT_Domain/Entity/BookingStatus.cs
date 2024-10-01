using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class BookingStatus: AuditEntity<Guid>
    {
        public string? Name { get; set; }
    }
}
