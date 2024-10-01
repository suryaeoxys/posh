using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public interface IRegisterRepository:IRepository<RegisterInfo>
    {
        Task<List<SelectListItem>> GetStates();
        Task<List<SelectListItem>> GetCities();
        Task<List<StateData>> GetStateById(string countryId);
        Task<List<CityData>> GetCityById(string stateId);
        Task<Guid?> AppExists(string countryName, string stateName, string cityName);
        Task<List<CountryData>> GetCountries();
        Task<int> AddDriverDocuments(DriverDocuments driverDocument);
        Task<int> UpdateDriverDocuments(DriverDocuments driverDocument);
        Task<DriverDocuments> GetDriverDocuments(Guid id);
        Task<int> DeleteDriverDocuments(Guid id);
        Task<int> RegisterVehicleDetails(VehicleDetail vehicleDetail);
        Task<int> UpdateVehicleDetails(VehicleDetail vehicleDetail);
        Task<VehicleDetail>GetVehicleDetailsById(Guid id);
        Task<int> DeleteVehicleDetails(Guid id);
        Task<int> UpdateAddress(GeneralAddress generalAddress);
        Task<int> RegisterAddress(GeneralAddress generalAddress);
        Task<GeneralAddress> GetAddressById(Guid id);
        Task<int> DeleteAddress(Guid id);
        Task<MasterRegisterResponse> AddDriverMasterDetails(MasterRegister masterRegisterDTO);
        Task<DriverDataResponse> GetDriverMasterDetails();
        Task<List<DriverUserData>> GetAllDriversBasicDetails();
        Task<DriverDataResponse> GetDriverMasterDetailsById(string userId);
        Task<bool> SetUserApprovalStatus(UserApprovalStatus approvalStatus);
        Task<int> AddOrUpdateDriverLocationInfo(DriverLocationUpdateModel locationData);
        Task<int> DeleteUser(string id);
        Task<string> UpdateUserProfile(UserProfileModel userProfile);
		Task<string> DeleteUserAccount(string id);





	}
}
