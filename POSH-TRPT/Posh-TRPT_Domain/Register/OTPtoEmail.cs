using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
	public class OTPtoEmail
	{
		public string? MailTo { get; set; }
		public string? MailFrom { get; set; }
		public string? MailFromAlias { get; set; }
		public string? Password { get; set; }
		public string? Subject { get; set; }
		public string? OTP { get; set; }
		public string? Host { get; set; }
        public string? Status { get; set; }
        public int Port { get; set; }
	}

	public class RideCompletion
	{
		public string? MailTo { get; set; }
		public string? MailFrom { get; set; }
		public string? MailFromAlias { get; set; }
		public string? Password { get; set; }
		public string? Subject { get; set; }
		public string? Attachment { get; set; }
		public string? Name { get; set; }
		public string? Host { get; set; }
		public string? Status { get; set; }
		public int Port { get; set; }
	}
}
