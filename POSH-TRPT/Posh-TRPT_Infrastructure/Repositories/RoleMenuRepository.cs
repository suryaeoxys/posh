using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.RoleMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class RoleMenuRepository : Repository<MenuMaster>, IRoleMenuRepository
    {

        public RoleMenuRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

		#region GetMenuMaster
		/// <summary>
		/// method to GetMenuMaster
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<MenuMaster>> GetMenuMaster()
        {
            var menuResult = Task.Run(() => this.DbContextObj().TblMenuMaster.ToList().OrderBy(s => s.Sequence));

            IEnumerable<MenuMaster> obj = await menuResult;

            return obj;
        }
		#endregion

		#region GetMenuMaster
		/// <summary>
		/// method to GetMenuMaster
		/// </summary>
		/// <param name="UserRole"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MenuMaster>> GetMenuMaster(string UserRole)
        {
            var menuResult = Task.Run(() => this.DbContextObj().TblMenuMaster.Where(s => s.User_Roll == UserRole).ToList());

            IEnumerable<MenuMaster> obj = await menuResult;


            return obj;
        } 
        #endregion
    }
}
