using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Models.DTO.MasterTableDTO;
using Posh_TRPT_Services.MasterTable;
using Posh_TRPT_Utility.ConstantStrings;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class MasterTableAPIController : ControllerBase
    {
        private readonly MasterTableService _masterTableService;
		private readonly ILogger<MasterTableAPIController> _logger;

		public MasterTableAPIController(MasterTableService masterTableService,ILogger<MasterTableAPIController> logger)
        {
            _masterTableService = masterTableService;
            _logger = logger;
        }
		/// <summary>
		/// API to Get Country
		/// </summary>
		/// <returns></returns>
		#region Country
		[HttpGet]       
        public async Task<IActionResult> GetCountry()
        {
            var countryList = await _masterTableService.GetCountryList();
            return Ok(countryList);
        }
		#endregion


		/// <summary>
		/// API to Add Country
		/// </summary>
		/// <param name="country"></param>
		/// <returns></returns>
		#region AddCountry
		[HttpPost]
        public async Task<IActionResult> AddCountry(CountryDTO country)
        {
            var countryList = await _masterTableService.AddCountry(country);
            return Ok(countryList);
        }
		#endregion


		/// <summary>
		/// API To Update Country
		/// </summary>
		/// <param name="country"></param>
		/// <returns></returns>
		#region UpdateCountry
		[HttpPost]
        public async Task<IActionResult> UpdateCountry(CountryDTO country)
        {
            var countryList = await _masterTableService.UpdateCountry(country);
            return Ok(countryList);
        }
		#endregion

		/// <summary>
		/// API To Delete country
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		#region DeleteCountry
		[HttpPost]
        public async Task<IActionResult> DeleteCountry([FromBody] Guid id)
        {
            var dd = Convert.ToString(id);
            var countryList = await _masterTableService.DeleteCountry(id);
            return Ok(countryList);
        } 
        #endregion


        /// <summary>
        /// API To Get State
        /// </summary>
        /// <returns></returns>
        #region State
        [HttpGet]
        public async Task<IActionResult> GetState()
        {
            var countryList = await _masterTableService.GetStateList();
            return Ok(countryList);
        }
		#endregion

		#region GetStateList
		/// <summary>
		/// API to get states
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetStateList()
        {
            var countryList = await _masterTableService.GetStateList();
            return Ok(countryList);
        }
		#endregion


		/// <summary>
		/// Api to get state List
		/// </summary>
		/// <param name="countryId"></param>
		/// <returns></returns>
		#region GetStateList
		[HttpPost]
        public async Task<IActionResult> GetStateList([FromBody] Guid countryId)
        {
            var dd = Convert.ToString(countryId);
            var countryList = await _masterTableService.GetStateList(countryId);
            return Ok(countryList);
        }
		#endregion


		#region AddState
		/// <summary>
		/// API to add state
		/// </summary>
		/// <param name="country"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddState(StateDTO country)
        {
            var countryList = await _masterTableService.AddState(country);
            return Ok(countryList);
        }
		#endregion


		/// <summary>
		/// API to delete State
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		#region DeleteState
		[HttpPost]
        public async Task<IActionResult> DeleteState([FromBody] Guid id)
        {
            var dd = Convert.ToString(id);
            var countryList = await _masterTableService.DeleteState(id);
            return Ok(countryList);
        }
		#endregion

		#region GetStateDetailById
		/// <summary>
		/// API to get state details by Id
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetStateDetailById([FromBody] Guid stateId)
        {
            var dd = Convert.ToString(stateId);
            var countryList = await _masterTableService.GetStateDetailById(stateId);
            return Ok(countryList);
        }
		#endregion

		#region GetCityDetailById
		/// <summary>
		/// API to get city info by Id
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetCityDetailById([FromBody] Guid stateId)
        {
            var dd = Convert.ToString(stateId);
            var countryList = await _masterTableService.GetCityDetailById(stateId);
            return Ok(countryList);
        }
		#endregion


		#region DeleteCity
		/// <summary>
		/// Api to delete city 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteCity([FromBody] Guid id)
        {
            var dd = Convert.ToString(id);
            var countryList = await _masterTableService.DeleteCity(id);
            return Ok(countryList);
        }
		#endregion


		#region AddCity
		/// <summary>
		/// API to add city
		/// </summary>
		/// <param name="country"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddCity(CityDTO country)
        {
            var countryList = await _masterTableService.AddCity(country);
            return Ok(countryList);
        }
		#endregion


		#region GetCityList
		/// <summary>
		/// API to get City List
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetCityList()
        {
            var countryList = await _masterTableService.GetCityList();
            return Ok(countryList);
        }
		#endregion


		#region GetRideCategory
		/// <summary>
		/// API to get ride category
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetRideCategory()
        {
            var countryList = await _masterTableService.GetRideCategoryList();
            return Ok(countryList);
        }
		#endregion


		#region AddRideCategory
		/// <summary>
		/// API to add ride category
		/// </summary>
		/// <param name="rideCategory"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddRideCategory(RideCategoryDTO rideCategory)
        {
            var countryList = await _masterTableService.AddRideCategory(rideCategory);
            return Ok(countryList);
        }
		#endregion



		#region UpdateRideCategory
		/// <summary>
		/// API to update ride category
		/// </summary>
		/// <param name="rideCategory"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> UpdateRideCategory(RideCategoryDTO rideCategory)
        {
            var countryList = await _masterTableService.UpdateRideCategory(rideCategory);
            return Ok(countryList);
        }
		#endregion



		#region DeleteRideCategory
		/// <summary>
		/// API to delete ride category
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> DeleteRideCategory([FromBody] Guid id)
        {
            var dd = Convert.ToString(id);
            var countryList = await _masterTableService.DeleteRideCategory(id);
            return Ok(countryList);
        }
		#endregion
		#region GetCategoryPriceList

		/// <summary>
		/// API to get price list of category
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetCategoryPriceList()
        {
            var countryList = await _masterTableService.GetCategoryPriceList();
            return Ok(countryList);
        }
		#endregion


		#region AddCategoryPrice
		/// <summary>
		/// API to add price category
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> AddCategoryPrice(CategoryPriceDTO model)
        {
            var dd = Convert.ToString(model.Id);
            var countryList = await _masterTableService.AddCategoryPrice(model);
            return Ok(countryList);
        }
		#endregion


		#region GetCategoryPriceDetailById
		/// <summary>
		/// API to get category price details
		/// </summary>
		/// <param name="categoryIdId"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> GetCategoryPriceDetailById([FromBody] Guid categoryIdId)
        {
            var dd = Convert.ToString(categoryIdId);
            var countryList = await _masterTableService.GetCategoryPriceDetailById(categoryIdId);
            return Ok(countryList);
        }
		#endregion



		#region GetStatusList
		/// <summary>
		/// method to get status list
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetStatusList()
        {
            var statusList = await _masterTableService.GetStatusList();
            return Ok(statusList);
        }
		#endregion

		#region DeleteCategoryPrice
		/// <summary>
		/// method to delete category price
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> DeleteCategoryPrice([FromBody] Guid id)
		{
			var dd = Convert.ToString(id);
			var countryList = await _masterTableService.DeleteCategoryPrice(id);
			return Ok(countryList);
		}
		#endregion

		#region GetCityByStateId
		/// <summary>
		/// API to get city info by StateId
		/// </summary>
		/// <param name="stateId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetCityByStateId([FromBody] Guid stateId)
		{
			var dd = Convert.ToString(stateId);
			var countryList = await _masterTableService.GetCityDetailByStateId(stateId);
			return Ok(countryList);
		} 
		#endregion
	}
}
