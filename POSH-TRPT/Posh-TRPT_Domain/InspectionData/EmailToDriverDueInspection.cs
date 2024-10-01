using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.InspectionData
{
    public class EmailToDriverDueInspection
    {
        public string? RequestUri { get; set; }
        public string? DriverName { get; set; }
        public string? MailTo { get; set; }
        public string? MailFrom { get; set; }
        public string? MailFromAlias { get; set; }
        public string? Password { get; set; }
        public string? Subject { get; set; }
        public string? Inspection_Expiry_Date { get; set; }
        public string? InspectionNote { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? EmailCC { get; set; }
        public string? PhoneNumber {  get; set; }
    }
}
