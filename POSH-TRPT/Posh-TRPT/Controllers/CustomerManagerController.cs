using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.CustomerDTO;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;
using System.Net.Http.Headers;
using System.Text;
namespace Posh_TRPT.Controllers
{
    [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
    public class CustomerManagerController : Controller
    {
        private readonly ILogger<CustomerManagerController> _logger;
        private readonly IConfiguration _configuration;
        public CustomerManagerController(ILogger<CustomerManagerController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// method to show customer info
        /// </summary>
        /// <returns></returns>
		#region Index
		public IActionResult Index()
        {
            return View();
        }
		#endregion

        /// <summary>
        /// method to get customers info
        /// </summary>
        /// <returns></returns>
		#region GetCustomers
		[HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    var response1 = await client.GetAsync(client.BaseAddress + "Customer/GetAllCustomerDetails");
                    var result = response1.Content.ReadAsStringAsync();
                    if (response1.IsSuccessStatusCode)
                    {
                        var driverList = JsonConvert.DeserializeObject<APIResponse<List<CustomerDTO>>>(result.Result)!;
                        if (driverList.Data is null)
                        {
                            return null!;
                        }
                        return Json(driverList.Data!);
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
        /// method to delete customer
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
		#region DeleteCustomer
		public async Task<IActionResult> DeleteCustomer(string Id)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, Configuration.ContentType);
                    var response1 = await client.PostAsync(client.BaseAddress + "Customer/DeleteCustomer", content);
                    var result = response1.Content.ReadAsStringAsync();
                    if (response1.IsSuccessStatusCode)
                    {
                        var driverList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (driverList.Data != 1)
                        {
                            return null!;
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
        /// method to get customer details by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
		#region GetCustomerDetailById
		[HttpPost]
        public async Task<IActionResult> GetCustomerDetailById(string Id)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent c = new StringContent(strPayload, Encoding.UTF8, Configuration.ContentType);
                    var response1 = await client.PostAsync(client.BaseAddress + "Customer/GetCustomerByID", c);
                    var result = response1.Content.ReadAsStringAsync();
                    if (response1.IsSuccessStatusCode)
                    {
                        var driverList = JsonConvert.DeserializeObject<APIResponse<CustomerDTO>>(result.Result)!;
                        if (driverList.Data is null)
                        {
                            return null!;
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
        /// method to update customet detail
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		#region UpdateCustomerDetails
		[HttpPost]
        public async Task<IActionResult> UpdateCustomerDetails(CustomerDTO data)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    string strPayload = JsonConvert.SerializeObject(data);
                    HttpContent c = new StringContent(strPayload, Encoding.UTF8, Configuration.ContentType);
                    var response1 = await client.PostAsync(client.BaseAddress + "Customer/UpdateCustomerDetails", c);
                    var result = response1.Content.ReadAsStringAsync();
                    if (response1.IsSuccessStatusCode)
                    {
                        var driverList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (driverList.Data != 1)
                        {
                            return null!;
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
    }
}
