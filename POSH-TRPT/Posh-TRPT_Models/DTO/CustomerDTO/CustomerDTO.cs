using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.CustomerDTO
{
    public  class CustomerDTO
    {
        public string? Id { get; set; }
        public string? MobileNumber { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
