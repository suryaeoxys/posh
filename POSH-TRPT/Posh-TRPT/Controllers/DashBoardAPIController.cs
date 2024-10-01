using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Services.DashBoard;
using Posh_TRPT_Services.PushNotification;

namespace Posh_TRPT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashBoardAPIController : ControllerBase
    {
        private readonly DashBoardService _dashBoardService;
        private readonly PushNotificationService _pushNotificationService;
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _configuration;
        public DashBoardAPIController(DashBoardService dashBoardService, ILogger<CustomerController> logger,
        PushNotificationService pushNotificationService, IConfiguration configuration)
        {
            _dashBoardService = dashBoardService;
            _logger = logger;
            _pushNotificationService = pushNotificationService;
            _configuration = configuration;
        }
        #region Get Drivers Riders Ride Counts
        [HttpGet]
        public async Task<IActionResult> GetDriverRiderCounts()
        {
            try
            {
                _logger.LogInformation("{0} InSide  GetDriverRiderRideCounts in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.GetDriverRiderRideCounts();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide After Executing SP Sp_GetDriverRiderRideCounts GetDriverRiderRideCounts in DashBoardAPIController Method -- TotalDriver:={1}, TotalRiders:={2}, TotalCurrentRider:={3}, TotalCompleteRides:={4}", DateTime.UtcNow, result.Data!.TotalDrivers, result.Data!.TotalRiders, result.Data!.RunningRides, result.Data!.TotalCompleteRides);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetDriverRiderRideCounts in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Monthly Canceled Complted Rides
        [HttpGet]
        public async Task<IActionResult> GetMonthlyCanceledCompltedRides()
        {
            try
            {
                _logger.LogInformation("{0} InSide GetMonthlyCamceledCompltedRides in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.MonthlyCompletedCanceledRidesData();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide After Executing SP Sp_GetMonthlyCompletedCanceledRides GetMonthlyCamceledCompltedRides in DashBoardAPIController Method -- ", DateTime.UtcNow);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetMonthlyCamceledCompltedRides in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Currently Running Rides
        [HttpGet]
        public async Task<IActionResult> CurrentlyRunningRides()
        {
            try
            {
                _logger.LogInformation("{0} InSide CurrentlyRunningRides in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.CurrentlyRunningRides();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide After Executing SP Sp_GetCurrentlyRunningRides CurrentlyRunningRides in DashBoardAPIController Method -- ", DateTime.UtcNow);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside CurrentlyRunningRides in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Drivers Data With Earnings
        [HttpGet]
        public async Task<IActionResult> GetDriversDataWithEarnings()
        {
            try
            {
                _logger.LogInformation("{0} InSide CurrentlyRunningRides in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.GetDriversDataWithEarnings();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide After Executing SP Sp_GetCurrentlyRunningRides CurrentlyRunningRides in DashBoardAPIController Method -- ", DateTime.UtcNow);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside CurrentlyRunningRides in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Total Earnings
        [HttpGet]
        public async Task<IActionResult> TotalEarningsDetails()
        {
            try
            {
                _logger.LogInformation("{0} InSide TotalEarningsDetails in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.TotalEarningsDetails();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide After Executing SP Sp_GetTotalEarningsByDate TotalEarningsDetails in DashBoardAPIController Method -- ", DateTime.UtcNow);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside TotalEarningsDetails in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region GetOrdersPercentageWithStatus
        [HttpGet]
        public async Task<IActionResult> GetOrdersPercentageWithStatus()
        {
            try
            {
                _logger.LogInformation("{0} InSide GetOrdersPercentageWithStatus in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.GetOrdersPercentageWithStatus();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide GetOrdersPercentageWithStatus in DashBoardAPIController Method --Success: {2} ", DateTime.UtcNow, result.Success);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside TotalEarningsDetails in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Top10RatedDrivers
        [HttpGet]
        public async Task<IActionResult> Top10RatedDrivers()
        {
            try
            {
                _logger.LogInformation("{0} InSide Top10RatedDrivers in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.Top10RatedDrivers();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide Top10RatedDrivers in DashBoardAPIController Method --Success: {2} ", DateTime.UtcNow, result.Success);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside Top10RatedDrivers in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region Top5EarnedDrivers
        [HttpGet]
        public async Task<IActionResult> Top5EarnedDrivers()
        {
            try
            {
                _logger.LogInformation("{0} InSide Top5EarnedDrivers in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _dashBoardService.Top5EarnedDrivers();
                if (result.Success)
                {
                    _logger.LogInformation("{0} InSide Top5EarnedDrivers in DashBoardAPIController Method --Success: {2} ", DateTime.UtcNow, result.Success);
                    return Ok(result);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside Top5EarnedDrivers in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

    }
}
