using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.RoleMenu
{
    public interface IRoleMenuRepository
    {
        Task<IEnumerable<MenuMaster>> GetMenuMaster();
        Task<IEnumerable<MenuMaster>> GetMenuMaster(String UserRole);
    }
}
