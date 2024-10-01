using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.DashBoard
{
    public class OrderCountsForPDF
    {
        public int TOTAL_RIDES { get; set; }
        public int COMPLETED_RIDES { get; set; } = 0;
        public int CANCELLED_RIDES { get; set; } = 0;
        public int DECLINED_RIDES { get; set; } = 0;
        public int SYSTEMDECLINED_RIDES { get; set; } = 0;
        public int TOTAL_DRIVERS { get; set; } 
        public int TOTAL_RIDERS { get; set; }
    }
}
