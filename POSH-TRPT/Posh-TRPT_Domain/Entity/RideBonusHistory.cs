using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class RideBonusHistory: AuditEntity<Guid>
    {
        public RideBonusHistory()
        {
            this.IsApplied = false;
        }
        [ForeignKey("DigitalWallet")]
        public Guid? DigitalWalletId { get; set; }
        public virtual DigitalWallet? DigitalWallet { get; set; }
        public string? UserId { get; set; }
        public decimal? CashBack { get; set; } = 0.0m;
        public decimal? Bonus { get; set; } = 0.0m;
        public Guid? BookingId { get; set; }
        public bool IsApplied { get; set; } 
    }
}
