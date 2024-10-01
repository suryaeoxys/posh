using AutoMapper;
using CorePush.Google;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.EmailUtils;
using Posh_TRPT_Utility.FileUtils;
using Posh_TRPT_Utility.JwtUtils;
using Posh_TRPT_Utility.Resources;
using Stripe;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class RegisterRepository : Repository<RegisterInfo>, IRegisterRepository
    {
        RegisterInfo registerInfo = new RegisterInfo();
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IStripePaymentRepository _paymentRepository;
        private readonly ILogger<RegisterRepository> _logger;
        private readonly StripeSettings _stripeSettings;
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;
        private static IWebHostEnvironment _environment => (IWebHostEnvironment)_httpContext.RequestServices.GetService(typeof(IWebHostEnvironment))!;
        public RegisterRepository(IOptions<StripeSettings> stripeSettings, IStripePaymentRepository paymentRepository, IConfiguration config, DbFactory dbFactory, IMapper mapper, ILogger<RegisterRepository> logger, IHttpContextAccessor context, UserManager<ApplicationUser> userManager) : base(dbFactory)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _config = config;
            _paymentRepository = paymentRepository;
            _stripeSettings = stripeSettings.Value;
            _logger = logger;

        }
        #region GetCities
        /// <summary>
        /// method to GetCities
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetCities()
        {
            var cities = await this.DbContextObj().Tbl_Cities.ToListAsync();
            foreach (var city in cities)
            {
                registerInfo.Cities.Add(new SelectListItem { Text = city.Name, Value = city.Id!.ToString() });
            }
            return registerInfo.Cities;
        }
        #endregion

        #region GetCountries
        /// <summary>
        /// method to GetCountries
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryData>> GetCountries()
        {
            try
            {
                var countries = _mapper.Map<List<CountryData>>(await this.DbContextObj().Tbl_Countries.ToListAsync());
                return countries;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetStateById
        /// <summary>
        /// method to GetStateById
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public async Task<List<StateData>> GetStateById(string countryId)
        {
            List<StateData> stateData = new List<StateData>();
            if (!string.IsNullOrEmpty(countryId))
            {
                var states = await (from state in this.DbContextObj().Tbl_States where state.Country!.Id.ToString() == countryId select state).ToListAsync();
                var countryName = await this.DbContextObj().Tbl_Countries.Where(c => c.Id.ToString() == countryId).FirstOrDefaultAsync()!;
                foreach (var state in states)
                {
                    stateData.Add(new StateData { StateId = state.Id.ToString(), StateName = state.Name, CountryName = countryName.Name });
                }
                return stateData;
            }
            return null!;
        }
        #endregion

        #region GetCityById
        /// <summary>
        /// method to GetCityById
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>

        public async Task<List<CityData>> GetCityById(string stateId)
        {
            List<CityData> cityData = new List<CityData>();
            if (!string.IsNullOrEmpty(stateId))
            {
                var cities = await (from city in this.DbContextObj().Tbl_Cities where city.State!.Id.ToString() == stateId select city).ToListAsync();
                var stateName = this.DbContextObj().Tbl_States.Where(c => c.Id.ToString() == stateId).FirstOrDefault()!.Name!;
                foreach (var city in cities)
                {
                    cityData.Add(new CityData { CityId = city.Id.ToString(), CityName = city.Name, StateName = stateName });
                }
                return cityData;
            }
            return null!;
        }
        #endregion

        #region AppExists
        /// <summary>
        /// method to get state, country and city name
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="stateName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public async Task<Guid?> AppExists(string countryName, string stateName, string cityName)
        {
            var cityId = await (from country in this.DbContextObj().Tbl_Countries.Where(s => s.Name!.ToUpper().Contains(countryName.ToUpper()!))

                                join state in this.DbContextObj().Tbl_States.Where(s => (s.Name!.ToUpper().Contains(stateName.ToUpper()!) || (s.StateCode!.ToUpper().Contains(stateName.ToUpper()!))))
                                on country.Id equals state.CountryId
                                join city in this.DbContextObj().Tbl_Cities.Where(s => s.Name!.ToUpper().Contains(cityName.ToUpper()!))
                                on state.Id equals city.StateId
                                select new { cityId = city.Id }
                           ).FirstOrDefaultAsync();
            if (cityId != null)
            {
                return cityId!.cityId;
            }
            return null;
        }
        #endregion

        #region GetStates
        /// <summary>
        /// method to GetStates
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetStates()
        {
            var states = await this.DbContextObj().Tbl_States.ToListAsync();
            foreach (var state in states)
            {
                registerInfo.States.Add(new SelectListItem { Text = state.Name, Value = state.Id!.ToString() });
            }
            return registerInfo.States;
        }
        #endregion

        #region AddDriverDocuments
        /// <summary>
        /// method to AddDriverDocuments
        /// </summary>
        /// <param name="driverDocument"></param>
        /// <returns></returns>
        public async Task<int> AddDriverDocuments(DriverDocuments driverDocument)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblDriverDocuments.AsNoTracking().Where(x => x.UserId == driverDocument.UserId).FirstOrDefaultAsync();
                if (data is null)
                {
                    await this.DbContextObj().TblDriverDocuments.AddAsync(driverDocument);
                    result = await this.DbContextObj().SaveChangesAsync();
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UpdateDriverDocuments
        /// <summary>
        /// method to UpdateDriverDocuments
        /// </summary>
        /// <param name="driverDocument"></param>
        /// <returns></returns>
        public async Task<int> UpdateDriverDocuments(DriverDocuments driverDocument)
        {
            int result;
            try
            {
                this.DbContextObj().Entry(driverDocument).State = EntityState.Modified;
                result = await this.DbContextObj().SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;

            }

            return result;
        }
        #endregion

        #region GetDriverDocuments

        /// <summary>
        /// method to GetDriverDocuments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<DriverDocuments> GetDriverDocuments(Guid id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var data = this.DbContextObj().TblDriverDocuments.AsNoTracking().Where(x => x.UserId == id.ToString() && !x.IsDeleted).FirstOrDefault();
                if (data is not null)
                { return data; }
                return data!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteDriverDocuments
        /// <summary>
        /// method to DeleteDriverDocuments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteDriverDocuments(Guid id)
        {
            int result = 0;
            var data = await this.DbContextObj().TblDriverDocuments.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.IsDeleted = true;
                this.DbContextObj().TblDriverDocuments.Update(data);
                result = await this.DbContextObj().SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region UpdateAddress
        /// <summary>
        /// method to UpdateAddress
        /// </summary>
        /// <param name="generalAddress"></param>
        /// <returns></returns>
        public async Task<int> UpdateAddress(GeneralAddress generalAddress)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblGeneralAddress.AsNoTracking().Where(x => x.Id == generalAddress.Id).FirstOrDefaultAsync();
                if (data is not null)
                {
                    data.UpdatedBy = generalAddress.UserId;
                    data.UpdatedDate = DateTime.UtcNow;
                    data.IsDeleted = generalAddress.IsDeleted;
                    data.UserId = generalAddress.UserId;
                    data.City = generalAddress.City;
                    data.Country = generalAddress.Country;
                    data.State = generalAddress.State;
                    data.Social_Security_Number = generalAddress.Social_Security_Number;
                    this.DbContextObj().Entry(data).State = EntityState.Modified;
                    result = await this.DbContextObj().SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region RegisterAddress
        /// <summary>
        /// method to RegisterAddress
        /// </summary>
        /// <param name="generalAddress"></param>
        /// <returns></returns>
        public async Task<int> RegisterAddress(GeneralAddress generalAddress)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblGeneralAddress.AsNoTracking().Where(x => x.UserId == generalAddress.UserId).FirstOrDefaultAsync();
                if (data is null)
                {
                    await this.DbContextObj().TblGeneralAddress.AddAsync(generalAddress);
                    result = await this.DbContextObj().SaveChangesAsync();
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteAddress
        /// <summary>
        /// method to DeleteAddress
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAddress(Guid id)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblGeneralAddress.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (data is not null)
                {
                    data.IsDeleted = true;
                    this.DbContextObj().Entry(data).State = EntityState.Modified;
                    result = await this.DbContextObj().SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteUser
        /// <summary>
        /// method to DeleteUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteUser(string id)
        {
            try
            {
                int finalResult = 0;
                var dataUser = await _userManager.FindByIdAsync(id);
                if (dataUser is not null)
                {
                    IList<string> userRoles = await _userManager.GetRolesAsync(dataUser);
                    if (userRoles.Count() > 0)
                    {
                        try
                        {
                            string role = userRoles[0];
                            bool isStripeCustumerDeleted = false;
                            bool isStripeConnectAccountDeleted = false;
                            IdentityResult identityResult = null!;
                            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                            switch (role)
                            {
                                case AuthorizationLevel.Roles.Customer:
                                    {
                                        if (dataUser.StripeCustomerId != null)
                                        {
                                            try
                                            {
                                                var service = new CustomerService();
                                                var data = await service.DeleteAsync(dataUser.StripeCustomerId).ConfigureAwait(false);
                                                StripeAccountDeleteResponse deleteResponse = JsonConvert.DeserializeObject<StripeAccountDeleteResponse>(data.ToJson())!;
                                                if (deleteResponse.Deleted)
                                                {
                                                    isStripeCustumerDeleted = true;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                throw;
                                            }

                                        }
                                        if (isStripeCustumerDeleted)
                                        {
                                            dataUser.PhoneNumber = string.Concat(id, "@removed.com");
                                            dataUser.PhoneNumberConfirmed = false;
                                            dataUser.EmailConfirmed = false;
                                            dataUser.Email = string.Concat(id, "@removed.com");
                                            dataUser.NormalizedEmail = string.Concat(id, "@removed.com");
                                            dataUser.IsLoggedIn = false;
                                            dataUser.IsActive = false;
                                            dataUser.IsDeleted = true;
                                            dataUser.UpdatedDate = DateTime.UtcNow;
                                            dataUser.UpdatedBy = id;
                                            identityResult = await _userManager.UpdateAsync(dataUser);
                                            if (identityResult.Succeeded)
                                            {
                                                finalResult = 1;
                                            }
                                            return await Task.FromResult(finalResult!);
                                        }
                                        else
                                        {
                                            dataUser.PhoneNumber = string.Concat(id, "@removed.com");
                                            dataUser.PhoneNumberConfirmed = false;
                                            dataUser.EmailConfirmed = false;
                                            dataUser.Email = string.Concat(id, "@removed.com");
                                            dataUser.NormalizedEmail = string.Concat(id, "@removed.com");
                                            dataUser.IsLoggedIn = false;
                                            dataUser.IsActive = false;
                                            dataUser.IsDeleted = true;
                                            dataUser.UpdatedDate = DateTime.UtcNow;
                                            dataUser.UpdatedBy = id;
                                            identityResult = await _userManager.UpdateAsync(dataUser);
                                            if (identityResult.Succeeded)
                                            {
                                                finalResult = 1;
                                            }
                                            return await Task.FromResult(finalResult!);
                                        }

                                    }
                                case AuthorizationLevel.Roles.Driver:
                                    {
                                        if (dataUser.StripeConnectedAccountId != null)
                                        {
                                            try
                                            {
                                                var service = new AccountService();
                                                var data = await service.DeleteAsync(dataUser.StripeConnectedAccountId).ConfigureAwait(false);
                                                StripeAccountDeleteResponse deleteResponse = JsonConvert.DeserializeObject<StripeAccountDeleteResponse>(data.ToJson())!;
                                                if (deleteResponse.Deleted)
                                                {
                                                    isStripeConnectAccountDeleted = true;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                throw;
                                            }
                                        }
                                        if (isStripeConnectAccountDeleted)
                                        {
                                            dataUser.PhoneNumber = string.Concat(id, "@removed.com");
                                            dataUser.Email = string.Concat(id, "@removed.com");
                                            dataUser.NormalizedEmail = string.Concat(id, "@removed.com");
                                            dataUser.UserName = string.Concat(id, "@removed.com");
                                            dataUser.NormalizedUserName = string.Concat(id, "@removed.com");
                                            dataUser.IsLoggedIn = false;
                                            dataUser.PhoneNumberConfirmed = false;
                                            dataUser.EmailConfirmed = false;
                                            dataUser.IsActive = false;
                                            dataUser.IsDeleted = true;
                                            dataUser.UpdatedDate = DateTime.UtcNow;
                                            dataUser.UpdatedBy = id;
                                            identityResult = await _userManager.UpdateAsync(dataUser);
                                            if (identityResult.Succeeded)
                                            {
                                                finalResult = 1;
                                            }
                                            return await Task.FromResult(finalResult!);
                                        }
                                        else
                                        {
                                            dataUser.PhoneNumber = string.Concat(id, "@removed.com");
                                            dataUser.Email = string.Concat(id, "@removed.com");
                                            dataUser.NormalizedEmail = string.Concat(id, "@removed.com");
                                            dataUser.UserName = string.Concat(id, "@removed.com");
                                            dataUser.NormalizedUserName = string.Concat(id, "@removed.com");
                                            dataUser.IsLoggedIn = false;
                                            dataUser.PhoneNumberConfirmed = false;
                                            dataUser.EmailConfirmed = false;
                                            dataUser.IsActive = false;
                                            dataUser.IsDeleted = true;
                                            dataUser.UpdatedDate = DateTime.UtcNow;
                                            dataUser.UpdatedBy = id;
                                            identityResult = await _userManager.UpdateAsync(dataUser);
                                            if (identityResult.Succeeded)
                                            {
                                                finalResult = 1;
                                            }
                                            return await Task.FromResult(finalResult!);
                                        }

                                    }


                            }
                            return await Task.FromResult(finalResult!);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                return await Task.FromResult(finalResult!);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetAddressById
        /// <summary>
        /// method to GetAddressById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GeneralAddress> GetAddressById(Guid id)
        {
            try
            {
                var data = await this.DbContextObj().TblGeneralAddress.Where(x => x.UserId == id.ToString() && !x.IsDeleted).FirstOrDefaultAsync()!;
                return data!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UpdateVehicleDetails
        /// <summary>
		/// method to UpdateVehicleDetails
		/// </summary>
		/// <param name="vehicleDetail"></param>
		/// <returns></returns>
        public async Task<int> UpdateVehicleDetails(VehicleDetail vehicleDetail)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblVehicleDetails.AsNoTracking().Where(x => x.Id == vehicleDetail.Id).FirstOrDefaultAsync();
                if (data is not null)
                {
                    data.UpdatedBy = vehicleDetail.UserId;
                    data.UpdatedDate = DateTime.UtcNow;
                    data.UserId = vehicleDetail.UserId;
                    data.Vehicle_Identification_Number = vehicleDetail.Vehicle_Identification_Number;
                    data.Color = vehicleDetail.Color;
                    data.IsActive = vehicleDetail.IsActive;
                    data.IsDeleted = vehicleDetail.IsDeleted;
                    data.Make = vehicleDetail.Make;
                    data.Model = vehicleDetail.Model;
                    data.Vehicle_Plate = vehicleDetail.Vehicle_Plate;
                    data.Year = vehicleDetail.Year;
                    this.DbContextObj().Entry(data).State = EntityState.Modified;
                    result = await this.DbContextObj().SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region AddOrUpdateDriverLocationInfo
        /// <summary>
        /// method to AddOrUpdateDriverLocationInfo
        /// </summary>
        /// <param name="locationData"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateDriverLocationInfo(DriverLocationUpdateModel locationData)
        {
            _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterRepository Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Angle :{4}  started.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, locationData.Angle);

            try
            {
                int result = 0;
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var data = await this.DbContextObj().Users.AsNoTracking().Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (data is not null)
                {
                    _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterRepository Start users record Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Angle:{4}  Name : {5} started.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, locationData.Angle, data.Name);

                    data.DeviceId = locationData.DeviceId;
                    data.Latitude = locationData.Latitude;
                    data.Longitude = locationData.Longitude;
                    data.Angle = locationData.Angle;
                    data.UpdatedDate = DateTime.UtcNow;
                    this.DbContextObj().Entry(data).State = EntityState.Modified;
                    result = await this.DbContextObj().SaveChangesAsync();

                    _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterRepository End users record Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Angle:{4}  Name : {5} started.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, locationData.Angle, data.Name);


                    var driverCurrentRecord = await this.DbContextObj().TblBookingDetail?.AsNoTracking().Where(s => s.DriverId == userId && (s.BookingStatusId.ToString()!.ToLower().Equals(GlobalConstants.GlobalValues.BookingStatus.ACCEPT.ToLower()!) || s.BookingStatusId.ToString()!.ToLower().Equals(GlobalConstants.GlobalValues.BookingStatus.STARTED.ToLower()!) || s.BookingStatusId.ToString()!.ToLower().Equals(GlobalConstants.GlobalValues.BookingStatus.COMPLETED.ToLower()!))).OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync()!;
                    if (driverCurrentRecord != null)
                    {
                        _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterRepository Start users booking record Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Angle:{4}  DriverId : {5} started.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, locationData.Angle, driverCurrentRecord.DriverId);

                        driverCurrentRecord.Angle = locationData.Angle;
                        driverCurrentRecord!.DriverLat = locationData.Latitude;
                        driverCurrentRecord.DriverLong = locationData.Longitude;
                        this.DbContextObj().Entry(driverCurrentRecord).State = EntityState.Modified;
                        result = await this.DbContextObj().SaveChangesAsync();
                        _logger.LogInformation("{0} InSide AddOrUpdateDriverLocationInfo Method of RegisterRepository End users booking record Started--   DeviceId:{1} Latitude:{2} Longitude:{3} Angle:{4}  DriverId : {5} started.", DateTime.UtcNow, locationData.DeviceId, locationData.Latitude, locationData.Longitude, locationData.Angle, driverCurrentRecord.DriverId);

                    }

                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region RegisterVehicleDetails

        /// <summary>
        /// method to RegisterVehicleDetails
        /// </summary>
        /// <param name="vehicleDetail"></param>
        /// <returns></returns>
        public async Task<int> RegisterVehicleDetails(VehicleDetail vehicleDetail)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblVehicleDetails.AsNoTracking().Where(x => x.UserId == vehicleDetail.UserId).FirstOrDefaultAsync();
                if (data is null)
                {
                    await this.DbContextObj().TblVehicleDetails.AddAsync(vehicleDetail);
                    result = await this.DbContextObj().SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteVehicleDetails
        /// <summary>
        /// method to DeleteVehicleDetails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteVehicleDetails(Guid id)
        {
            try
            {
                int result = 0;
                var data = await this.DbContextObj().TblVehicleDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (data is not null)
                {
                    data.IsDeleted = true;
                    data.IsActive = false;
                    this.DbContextObj().Entry(data).State = EntityState.Modified;
                    result = await this.DbContextObj().SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetVehicleDetailsById
        /// <summary>
        /// method to GetVehicleDetailsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VehicleDetail> GetVehicleDetailsById(Guid id)
        {
            try
            {
                var data = await this.DbContextObj().TblVehicleDetails.Where(x => x.UserId == id.ToString()).FirstOrDefaultAsync()!;
                return data!;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region AddDriverMasterDetails
        /// <summary>
        /// method to AddDriverMasterDetails
        /// </summary>
        /// <param name="masterRegister"></param>
        /// <returns></returns>
        public async Task<MasterRegisterResponse> AddDriverMasterDetails(MasterRegister masterRegister)
        {
            try
            {
                MasterRegisterResponse registerResponse = new MasterRegisterResponse();
                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                masterRegister.Id = Guid.Parse(userId!);
                bool isUpdate = false;

                if (user is not null)
                {
                    if (masterRegister.ProfilePhoto != null)
                    {
                        user.ProfilePhoto = UploadUtility.ProfilePhotoUpload(masterRegister.ProfilePhoto ??= null!);
                    }
                    bool isDocCompleted = Insert_UpdateDoc(masterRegister).Result;
                    Insert_UpdateAddress(masterRegister, out isUpdate);
                    Insert_UpdateVehicle(masterRegister);

                    user.UpdatedBy = user.Id;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.StatusId = (!isDocCompleted) ? Guid.Parse(GlobalConstants.GlobalValues.Pending) : Guid.Parse(GlobalConstants.GlobalValues.Approval_In_Progress);
                    await _userManager.UpdateAsync(user);
                    registerResponse.GeneralMessage = (isUpdate) ? "Record has been updated successfully" : "Record has been added successfully";

                    return registerResponse;
                }
                registerResponse.GeneralMessage = DriverRegisterResource.UserRelogin;
                return registerResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Insert_UpdateVehicle
        /// <summary>
        /// method to Insert_UpdateVehicle
        /// </summary>
        /// <param name="masterRegister"></param>
        private void Insert_UpdateVehicle(MasterRegister masterRegister)
        {

            try
            {
                VehicleDetail vehicleDetail = new VehicleDetail();
                var vehicleRecord = this.DbContextObj().TblVehicleDetails.Where(s => s.UserId == Convert.ToString(masterRegister.UserId)).FirstOrDefault();
                if (vehicleRecord is null)
                {
                    vehicleDetail.Vehicle_Identification_Number = masterRegister.Vehicle_Identification_Number!;
                    vehicleDetail.Make = masterRegister.Make!;
                    vehicleDetail.Model = masterRegister.Model!;
                    vehicleDetail.Year = masterRegister.Year!;
                    vehicleDetail.Color = masterRegister.Color!;
                    vehicleDetail.Vehicle_Plate = masterRegister.Vehicle_Plate!;
                    vehicleDetail.IsActive = true;
                    vehicleDetail.IsDeleted = false;
                    vehicleDetail.UserId = Convert.ToString(masterRegister.UserId)!;
                    vehicleDetail.CreatedDate = DateTime.UtcNow;
                    vehicleDetail.CreatedBy = masterRegister.CreatedBy!;
                    vehicleDetail.UpdatedDate = DateTime.UtcNow;
                    vehicleDetail.UpdatedBy = masterRegister.CreatedBy!;
                    vehicleDetail.Inspection_Expiry_Date = masterRegister.Inspection_Expiry_Date;
                    this.DbContextObj().TblVehicleDetails.Add(vehicleDetail);
                }
                else
                {
                    vehicleRecord.Vehicle_Identification_Number = masterRegister.Vehicle_Identification_Number!;
                    vehicleRecord.Make = masterRegister.Make!;
                    vehicleRecord.Model = masterRegister.Model!;
                    vehicleRecord.Year = masterRegister.Year!;
                    vehicleRecord.Color = masterRegister.Color!;
                    vehicleRecord.Vehicle_Plate = masterRegister.Vehicle_Plate!;
                    vehicleRecord.IsActive = masterRegister.IsActive;
                    vehicleRecord.IsDeleted = masterRegister.IsVehicleDeleted;
                    vehicleRecord.UpdatedDate = DateTime.UtcNow;
                    vehicleRecord.UpdatedBy = masterRegister.CreatedBy!;
                    vehicleRecord.Inspection_Expiry_Date = masterRegister.Inspection_Expiry_Date;
                    //  this.DbContextObj().Entry(vehicleRecord).State = EntityState.Modified;
                    this.DbContextObj().TblVehicleDetails.Update(vehicleRecord);
                }
                this.DbContextObj().SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Insert_UpdateAddress
        /// <summary>
        /// method to Insert_UpdateAddress
        /// </summary>
        /// <param name="masterRegister"></param>
        /// <param name="isUpdateRecord"></param>
        private void Insert_UpdateAddress(MasterRegister masterRegister, out bool isUpdateRecord)
        {
            try
            {
                GeneralAddress generalAddress = new GeneralAddress();
                var addressRecord = this.DbContextObj().TblGeneralAddress.Where(s => s.UserId == Convert.ToString(masterRegister.UserId)).FirstOrDefault();
                if (addressRecord is null)
                {
                    generalAddress.Country = masterRegister.Country!;
                    generalAddress.State = masterRegister.State!;
                    generalAddress.City = masterRegister.City!;
                    generalAddress.Social_Security_Number = masterRegister.Social_Security_Number!;
                    generalAddress.UserId = Convert.ToString(masterRegister.UserId)!;
                    generalAddress.IsDeleted = masterRegister.IsAddressDeleted;
                    generalAddress.CreatedDate = DateTime.UtcNow;
                    generalAddress.CreatedBy = masterRegister.CreatedBy!;
                    generalAddress.UpdatedDate = DateTime.UtcNow;
                    generalAddress.UpdatedBy = masterRegister.CreatedBy!;
                    this.DbContextObj().TblGeneralAddress.Add(generalAddress);
                    isUpdateRecord = false;
                }
                else
                {
                    addressRecord.Country = masterRegister.Country!;
                    addressRecord.State = masterRegister.State!;
                    addressRecord.City = masterRegister.City!;
                    addressRecord.Social_Security_Number = masterRegister.Social_Security_Number!;
                    addressRecord.IsDeleted = masterRegister.IsAddressDeleted;
                    addressRecord.UpdatedDate = DateTime.UtcNow;
                    addressRecord.UpdatedBy = masterRegister.CreatedBy!;
                    this.DbContextObj().Entry(addressRecord).State = EntityState.Modified;
                    isUpdateRecord = true;
                }
                this.DbContextObj().SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Insert_UpdateDoc
        /// <summary>
        /// method to Insert_UpdateDoc
        /// </summary>
        /// <param name="masterRegister"></param>
        /// <returns></returns>
        private async Task<bool> Insert_UpdateDoc(MasterRegister masterRegister)
        {
            try
            {
                DriverDocuments driverDocuments = new DriverDocuments();
                var docRecord = this.DbContextObj().TblDriverDocuments.Where(s => s.UserId == Convert.ToString(masterRegister.UserId)).FirstOrDefault();
                if (docRecord is null)
                {
                    driverDocuments.InsuarnceDocName = UploadUtility.DocumentsUpload(masterRegister.InsuarnceDoc ??= null);
                    driverDocuments.PassportDocName = UploadUtility.DocumentsUpload(masterRegister.PassportDoc ??= null);
                    driverDocuments.DrivingLicenceDocName = UploadUtility.DocumentsUpload(masterRegister.DrivingLicenceDoc ??= null);
                    driverDocuments.VehicleRegistrationDocName = UploadUtility.DocumentsUpload(masterRegister.VehicleRegistrationDoc ??= null);
                    driverDocuments.CreatedBy = Convert.ToString(masterRegister.UserId)!;
                    driverDocuments.CreatedDate = DateTime.UtcNow;
                    driverDocuments.IsDeleted = masterRegister.IsDocDeleted;
                    driverDocuments.UpdatedBy = Convert.ToString(masterRegister.UserId)!;
                    driverDocuments.UpdatedDate = DateTime.UtcNow;
                    driverDocuments.UserId = Convert.ToString(masterRegister.UserId)!;
                    driverDocuments.VehicleInspectionDocName = UploadUtility.DocumentsUpload(masterRegister.VehicleInspectionDoc ??= null);
                    await this.DbContextObj().TblDriverDocuments.AddAsync(driverDocuments);
                }
                else
                {
                    if (masterRegister.InsuarnceDoc != null)
                    {
                        docRecord.InsuarnceDocName = UploadUtility.DocumentsUpload(masterRegister.InsuarnceDoc ??= null);
                    }
                    if (masterRegister.PassportDoc != null)
                    {
                        docRecord.PassportDocName = UploadUtility.DocumentsUpload(masterRegister.PassportDoc ??= null);
                    }
                    if (masterRegister.DrivingLicenceDoc != null)
                    {
                        docRecord.DrivingLicenceDocName = UploadUtility.DocumentsUpload(masterRegister.DrivingLicenceDoc ??= null);
                    }
                    if (masterRegister.VehicleRegistrationDoc != null)
                    {
                        docRecord.VehicleRegistrationDocName = UploadUtility.DocumentsUpload(masterRegister.VehicleRegistrationDoc ??= null);
                    }
                    if (masterRegister.VehicleInspectionDoc != null)
                    {
                        docRecord.VehicleInspectionDocName = UploadUtility.DocumentsUpload(masterRegister.VehicleInspectionDoc ??= null);
                    }
                    docRecord.UpdatedBy = Convert.ToString(masterRegister.UserId)!;
                    docRecord.UpdatedDate = DateTime.UtcNow;
                    docRecord.IsDeleted = masterRegister.IsDocDeleted;
                    this.DbContextObj().Entry(docRecord).State = EntityState.Modified;
                }
                this.DbContextObj().SaveChanges();
                bool isDocCompleted = this.DbContextObj().TblDriverDocuments.Where(s => s.UserId == Convert.ToString(masterRegister.UserId) && !string.IsNullOrEmpty(s.DrivingLicenceDocName) && !string.IsNullOrEmpty(s.VehicleRegistrationDocName) && !string.IsNullOrEmpty(s.InsuarnceDocName)).Any();

                return isDocCompleted;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        Expression<Func<RegisterInfo, bool>> FilterByIsDeleted()
        {
            return x => x.IsDeleted == false;
        }
        Expression<Func<RegisterInfo, bool>> FilterByByUsername(string name)
        {
            return x => x.Name == name;
        }

        #region GetDriverMasterDetails
        /// <summary>
        /// method to GetDriverMasterDetails
        /// </summary>
        /// <returns></returns>
        public async Task<DriverDataResponse> GetDriverMasterDetails()
        {
            try
            {
                DriverDataResponse driverDetailsResponse = new DriverDataResponse();

                var userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                string userAccountId = this.DbContextObj().Users?.AsNoTracking().Where(x => x.Id.Equals(userId)).Select(x => x.StripeConnectedAccountId).FirstOrDefault()!;
                if (!string.IsNullOrEmpty(userAccountId))
                {
                    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                    var options = new BalanceGetOptions();
                    var requestOptions = new RequestOptions
                    {
                        StripeAccount = userAccountId,
                    };
                    var service = new BalanceService();
                    var accountBalance = await service.GetAsync(options, requestOptions);
                    var response = JsonConvert.DeserializeObject<StripeAccountBalance>(accountBalance.ToJson())!;
                    if (response.Available!.Count() > 0)
                    {
                        if (response.Available![0]?.Currency == "usd")
                        {
                            driverDetailsResponse.TotalEarnings = (Convert.ToDouble(response.Available![0].Amount) / 100);
                        }
                    }

                }
                var rateingList = await this.DbContextObj().TblTipsReviews?.Where(x => x.DriverId!.Equals(userId)).Select(c => c.RatingByRider).ToListAsync()!;
                var rateingSum = await this.DbContextObj().TblTipsReviews?.Where(x => x.DriverId!.Equals(userId)).Select(c => c.RatingByRider).SumAsync()!;
                int countRating = rateingList.Count;
                if (countRating > 0 && rateingSum > 0.0)
                {
                    driverDetailsResponse.Rating = Math.Round((rateingSum / (double)countRating), 2, MidpointRounding.AwayFromZero);
                }
                driverDetailsResponse.User!.Id = user?.Id;
                driverDetailsResponse.User!.Comment = user?.Comment;
                driverDetailsResponse.User!.DocStatus = this.DbContextObj().TblStatus.Where(z => z.Id.Equals(user!.StatusId)).FirstOrDefault()!.Name;
                driverDetailsResponse.User!.Email = user?.Email;
                driverDetailsResponse.User!.Mobile = user?.PhoneNumber;
                driverDetailsResponse.User!.DriverName = user?.Name;
                driverDetailsResponse.User!.DOB = string.Concat("0", Convert.ToDateTime(user!.DOB).Month.ToString() + "/" + Convert.ToDateTime(user!.DOB).Day.ToString() + "/" + Convert.ToDateTime(user!.DOB).Year.ToString());
                driverDetailsResponse.User!.ProfilePhoto = _config["Request:Url"] + GlobalResourceFile.ProfilePic + "/" + user?.ProfilePhoto;
                var driverData = await (
                from document in this.DbContextObj().TblDriverDocuments
                where document.UserId == userId
                join address in this.DbContextObj().TblGeneralAddress on userId equals address.UserId
                join vehicle in this.DbContextObj().TblVehicleDetails on userId equals vehicle.UserId
                join country in this.DbContextObj().Tbl_Countries on address.Country equals country.Id
                join state in this.DbContextObj().Tbl_States on address.State equals state.Id
                join city in this.DbContextObj().Tbl_Cities on address.City equals city.Id
                where document.UserId.Equals(user!.Id)
                select new
                {
                    document.DrivingLicenceDocName,
                    document.VehicleRegistrationDocName,
                    document.InsuarnceDocName,
                    CountryName = country.Name,
                    DocumentId = document.Id,
                    CountryId = country.Id,
                    StateId = state.Id,
                    CityId = city.Id,
                    StateName = state.Name,
                    CityName = city.Name,
                    address.Country,
                    AddressId = address.Id,
                    address.City,
                    address.State,
                    address.Social_Security_Number,
                    vehicle.Vehicle_Identification_Number,
                    vehicle.Make,
                    vehicle.Model,
                    VechcleId = vehicle.Id,
                    vehicle.Year,
                    vehicle.Color,
                    vehicle.Vehicle_Plate,
                    RideCategoryId = vehicle.RideCategoryId
                }).FirstOrDefaultAsync();
                driverDetailsResponse.DocumentsData!.Id = driverData?.DocumentId;
                if (driverDetailsResponse.DocumentsData!.Id.HasValue)
                {
                    driverDetailsResponse.DocumentsData!.DrivingLicenceDocName = driverData?.DrivingLicenceDocName != null ? _config["Request:Url"] + GlobalResourceFile.UploadDocument + "/" + driverData?.DrivingLicenceDocName : null;
                    driverDetailsResponse.DocumentsData!.VehicleRegistrationDocName = driverData?.VehicleRegistrationDocName != null ? _config["Request:Url"] + GlobalResourceFile.UploadDocument + "/" + driverData?.VehicleRegistrationDocName : null;
                    driverDetailsResponse.DocumentsData!.InsuarnceDocName = driverData?.InsuarnceDocName != null ? _config["Request:Url"] + GlobalResourceFile.UploadDocument + "/" + driverData?.InsuarnceDocName : null;

                }
                driverDetailsResponse.CountryData!.Id = driverData?.CountryId;
                driverDetailsResponse.CountryData!.Name = driverData?.CountryName;
                driverDetailsResponse.StateData!.Id = driverData?.StateId;
                driverDetailsResponse.StateData!.Name = driverData?.StateName;
                driverDetailsResponse.CityData!.Id = driverData?.CityId;
                driverDetailsResponse.CityData!.Name = driverData?.CityName;
                driverDetailsResponse.AddressData!.StateName = driverData?.StateName;
                driverDetailsResponse.AddressData!.Id = driverData?.AddressId;
                driverDetailsResponse.AddressData!.CountryName = driverData?.CountryName;
                driverDetailsResponse.AddressData!.CityName = driverData?.CityName;
                driverDetailsResponse.AddressData!.Social_Security_Number = driverData?.Social_Security_Number;
                driverDetailsResponse.VehicleData!.Vehicle_Identification_Number = driverData?.Vehicle_Identification_Number;
                driverDetailsResponse.VehicleData!.Make = driverData?.Make;
                driverDetailsResponse.VehicleData!.Model = driverData?.Model;
                driverDetailsResponse.VehicleData!.Year = driverData?.Year;
                driverDetailsResponse.VehicleData!.Color = driverData?.Color;
                driverDetailsResponse.VehicleData!.Vehicle_Plate = driverData?.Vehicle_Plate;
                driverDetailsResponse.VehicleData!.Id = driverData?.VechcleId;
                driverDetailsResponse.VehicleData!.RideCategoryId = driverData?.RideCategoryId;
                return driverDetailsResponse;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetAllDriversBasicDetails
        /// <summary>
        /// method to GetAllDriversBasicDetails
        /// </summary>
        /// <returns></returns>
        public async Task<List<DriverUserData>> GetAllDriversBasicDetails()
        {
            try
            {
                var userData = await (from user in this.DbContextObj().Users.Where(x => x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.CreatedDate)
                                      join role in this.DbContextObj().UserRoles on user.Id equals role.UserId
                                      join driverole in this.DbContextObj().Roles on role.RoleId equals driverole.Id
                                      where driverole.Name == AuthorizationLevel.Roles.Driver
                                      join status in this.DbContextObj().TblStatus on user.StatusId equals status.Id
                                      join vehicle in this.DbContextObj().TblVehicleDetails on user.Id equals vehicle.UserId
                                      where user.IsDeleted == false && user.IsActive == true
                                      orderby user.UpdatedDate descending, user.CreatedDate descending
                                      select new DriverUserData
                                      {
                                          Id = user.Id,
                                          Email = user.Email,
                                          Mobile = user.PhoneNumber,
                                          DriverName = user.Name,
                                          Inspection_Expiry_Date = (vehicle.Inspection_Expiry_Date != null)? string.Concat("0", Convert.ToDateTime(vehicle.Inspection_Expiry_Date).Month.ToString() + "/" + Convert.ToDateTime(vehicle.Inspection_Expiry_Date).Day.ToString() + "/" + Convert.ToDateTime(vehicle.Inspection_Expiry_Date).Year.ToString()) : "",
                                          DocStatus = this.DbContextObj().TblStatus.Where(x => x.Id == user.StatusId).First().Name,
                                          DOB = string.Concat("0", Convert.ToDateTime(user!.DOB).Month.ToString() + "/" + Convert.ToDateTime(user!.DOB).Day.ToString() + "/" + Convert.ToDateTime(user!.DOB).Year.ToString())
                                      }) 
                                   .ToListAsync();
                return userData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetDriverMasterDetailsById
        /// <summary>
        /// method to GetDriverMasterDetailsById
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<DriverDataResponse> GetDriverMasterDetailsById(string userId)
        {
            try
            {
                string wwwPath = _environment.WebRootPath;
                string connectionPathPF = Path.Combine(wwwPath, GlobalResourceFile.ProfilePic);
                string connectionPathDoc = Path.Combine(wwwPath, GlobalResourceFile.UploadDocument);
                DriverDataResponse driverDetailsResponse = new DriverDataResponse();
                var user = await _userManager.FindByIdAsync(userId);
                driverDetailsResponse.User!.Id = user!.Id;
                driverDetailsResponse.User!.Email = user!.Email;
                driverDetailsResponse.User!.Mobile = user!.PhoneNumber;
                driverDetailsResponse.User!.DriverName = user!.Name;
                driverDetailsResponse.User!.ProfilePhoto = string.IsNullOrEmpty(user?.ProfilePhoto) ? "/" + "Images" + "/person.png" : "/" + GlobalResourceFile.ProfilePic + "/" + user!.ProfilePhoto;
                driverDetailsResponse.User!.DOB = string.Concat("0", Convert.ToDateTime(user!.DOB).Month.ToString() + "/" + Convert.ToDateTime(user!.DOB).Day.ToString() + "/" + Convert.ToDateTime(user!.DOB).Year.ToString());
                driverDetailsResponse.User!.DocStatus = user.StatusId.ToString();
                driverDetailsResponse.User!.Comment = user!.Comment;


                var driverData = await (from document in this.DbContextObj().TblDriverDocuments
                                        where document.UserId == userId
                                        join address in this.DbContextObj().TblGeneralAddress on userId equals address.UserId
                                        join vehicle in this.DbContextObj().TblVehicleDetails on userId equals vehicle.UserId
                                        join country in this.DbContextObj().Tbl_Countries on address.Country equals country.Id
                                        join state in this.DbContextObj().Tbl_States on address.State equals state.Id
                                        join city in this.DbContextObj().Tbl_Cities on address.City equals city.Id
                                        where document.UserId.Equals(user!.Id)
                                        select new
                                        {
                                            document.DrivingLicenceDocName,
                                            document.VehicleRegistrationDocName,
                                            document.InsuarnceDocName,
                                            document.VehicleInspectionDocName,
                                            CountryName = country.Name,
                                            DocumentId = document.Id,
                                            CountryId = country.Id,
                                            StateId = state.Id,
                                            CityId = city.Id,
                                            StateName = state.Name,
                                            CityName = city.Name,
                                            address.Country,
                                            AddressId = address.Id,
                                            address.City,
                                            address.State,
                                            address.Social_Security_Number,
                                            vehicle.Vehicle_Identification_Number,
                                            vehicle.Make,
                                            vehicle.Model,
                                            VechcleId = vehicle.Id,
                                            vehicle.Year,
                                            vehicle.Color,
                                            vehicle.Vehicle_Plate,
                                            vehicle.Inspection_Expiry_Date,
                                            RideCategoryId = vehicle.RideCategoryId
                                        }).FirstOrDefaultAsync();
                driverDetailsResponse.DocumentsData!.DrivingLicenceDocName = (driverData?.DrivingLicenceDocName == null) ? "/" + "Images" + "/paper-min.png" : "/" + GlobalResourceFile.UploadDocument + "/" + driverData?.DrivingLicenceDocName;
                driverDetailsResponse.DocumentsData!.VehicleRegistrationDocName = (driverData?.VehicleRegistrationDocName == null) ? "/" + "Images" + "/paper-min.png" : "/" + GlobalResourceFile.UploadDocument + "/" + driverData?.VehicleRegistrationDocName;
                driverDetailsResponse.DocumentsData!.InsuarnceDocName = (driverData?.InsuarnceDocName == null) ? "/" + "Images" + "/paper-min.png" : "/" + GlobalResourceFile.UploadDocument + "/" + driverData?.InsuarnceDocName;
                driverDetailsResponse.DocumentsData!.VehicleInspectionDocName = (driverData?.VehicleInspectionDocName == null) ? "/" + "Images" + "/paper-min.png" : "/" + GlobalResourceFile.UploadDocument + "/" + driverData?.VehicleInspectionDocName;
                driverDetailsResponse.DocumentsData!.Id = driverData?.DocumentId;
                driverDetailsResponse.CountryData!.Id = driverData?.CountryId;
                driverDetailsResponse.CountryData!.Name = driverData?.CountryName;
                driverDetailsResponse.StateData!.Id = driverData?.StateId;
                driverDetailsResponse.StateData!.Name = driverData?.StateName;
                driverDetailsResponse.CityData!.Id = driverData?.CityId;
                driverDetailsResponse.CityData!.Name = driverData?.CityName;
                driverDetailsResponse.AddressData!.StateName = driverData?.StateName;
                driverDetailsResponse.AddressData!.Id = driverData?.AddressId;
                driverDetailsResponse.AddressData!.CountryName = driverData?.CountryName;
                driverDetailsResponse.AddressData!.CityName = driverData?.CityName;
                driverDetailsResponse.AddressData!.Social_Security_Number = driverData?.Social_Security_Number;
                driverDetailsResponse.VehicleData!.Vehicle_Identification_Number = driverData?.Vehicle_Identification_Number;
                driverDetailsResponse.VehicleData!.Make = driverData?.Make;
                driverDetailsResponse.VehicleData!.Model = driverData?.Model;
                driverDetailsResponse.VehicleData!.Year = driverData?.Year;
                driverDetailsResponse.VehicleData!.Color = driverData?.Color;
                driverDetailsResponse.VehicleData!.Vehicle_Plate = driverData?.Vehicle_Plate;
                driverDetailsResponse.VehicleData!.Id = driverData?.VechcleId;
                driverDetailsResponse.VehicleData!.RideCategoryId = driverData?.RideCategoryId;
                driverDetailsResponse.VehicleData!.Inspection_Expiry_Date = (driverData!.Inspection_Expiry_Date != null) ? string.Concat("0", Convert.ToDateTime(driverData!.Inspection_Expiry_Date).Month.ToString() + "/" + Convert.ToDateTime(driverData.Inspection_Expiry_Date).Day.ToString() + "/" + Convert.ToDateTime(driverData.Inspection_Expiry_Date).Year.ToString()) : "";
                return driverDetailsResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SetUserApprovalStatus
        /// <summary>
        /// method to SetUserApprovalStatus
        /// </summary>
        /// <param name="approvalStatus"></param>
        /// <returns></returns>
        public async Task<bool> SetUserApprovalStatus(UserApprovalStatus approvalStatus)
        {
            try
            {
                var requestUri = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{_httpContext.Request.PathBase}";
                if (!string.IsNullOrEmpty(approvalStatus.UserId))
                {
                    var user = await _userManager.FindByIdAsync(approvalStatus.UserId);
                    var vehicleData = this.DbContextObj().TblVehicleDetails.Where(x => x.UserId == user.Id).FirstOrDefault();
                    if (vehicleData != null)
                    {
                        vehicleData.RideCategoryId = approvalStatus.RideCategoryId;
                        this.DbContextObj().TblVehicleDetails.Update(vehicleData);
                        this.DbContextObj().SaveChanges();
                    }
                    user.StatusId = approvalStatus.StatusId;
                    user.IsDocumentVerified = true;
                    user.IsVerified = true;
                    user.IsTwilioVerified = true;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = approvalStatus.UserId!;
                    user.Comment = approvalStatus.Message;
                    var result = await _userManager.UpdateAsync(user);
                    if (user.StatusId!.Value.Equals(Guid.Parse(GlobalConstants.GlobalValues.Approved.ToLower())))
                    {
                        StripeCreateAccount createAccount = new StripeCreateAccount { AccountType = _stripeSettings.AccountType, Country = _stripeSettings.Country, Email = user.Email, Capabilities_card_payments_requested = true, Capabilities_transfers_requested = true };
                        await _paymentRepository.CreateAccount(createAccount).ConfigureAwait(false);
                    }
                    var emailToDriver = new EmailDriver
                    {
                        Port = Convert.ToInt32(_config["EmailConfiguration:Port"]!),
                        RequestUri = requestUri,
                        DriverName = user.Name,
                        MailTo = user.Email,
                        MailFrom = _config["EmailConfiguration:AdminEmail"]!,
                        Password = _config["EmailConfiguration:Password"]!,
                        DocStatus = approvalStatus.StatusId,
                        Reason = approvalStatus.Message,
                        Host = _config["EmailConfiguration:Host"]!,
                        Subject = _config["EmailConfiguration:AdminEmailSubject"]!,
                        MailFromAlias = _config["EmailConfiguration:Alias"]!
                    };


                    NotificationModel notificationModel = new NotificationModel
                    {
                        DeviceId = user.DeviceId,
                        IsAndroidDevice = user.Platform!.Where(s => s.Equals(GlobalResourceFile.Android) || s.Equals(GlobalResourceFile.Mobile)) != null ? true : false,
                        Title = this.DbContextObj().TblStatus.Where(s => s.Id == approvalStatus.StatusId).First().Name,
                        Body = approvalStatus.Message

                    };

                    Parallel.Invoke(() => SendNotification(notificationModel), () => EmailUtility.SendEmail(emailToDriver));

                    return result.Succeeded;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region SendNotification
        /// <summary>
        /// method to SendNotification
        /// </summary>
        /// <param name="notificationModel"></param>
        public async void SendNotification(NotificationModel notificationModel)
        {
            try
            {
                if (notificationModel.IsAndroidDevice)
                {
                    _logger.LogInformation("{0} InSide  SendNotification in BookingSystemRepository Method :  DriverDeviceId = {1} ", DateTime.UtcNow, notificationModel?.DeviceId);
                    string fileName = Path.Combine(_environment.WebRootPath, "helpful-cosine-410722-firebase-adminsdk-yftwt-fbcdb2d817.json"); //Download from Firebase Console ServiceAccount
                    string scopes = _config["GoogleNotification:Scopes"]!;
                    var bearertoken = ""; // Bearer Token in this variable
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        bearertoken = GoogleCredential
                          .FromStream(stream) // Loads key file
                          .CreateScoped(scopes) // Gathers scopes requested
                          .UnderlyingCredential // Gets the credentials
                          .GetAccessTokenForRequestAsync().Result; // Gets the Access Token

                    }
                    ///--------Calling FCM-----------------------------

                    var clientHandler = new HttpClientHandler();
                    var client = new HttpClient(clientHandler);

                    client.BaseAddress = new Uri(_config["GoogleNotification:BaseAddress"]!); // FCM HttpV1 API

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Configuration.Content));

                    //client.DefaultRequestHeaders.Accept.Add("Authorization", "Bearer " + bearertoken);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtAuthenticationDefaults.BearerPrefix, bearertoken); // Authorization Token in this variable

                    //---------------Assigning Of data To Model --------------

                    Root rootObj = new Root();
                    rootObj.Message = new Posh_TRPT_Domain.PushNotification.Message();
                    rootObj.Message.Token = notificationModel.DeviceId; //FCM Token id
                    rootObj.Message.Data = new Posh_TRPT_Domain.PushNotification.Data();
                    rootObj.Message.Data.Title = notificationModel.Title;
                    rootObj.Message.Data.Body = notificationModel.Body;
                    //rootObj.Message.Data.UserData = userData;
                    rootObj.Message.Notification = new Notification();
                    rootObj.Message.Notification.Title = notificationModel.Title;
                    rootObj.Message.Notification.Body = notificationModel.Body;

                    //-------------Convert Model To JSON ----------------------

                    var jsonObj = System.Text.Json.JsonSerializer.Serialize<Object>(rootObj);

                    //------------------------Calling Of FCM Notify API-------------------

                    var data = new StringContent(jsonObj, Encoding.UTF8, Configuration.Content);
                    data.Headers.ContentType = new MediaTypeHeaderValue(Configuration.Content);

                    var response = client.PostAsync(_config["GoogleNotification:BaseAddress"]!, data).Result; // Calling The FCM httpv1 API

                    //---------- Deserialize Json Response from API ----------------------------------

                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var responseObj = System.Text.Json.JsonSerializer.Serialize<object>(jsonResponse);
                    _logger.LogInformation("{0} Inside After send notification getting response : {1}", DateTime.UtcNow, jsonResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("{0} InSide  SendNotification in BookingSystemRepository Method -- Success : ==> {1}", DateTime.UtcNow, response.IsSuccessStatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide  SendNotification in BookingSystemRepository Method -- Notification_Error : {1}", DateTime.UtcNow, ex.Message);
            }
        }
        #endregion

        #region UpdateUserProfile
        /// <summary>
        /// method to UpdateUserProfile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public async Task<string> UpdateUserProfile(UserProfileModel userProfile)
        {
            try
            {
                var userData = await _userManager.FindByIdAsync(userProfile.UserId);
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
                string ProfilePic = string.Empty!;
                string result = CommonResource.UpdateFailed;
                switch (role)
                {
                    case AuthorizationLevel.Roles.Customer:
                        {
                            if (userData != null && userData.StripeCustomerId != null)
                            {
                                userProfile!.Name = string.IsNullOrEmpty(userProfile?.Name) ? userData?.Name : userProfile?.Name;
                                userProfile!.Dob = string.IsNullOrEmpty(userProfile?.Dob.ToString()) ? userData?.DOB : userProfile?.Dob;
                                userProfile!.Email = string.IsNullOrEmpty(userProfile?.Email) ? userData?.Email : userProfile?.Email;
                                if (userProfile!.ProfilePic != null)
                                {
                                    ProfilePic = UploadUtility.ProfilePhotoUpload(userProfile!.ProfilePic);
                                }

                                var options = new CustomerUpdateOptions
                                {

                                    Email = userProfile?.Email
                                };
                                var service = new CustomerService();
                                var user = await service.UpdateAsync(userData?.StripeCustomerId, options).ConfigureAwait(false);
                                AccountUpdateResponse res = JsonConvert.DeserializeObject<AccountUpdateResponse>(user.ToJson())!;
                                if (res!.Email!.Equals(userProfile!.Email))
                                {

                                    userData!.Name = userProfile.Name;
                                    userData!.DOB = Convert.ToDateTime(string.Concat("0", Convert.ToDateTime(userProfile!.Dob).Month.ToString() + "/" + Convert.ToDateTime(userProfile!.Dob).Day.ToString() + "/" + Convert.ToDateTime(userProfile!.Dob).Year.ToString()));
                                    userData!.ProfilePhoto = string.IsNullOrEmpty(ProfilePic) ? userData.ProfilePhoto : ProfilePic;
                                    userData!.Email = userProfile.Email;
                                    userData!.NormalizedEmail = userProfile.Email.ToUpper();
                                    userData!.UpdatedDate = DateTime.UtcNow;
                                    userData!.UpdatedBy = userData!.Id;
                                    var resultData = await _userManager.UpdateAsync(userData);
                                    if (resultData.Succeeded)
                                    {
                                        result = CommonResource.UpdateSuccess;
                                    }
                                    return await Task.FromResult(result);
                                }
                                return await Task.FromResult(result);

                            }
                            break;
                        }
                    case AuthorizationLevel.Roles.Driver:
                        {

                            if (userData != null)
                            {
                                userProfile!.Name = string.IsNullOrEmpty(userProfile?.Name) ? userData?.Name : userProfile?.Name;
                                userProfile!.Dob = string.IsNullOrEmpty(userProfile?.Dob.ToString()) ? userData?.DOB : userProfile?.Dob;


                                if (userProfile!.ProfilePic != null)
                                {
                                    ProfilePic = UploadUtility.ProfilePhotoUpload(userProfile!.ProfilePic);
                                }

                                userData!.Name = userProfile.Name;
                                userData!.DOB = Convert.ToDateTime(string.Concat("0", Convert.ToDateTime(userProfile!.Dob).Month.ToString() + "/" + Convert.ToDateTime(userProfile!.Dob).Day.ToString() + "/" + Convert.ToDateTime(userProfile!.Dob).Year.ToString()));
                                userData!.ProfilePhoto = string.IsNullOrEmpty(ProfilePic) ? userData.ProfilePhoto : ProfilePic;
                                userData!.UpdatedDate = DateTime.UtcNow;
                                userData!.UpdatedBy = userData!.Id;
                                var resultData = await _userManager.UpdateAsync(userData);
                                if (resultData.Succeeded)
                                {
                                    result = CommonResource.UpdateSuccess;
                                }
                                return await Task.FromResult(result);

                            }
                            break;
                        }
                }
                return await Task.FromResult(LoginResource.SomthingWentWrong);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DeleteUserAccount
        /// <summary>
        /// Method to delete UserAccount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteUserAccount(string id)
        {
            try
            {
                string role = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value!;
                bool isStripeCustumerDeleted = false;
                bool isStripeConnectAccountDeleted = false;
                string finalResult = GlobalResourceFile.Failed;
                IdentityResult identityResult = null!;
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
                switch (role)
                {
                    case AuthorizationLevel.Roles.Customer:
                        {
                            var record = await _userManager.FindByIdAsync(id);
                            if (record.StripeCustomerId != null)
                            {
                                try
                                {
                                    //delete ac customer account
                                    var service = new CustomerService();
                                    var data = await service.DeleteAsync(record.StripeCustomerId).ConfigureAwait(false);
                                    StripeAccountDeleteResponse deleteResponse = JsonConvert.DeserializeObject<StripeAccountDeleteResponse>(data.ToJson())!;
                                    if (deleteResponse.Deleted)
                                    {
                                        isStripeCustumerDeleted = true;
                                    }
                                    if (isStripeCustumerDeleted)
                                    {
                                        record.PhoneNumber = string.Concat(id, "@removed.com");
                                        record.PhoneNumberConfirmed = false;
                                        record.EmailConfirmed = false;
                                        record.Email = string.Concat(id, "@removed.com");
                                        record.NormalizedEmail = "deleted@removed.com";
                                        record.IsLoggedIn = false;
                                        record.IsActive = false;
                                        record.IsDeleted = true;
                                        record.UpdatedDate = DateTime.UtcNow;
                                        record.StripeCustomerId = "idDeleted";
                                        record.UpdatedBy = id;
                                        identityResult = await _userManager.UpdateAsync(record);
                                        if (identityResult.Succeeded)
                                        {
                                            finalResult = GlobalResourceFile.Success;
                                        }
                                        return await Task.FromResult(finalResult!);
                                    }


                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                            }
                            if (!isStripeCustumerDeleted)
                            {
                                record.PhoneNumber = string.Concat(id, "@removed.com");
                                record.PhoneNumberConfirmed = false;
                                record.EmailConfirmed = false;
                                record.Email = string.Concat(id, "@removed.com");
                                record.NormalizedEmail = string.Concat(id, "@removed.com");
                                record.IsLoggedIn = false;
                                record.StripeCustomerId = "idDeleted";
                                record.IsActive = false;
                                record.IsDeleted = true;
                                record.UpdatedDate = DateTime.UtcNow;
                                record.UpdatedBy = id;
                                identityResult = await _userManager.UpdateAsync(record);
                                if (identityResult.Succeeded)
                                {
                                    finalResult = GlobalResourceFile.Success;
                                }
                                return await Task.FromResult(finalResult!);
                            }
                            break;
                        }
                    case AuthorizationLevel.Roles.Driver:
                        {
                            var record = await _userManager.FindByIdAsync(id);
                            if (record.StripeConnectedAccountId != null)
                            {
                                try
                                {
                                    var service = new AccountService();
                                    var data = await service.DeleteAsync(record.StripeConnectedAccountId).ConfigureAwait(false);
                                    StripeAccountDeleteResponse deleteResponse = JsonConvert.DeserializeObject<StripeAccountDeleteResponse>(data.ToJson())!;
                                    if (deleteResponse.Deleted)
                                    {

                                        isStripeConnectAccountDeleted = true;
                                    }
                                    if (isStripeConnectAccountDeleted)
                                    {
                                        record.PhoneNumber = string.Concat(id, "@removed.com");
                                        record.Email = string.Concat(id, "@removed.com");
                                        record.NormalizedEmail = string.Concat(id, "@removed.com");
                                        record.UserName = string.Concat(id, "@removed.com");
                                        record.NormalizedUserName = string.Concat(id, "@removed.com");
                                        record.IsLoggedIn = false;
                                        record.PhoneNumberConfirmed = false;
                                        record.StripeConnectedAccountId = "idDeleted";
                                        record.EmailConfirmed = false;
                                        record.IsActive = false;
                                        record.IsDeleted = true;
                                        record.UpdatedDate = DateTime.UtcNow;
                                        record.UpdatedBy = id;
                                        identityResult = await _userManager.UpdateAsync(record);
                                        if (identityResult.Succeeded)
                                        {
                                            finalResult = GlobalResourceFile.Success;
                                        }
                                        return await Task.FromResult(finalResult!);
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                            if (!isStripeConnectAccountDeleted)
                            {
                                record.PhoneNumber = string.Concat(id, "@removed.com");
                                record.Email = string.Concat(id, "@removed.com");
                                record.NormalizedEmail = string.Concat(id, "@removed.com");
                                record.UserName = string.Concat(id, "@removed.com");
                                record.NormalizedUserName = string.Concat(id, "@removed.com");
                                record.IsLoggedIn = false;
                                record.PhoneNumberConfirmed = false;
                                record.EmailConfirmed = false;
                                record.StripeConnectedAccountId = "idDeleted";
                                record.IsActive = false;
                                record.IsDeleted = true;
                                record.UpdatedDate = DateTime.UtcNow;
                                record.UpdatedBy = id;
                                identityResult = await _userManager.UpdateAsync(record);
                                if (identityResult.Succeeded)
                                {
                                    finalResult = GlobalResourceFile.Success;
                                }
                                return await Task.FromResult(finalResult!);
                            }
                            break;
                        }
                }
                return await Task.FromResult(finalResult!);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
