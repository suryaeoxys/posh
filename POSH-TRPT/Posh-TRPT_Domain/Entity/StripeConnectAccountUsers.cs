using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
	public class StripeConnectAccountUsers:AuditEntity<Guid>
	{
		
        public string? ConnectAccountId { get; set; }
        public string? AccountType { get; set; }
		[ForeignKey("ApplicationUser")]
		public string? UserId { get; set; }
        [NotMapped]
		public virtual ApplicationUser? ApplicationUser { get; set; }
		public bool Payouts_Enabled { get; set; }
        public string? Country { get; set; }
        public DateTime? Created { get; set; }
        public string? Default_Currency { get; set; }
        public string? Email { get; set; }
        public string? External_Accounts_URL { get; set; }
        public string? Login_Links_URL { get; set; }
    }
}
