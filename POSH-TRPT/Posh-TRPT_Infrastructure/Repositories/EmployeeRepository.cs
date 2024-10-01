
using Posh_TRPT_Domain.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
    
        public EmployeeRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public Employee NewUser(string userName, string email)
        {
            var user = new Employee()
            {
                UserName = userName,
                Email = email
            };

            this.Add(user);

            return user;
        }

        public async Task<IEnumerable<Employee>> GetUsers()
        {
            return await List(FilterByIsDeleted()).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByUsername(string username)
        {
            return await List(FilterByByUsername(username)).ToListAsync();
        }

        Expression<Func<Employee, bool>> FilterByIsDeleted()
        {
            return x => x.IsDeleted == false;
        }

        Expression<Func<Employee, bool>> FilterByByUsername(string username)
        {
            return x => x.UserName == username;
        }
    }
}
