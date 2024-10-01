using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.DashBoard;

namespace Posh_TRPT.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public ReportController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region GetFilteredDataOfOrders
        //*****************************************************************************************
        // Name                 :   GetFilteredDataOfOrders
        // Return type          :   IEnumerable<DriverRating>
        // Input Parameter(s)   :   N/A
        // Purpose              :   To create a Graph at dashboard 
        // History Header       :   Version  - Creation Date - Last Modification Date     -  Developer Name
        // History              :   1.0     -  July 30 2024  -                            -  Chetu Developer
        //*****************************************************************************************
        [HttpGet]
        public async Task<IActionResult> GetFilteredDataOfOrders(string startDate, string endDate, int statusType, Guid? driverId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    _logger.LogInformation("{0} InSide  GetFilteredDataOfOrders in DashBoardController Method ", DateTime.UtcNow);
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + $"ReportAPI/GetFilteredDataOfOrders?startDate={startDate}&endDate={endDate}&statusType={statusType}&driverId={driverId}");
                    var result = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var data = JsonConvert.DeserializeObject<APIResponse<ReportOrderDataDTO>>(result);
                        _logger.LogInformation("{0} InSide GetFilteredDataOfOrders in DashBoardController Method --", DateTime.UtcNow);
                        if (data?.Data != null & data?.Data?.reportOrderData!.Count() > 0)
                        {
                            data!.Data!.reportOrderData!.ForEach(ReportOrderData =>
                            {
                                ReportOrderData.NewDate = ReportOrderData.Date?.ToString("yyyy-MM-dd");
                                ReportOrderData.PickTime = ReportOrderData.PickUpTime?.ToString("hh\\:mm");
                                ReportOrderData.DropOffTime = ReportOrderData.DropTime?.ToString("hh\\:mm");
                            });
                            return Json(data.Data);
                        }
                        return Json(data);
                    }
                    return null!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} Inside GetFilteredDataOfOrders in DashBoardController Method --- Error {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
        #endregion
    }
}
