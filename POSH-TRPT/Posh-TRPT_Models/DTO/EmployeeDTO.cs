using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO
{
    public partial class EmployeeDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }
    }
}
