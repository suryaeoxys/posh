using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.API
{
    public  class RouteDistanceRequestAPI
    {
        public dynamic? origins { get; set; }
        public dynamic? destinations { get; set; }
        public dynamic? key { get; set; }
    }
}
