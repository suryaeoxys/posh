using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.CustomerDTO;
using Posh_TRPT_Models.DTO.PushNotificationDTO;
using Posh_TRPT_Services.Customer;
using Posh_TRPT_Services.PushNotification;
using Posh_TRPT_Services.Register;
using Posh_TRPT_Utility.ConstantStrings;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly PushNotificationService _pushNotificationService;
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _configuration;

        public CustomerController(CustomerService customerService, ILogger<CustomerController> logger, PushNotificationService pushNotificationService, IConfiguration configuration)
        {
           
            _customerService = customerService;
            _logger = logger;
            _pushNotificationService = pushNotificationService;
            _configuration = configuration;
        }

        /// <summary>
        /// Action method to get all customers details
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerDetails()
        {
            _logger.LogInformation("{0} InSide  GetAllCustomerDetails in CustomerControllerAPI Method", DateTime.UtcNow);
            var data = await _customerService.GetCustomerDetails();
            _logger.LogInformation("{0} InSide  GetAllCustomerDetails in CustomerControllerAPI Method No_Of_Customers:{1}", DateTime.UtcNow,data.Data!.Count);
            return Ok(data);
        }
        /// <summary>
        /// Action method to DeleteCustomer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> DeleteCustomer([FromBody] string id)
        {
            _logger.LogInformation("{0} InSide  DeleteCustomer in CustomerControllerAPI Method", DateTime.UtcNow);
            var driver = await _customerService.DeleteCustomer(id);
            _logger.LogInformation("{0} InSide  DeleteCustomer in CustomerControllerAPI Method Id:{1} DeleteMessage:{2}", DateTime.UtcNow,id,driver.Message);
            return Ok(driver);
        }



        /// <summary>
        /// method to get customer details by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
		#region GetCustomerByID
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> GetCustomerByID([FromBody] string userId)
        {
            _logger.LogInformation("{0} InSide  GetCustomerByID in CustomerControllerAPI Method UserId:{1} started:", DateTime.UtcNow, userId);
            var data = await _customerService.GetCustomerDetailsById(userId);
            _logger.LogInformation("{0} InSide  DeleteCustomer in CustomerControllerAPI Method UserId:{1} DeleteMessage:{2} ended.", DateTime.UtcNow, userId, data.Message);
            return Ok(data);
        }
		#endregion



        /// <summary>
        /// method to update customer details
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
		#region UpdateCustomerDetails
		[Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> UpdateCustomerDetails([FromBody] CustomerDTO details)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("{0} InSide  UpdateCustomerDetails Method in CustomerControllerAPI  PhoneNumber:{1} Name:{2} Id:{3} Started", DateTime.UtcNow, details.MobileNumber, details.Name, details.Id);
                var data = await _customerService.UpdateCutomerDetailsByAdmin(details);
                _logger.LogInformation("{0} InSide  UpdateCustomerDetails Method in CustomerControllerAPI  PhoneNumber:{1} Name:{2} Id:{3} Message:{4}", DateTime.UtcNow, details.MobileNumber, details.Name, details.Id, data.Message);
                return Ok(data);

            }
            return BadRequest();
        } 
        #endregion
        #region GetCustomerBasicDetails
        /// <summary>
        /// get customer basic details
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpGet]
		public async Task<IActionResult> GetCustomerBasicDetails()
		{
			var data = await _customerService.GetCustomerBasicDetails();
			return Ok(data);
		}
        #endregion

        /// <summary>
        /// method to get nearest driver details
        /// </summary>
        /// <param name="nearestDriver"></param>
        /// <returns></returns>
        #region GetNearestDriversDetail
        [Authorize(Roles = AuthorizationLevel.Roles.Customer)]
        [HttpGet]
        public async Task<IActionResult> GetNearestDriversDetail([FromBody]NearestDriverRequest nearestDriver)
        {
            if(ModelState.IsValid)
            {
                var data = await _customerService.GetNearestDriversDetail(nearestDriver);
                if(data.Data is not null)
                {
                    NotificationModelDTO notificationModelDTO = new NotificationModelDTO();
                    notificationModelDTO.DeviceId = data.Data?.DeviceId;
                    notificationModelDTO.IsAndroidDevice = true;
                    notificationModelDTO.Title = _configuration["RiderRequestForNewRide:Title"];
                    notificationModelDTO.Body = _configuration["RiderRequestForNewRide:Body"];
                    var pushNotification = await _pushNotificationService.SendNotification(notificationModelDTO);
                    return Ok(data);
                }
                return NoContent();
               
            }
           return BadRequest(ModelState);
        } 
        #endregion
    }
}
