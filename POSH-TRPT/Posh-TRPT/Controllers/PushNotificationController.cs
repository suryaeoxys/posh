using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Models.DTO.PushNotificationDTO;
using Posh_TRPT_Services.PushNotification;
using Posh_TRPT_Utility.ConstantStrings;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        private readonly PushNotificationService _pushNotificationService;
        private readonly ILogger<MasterTableAPIController> _logger;

        public PushNotificationController(PushNotificationService pushNotificationService, ILogger<MasterTableAPIController> logger)
        {
            _pushNotificationService = pushNotificationService;
            _logger = logger;
        }

		#region SendNotification
		/// <summary>
		/// method to send push notification
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModelDTO model)
        {
            var pushNotification = await _pushNotificationService.SendNotification(model);
            return Ok(pushNotification);
        } 
        #endregion
    }
}