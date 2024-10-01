using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Posh_TRPT_Domain.Register
{
    public class EmailDriver
    {
        public string? RequestUri { get; set; }
        public string? DriverName { get; set; }
        public string? MailTo { get; set; }
        public string? MailFrom { get; set; }
		public string? MailFromAlias { get; set; }
		public string? Password { get; set; }
        public string? Subject { get; set; }
        public Guid? DocStatus { get; set; }
        public string? Reason { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }
}
