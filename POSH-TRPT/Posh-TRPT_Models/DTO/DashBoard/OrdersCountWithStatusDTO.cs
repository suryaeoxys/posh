using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.DashBoard
{
    public class OrdersCountWithStatusDTO
    {
        public int Orders { get; set; }
        public string? Status { get; set; }
        public double? OrderPercentage { get; set; }
    }
}
