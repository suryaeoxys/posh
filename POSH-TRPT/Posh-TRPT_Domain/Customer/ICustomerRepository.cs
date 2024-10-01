using Posh_TRPT_Domain.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Customer
{
    public interface ICustomerRepository
    {

        Task<List<UserMobileData>> GetCustomerDetails();
        Task<UserMobileData> GetCustomerById(string Id);
        Task<int> UpdateCustomerByAdmin(UserMobileData Details);
        Task<int> DeleteCustomerByAdmin(string Id);
        Task<UserMobileData> GetCustomerBasicDetails();
        Task<DriverMobileData> GetNearestDriversDetail(NearestDriverRequest nearestDriver);


    }
}
