using Posh_TRPT_Domain.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.DashBoard
{
    public class DriversDataResponse
    {
        public DriversDataResponse()
        {
            this.User = new DriverUserData();
            this.TotalEarnings = 0.0;
            this.Rating = 0.0;
        }
        public double TotalEarnings { get; set; }
        public double Rating { get; set; }
        public DriverUserData? User { get; set; }
    }
}
