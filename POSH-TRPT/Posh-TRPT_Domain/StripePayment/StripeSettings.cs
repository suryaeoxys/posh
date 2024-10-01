using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
    public class StripeSettings
    {
        public string? SecretKey { get; set; }
        public string? PublishKey { get; set; }
        public string? StripeBaseUrl { get; set; }
        public string? StripeEphemeralKey { get; set; }
        public string? StripePaymentIntent { get; set; }
		public string? StripeCustomers { get; set; }
        public string? Accounts { get; set; }
		public string? ReAuth { get; set; }
		public string? Return { get; set; }
        public string? Account_Onboarding { get; set; }
        public string? AccountType { get; set; }
        public string? Country { get; set; }
        public string? Setup_Intent { get; set; }
        public string? Payment_Methods { get; set; }
        public string? IsAvaliable { get; set; }
        public string? Balance { get; set; }
        public string? Success { get; set; }
        public string? Cancel { get; set; }
        public string? Price { get; set; }
        public string? Limit { get; set; }
        public string? Product { get; set; }
        public string? Capture { get; set; }
        public string? StripePaymentIntentWithSlash { get; set; }
        //public string? Financials { get; set; }

    }
}
