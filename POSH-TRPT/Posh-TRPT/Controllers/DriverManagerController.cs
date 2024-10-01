using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;
using System.Net.Http.Headers;
using System.Text;
using static Posh_TRPT_Utility.ConstantStrings.AuthorizationLevel;

namespace Posh_TRPT.Controllers
{
    [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
    public class DriverManagerController : Controller
    {
        private readonly ILogger<DriverManagerController> _logger;
        private readonly IConfiguration _configuration;

        public DriverManagerController(ILogger<DriverManagerController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
		/// <summary>
		/// Action method to view the all driver informations
		/// </summary>
		/// <returns></returns>
		#region Index
		[HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("DateTime: {0} Index Method of DriverManager MVC Controller Started:", DateTime.UtcNow);
            return View();

        } 
        #endregion

        [HttpGet]
		/// <summary>
		/// Action method to retrieve the all driver informations
		/// </summary>
		/// <returns></returns>
		#region GetDrivers
		[HttpGet]
        public async Task<IActionResult> GetDrivers()
        {
            try
            {
                _logger.LogInformation("DateTime: {0} GetDrivers Method of DriverManager MVC Controller Started:", DateTime.UtcNow);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    var responseMessage = await client.GetAsync(client.BaseAddress + Configuration.RegistrationGetDrivers);
                    var result = responseMessage.Content.ReadAsStringAsync();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("DateTime: {0} GetDrivers Method of DriverManager MVC Controller Started: ResponseMessage:{1}", DateTime.UtcNow, responseMessage.StatusCode);
                        var driverList = JsonConvert.DeserializeObject<APIResponse<List<DriverUserData>>>(result.Result)!;
                        if (driverList.Data is null)
                        {
                            _logger.LogInformation("DateTime: {0} GetDrivers Method of DriverManager MVC Controller Started: ResponseMessage:{1}", DateTime.UtcNow, responseMessage.StatusCode);
                            return null!;
                        }
                        return Json(driverList.Data!);
                    }
                    _logger.LogInformation("DateTime: {0} GetDrivers Method of DriverManager MVC Controller Started: ResponseMessage:{1}", DateTime.UtcNow, responseMessage.StatusCode);
                    return null!;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("DateTime: {0} GetDrivers Method of DriverManager MVC Controller Started: ResponseMessage:{1}", DateTime.UtcNow, ex.Message);
                throw;
            }

        }
		#endregion


		/// <summary>
		/// Action to get Driver Details by userId
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>                  
		#region GetDriverDetailById
		[HttpPost]
        public async Task<IActionResult> GetDriverDetailById([FromForm] string userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpContext.Session.SetString("Status", "");
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    string strPayload = JsonConvert.SerializeObject(userId);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var responseResult = await client.PostAsync(client.BaseAddress + "Registration/GetDriverMasterDetailsById", content);
                    var result = responseResult.Content.ReadAsStringAsync();
                    if (responseResult.IsSuccessStatusCode)
                    {
                        var driverList = JsonConvert.DeserializeObject<APIResponse<DriverDataResponse>>(result.Result)!;
                        if (driverList.Data is null)
                        {
                            return null!;
                        }
                        if (driverList.Data.User!.DocStatus!.ToUpper().Equals(GlobalConstants.GlobalValues.Pending))
                        {

                            HttpContext.Session.SetString("Status", GlobalConstants.GlobalValues.Pending.ToLower());
                        }
                        else
                        {
                            HttpContext.Session.SetString("Status", "");
                        }
                        return Json(driverList.Data);

                    }
                    return null!;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
		#endregion


		/// <summary>
		/// Action to delete user by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		#region DeleteUser
		[HttpPost]
        public async Task<IActionResult> DeleteUser([FromForm] string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    string strPayload = JsonConvert.SerializeObject(id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var responseResult = await client.PostAsync(client.BaseAddress + "Registration/DeleteUser", content);
                    var result = responseResult.Content.ReadAsStringAsync();
                    if (responseResult.IsSuccessStatusCode)
                    {
                        var userResult = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (userResult.Data != 1)
                        {
                            return null!;
                        }
                        return Json(userResult.Data);
                    }
                    return null!;
                }
            }
            catch (Exception)
            {
                throw;
            }

        } 


        #endregion
        /// <summary>
        /// Action to set status of user approval
        /// </summary>
        /// <param name="approvalStatus"></param>
        /// <returns></returns>                  
        #region SetUserApprovalStatus
        [HttpPost]
        public async Task<IActionResult> SetUserApprovalStatus([FromForm] UserApprovalStatus approvalStatus)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    string strPayload = JsonConvert.SerializeObject(approvalStatus);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var responseResult = await client.PostAsync(client.BaseAddress + "Registration/SetUserApprovalStatus", content);
                    var result = responseResult.Content.ReadAsStringAsync();
                    if (responseResult.IsSuccessStatusCode)
                    {
                        var driverList = JsonConvert.DeserializeObject<APIResponse<bool>>(result.Result)!;
                        if (driverList.Data)
                        {
                            return Json(true);
                        }
                        return Json(false);
                    }
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }

        } 
        #endregion
    }
}
