using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class EmailAdmin
    {
        public string? SuperAdminName { get; set; }
        public string? EmailFrom { get; set; }
		public string? MailFromAlias { get; set; }
		public string? EmailTo { get; set; }
        public string? Password { get; set; }
        public string? Subject { get; set; }
        public string? RequestUri { get; set; }
        public string? Host { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserContact { get; set; }
		public int Port { get; set; }
	}
}
