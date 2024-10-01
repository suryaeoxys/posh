using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Utility.ConstantStrings
{
    public static class AuthorizationLevel
    {
        public static class Roles
        {
            public const string SuperAdmin = "SuperAdmin";
            public const string Driver = "Driver";
            public const string Customer = "Customer";
            public const string DriverCustomer = "DriverCustomer";
			public const string DriverCustomerSuperAdmin = "DriverCustomerSuperAdmin"; 
				 public const string DriverSuperAdmin = "DriverSuperAdmin";

		}
    }
 
}
