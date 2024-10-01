using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.LoginDTO;
using Posh_TRPT_Models.DTO.RegisterDTO;
using Posh_TRPT_Models.DTO.TokenDTO;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.Resources;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using Twilio.TwiML.Voice;

namespace Posh_TRPT.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signManager;
        public AccountController(IConfiguration configuration, ILogger<AccountController> logger, SignInManager<ApplicationUser> signManager)
        {

            _configuration = configuration; 
            _logger = logger;
            _signManager = signManager;

        }
        #region SignIn
        /// <summary>
        /// Action method to Sign In
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            if(Request.Cookies.TryGetValue("RememberMeFun",out string remembervalue))
            {
                var value = remembervalue!.Split("|");
                if (value.Length == 2)
                {
                    ViewBag.Email = value[0];   
                    ViewBag.Password = value[1];
                }

            }
            _logger.LogInformation("InSide SignIn Get Method of Account Controller Started");
            return View();
        }
        #endregion
        #region LogOut
        /// <summary>
        /// LogOut
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {

            try
            {
                _logger.LogInformation("{0} InSide SignOut Method of Account Controller Started", DateTime.UtcNow);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var token = HttpContext.Session.GetString(Configuration.Token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Configuration.Bearer, token);
                    var response1 = await client.GetAsync(client.BaseAddress + ClientURL.SignOut);
                    var result = response1.Content.ReadAsStringAsync();
                    if (response1.IsSuccessStatusCode)
                    {
                        var dresult = JsonConvert.DeserializeObject<APIResponse<object>>(result.Result)!;
                        if (dresult.Success)
                        {
                            foreach (var cookie in Request.Cookies.Keys)
                            {
                                Response.Cookies.Delete(cookie);
                            }
                            HttpContext.Session.Remove(Configuration.Token);
                            return RedirectToAction(GlobalResourceFile.SignIn, GlobalResourceFile.Account);
                        }
                        return RedirectToAction(GlobalResourceFile.SignIn, GlobalResourceFile.Account);
                    }
                    return RedirectToAction(GlobalResourceFile.SignIn, GlobalResourceFile.Account);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide SignOut Method of Account Controller Exception: {1}", DateTime.UtcNow, ex.Message);
                throw;
            }

        }
        #endregion
        #region SignIn Post
        /// <summary>
        ///     Action method to Sign In with username and password
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous,ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([FromForm] LoginDTO login)
        {
            try
            {
                _logger.LogInformation("{0} InSide SignIn Post Method of Account Controller Started Email:{1} DeviceId:{2}", DateTime.UtcNow, login.Email, login.DeviceId);
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                        client.DefaultRequestHeaders.Accept.Clear();
                        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, ClientURL.ResourceType);
                        var response = await client.PostAsync(client.BaseAddress + ClientURL.Login, httpContent).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {
                            if (login.RememberMe)
                            {
                                Response.Cookies.Append("RememberMeFun", $"{login.Email}|{login.Password}",
                                new CookieOptions
								{
                                    Expires = DateTime.UtcNow.AddDays(25),
									HttpOnly = true,
									SameSite = SameSiteMode.Strict,
									Secure = true
								});;

							}
                            ViewBag.ErrorMessage = null;
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var root = JsonConvert.DeserializeObject<APIResponse<TokenInfoUserDTO>>(responseBody.ToString());
                            if (root!.Status == System.Net.HttpStatusCode.Unauthorized && !root!.Success)
                            {
                                ViewBag.ErrorMessage = LoginResource.UnauthorizedUser;
                                _logger.LogInformation("{0} InSide SignIn Post Method of Account Controller Started Email:{1} DeviceId:{2} Error:{3}", DateTime.UtcNow, login.Email, login.DeviceId, LoginResource.UnauthorizedUser);
                                return View(ViewBag.ErrorMessage);
                            }
                            HttpContext.Session.SetString(Configuration.Token, root!.Data!.AccessToken!);
                            return RedirectToAction(Configuration.Index, Configuration.Home);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = LoginResource.UnauthorizedLogin;
                            _logger.LogInformation("{0} InSide SignIn Post Method of Account Controller Started Email:{1} DeviceId:{2} Error:{3}", DateTime.UtcNow, login.Email, login.DeviceId, LoginResource.UnauthorizedLogin);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide SignIn Post Method of Account Controller Started Email:{1} DeviceId:{2} Exception Message:{3}", DateTime.UtcNow, login.Email, login.DeviceId, ex.Message);
                throw;
            }
            return View();
        } 
        #endregion
       
        #region ForgotPassword
        /// <summary>
        ///     Action method to 
        /// </summary>
        /// <returns></returns>
        public IActionResult ForgotPassword()
        {
            return View();
        }
        #endregion
        #region ForgotPassword Post
        /// <summary>
        ///     Action method to Get the otp on email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    client.DefaultRequestHeaders.Accept.Clear();
                    HttpContext.Session.SetString("EmailtoReset", email);
                    var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
					var response = await client.PostAsync(ClientURL.ForgotPassword, content).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse<object>>(responseBody);

                        if (apiResponse?.Data != null)
                        {
                            string input = apiResponse.Data.ToString() ?? string.Empty;
                            HttpContext.Session.SetString("UserDetail", input);

                            if (!string.IsNullOrEmpty(input))
                            {
                                string[] parts = input.Split('.');
                                if (parts.Length >= 2)
                                {
                                    string[] subParts = parts[1].Split(' ');
                                    ViewBag.OTP = subParts[0];
								}
                            }
							return await System.Threading.Tasks.Task.FromResult(Json(new { redirectTo = Url.Action("GetOTPToResetPassword", Configuration.Account) }));
							//return Json(new { redirectTo = Url.Action("GetOTPToResetPassword", Configuration.Account) });
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to retrieve OTP for email: {0}. Status code: {1}", email, response.StatusCode);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred in ForgotPassword method: {0}", ex.Message);
                throw;
            }
        }
        #endregion

        #region GetOTPToResetPassword
        /// <summary>
        ///     Action method to GetOtp
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetOTPToResetPassword()
        {
            return View();
        }
        #endregion


        #region GetOTPToResetPassword Post
        /// <summary>
        ///     Action method to validate the otp
        /// </summary>
        /// <param name="forgotPasswordVerify"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> GetOTPToResetPassword(ForgotPasswordVerifyCodeDTO forgotPasswordVerify)
        {
            try
            {
                var hashCode = HttpContext.Session.GetString("UserDetail");
                var email = HttpContext.Session.GetString("EmailtoReset");
                forgotPasswordVerify.HashCode = hashCode;
                forgotPasswordVerify.Email = email;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var content = new StringContent(JsonConvert.SerializeObject(forgotPasswordVerify), Encoding.UTF8, ClientURL.ResourceType);
                    var response = await client.PostAsync(ClientURL.ForgotPasswordVerifyCode, content).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse<object>>(responseBody);

                        if (apiResponse?.Success == true)
                        {
                            return RedirectToAction("SetNewPassword", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Please enter correct OTP.");
                            return View("GetOTPToResetPassword", forgotPasswordVerify.Otp);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Please enter correct OTP.");
                        return View("GetOTPToResetPassword", forgotPasswordVerify.Otp);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred in GetOTPToResetPassword method: {0}", ex.Message);
                throw;
            }
        }
        #endregion
        #region SetNewPassword
        /// <summary>
        ///     Action method to SetNewPassword
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SetNewPassword()
        {
            return View();
        }
        #endregion

        [HttpPost]
        [AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> SetNewPassword(string newPassword, string email)
        {
            try
            {
                var emailId = HttpContext.Session.GetString("EmailtoReset");
                email = emailId!;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["LocalUrl:BaseUrl"]!);
                    var url = $"{ClientURL.SetNewPassword}?password={Uri.EscapeDataString(newPassword)}&email={Uri.EscapeDataString(email)}";
                    var response = await client.PostAsync(url, null);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse<object>>(responseBody);

                        if (apiResponse?.Data != null)
                        {
                            return RedirectToAction("SignIn", Configuration.Account);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
            return View();
        }

    }
}
