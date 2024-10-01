using Microsoft.EntityFrameworkCore;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.DashBoard
{
    public class OrdersCountWithStatus
    {
        public int Orders { get; set; }
        public string? Status { get; set; }
        public double? OrderPercentage { get; set; } 
    }
}
