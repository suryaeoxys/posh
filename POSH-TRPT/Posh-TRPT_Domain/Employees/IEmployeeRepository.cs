using Posh_TRPT_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee NewUser(string userName
            , string email);

        Task<IEnumerable<Employee>> GetUsers();
        Task<IEnumerable<Employee>> GetByUsername(string username);

    }
}
