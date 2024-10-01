using AutoMapper;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.StripePaymentDTO;
using Posh_TRPT_Services.BookingSystem;
using Posh_TRPT_Services.StripePayment;
using Posh_TRPT_Utility.ConstantStrings;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using static Posh_TRPT_Utility.ConstantStrings.AuthorizationLevel;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class StripePaymentController : ControllerBase
    {
        private readonly StripeSettings _stripeSettings;
        public string Result_Id = string.Empty;
        private readonly StripePaymentService _paymentService;
		public StripePaymentController(IOptions<StripeSettings> stripeSettings, StripePaymentService paymentService)
        {
            _stripeSettings = stripeSettings.Value;
            _paymentService = paymentService;
        }

		#region CreateCustomer
		/// <summary>
		/// Stripe method to create new customer
		/// </summary>
		/// <param name="email"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromQuery] string email, string name, string userId)
        {

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
            {
                var result = await _paymentService.CreateCustomer(email, name, userId);
                return Ok(result);
            }
            return BadRequest("Name:" + $"{name}" + " " + "Email:" + $"{email}"+ "UserId:"+ $"{userId}");
        }
		#endregion

		/// <summary>
		/// method to get GetEphemeralKey
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
        ///

		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpPost]
        public async Task<IActionResult> GetEphemeralKey(string customerId)
        {
            if(!string.IsNullOrEmpty(customerId))
            {
                var result = await _paymentService.GetEmhemeralKey(customerId);
                return Ok(result);
			}
            return BadRequest();
        }

        /// <summary>
        /// Method to GetAllCustomer 
        /// </summary>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> GetAllCustomers([FromQuery]string customerEmail)
        {
           if(!string.IsNullOrEmpty(customerEmail))
            {
                var customerData=await _paymentService.GetAllCustomers(customerEmail);
                if(customerData.Data!=null)
                    return Ok(customerData);
                return NotFound();
            }
           return BadRequest();
        }


		/// <summary>
		/// method to create CreatePaymentIntent
		/// </summary>
		/// <param name="Currency"></param>
		/// <param name="Amount"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpPost]
        public async Task<IActionResult> CreatePaymentIntent([FromQuery] string Currency, decimal Amount, bool isWalletApplied,double CashBackPrice)
        {
          if(!string.IsNullOrEmpty(Currency))
            {
				var result = await _paymentService.CreatePaymentIntent(Currency,Amount,isWalletApplied, CashBackPrice);
				return Ok(result);
			}
          return BadRequest();
        }


		/// <summary>
		/// Method to Create Account
		/// </summary>
		/// <param name="createAccount"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] StripeCreateAccount createAccount)
        {

            if (ModelState.IsValid)
            {
                var result = await _paymentService.CreateAccount(createAccount);
                return Ok(result);
            }
            return BadRequest("Name:" + $"{createAccount.AccountType}" + " " + "Email:" + $"{createAccount.Country}" + "UserId:" + $"{createAccount.Email}");
        }

        /// <summary>
        /// method to return account link
        /// </summary>
        /// <param name="connectAcco"></param>
        /// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Driver)]
		[HttpPost]
		public async Task<IActionResult> ConnectAccountLink([FromBody] ConnectAccoLink connectAcco)
		{

			if (ModelState.IsValid)
			{
				var result = await _paymentService.ConnectAccountLink(connectAcco);
				return Ok(result);
			}
			return BadRequest();
		}

		#region GetConnectAccountStatusUrl
		/// <summary>
		/// method to get driver's Payout status
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Driver)]
		[HttpGet]
		public async Task<IActionResult> GetConnectAccountStatusUrl(string userId)
		{
			var result = await _paymentService.GetConnectAccountStatusUrl(userId);
			return Ok(result);
		}
		#endregion

		#region StripeSetupIntent
		/// <summary>
		/// API for Stripe payment setup
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
        [HttpPost]
        public async Task<IActionResult> StripeSetupIntent()
        {
            var result = await _paymentService.StripeSetupIntent();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }
		#endregion

		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpGet]
        public async Task<IActionResult>IsPaymentMethodAvailableForCustomer()
        {
			var result = await _paymentService.IsPaymentMethodAvailableForCustomer();

				return Ok(result);

		}

		[Authorize(Roles = AuthorizationLevel.Roles.Customer)]
		[HttpPost]
		public async Task<IActionResult> StripeDriverPaymentSystem([FromBody]StripeDriverPaymentInfo driverPaymentInfo)
        {
            var result = await _paymentService.StripeDriverPaymentSystem(driverPaymentInfo);
            return Ok(result);

		}
		[HttpGet("/order/success")]
		public ActionResult OrderSuccess([FromQuery] string session_id)
		{
			var sessionService = new SessionService();
			Session session = sessionService.Get(session_id);

			var customerService = new CustomerService();
			Customer customer = customerService.Get(session.CustomerId);

			return new ContentResult { ContentType = "text/html", Content =$"<html><body><h1>Thanks for the posh ride, {customer.Name}!</h1></body></html>" };
		}


        #region StripeDriverBalance
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpGet]
        public async Task<IActionResult> StripeDriverBalance()
        {
            var result = await _paymentService.StripeDriverBalance();
            return Ok(result);

        }
        #endregion

        #region MakeDefaultPaymentMethod
        [Authorize(Roles = AuthorizationLevel.Roles.Customer)]
        [HttpPost]
        public async Task<IActionResult> MakeDefaultPaymentMethod(string PaymentMethodId)
        {

            if(!string.IsNullOrEmpty(PaymentMethodId))
            {
                var result = await _paymentService.MakeDefaultPaymentMethod(PaymentMethodId);
                return Ok(result);
            }
            return BadRequest(PaymentMethodId);

        }
        #endregion

        #region AddMoneyToWallet
        /// <summary>
        /// Method to add money to wallate
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddMoneyToWallet(DigitalWalletData money)
        {
            APIResponse<string> result = null!;
            if (money != null)
            {
                result = await _paymentService.AddMoney(money);
                return Ok(result);
            }
            return BadRequest(result);
        } 
        #endregion

    }


}
