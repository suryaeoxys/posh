using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class MenuMaster: AuditEntity<Guid>
    {
        public string? MenuID { get; set; }
        public string? MenuName { get; set; }
        public string? Parent_MenuID { get; set; }
        public string? User_Roll { get; set; }
        public string? MenuFileName { get; set; }
        public string? MenuURL { get; set; }
        public string? USE_YN { get; set; }
        public int Sequence { get; set; }
        
    }
}
