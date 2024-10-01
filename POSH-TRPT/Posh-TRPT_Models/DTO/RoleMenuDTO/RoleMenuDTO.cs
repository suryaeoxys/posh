using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RoleMenuDTO
{
    public class RoleMenuDTO
    {
       
        public int MenuIdentity { get; set; }
        public string? MenuID { get; set; }
        public string? MenuName { get; set; }
        public string? Parent_MenuID { get; set; }
        public string? User_Roll { get; set; }
        public string? MenuFileName { get; set; }
        public string? MenuURL { get; set; }
        public string? USE_YN { get; set; }
        public DateTime CreatedDate { get; set; }
    }

  
}
