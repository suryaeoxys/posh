using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.MasterTableDTO;
using Posh_TRPT_Utility.ConstantStrings;
using System.Text;
using static Posh_TRPT_Utility.ConstantStrings.AuthorizationLevel;

namespace Posh_TRPT.Controllers
{
    [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
    public class MasterTableController : Controller
    {


        private readonly IConfiguration _configuration;
        private readonly ILogger<MasterTableController> _logger;
        public MasterTableController(IConfiguration configuration, ILogger<MasterTableController> logger)
        {

            _configuration = configuration;
            _logger = logger;

        }


		#region Country Related Method

		/// <summary>
		/// Action method to view Country
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public IActionResult Index()
        {
            return View();

         }
		#endregion


		#region GetCountry
		/// <summary>
		/// Action to get country
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetCountry()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + "MasterTableAPI/GetCountry");
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<CountryDTO>>>(result.Result)!;
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

		#region AddCountry

		/// <summary>
		/// Action to add country
		/// </summary>
		/// <param name="result1"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddCountry(CountryDTO result1)
        {
            try
            {
                result1.Id = (result1.Id == Guid.Empty || result1.Id == null) ? Guid.NewGuid() : result1.Id;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result1);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/AddCountry", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;

                        return Json(countriesList);
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

		#region DeleteCountry
		/// <summary>
		/// Action to delete country
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            try
            {
                var dd = Convert.ToString(id);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/DeleteCountry", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (countriesList.Data != 1)
                        {
                            return Json(countriesList.Data);
                        }
                        return Json(countriesList.Data);
                    }
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
		#endregion

		#region UpdateCountry
		/// <summary>
		/// Method to update country
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> UpdateCountry(CountryDTO result)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/UpdateCountry", content);
                    var resultData = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<bool>>(resultData.Result)!;
                        if (!countriesList.Data)
                        {
                            return null!;
                        }
                        return Json(countriesList);
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

		#region State Related Method
		/// <summary>
		/// Method to view state
		/// </summary>
		/// <returns></returns>
		public IActionResult State()
        {

            return View();
        } 
        #endregion

        #region GetState
        /// <summary>
        /// Action to get state
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetState()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + "MasterTableAPI/GetState");
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<StateDTO>>>(result.Result)!;
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


		#region GetStateDetailById
		/// <summary>
		/// Action to get State by Id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetStateDetailById(Guid Id)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/GetStateDetailById", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<StateDTO>>(result.Result)!;
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

		#region GetState
		/// <summary>
		/// Action to get state
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetState(Guid Id)
        {
            try
            {
                var dd = Convert.ToString(Id);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/GetStateList", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<StateDTO>>>(result.Result)!;
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

		#region AddState
		/// <summary>
		/// Action to add state
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddState(StateDTO result)
        {

            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/AddState", content);
                    var resultData = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(resultData.Result)!;
                        if (countriesList.Success)
                        {
                            return Json(countriesList.Data);
                        }
                        return Json(countriesList.Data);
                    }
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
		#endregion

		#region DeleteState

		/// <summary>
		/// Action to delete state
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteState(Guid id)
        {
            try
            {
                var dd = Convert.ToString(id);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/DeleteState", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (countriesList.Data != 1)
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

		#region ViewCity
		/// <summary>
		/// Action to view city
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public IActionResult City()
        {
            return View();
        }
		#endregion


		#region AddCity
		/// <summary>
		/// Action to add city
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddCity(CityDTO result)
        {

            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/AddCity", content);
                    var resultData = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(resultData.Result)!;
                        if (countriesList.Success)
                        {
                            return Json(countriesList.Data);
                        }
                        return Json(countriesList.Data);
                    }
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
		#endregion


		#region GetCity
		/// <summary>
		/// Action to get city
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetCity()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + "MasterTableAPI/GetCityList");
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<CityDTO>>>(result.Result)!;
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

		#region GetCityDetailById
		/// <summary>
		/// Action to get city by id
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetCityDetailById(Guid Id)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/GetCityDetailById", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<CityDTO>>(result.Result)!;
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


		#region GetCityByStateId
		/// <summary>
		/// Action to get city by Stateid
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetCityByStateId(Guid Id)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/GetCityByStateId", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<IEnumerable<CityDTO>>>(result.Result)!;
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

		#region DeleteCity

		/// <summary>
		/// Action to delete city
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            try
            {
                var dd = Convert.ToString(id);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/DeleteCity", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (countriesList.Data != 1)
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

		#region RideCategory Related Method
		/// <summary>
		/// Action to view category
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public IActionResult RideCategory()
        {
            return View();
        }
		#endregion

		#region GetRideCategory

		/// <summary>
		/// Action to get ride category
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetRideCategory()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + "MasterTableAPI/GetRideCategory");
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<RideCategoryDTO>>>(result.Result)!;
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


		#region AddRideCategory
		/// <summary>
		/// Action to add ride category
		/// </summary>
		/// <param name="result1"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddRideCategory(RideCategoryDTO result1)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result1);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/AddRideCategory", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (!countriesList.Success)
                        {
                            return Json(countriesList.Data);
                        }
                        return Json(countriesList.Data);
                    }
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
		#endregion


		#region DeleteRideCategory
		/// <summary>
		/// Action to delete ride category
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteRideCategory(Guid id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/DeleteRideCategory", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (countriesList.Data != 1)
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

		#region UpdateRideCategory

		/// <summary>
		/// Action to update ride category
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> UpdateRideCategory(RideCategoryDTO result)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/UpdateRideCategory", content);
                    var resultData = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<bool>>(resultData.Result)!;
                        if (!countriesList.Data)
                        {
                            return null!;
                        }
                        return Json(countriesList);
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


		#region CategoryPrice
		/// <summary>
		/// Action to view Category Price
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public IActionResult CategoryPrice()
        {
            return View();
        }
		#endregion


		#region GetCategoryPriceList
		/// <summary>
		/// Action to get category price
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetCategoryPriceList()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + "MasterTableAPI/GetCategoryPriceList");
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<CategoryPriceDTO>>>(result.Result)!;
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


		#region AddCategoryPrice
		/// <summary>
		/// Action to add category price
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddCategoryPrice(CategoryPriceDTO result)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(result);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/AddCategoryPrice", content);
                    var resultData = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(resultData.Result)!;

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


		#region GetCategoryPriceDetailById
		/// <summary>
		/// Action to get category price
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetCategoryPriceDetailById(Guid Id)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(Id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/GetCategoryPriceDetailById", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<CategoryPriceDTO>>(result.Result)!;
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



		#region GetStatusList
		/// <summary>
		/// Action to get GetStatusList
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetStatusList()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var response = await client.GetAsync(client.BaseAddress + "MasterTableAPI/GetStatusList");
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<List<StatusDTO>>>(result.Result)!;
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


		#region DeleteCategoryPrice
		/// <summary>
		/// ACTION TO DELETE CATEGORY Price
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteCategoryPrice(Guid id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    string strPayload = JsonConvert.SerializeObject(id);
                    HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(client.BaseAddress + "MasterTableAPI/DeleteCategoryPrice", content);
                    var result = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var countriesList = JsonConvert.DeserializeObject<APIResponse<int>>(result.Result)!;
                        if (countriesList.Data != 1)
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
    }
}
