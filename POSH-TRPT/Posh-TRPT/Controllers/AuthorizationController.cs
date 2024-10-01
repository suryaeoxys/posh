using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Token;
using Posh_TRPT_Utility.Resources;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.LoginDTO;
using Posh_TRPT_Models.DTO.TokenDTO;
using Posh_TRPT_Services.Token;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Posh_TRPT_Services.Register;
using Posh_TRPT_Domain.Register;
using Microsoft.EntityFrameworkCore;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Services.StripePayment;
using Posh_TRPT_Domain.StripePayment;
using Microsoft.Extensions.Options;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StripePaymentService _paymentService;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;
        private readonly StripeSettings _stripeSettings;
        private readonly IHttpContextAccessor _context;
        private IMapper _mapper;

        public AuthorizationController(IOptions<StripeSettings> stripeSettings, StripePaymentService paymentService, IHttpContextAccessor context, IMapper mapper, ILogger<AuthorizationController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signManager, RoleManager<IdentityRole> roleManager, TokenService tokenService, RegisterService registerService)
        {
            _logger = logger;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
            _paymentService = paymentService;
            _mapper = mapper;
            _stripeSettings = stripeSettings.Value;

        }
        #region Login
        /// <summary>
        /// API to Login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO login)
        {
            string role = string.Empty;
            APIResponse<TokenInfoUserDTO> tokenInfoResult = null!;
            APIResponse<ConnectAccountReturnURL> accountUrl = null!;

            try
            {
                if (ModelState.IsValid)
                {

                    var user = await _userManager.FindByEmailAsync(login.Email).ConfigureAwait(false);



                    _logger.LogInformation("Date Time :{0} Login Method of Authorization Controller Started : Email:{1}, DeviceId:{2} ", DateTime.UtcNow, login.Email, login.DeviceId);

                    if (user is not null && !user.IsDeleted && user.IsActive)
                    {
                      
                        if (await _userManager.CheckPasswordAsync(user!, login.Password) && (await _signManager.PasswordSignInAsync(user, login.Password, false, false)).Succeeded)
                        {

                            // ADD THE ROLE BASE CLAIM
                            var userRoles = await _userManager.GetRolesAsync(user!);
                            var authClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name,user.Name!),
                                new Claim(ClaimTypes.Email,user!.Email),
                                new Claim(ClaimTypes.MobilePhone,user!.PhoneNumber),
                                new Claim(ClaimTypes.NameIdentifier,user!.Id),
                                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                            };
                            foreach (var userRole in userRoles)
                            {
                                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                            }


                            // CREATE/UPDATE TOKEN
                            var token = _tokenService.GetToken(authClaims);
                            _context.HttpContext!.Session.SetString(Configuration.Token, token!.Result!.Data!.TokenString!);

                            var refreshToken = _tokenService.GetRefreshToken();
                            var tokenInfo = _tokenService.GetUserFromTokenInfo(user.UserName);
                            
                            if (tokenInfo.Result.Data is null)
                            {
                                var info = new TokenInfoUserDTO { AccessToken = token.Result.Data!.TokenString, UserName = user.UserName, RefreshToken = refreshToken.Result.Data, RefreshTokenExpiry = DateTime.UtcNow.AddDays(7), IsDeleted = false, CreatedBy = user.Id, CreatedDate = DateTime.UtcNow, UpdatedBy = user.Id, UpdatedDate = DateTime.UtcNow };
                                tokenInfoResult = await _tokenService.AddUpdateToken(info);
                                tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
                                tokenInfoResult!.Data.CreatedBy = user.Id;

                            }
                            else
                            {
                                var principle = await _tokenService.GetPrincipleFromExpiredToken(token.Result.Data.TokenString!);
                                var newRefreshToken = refreshToken.Result.Data!;
                                tokenInfoResult = await _tokenService.AddUpdateToken(new TokenInfoUserDTO
                                {
                                    Id = tokenInfo.Result.Data.Id,
                                    AccessToken = token.Result.Data.TokenString!,
                                    RefreshToken = newRefreshToken,
                                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(7),
                                    IsDeleted = false,
                                    UpdatedDate = DateTime.UtcNow,
                                    DeviceId = user.DeviceId,
                                    UpdatedBy = user.Id,                                    
                                    UserName = tokenInfo.Result.Data.UserName
                                });
                                _logger.LogInformation("Date Time :{0} Login Method Ended : Email:{1} DeviceId:{2}", DateTime.UtcNow, user.Email, user.DeviceId);
                               
                                tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
                                tokenInfoResult!.Data!.Id = Guid.Parse(user.Id);
                                tokenInfoResult!.Data!.CreatedBy = user.Id;

                                #region GET URL TO CREATE CONNECT ACCOUNT FOR DRIVER
                                StripeCreateAccount createAccount = new StripeCreateAccount { UserId = user.Id, AccountType = _stripeSettings.AccountType, Country = _stripeSettings.Country, Email = user.Email, Capabilities_card_payments_requested = true, Capabilities_transfers_requested = true };
                                if (user.StatusId!.Value.ToString().ToLower().Equals(GlobalConstants.GlobalValues.Approved.ToLower()!) && user.Payouts_Enabled == false)
                                {

                                    accountUrl = await _paymentService.CreateAccount(createAccount).ConfigureAwait(false);
                                    if(accountUrl.Data!=null)
                                    {
                                        tokenInfoResult!.Data!.PayoutStatus = accountUrl!.Data!.PayoutStatus;
                                        tokenInfoResult!.Data!.URL = accountUrl.Data!.URL;
                                    }
                                    else
                                    {
                                        tokenInfoResult!.Data!.PayoutStatus = false;
                                        tokenInfoResult!.Data!.URL = null!;
                                    }
                                    
                                }
                                else if (!user.Id.Equals(GlobalConstants.GlobalValues.SuperPermissionID.SuperAdmin))
                                {
                                    accountUrl = await _paymentService.CreateAccount(createAccount).ConfigureAwait(false);
                                    if (accountUrl.Data != null)
                                    {
                                        tokenInfoResult!.Data!.PayoutStatus = accountUrl.Data!.PayoutStatus;
                                        tokenInfoResult!.Data!.URL = accountUrl.Data!.URL;
                                    }
                                    else
                                    {
                                        tokenInfoResult!.Data!.PayoutStatus = false;
                                        tokenInfoResult!.Data!.URL = null!;
                                    }

                                } 
                                #endregion


                            }

                            // UPDATE THE DEVICE ID IN ASPNETUSER TABLE
                            user.DeviceId = login.DeviceId;
                            user.UpdatedDate = DateTime.UtcNow;
                            user.UpdatedBy = user.Id;
                            user.IsLoggedIn = true;
                            await _userManager.UpdateAsync(user);

                            _logger.LogInformation("Date Time :{0} Login Method of Authorization Controller Started : Email:{1}, DeviceId:{2} Success:{3}", DateTime.UtcNow, login.Email, login.DeviceId, LoginResource.LoginSuccess);
                            return Ok(new APIResponse<TokenInfoUserDTO>() { Data = tokenInfoResult!.Data!, Message = LoginResource.LoginSuccess, Status = System.Net.HttpStatusCode.OK, Success = true, Error = null! });
                        }
                        _logger.LogError("Date Time :{0} Login Method of Authorization Controller Started : Email:{1}, DeviceId:{2} Error:{3}", DateTime.UtcNow, login.Email, login.DeviceId, LoginResource.UnauthorizedUser);
                        return Unauthorized(new APIResponse<TokenInfoUserDTO>() { Data = null!, Message = LoginResource.UnauthorizedUser, Status = System.Net.HttpStatusCode.Unauthorized, Success = false, Error = new CustomException(LoginResource.UnauthorizedUser) });
                    }
                    _logger.LogError("Date Time :{0} Login Method of Authorization Controller Started : Email:{1}, DeviceId:{2} Error:{3}", DateTime.UtcNow, login.Email, login.DeviceId, LoginResource.UserNotExists);
                    return Unauthorized(new APIResponse<TokenInfoUserDTO>() { Data = null!, Message = LoginResource.UserNotExists, Status = System.Net.HttpStatusCode.Unauthorized, Success = false, Error = new CustomException(LoginResource.UserNotExists) });
                }
                _logger.LogError("Date Time :{0} Login Method of Authorization Controller Started : Email:{1}, DeviceId:{2} Error:{3}", DateTime.UtcNow, login.Email, login.DeviceId, LoginResource.UnauthorizedLogin);
                return BadRequest(new APIResponse<TokenInfoUserDTO>() { Data = null!, Message = LoginResource.UnauthorizedLogin, Status = System.Net.HttpStatusCode.BadRequest, Success = false, Error = new CustomException(LoginResource.UnauthorizedLogin) });

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region LogOut
        /// <summary>
        /// LogOut
        /// </summary>

        [HttpGet]
        public async Task< IActionResult> LogOut()
        {
            _logger.LogInformation("Date Time :{0} LogOut Method of Authorization Controller Started:", DateTime.UtcNow);


            string userId = _context.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // UPDATE THE DEVICE ID IN ASPNETUSER TABLE
                user.UpdatedDate = DateTime.UtcNow;
                user.UpdatedBy = user.Id;
                user.IsLoggedIn = false;
                await _userManager.UpdateAsync(user);

                HttpContext.Session.SetString(Configuration.Token, string.Empty);
                HttpContext.Session.Remove(Configuration.Token);
                HttpContext.Session.Clear();
                return Ok(new APIResponse<object>
                {
                    Data = null!,
                    Status = System.Net.HttpStatusCode.OK,
                    Success = true,    
                    Message = "Successfully Logged out!",
                });
            }
            else
            {
                return Ok(new APIResponse<object>
                {
                    Data = null!,
                    Status = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Something went wrong!",
                });
            }
           
        }
        #endregion
    }
}
