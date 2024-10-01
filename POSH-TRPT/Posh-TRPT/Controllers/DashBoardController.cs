using CorePush.Apple;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;
using Newtonsoft.Json;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.DashBoard;
using Posh_TRPT_Utility.ConstantStrings;
using System.Collections.Generic;

namespace Posh_TRPT.Controllers
{
    [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
    public class DashBoardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public DashBoardController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        /// <summary>
        /// Get Driver, Rider and rides Counts
        /// </summary>
        /// <returns></returns>
        #region Get Total Counts
        [HttpGet]
        public async Task<IActionResult> GetDriverRiderCounts()
        {
            try
            {
                _logger.LogInformation("{0} InSide  GetDriverRiderRideCounts in DashBoardController Method ", DateTime.UtcNow);
                DriverRiderRideCountsDTO data = new DriverRiderRideCountsDTO();
                data = await CallRestAPI<DriverRiderRideCountsDTO>("DashBoardAPI/GetDriverRiderCounts");
                return Json(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        /// <summary>
        /// Get Monthly Completed and cancelled Rides for the dashboard
        /// </summary>
        /// <returns></returns>
        #region Get Monthly Completed and Cancelled Rides
        [HttpGet]
        public async Task<IActionResult> GetMonthlyCompletedCanceledRides()
        {
            try
            {
                _logger.LogInformation("{0} InSide  GetMonthlyCompletedCanceledRides in DashBoardController Method ", DateTime.UtcNow);
                APIResponse<List<MonthlyCompletedCanceledRideDTO>> result = new APIResponse<List<MonthlyCompletedCanceledRideDTO>>();
                result = await CallRestAPIForAList<MonthlyCompletedCanceledRideDTO>("DashBoardAPI/GetMonthlyCanceledCompltedRides");
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetMonthlyCompletedCanceledRides in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Drivers Data With Earnings
        /// <summary>
        /// Get Drivers Data With Earnings for the dashboard
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetDriversDataWithEarnings()
        {
            try
            {
                _logger.LogInformation("{0} InSide  GetDriversDataWithEarnings in DashBoardController Method ", DateTime.UtcNow);
                APIResponse<List<DriversDataResponseDTO>> result = new APIResponse<List<DriversDataResponseDTO>>();
                result = await CallRestAPIForAList<DriversDataResponseDTO>("DashBoardAPI/GetDriversDataWithEarnings");
                return Json(result.Data);

            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetDriversDataWithEarnings in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Total Earnings by date
        /// <summary>
        /// Get Earnings for the dashboard
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> TotalEarningsDetails()
        {
            try
            {
                _logger.LogInformation("{0} InSide  TotalEarningsDetails in DashBoardController Method ", DateTime.UtcNow);
                APIResponse<List<TotalEarningByDateDTO>> result = new APIResponse<List<TotalEarningByDateDTO>>();
                result = await CallRestAPIForAList<TotalEarningByDateDTO>("DashBoardAPI/TotalEarningsDetails");
                if (result.Success && result.Data!.Count > 0)
                {
                    result.Data.ForEach(x => x.NewDate = x.Date.ToString("MM/dd/yyyy"));
                    result.Data = result.Data.OrderByDescending(X => X.NewDate).ToList();
                    return Json(result.Data);
                }
                return Json(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetDriversDataWithEarnings in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region GetOrdersPercentageWithStatus
        /// <summary>
        /// Get Earnings for the dashboard
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetOrdersPercentageWithStatus()
        {
            try
            {
                _logger.LogInformation("{0} InSide  GetOrdersPercentageWithStatus in DashBoardController Method ", DateTime.UtcNow);
                APIResponse<List<OrdersCountWithStatusDTO>> result = new APIResponse<List<OrdersCountWithStatusDTO>>();
                result = await CallRestAPIForAList<OrdersCountWithStatusDTO>("DashBoardAPI/GetOrdersPercentageWithStatus");
                return Json(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetDriversDataWithEarnings in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Top 10 Rated Drivers
        //*****************************************************************************************
        // Name                 :   Top 10 Rated Drivers
        // Return type          :   IEnumerable<DriverRating>
        // Input Parameter(s)   :   N/A
        // Purpose              :   To create a Graph at dashboard 
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  July 30 2024  -                            -  Chetu Developer
        //*****************************************************************************************
        [HttpGet]
        public async Task<IActionResult> Top10RatedDrivers()
        {
            try
            {
                _logger.LogInformation("{0} InSide  Top10RatedDrivers in DashBoardController Method ", DateTime.UtcNow);
                APIResponse<List<DriverRatingDTO>> result = new APIResponse<List<DriverRatingDTO>>();
                result = await CallRestAPIForAList<DriverRatingDTO>("DashBoardAPI/Top10RatedDrivers");
                return Json(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside Top10RatedDrivers in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Top5EarnedDrivers
        //*****************************************************************************************
        // Name                 :   Top5EarnedDrivers
        // Return type          :   IEnumerable<DriverRating>
        // Input Parameter(s)   :   N/A
        // Purpose              :   To create a Graph at dashboard 
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  Aug 05 2024  -                            -  Chetu Developer
        //*****************************************************************************************
        [HttpGet]
        public async Task<IActionResult> Top5EarnedDrivers()
        {
            try
            {
                APIResponse<List<DriverEarningDTO>> result = new APIResponse<List<DriverEarningDTO>>();
                result = await CallRestAPIForAList<DriverEarningDTO>("DashBoardAPI/Top5EarnedDrivers");
                return Json(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside Top10RatedDrivers in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region CallRestAPIForAList
        //*****************************************************************************************
        // Name                 :   CallRestAPIForAList
        // Return type          :   List<T>
        // Input Parameter(s)   :   api Url
        // Purpose              :   To Call the rest API which returns a list of records.
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  16 sep 2024                                -  Chetu Developer
        //*****************************************************************************************
        public async Task<APIResponse<List<T>>> CallRestAPIForAList<T>(string apiUrl) where T : new()
        {
            APIResponse<List<T>> list = new APIResponse<List<T>>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                var response = await client.GetAsync(client.BaseAddress + apiUrl);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<APIResponse<List<T>>>(result);
                    if (data!.Data != null)
                    {
                        return list = data;
                    }
                }
                return list;
            }
        }
        #endregion

        #region CallRestAPI

        //*****************************************************************************************
        // Name                 :   CallRestAPI
        // Return type          :   <T>
        // Input Parameter(s)   :   api Url
        // Purpose              :   To Call the rest API which returns a single record.
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  15 sep 2024                                -  Chetu Developer
        //*****************************************************************************************
        public async Task<T> CallRestAPI<T>(string apiUrl) where T : new()
        {
            T list = new T();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                var response = await client.GetAsync(client.BaseAddress + apiUrl);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<APIResponse<T>>(result);
                    if (data!.Data != null)
                    {
                        return list = data.Data;
                    }
                }
                return list;
            }
        } 
        #endregion
    }
}
