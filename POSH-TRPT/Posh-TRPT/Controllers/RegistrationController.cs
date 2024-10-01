using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.RegisterDTO;
using Posh_TRPT_Models.DTO.TokenDTO;
using Posh_TRPT_Services.Register;
using Posh_TRPT_Services.StripePayment;
using Posh_TRPT_Services.Token;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.EmailUtils;
using Posh_TRPT_Utility.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _context;
		private readonly StripePaymentService _paymentService;
		private readonly RegisterService _registerService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly Random _random = new Random();
        private IMapper _mapper;
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;
        public RegistrationController(StripePaymentService paymentService,IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor context, IMapper mapper, ILogger<RegistrationController> logger, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, TokenService tokenService, RegisterService registerService)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _registerService = registerService;
            _context = context;
            _mapper = mapper;
            _environment = environment;
            _configuration = configuration;
            _paymentService= paymentService;
        }
        #region GetCountries
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var conList = await _registerService.GetCountries();
            return Ok(conList);
        }
        #endregion

        #region GetStateById
        /// <summary>
        /// Get List of States based on Country Id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetStateById(string countryId)
        {
            var stateList = await _registerService.GetStateById(countryId);
            return Ok(stateList);
        }
        #endregion

        #region GetCityById
        /// <summary>
        /// Get List of Cities through State Id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCityById(string stateId)
        {
            var cityList = await _registerService.GetCityById(stateId);
            return Ok(cityList);
        }
        #endregion

        #region AppExists
        /// <summary>
        /// Get List of Cities through State Id
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AppExists(string countryName, string stateName,string cityName)
        {
            var stateId = await _registerService.AppExists(countryName,stateName, cityName);
            return Ok(stateId);
        }
        #endregion

        #region RiderRegistration
        /// <summary>
        /// Register Rider
        /// </summary>
        /// <param name="userRegistration"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RiderRegistration([FromBody] UserMobileData userRegistration)
        {
            APIResponse<object> response = new APIResponse<object>();
            try
            {
                _logger.LogInformation("{0} InSide  RiderRegistration in RegistrationController Method --   Contact Number : {1}  Device: {2} ", DateTime.UtcNow, userRegistration.MobileNumber,  userRegistration.DeviceId);
                if (ModelState.IsValid)
                {
                    var userMobileExists = await _userManager.Users.Where(x => x.PhoneNumber == userRegistration.MobileNumber).FirstOrDefaultAsync();
					APIResponse<string> StripeCustomerId = new APIResponse<string>();

					if (userMobileExists is null)
                    {
						

                        var user = new ApplicationUser
                        {
                            Name = userRegistration.Name,
                            UserName = RandomString(10),
                            PhoneNumber = userRegistration.MobileNumber,
                            Email = !string.IsNullOrWhiteSpace(userRegistration.Email) ? userRegistration.Email : null,
                            PhoneNumberConfirmed = true,
                            NormalizedEmail = !string.IsNullOrWhiteSpace(userRegistration.Email) ? userRegistration.Email.ToUpper() : null,
                            DeviceId = userRegistration.DeviceId,
                            CreatedBy = string.Empty,
                            CreatedDate = DateTime.UtcNow,
                            StripeCustomerId = StripeCustomerId.Data,
							UpdatedBy = string.Empty,
                            UpdatedDate = null,
                            IsDeleted = false,
                            IsActive = true,
                            LocalTimeZone = userRegistration.LocalTimeZone
                    };

                        var result = await _userManager.CreateAsync(user, string.Concat("Rider@1", RandomString(10)));

                        if (result.Succeeded)
                        {


                          

                            var roleExists = await _roleManager.RoleExistsAsync(AuthorizationLevel.Roles.Customer);
                            if (roleExists)
                            {
                                await _userManager.AddToRoleAsync(user, AuthorizationLevel.Roles.Customer);
                            }
                           
                            _logger.LogInformation("{0} InSide  RiderRegistration in RegistrationController Method --   Contact Number : {1}  Email-Id : {2}  Device: {3}  Message {4}", DateTime.UtcNow, userRegistration.MobileNumber, userRegistration.Email, userRegistration.DeviceId, GlobalResourceFile.CustomerCreated);


                            var userData = await _userManager.Users.Where(x => x.PhoneNumber == userRegistration.MobileNumber).FirstAsync();
                            var userRoles = await _userManager.GetRolesAsync(user!);


                            DigitalWalletData walletData = new DigitalWalletData
                            {
                                Id = Guid.NewGuid(),
                                UserId = userData.Id,
                                BookingId = Guid.Empty,
                                //Balance = Math.Truncate((decimal)(record.ServiceFee * 0.05m)! * 100) / 100,
                                Balance = 0.0m,
                                IsApplied = false,
                                IsDeleted = false,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = userData.Id,
                                UpdatedDate = null!,
                                UpdatedBy = null!,
                                IsActive = true
                            };

                            var authClaims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.MobilePhone,userRegistration.MobileNumber!),
                                    new Claim(ClaimTypes.NameIdentifier,userData.Id!),
                                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                                };

                            foreach (var userRole in userRoles)
                            {
                                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                            }


                            var token = _tokenService.GetToken(authClaims);
                            var refreshToken = _tokenService.GetRefreshToken();
                            var tokenInfo = _tokenService.GetUserFromTokenInfo(user.UserName!);
                            APIResponse<TokenInfoUserDTO> tokenInfoResult = null!;
                            if (tokenInfo.Result.Data is null)
                            {
                                var info = new TokenInfoUserDTO { AccessToken = token.Result.Data!.TokenString, UserName = user.UserName, RefreshToken = refreshToken.Result.Data, RefreshTokenExpiry = DateTime.UtcNow.AddDays(7), IsDeleted = false, CreatedBy = user.Id, CreatedDate = DateTime.UtcNow, UpdatedBy = user.Id, UpdatedDate = DateTime.UtcNow };
                                tokenInfoResult = await _tokenService.AddUpdateToken(info);
                                tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
                                tokenInfoResult!.Data.DeviceId = user.DeviceId;
                            }
                            else
                            {
                                var principle = await _tokenService.GetPrincipleFromExpiredToken(token.Result!.Data!.TokenString!);
                                var newRefreshToken = refreshToken.Result!.Data!;
                                tokenInfoResult = await _tokenService.AddUpdateToken(new TokenInfoUserDTO
                                {
                                    Id = Guid.Parse(userData.Id),
                                    AccessToken = token.Result.Data.TokenString!,
                                    RefreshToken = newRefreshToken,
                                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(7),
                                    IsDeleted = false,
                                    UpdatedDate = DateTime.UtcNow,
                                    UpdatedBy = userData.Id,
                                    CreatedBy = userData.Id,
                                    CreatedDate = DateTime.UtcNow,
                                    UserName = tokenInfo.Result.Data.UserName
                                });
                                tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
                                tokenInfoResult!.Data.DeviceId = user.DeviceId;
                            }

                            _context.HttpContext!.Session.SetString(Configuration.Token, token!.Result!.Data!.TokenString!);
							if (!string.IsNullOrEmpty(userRegistration.Email) && !string.IsNullOrEmpty(userRegistration.Name))
							{
								StripeCustomerId = await _paymentService.CreateCustomer(userRegistration.Email, userRegistration.Name, userData.Id!);
							}
							_logger.LogInformation("{0} InSide RiderRegistration in RegistrationController Method Ended --   Contact Number : {1}  Email-Id : {2} DeviceId:{3} DriverCreationSuccess:{4}", DateTime.UtcNow, userRegistration.MobileNumber, userRegistration.Email, userRegistration.DeviceId, result.Succeeded);                           
							response.Error = null;
                            response.Success = true;
                            response.Message = GlobalResourceFile.CustomerCreated;
                            response.Status = HttpStatusCode.Created;
                            response.Data = new DriverRegisterResponseDTO { StripeCustId = StripeCustomerId.Data, AccessToken = token!.Result!.Data!.TokenString!, UserId = userData.Id, Response = GlobalResourceFile.CustomerCreated };
                            var registerCustomeEmail = new RegisterCustomerEmail
                            {
                                Port = Convert.ToInt32(_configuration["EmailConfiguration:Port"]!),
                                Username = userRegistration.Name,
                                MailTo = userData.Email,
                                MailFrom = _configuration["EmailConfiguration:AdminEmail"]!,
								MailFromAlias = _configuration["EmailConfiguration:Alias"]!,
								Password = _configuration["EmailConfiguration:Password"]!,
                                Host = _configuration["EmailConfiguration:Host"]!,
                                Subject = _configuration["EmailConfiguration:AdminEmailCustomerSubject"]!,
                                Status = GlobalConstants.GlobalValues.SendRegisterCustomerEmail
                            };
                            var emailResult=EmailUtility.SendRegisterCustomerEmail(registerCustomeEmail);
                            _logger.LogInformation("{0} InSide  RiderRegistration in RegistrationController Method --   Contact Number : {1}  Email-Id : {2}  Device: {3}  EmailResult {4} APIResponse{5}:", DateTime.UtcNow, userRegistration.MobileNumber, userRegistration.Email, userRegistration.DeviceId, emailResult,response.Status);
                            return Ok(response);
                        }

                        _logger.LogInformation("{0} InSide  RiderRegistration in RegistrationController Method --   Contact Number : {1}  Email-Id : {2}  Device: {3}  Error {4}", DateTime.UtcNow, userRegistration.MobileNumber, userRegistration.Email, userRegistration.DeviceId, result.Errors.First().Code);


                        response.Error = new CustomException(result.Errors.First().Code);
                        response.Success = false;
                        response.Message = result.Errors.First().Code;
                        response.Status = HttpStatusCode.Ambiguous;
                        response.Data = null;
                        return Ok(response);

                    }
                    response.Error = null;
                    response.Success = false;
                    response.Message = GlobalResourceFile.UserExists;
                    response.Status = HttpStatusCode.Conflict;
                    response.Data = null;
                    _logger.LogInformation("{0} InSide  CustomerMobileRegister in RegistrationController Method --   Contact Number : {1}  Email-Id : {2}  Device: {3}  Error {4}", DateTime.UtcNow, userRegistration.MobileNumber, userRegistration.Email, userRegistration.DeviceId, response.Message);
                    return Ok(response);

                }

                return BadRequest(new APIResponse<TokenInfoUserDTO>() { Data = null!, Message = LoginResource.UnauthorizedLogin, Status = System.Net.HttpStatusCode.BadRequest, Success = false, Error = new CustomException(LoginResource.UnauthorizedLogin) });

            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide  CustomerMobileRegister in RegistrationController Method --   Contact Number : {1}  Email-Id : {2}  Device: {3}  Error {4}", DateTime.UtcNow, userRegistration.MobileNumber, userRegistration.Email, userRegistration.DeviceId, ex.Message);
                throw;
            }
        }
        #endregion

        #region DriverRegister
        /// <summary>
        /// endpoint to DriverRegister
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> DriverRegister([FromForm] DriverRegisterDTO register)
        {
            var requestUri = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{_httpContext.Request.PathBase}";
            string SuperAdmin = string.Empty;
            APIResponse<DriverRegisterResponseDTO> response = new APIResponse<DriverRegisterResponseDTO>();
            _logger.LogInformation("{0} InSide DriverRegister in Controller Method Started--   Contact Number : {1}  Email-Id : {2} DeviceId:{3} ", DateTime.UtcNow, register.PhoneNumber, register.Email, register.DeviceId);
            SuperAdmin = "Temitope";

            string code = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var userExists = _userManager.Users.Where(x => x.Email.Equals(register.Email) || x.PhoneNumber.Equals(register.PhoneNumber)).FirstOrDefault();
                    if (userExists is null)
                    {
                        var user = new ApplicationUser
                        {
                            Name = register.FullName,
                            Email = register.Email,
                            UserName = register.Email,
                            PhoneNumber = register.PhoneNumber,
                            NormalizedEmail = register.Email.ToUpper(),
                            PhoneNumberConfirmed = true,
                            DOB = register.DOB,
                            IsDeleted = false,
                            IsActive = true,
                            IsDocumentVerified = false,
                            IsVerified = false,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = null,
                            CreatedBy = string.Empty,
                            UpdatedBy = string.Empty,
                            EmailConfirmed = false,
                            Platform = register.Platform,
                            DeviceId = register.DeviceId,
                            StatusId = Guid.Parse(GlobalConstants.GlobalValues.Pending),
                            BookingStatusId = Guid.Parse(GlobalConstants.GlobalValues.BookingStatus.UNASSIGNED),
                            LocalTimeZone = register.LocalTimeZone
                        };
                        var result = await _userManager.CreateAsync(user, register.Password);
                        if (result.Succeeded)
                        {
                            
                            var roleExists = await _roleManager.RoleExistsAsync(AuthorizationLevel.Roles.Driver);
                            if (roleExists)
                            {
                                await _userManager.AddToRoleAsync(user, AuthorizationLevel.Roles.Driver);
                            }
                            else
                            {
                                await _roleManager.CreateAsync(new IdentityRole { Name = AuthorizationLevel.Roles.Driver, NormalizedName = AuthorizationLevel.Roles.Driver.ToUpper() });
                                await _userManager.AddToRoleAsync(user, AuthorizationLevel.Roles.Driver);
                            }
                            var userData = await _userManager.FindByEmailAsync(register.Email);
                            var userRoles = await _userManager.GetRolesAsync(user!);
                            var authClaims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.MobilePhone,user!.PhoneNumber),
                                     new Claim(ClaimTypes.Name,user!.UserName),
                                    new Claim(ClaimTypes.Email,user!.Email),
                                    new Claim(ClaimTypes.NameIdentifier,userData.Id),
                                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                                };
                            foreach (var userRole in userRoles)
                            {
                                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                            }
                            var token = _tokenService.GetToken(authClaims);
                            _context.HttpContext!.Session.SetString(Configuration.Token, token!.Result!.Data!.TokenString!);
                            var refreshToken = _tokenService.GetRefreshToken();
                            var tokenInfo = _tokenService.GetUserFromTokenInfo(user.UserName);
                            APIResponse<TokenInfoUserDTO> tokenInfoResult = null!;
                            if (tokenInfo.Result.Data is null)
                            {
                                var info = new TokenInfoUserDTO { AccessToken = token.Result.Data!.TokenString, UserName = user.UserName, RefreshToken = refreshToken.Result.Data, RefreshTokenExpiry = DateTime.UtcNow.AddDays(7), IsDeleted = false, CreatedBy = user.Id, CreatedDate = DateTime.UtcNow, UpdatedBy = user.Id, UpdatedDate = DateTime.UtcNow };
                                tokenInfoResult = await _tokenService.AddUpdateToken(info);
                                tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
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
                                    UpdatedBy = userData.Id,
                                    CreatedBy = userData.Id,
                                    CreatedDate = DateTime.UtcNow,
                                    UserName = tokenInfo.Result.Data.UserName
                                });
                                tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
                            }
                            _context.HttpContext!.Session.SetString(Configuration.Token, token!.Result!.Data!.TokenString!);
                            _logger.LogInformation("{0} InSide DriverRegister in Controller Method Ended --   Contact Number : {1}  Email-Id : {2} DeviceId:{3} DriverCreationSuccess:{4}", DateTime.UtcNow, register.PhoneNumber, register.Email, register.DeviceId, result.Succeeded);
                            user.DeviceId = register.DeviceId;
                            user.UpdatedDate = DateTime.UtcNow;
                            user.IsLoggedIn = true;
                            user.UpdatedBy = user.Id;
                            await _userManager.UpdateAsync(user);
                            response.Error = null;
                            response.Success = true;
                            response.Message = GlobalResourceFile.DriverProfileCreated;
                            response.Status = HttpStatusCode.Created;
                            response.Data = new DriverRegisterResponseDTO { AccessToken = token!.Result!.Data!.TokenString!, UserId = userData.Id, Response = GlobalResourceFile.DriverProfileCreated };
                            var emailToAdmin = new EmailAdmin
                            {
                                Port = Convert.ToInt32(_configuration["EmailConfiguration:Port"]!),
                                EmailFrom = _configuration["EmailConfiguration:AdminEmail"]!,
								MailFromAlias = _configuration["EmailConfiguration:Alias"]!,
								EmailTo = _configuration["EmailConfiguration:AdminEmail"]!,
                                Password = _configuration["EmailConfiguration:Password"]!,
                                Subject = _configuration["EmailConfiguration:AdminEmailSubject"]!,
                                RequestUri = requestUri,
                                Host = _configuration["EmailConfiguration:Host"]!,
                                UserName = userData.Name,
                                UserEmail = userData.Email,
                                UserContact = userData.PhoneNumber
                            };
                            var emailToDriver = new EmailDriver
                            {
                                Port = Convert.ToInt32(_configuration["EmailConfiguration:Port"]!),
                                RequestUri = requestUri,
                                DriverName = userData.Name,
                                MailTo = userData.Email,
                                MailFrom = _configuration["EmailConfiguration:AdminEmail"]!,
								MailFromAlias = _configuration["EmailConfiguration:Alias"]!,
								Password = _configuration["EmailConfiguration:Password"]!,
                                DocStatus = userData.StatusId,
                                Reason = userData.Comment,
                                Host = _configuration["EmailConfiguration:Host"]!,
                                Subject = _configuration["EmailConfiguration:AdminEmailSubject"]!
                            };
                            EmailUtility.SendEmail(emailToDriver);
                            EmailUtility.SendEmailToAdmin(emailToAdmin);
                            return Ok(response);
                        }
                        else
                        {
                            response.Error = new CustomException(GlobalResourceFile.DriverProfileCreationFailed);
                            response.Success = false;
                            response.Message = GlobalResourceFile.DriverProfileCreationFailed;
                            response.Status = HttpStatusCode.BadRequest;
                            response.Data = new DriverRegisterResponseDTO { AccessToken = string.Empty!, UserId = string.Empty, Response = GlobalResourceFile.DriverProfileCreationFailed };
                            _logger.LogError("{0} InSide  DriverRegister in Controller Method --   Contact Number : {1}  Email-Id : {2}  Error {3}", DateTime.UtcNow, register.PhoneNumber, register.Email, response.Error);
                            return BadRequest(response);
                        }
                    }
                    else
                    {
                        response.Error = new CustomException(GlobalResourceFile.UserExists);
                        response.Success = false;
                        response.Message = GlobalResourceFile.UserExists;
                        response.Status = HttpStatusCode.OK;
                        response.Data = null;
                        _logger.LogError("{0} InSide  DriverRegister in Controller Method --   Contact Number : {1}  Email-Id : {2}  Error {3}", DateTime.UtcNow, register.PhoneNumber, register.Email, response.Error);
                        return Ok(response);
                    }
                }
                else
                {
                    response.Error = new CustomException(GlobalResourceFile.ModelStateInvalid);
                    response.Success = false;
                    response.Message = GlobalResourceFile.ModelStateInvalid;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Data = null;
                    _logger.LogError("{0} InSide  DriverRegister in Controller Method --   Contact Number : {1}  Email-Id : {2}  Error {3}", DateTime.UtcNow, register.PhoneNumber, register.Email, response.Error);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide  DriverRegister in Controller Method --   Contact Number : {1}  Email-Id : {2}  Error {3}", DateTime.UtcNow, register.PhoneNumber, register.Email, ex.Message);
                throw;
            }
        }
        #endregion

        #region GenerateOTP
        /// <summary>
        /// endpoint to GenerateOTP
        /// </summary>
        /// <param name="codeGenerateDTO"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> GenerateOTP(CodeGenerateDTO codeGenerateDTO)
        {
            APIResponse<string> code = null!;
            code = await _tokenService.GetVerificationCode(codeGenerateDTO);
            return Ok(code);
        }
        #endregion

        #region VerifyOTP
        /// <summary>
        /// endpoint to VerifyOTP
        /// </summary>
        /// <param name="riderCodeVerifyDTO"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> VerifyOTP(RiderCodeVerifyDTO riderCodeVerifyDTO)
        {

            APIResponse<bool> isVerifiedCode = null!;
            isVerifiedCode = await _tokenService.VerifyCode(riderCodeVerifyDTO);

            return Ok(isVerifiedCode);
        }
        #endregion

        #region VerifyOTP
        /// <summary>
        /// endpoint to VerifyOTP
        /// </summary>
        /// <param name="riderCodeVerifyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RiderVerifyOTP(RiderCodeVerifyDTO riderCodeVerifyDTO)
        {

            APIResponse<bool> isVerifiedCode = null!;
			APIResponse<string> StripeCustomerId = new APIResponse<string>();
			isVerifiedCode = await _tokenService.VerifyCode(riderCodeVerifyDTO);
            _logger.LogInformation("{0} InSide RiderVerifyOTP Method of RegistrationController Started--   Contact Number : {1} Hashcode : {2} OTP:{3} CountryCode:{4} DeviceId:{5} started", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber, riderCodeVerifyDTO.HashCode, riderCodeVerifyDTO.Otp, riderCodeVerifyDTO.CountryCode, riderCodeVerifyDTO.DeviceId);
            if (isVerifiedCode.Data)
            {
                var user = await _userManager.Users.Where(x => x.PhoneNumber == riderCodeVerifyDTO.MobileNumber).FirstOrDefaultAsync();
                if (user is not null && !user.IsDeleted && user.IsActive)
                {
                    user.DeviceId = riderCodeVerifyDTO.DeviceId;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.Id;
                    await _userManager.UpdateAsync(user);
                    var userRoles = await _userManager.GetRolesAsync(user!);

                    if (userRoles[0] != AuthorizationLevel.Roles.Customer)
                    {
                        _logger.LogInformation("{0} InSide RiderVerifyOTP Method of RegistrationController --   Contact Number : {1} Hashcode : {2} OTP:{3} CountryCode:{4} DeviceId:{5} Message:{6}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber, riderCodeVerifyDTO.HashCode, riderCodeVerifyDTO.Otp, riderCodeVerifyDTO.CountryCode, riderCodeVerifyDTO.DeviceId, "Rider is not exists");
                        return Ok(new APIResponse<object>() { Data = null, Message = "Rider is not exists", Status = System.Net.HttpStatusCode.OK, Success = true, Error = null! });
                    }                       

                    var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user!.UserName),
                            new Claim(ClaimTypes.MobilePhone,user!.PhoneNumber),
                            new Claim(ClaimTypes.NameIdentifier,user!.Id),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                        };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = _tokenService.GetToken(authClaims);
                    _context.HttpContext!.Session.SetString(Configuration.Token, token!.Result!.Data!.TokenString!);
                    var refreshToken = _tokenService.GetRefreshToken();
                    var tokenInfo = _tokenService.GetUserFromTokenInfo(user.UserName);
                    APIResponse<TokenInfoUserDTO> tokenInfoResult = null!;
                    if (tokenInfo.Result.Data is null)
                    {
                        var info = new TokenInfoUserDTO { AccessToken = token.Result.Data!.TokenString, UserName = user.UserName, RefreshToken = refreshToken.Result.Data, RefreshTokenExpiry = DateTime.UtcNow.AddDays(7), IsDeleted = false, CreatedBy = user.Id, CreatedDate = DateTime.UtcNow, UpdatedBy = user.Id, UpdatedDate = DateTime.UtcNow };
                        tokenInfoResult = await _tokenService.AddUpdateToken(info);
                        tokenInfoResult!.Data!.DeviceId = user.DeviceId;
                        tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
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
                            CreatedBy = user.Id,
                            CreatedDate = DateTime.UtcNow,
                            UserName = user.Name

                        });
                       
                        tokenInfoResult!.Data!.AccessToken = token.Result.Data!.TokenString!;
                        tokenInfoResult!.Data!.DeviceId = user.DeviceId;
                        tokenInfoResult!.Data!.Id = Guid.Parse(user.Id);

						if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Name))
						{
                            if(string.IsNullOrEmpty(user.StripeCustomerId))
                            {
								StripeCustomerId = await _paymentService.CreateCustomer(user.Email, user.Name, user.Id!);
								tokenInfoResult!.Data!.StripeCustId = StripeCustomerId.Data!;
							}
                            else
                            {
								tokenInfoResult!.Data!.StripeCustId = user.StripeCustomerId;
							}
						}
					}
                    _logger.LogInformation("{0} InSide RiderVerifyOTP Method of RegistrationController --   Contact Number : {1} Hashcode : {2} OTP:{3} CountryCode:{4} DeviceId:{5} Message:{6}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber, riderCodeVerifyDTO.HashCode, riderCodeVerifyDTO.Otp, riderCodeVerifyDTO.CountryCode, riderCodeVerifyDTO.DeviceId, LoginResource.LoginSuccess);
                    return Ok(new APIResponse<TokenInfoUserDTO>() { Data = tokenInfoResult!.Data!, Message = LoginResource.LoginSuccess, Status = System.Net.HttpStatusCode.OK, Success = true, Error = null! });
                }
                else
                {
                    _logger.LogInformation("{0} InSide RiderVerifyOTP Method of RegistrationController --   Contact Number : {1} Hashcode : {2} OTP:{3} CountryCode:{4} DeviceId:{5} Message:{6}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber, riderCodeVerifyDTO.HashCode, riderCodeVerifyDTO.Otp, riderCodeVerifyDTO.CountryCode, riderCodeVerifyDTO.DeviceId, "Rider is not exists");
                    return Ok(new APIResponse<object>() { Data = null, Message = "Rider is not exists", Status = System.Net.HttpStatusCode.OK, Success = true, Error = null! });
                }
            }


            return Ok(isVerifiedCode);
        }

        #endregion

        #region RegisterVehicleDetails
        /// <summary>
        /// endpoint to RegisterVehicleDetails
        /// </summary>
        /// <param name="vehicleDetail"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> RegisterVehicleDetails([FromBody] VehicleDetailDTO vehicleDetail)
        {
            APIResponse<int> data = null!;
            if (ModelState.IsValid)
            {
                data = await _registerService.RegisterVehicleDetails(_mapper.Map<VehicleDetail>(vehicleDetail));
                return Ok(data);

            }
            return BadRequest(data);
        }
        #endregion

        #region UpdateVehicleDetails
        /// <summary>
        /// endpoint to UpdateVehicleDetails
        /// </summary>
        /// <param name="vehicleDetail"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> UpdateVehicleDetails([FromBody] VehicleDetailDTO vehicleDetail)
        {
            APIResponse<int> data = null!;
            if (ModelState.IsValid)
            {
                data = await _registerService.UpdateVehicleDetails(_mapper.Map<VehicleDetail>(vehicleDetail));
                return Ok(data);

            }
            return BadRequest(ModelState);
        }
        #endregion

        #region GetVehicleDetailsById
        /// <summary>
        /// endpoint to GetVehicleDetailsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetVehicleDetailsById(Guid id)
        {
            APIResponse<VehicleDetailDTO> data = null!;
            if (id != Guid.Empty)
            {
                data = await _registerService.GetVehicleDetailsById(id);
                return Ok(data);

            }
            return BadRequest(data);
        }
        #endregion

        #region DeleteVehicleDetails
        /// <summary>
        /// endpoint to DeleteVehicleDetails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteVehicleDetails(Guid id)
        {
            APIResponse<int> data = null!;
            if (id != Guid.Empty)
            {
                data = await _registerService.DeleteVehicleDetails(id);
                return Ok(data);

            }
            return BadRequest(data);
        }
        #endregion

        #region UpdateAddress
        /// <summary>
        /// endpoint to UpdateAddress
        /// </summary>
        /// <param name="generalAddress"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress([FromBody] GeneralAddressDTO generalAddress)
        {
            APIResponse<int> data = null!;
            if (ModelState.IsValid)
            {
                data = await _registerService.UpdateAddress(_mapper.Map<GeneralAddress>(generalAddress));
                return Ok(data);

            }
            return BadRequest(ModelState);
        }
        #endregion

        #region RegisterAddress
        /// <summary>
        /// endpoint to RegisterAddress
        /// </summary>
        /// <param name="generalAddress"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> RegisterAddress([FromBody] GeneralAddressDTO generalAddress)
        {
            APIResponse<int> data = null!;
            if (ModelState.IsValid)
            {
                data = await _registerService.RegisterAddress(_mapper.Map<GeneralAddress>(generalAddress));
                return Ok(data);

            }
            return BadRequest(ModelState);
        }
        #endregion

        #region GetAddressById
        /// <summary>
        /// endpoint to GetAddressById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            APIResponse<GeneralAddressDTO> data = null!;
            if (id != Guid.Empty)
            {
                data = await _registerService.GetAddressById(id);
                return Ok(data);

            }
            return BadRequest(data);
        }
        #endregion

        #region DeleteAddress
        /// <summary>
        /// endpoint to DeleteAddress
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            APIResponse<int> data = null!;
            if (id != Guid.Empty)
            {
                data = await _registerService.DeleteVehicleDetails(id);
                return Ok(data);

            }
            return BadRequest(data);
        }
        #endregion

        #region AddDriverMasterDetails
        /// <summary>
        /// AddDriverMasterDetails to driver profile
        /// </summary>
        /// <param name="masterRegister"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> AddDriverMasterDetails([FromForm] MasterRegister masterRegister)
        {
            if (ModelState.IsValid)
            {
                var result = await _registerService.AddDriverMasterDetails(masterRegister);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region GetDriverMasterDetails
        /// <summary>
        /// API to get driver details by driver
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpGet]
        public async Task<IActionResult> GetDriverMasterDetails()
        {
            var data = await _registerService.GetDriverMasterDetails();
            return Ok(data);
        }
        #endregion

        #region GetAllDriversBasicDetails
        /// <summary>
        /// API to get All driver basic details
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAllDriversBasicDetails()
        {
            var data = await _registerService.GetAllDriversBasicDetails();
            return Ok(data);
        }
        #endregion

        #region GetDriverMasterDetailsById
        /// <summary>
        ///     API to get driver details by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> GetDriverMasterDetailsById([FromBody] string userId)
        {
            var data = await _registerService.GetDriverMasterDetailsById(userId);
            return Ok(data);
        }
        #endregion

        #region SetUserApprovalStatus
        /// <summary>
        ///     API to set user approval status
        /// </summary>
        /// <param name="approvalStatus"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> SetUserApprovalStatus([FromBody] UserApprovalStatus approvalStatus)
        {
            if (ModelState.IsValid)
            {
                var data = await _registerService.SetUserApprovalStatus(approvalStatus);
                return Ok(data);
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region DeleteUser
        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = AuthorizationLevel.Roles.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] string id)
        {
            APIResponse<int> data = null!;
            if (!string.IsNullOrEmpty(id))
            {
                data = await _registerService.DeleteUser(id);
                return Ok(data);

            }
            return BadRequest(data);
        }
        #endregion

        [NonAction]
        // Generates a random string with a given size.
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

		#region AddOrUpdateDriverLocationInfo
		/// <summary>
		/// Method to update driver location
		/// </summary>
		/// <param name="locationData"></param>
		/// <returns></returns>
		[Authorize(Roles = AuthorizationLevel.Roles.Driver)]
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateDriverLocationInfo([FromBody] DriverLocationUpdateModel locationData)
        {
            if (ModelState.IsValid)
            {
                var data = await _registerService.AddOrUpdateDriverLocationInfo(locationData);
                return Ok(data);
            }
            return BadRequest(ModelState);
        }
		#endregion

		#region UpdateUserProfile
		/// <summary>
		/// Api method to UpdateUserProfile
		/// </summary>
		/// <param name="userProfile"></param>
		/// <returns></returns>
		[Authorize(Policy = AuthorizationLevel.Roles.DriverCustomer)]
		[HttpPost]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileModel userProfile)
        {
            if(ModelState.IsValid)
            {               
                var result = await _registerService.UpdateUserProfile(userProfile);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region DeleteUserAccount
        /// <summary>
        /// Api method to DeleteUserAccount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = AuthorizationLevel.Roles.DriverCustomerSuperAdmin)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAccount(string id)
        {
            APIResponse<string> result = null!;
            if (!string.IsNullOrEmpty(id))
            {
                result = await _registerService.DeleteUserAccount(id);
                return Ok(result);
            }
            return BadRequest(result);
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// method to ForgotPassword
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //[Authorize(Policy = AuthorizationLevel.Roles.DriverSuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            APIResponse<string> code = null!;
            code = await _tokenService.GetVerificationCodeForForgotPassword(email);
            return Ok(code);
        }
        #endregion

        #region ForgotPasswordVerifyCode
        /// <summary>
        /// Api for ForgotPasswordVerifyCode
        /// </summary>
        /// <param name="forgotPasswordVerify"></param>
        /// <returns></returns>
        //[Authorize(Policy = AuthorizationLevel.Roles.DriverSuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordVerifyCode(ForgotPasswordVerifyCodeDTO forgotPasswordVerify)
        {

            APIResponse<bool> isVerifiedCode = null!;
            isVerifiedCode = await _tokenService.ForgotPasswordVerifyCode(forgotPasswordVerify);
            return Ok(isVerifiedCode);
        }
        #endregion

        //[Authorize(Policy = AuthorizationLevel.Roles.DriverSuperAdmin)]
        #region SetNewPassword
        [HttpPost]
        public async Task<IActionResult> SetNewPassword(string password,string email)
        {
            APIResponse<string> result = null!;
            if(string.IsNullOrEmpty(password))
            {
                return BadRequest(GlobalResourceFile.NullValve);
            }
            result = await _tokenService.SetNewPassword(password,email);
            return Ok(result);
        }
        #endregion
    }
}
