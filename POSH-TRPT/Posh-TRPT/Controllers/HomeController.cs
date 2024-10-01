using Posh_TRPT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Posh_TRPT_Utility.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Posh_TRPT_Utility.ConstantStrings;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Domain.Register;

namespace Posh_TRPT.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
		/// <summary>
		/// Action methos to show Index view
		/// </summary>
		/// <returns></returns>
		#region Index
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
		#endregion


		/// <summary>
		/// Action method to view country
		/// </summary>
		/// <returns></returns>
		#region Country
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult Country()
        {
            return View();
        }
		#endregion


		/// <summary>
		/// Action method to view state
		/// </summary>
		/// <returns></returns>
		#region State
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult State()
        {
            return View();
        }
		#endregion


		/// <summary>
		/// Action method to view city
		/// </summary>
		/// <returns></returns>
		#region City
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult City()
        {
            return View();
        }
		#endregion


		/// <summary>
		/// Action method to get countries
		/// </summary>
		/// <returns></returns>
		#region GetCountries
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response1 = await client.GetAsync(client.BaseAddress + Configuration.RegistrationGetCountries);
                    var result = response1.Content.ReadAsStringAsync();
                    if (response1.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<CountryData>>>(result.Result)!;
                        if (countriesList.Data is null)
                        {
                            return null!;
                        }
                        return Json(countriesList.Data);
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     
    }
}