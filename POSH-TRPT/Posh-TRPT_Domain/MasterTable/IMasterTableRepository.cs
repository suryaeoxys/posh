using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.MasterTable
{
    public interface IMasterTableRepository
    {
        Task<IEnumerable<Country>> GetCountryList();
        Task<IEnumerable<Status>> GetStatusList();
        Task<IEnumerable<RideCategory>> GetRideCategoryList();
        Task<int> AddRideCategory(RideCategory country);
        Task<int> AddCountry(Country country);
        Task<int> UpdateCountry(Country country);
        Task<int> UpdateRideCategory(RideCategory country);
        Task<int> AddState(State country);
        Task<IEnumerable<State>> GetStateList(Guid countryId);
        Task<State> GetStateDetailById(Guid countryId);
        Task<City> GetCityDetailById(Guid countryId);
        Task<IEnumerable<State>> GetStateList();

        Task<int> AddCity(City country);

        Task<IEnumerable<City>> GetCityList();
        Task<int> DeleteCountry(Guid Id);
        Task<int> DeleteState(Guid Id);
        Task<int> DeleteCity(Guid Id);
        Task<int> DeleteRideCategory(Guid Id);

        Task<int> DeleteCategoryPrice(Guid id);
        Task<IEnumerable<CategoryPrice>> GetCategoryPriceList();
        Task<int> AddCategoryPrice(CategoryPrice model);
        Task<CategoryPrice> GetCategoryPriceDetailById(Guid categoryId);
        Task<List<City>> GetCityByStateId(Guid stateId);
    }
}
