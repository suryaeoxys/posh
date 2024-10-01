using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Employees 
{

    [Table("TblEmployees")]
    public partial class Employee : DeleteEntity<int>
    {
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
