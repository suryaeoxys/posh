using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Services.PushNotification;
using Posh_TRPT_Services.Report;

namespace Posh_TRPT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportAPIController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly PushNotificationService _pushNotificationService;
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _configuration;
        public ReportAPIController(ReportService reportService, ILogger<CustomerController> logger,
        PushNotificationService pushNotificationService, IConfiguration configuration)
        {
            _reportService = reportService;
            _logger = logger;
            _pushNotificationService = pushNotificationService;
            _configuration = configuration;
        }

        #region GetFilteredDataOfOrders
        [HttpGet]
        public async Task<IActionResult> GetFilteredDataOfOrders(string startDate, string endDate, int statusType, Guid? driverId)
        {
            try
            {
                _logger.LogInformation("{0} InSide GetFilteredDataOfOrders in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _reportService.GetFilteredDataOfOrders(startDate, endDate, statusType, driverId);
                if (result != null)
                {
                    _logger.LogInformation("{0} InSide GetFilteredDataOfOrders in DashBoardAPIController Method --Success: {2} ", DateTime.UtcNow, result.Success);
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetFilteredDataOfOrders in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion

        #region GetAllStatusForReport
        [HttpGet]
        public async Task<IActionResult> GetAllStatusForReport()
        {
            try
            {
                _logger.LogInformation("{0} InSide GetAllOrdersForReport in DashBoardAPIController Method ", DateTime.UtcNow);
                var result = await _reportService.GetAllStatusForReport();
                if (result != null)
                {
                    _logger.LogInformation("{0} InSide GetAllOrdersForReport in DashBoardAPIController Method --Success: {2} ", DateTime.UtcNow, result.Success);
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetAllOrdersForReport in DashBoardAPIController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion
    }
}
