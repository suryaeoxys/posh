using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class DigitalWallet: AuditEntity<Guid>
    {
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public decimal? Balance { get; set; }
        public Guid? BookingId { get; set; }
        public bool IsApplied { get; set; }

    }
}
