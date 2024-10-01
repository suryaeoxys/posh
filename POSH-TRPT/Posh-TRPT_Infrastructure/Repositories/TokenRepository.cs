using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OtpNet;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.Token;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.ConstantStrings;
using Posh_TRPT_Utility.EmailUtils;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class TokenRepository : Repository<TokenInfoUser>, ITokenRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenRepository> _logger;
        public string accountSid = string.Empty;
        private readonly UserManager<ApplicationUser> _userManager;
        private string authToken = string.Empty;
        public TokenRepository(UserManager<ApplicationUser> userManager,ILogger<TokenRepository> logger, DbFactory dbFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(dbFactory)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            this.accountSid = _configuration["TwillioConfiguration:accountSid"]!;
            this.authToken = _configuration["TwillioConfiguration:authToken"]!;
            _logger = logger;

        }

		#region GetPrincipleFromExpiredToken
		/// <summary>
		/// method to GetPrincipleFromExpiredToken
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="SecurityTokenException"></exception>
		public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException(JwtConfiguration.InvalidToken);
            return principle;

        }
		#endregion

		#region GetRefreshToken
		/// <summary>
		/// method to GetRefreshToken
		/// </summary>
		/// <returns></returns>
		public string GetRefreshToken()
        {
            var randomString = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomString);
                return Convert.ToBase64String(randomString);
            }
        }

		#endregion

		#region GetToken
		/// <summary>
		/// method to GetToken
		/// </summary>
		/// <param name="claim"></param>
		/// <returns></returns>
		public TokenResponse GetToken(IEnumerable<Claim> claim)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddDays(7),
                claims: claim,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenResponse { TokenString = tokenString, ValidTo = token.ValidTo };
        }
		#endregion


		#region GetUserFromTokenInfo
		/// <summary>
		/// method to GetUserFromTokenInfo
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public TokenInfoUser? GetUserFromTokenInfo(string username)
        {
            var userTokenInfo = Get(FilterByByUsername(username));
            if (userTokenInfo is null)
            {
                return null;
            }
            return new TokenInfoUser
            {
                Id = userTokenInfo!.Id,
                UserName = userTokenInfo.UserName,
                RefreshToken = userTokenInfo.RefreshToken,
                RefreshTokenExpiry = userTokenInfo.RefreshTokenExpiry,
                IsDeleted = userTokenInfo.IsDeleted
            ,
                CreatedDate = userTokenInfo.CreatedDate,
                UpdatedDate = userTokenInfo.UpdatedDate,
                CreatedBy = userTokenInfo.CreatedBy
            ,
                UpdatedBy = userTokenInfo.UpdatedBy
            };
        }
		#endregion

		#region SaveNewRefreshToken

		/// <summary>
		/// method to SaveNewRefreshToken
		/// </summary>
		/// <param name="tokenInfoUser"></param>
		/// <returns></returns>
		public TokenInfoUser SaveNewRefreshToken(TokenInfoUser? tokenInfoUser)
        {
            try
            {
                var data = Get(FilterByById(tokenInfoUser!.Id));
                if (data is not null)
                {
                    data.RefreshToken = tokenInfoUser.RefreshToken;
                    data.RefreshTokenExpiry = tokenInfoUser.RefreshTokenExpiry;
                    data.UserName = tokenInfoUser.UserName;
                    data.UpdatedBy = tokenInfoUser.UpdatedBy;
                    data.CreatedBy = tokenInfoUser.CreatedBy;
                    data.UpdatedDate = tokenInfoUser.UpdatedDate;
                    data.CreatedDate = tokenInfoUser.CreatedDate;
                    data.IsDeleted = tokenInfoUser.IsDeleted;
                    this.Update(data);
                    return data;
                }
                else
                {
                    var record = new TokenInfoUser
                    {
                        Id = Guid.NewGuid(),
                        RefreshToken = tokenInfoUser.RefreshToken,
                        RefreshTokenExpiry = tokenInfoUser.RefreshTokenExpiry,
                        UserName = tokenInfoUser.UserName,
                        CreatedBy = tokenInfoUser.CreatedBy,
                        CreatedDate = tokenInfoUser.CreatedDate,
                        UpdatedBy = tokenInfoUser.UpdatedBy,
                        UpdatedDate = tokenInfoUser.UpdatedDate,
                        IsDeleted = false

                    };
                    this.Add(record);
                    return record;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
		#endregion

		#region GenerateVerificationCode
		/// <summary>
		/// method to GenerateVerificationCode
		/// </summary>
		/// <param name="mobileNumer"></param>
		/// <param name="emailId"></param>
		/// <returns></returns>
		public string GenerateVerificationCode(string mobileNumer, string emailId)
        {
            DateTime expireTime = DateTime.UtcNow.AddSeconds(5 * 60);
            Random generator = new Random();

            String resultNumber = generator.Next(0, 1000000).ToString("D6");
            if (resultNumber.Distinct().Count() == 1)
            {
                resultNumber = GenerateVerificationCode(mobileNumer, emailId);
            }

            try
            {
                _logger.LogInformation("{0} InSide GenerateVerificationCode in TokenRepository Method Started--   Contact Number : {1}  Email-Id : {2} ", DateTime.UtcNow, mobileNumer, emailId);

                Parallel.Invoke(
                    () => SendEmailOTP(emailId, resultNumber), () => SendMobileOTP(mobileNumer, resultNumber)
                );


            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide GenerateVerificationCode in TokenRepository Method --   Contact Number : {1}  Email-Id : {2}  Error {3} ErrorInnerException{4} Code:{5}", DateTime.UtcNow, mobileNumer, emailId, ex.Message, ex.InnerException, resultNumber);

            }

            var data = string.Concat(mobileNumer, ".", resultNumber, ".", expireTime);

            return string.Concat(Encrypt(data), ".", resultNumber, ".", expireTime);

        }
		#endregion

		#region GetVerificationCodeForForgotPassword
		/// <summary>
		/// method to GetVerificationCodeForForgotPassword
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public async Task<string> GetVerificationCodeForForgotPassword(string email)
		{
            var isExists=await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (isExists != null)
            {
                DateTime expireTime = DateTime.UtcNow.AddSeconds(5 * 60);
                Random generator = new Random();

                String resultNumber = generator.Next(0, 1000000).ToString("D6");
                if (resultNumber.Distinct().Count() == 1)
                {
                    resultNumber = await GetVerificationCodeForForgotPassword(email);
                }

                try
                {
                    _logger.LogInformation("{0} InSide GetVerificationCodeForForgotPassword in TokenRepository Method Started--   Email : {1} ", DateTime.UtcNow,
                        email);
                    var IsInRole = await _userManager.IsInRoleAsync(isExists, AuthorizationLevel.Roles.SuperAdmin);
                    if (IsInRole)
                    {
                        Parallel.Invoke(
                       () => SendEmailOTP2(email, resultNumber));
                    }
                    else
                    {
                        Parallel.Invoke(
                            () => SendEmailOTP1(email, resultNumber));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("{0} InSide GetVerificationCodeForForgotPassword in TokenRepository Method --   Email-Id : {1}  Error {2} ErrorInnerException{3} Code:{4}", DateTime.UtcNow, email, ex.Message, ex.InnerException, resultNumber);

                }

                var data = string.Concat(email, ".", resultNumber, ".", expireTime);

                return string.Concat(Encrypt(data), ".", resultNumber, ".", expireTime);
            }
            return LoginResource.UserNotExists;

		}
		#endregion

		#region SendMobileOTP

		/// <summary>
		/// method to SendMobileOTP
		/// </summary>
		/// <param name="mobileNumer"></param>
		/// <param name="OTP"></param>
		private void SendMobileOTP(String mobileNumer, string OTP)
        {
            TwilioClient.Init(accountSid, authToken);
            try
            {
                var message = MessageResource.Create(
                                        body: (string.Format(_configuration["TwillioConfiguration:messageBody"],OTP)),
                                        from: new Twilio.Types.PhoneNumber(_configuration["TwillioConfiguration:twilioPhoneNumber"]),
                                        to: new Twilio.Types.PhoneNumber(mobileNumer)
                                        );
                _logger.LogInformation("{0} InSide SendMobileOTP in TokenRepository Method Ended--   Contact Number : {1}   Message: {2}", DateTime.UtcNow, mobileNumer, message.Sid);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide SendMobileOTP in TokenRepository Method --   Contact Number : {1}   Error {2} ErrorInnerException{3} Code:{4}", DateTime.UtcNow, mobileNumer, ex.Message, ex.InnerException, OTP);


            }


        }
		#endregion

		#region SendEmailOTPForCutomerRegistration
		/// <summary>
		/// method to SendEmailOTP
		/// </summary>
		/// <param name="emailId"></param>
		/// <param name="OTP"></param>
		private void SendEmailOTP(String emailId, string OTP)
        {
            var sendOtp = new OTPtoEmail
            {
                Status = GlobalConstants.GlobalValues.SendOTP,
                Port = Convert.ToInt32(_configuration["EmailConfiguration:Port"]!),
                MailFrom = _configuration["EmailConfiguration:AdminEmail"]!,
				MailFromAlias = _configuration["EmailConfiguration:Alias"]!,
				Password = _configuration["EmailConfiguration:Password"]!,
                Subject = _configuration["EmailConfiguration:AdminEmailCustomerSubject"]!,
                OTP = OTP,
                Host = _configuration["EmailConfiguration:Host"]!,
                MailTo = emailId
            };

            var Message = EmailUtility.SendRegisterCustomerOTPEmail(sendOtp);


            _logger.LogInformation("{0} InSide SendEmailOTP in TokenRepository Method Ended--    Email-Id : {1}  Message: {2}", DateTime.UtcNow, emailId, Message);
        }
        #endregion

        #region SendEmailOTPForNewDriverRegistration
        private void SendEmailOTP1(String emailId, string OTP)
        {
            var sendOtp = new OTPtoEmail
            {
                Status = GlobalConstants.GlobalValues.SendOTP,
                Port = Convert.ToInt32(_configuration["EmailConfiguration:Port"]!),
                MailFrom = _configuration["EmailConfiguration:AdminEmail"]!,
                MailFromAlias = _configuration["EmailConfiguration:Alias"]!,
                Password = _configuration["EmailConfiguration:Password"]!,
                Subject = _configuration["EmailConfiguration:AdminEmailSubject"]!,
                OTP = OTP,
                Host = _configuration["EmailConfiguration:Host"]!,
                MailTo = emailId
            };

            var Message = EmailUtility.SendVerificationEmailForgotPassword(sendOtp);


            _logger.LogInformation("{0} InSide SendEmailOTP in TokenRepository Method Ended--    Email-Id : {1}  Message: {2}", DateTime.UtcNow, emailId, Message);
        }
        #endregion


        #region SendEmailOTPToAdminForForgetPassord
        private void SendEmailOTP2(String emailId, string OTP)
        {
            var sendOtp = new OTPtoEmail
            {
                Status = GlobalConstants.GlobalValues.SendOTP,
                Port = Convert.ToInt32(_configuration["EmailConfiguration:Port"]!),
                MailFrom = _configuration["EmailConfiguration:AdminEmail"]!,
                MailFromAlias = _configuration["EmailConfiguration:Alias"]!,
                Password = _configuration["EmailConfiguration:Password"]!,
                Subject = _configuration["EmailConfiguration:AdminEmailSubjectForForgotPassword"]!,
                OTP = OTP,
                Host = _configuration["EmailConfiguration:Host"]!,
                MailTo = emailId
            };

            var Message = EmailUtility.SendVerificationEmailForgotPassword(sendOtp);


            _logger.LogInformation("{0} InSide SendEmailOTP in TokenRepository Method Ended--    Email-Id : {1}  Message: {2}", DateTime.UtcNow, emailId, Message);
        }
        #endregion

        Expression<Func<TokenInfoUser, bool>> FilterByIsDeleted()
        {
            return x => x.IsDeleted == false;
        }

		#region VerificationGeneratedCode

		/// <summary>
		/// method to VerificationGeneratedCode
		/// </summary>
		/// <param name="mobileNumber"></param>
		/// <param name="hashcode"></param>
		/// <param name="otp"></param>
		/// <returns></returns>
		public bool VerificationGeneratedCode(string mobileNumber, string hashcode, string otp)
        {
            var securecode = hashcode.Split(".")[0];
            var expireTime = hashcode.Split(".")[2];

            var currentTime = DateTime.UtcNow;

            if (Convert.ToDateTime(currentTime) > Convert.ToDateTime(expireTime))
            {
                return false;
            }

            var time = 5 * 60;

            DateTime expireTime1 = DateTime.UtcNow.AddSeconds(time);

            var data = string.Concat(mobileNumber, ".", otp, ".", expireTime);
            string result = string.Concat(Encrypt(data), ".", otp, ".", expireTime);
            if (result == hashcode)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ForgotPasswordVerificationGeneratedCode
        public bool ForgotPasswordVerificationGeneratedCode(string email, string hashcode, string otp)
        {
            var securecode = hashcode.Split(".")[0];
            var expireTime = hashcode.Split(".")[2];

            var currentTime = DateTime.UtcNow;

            if (Convert.ToDateTime(currentTime) > Convert.ToDateTime(expireTime))
            {
                return false;
            }

            var time = 5 * 60;

            DateTime expireTime1 = DateTime.UtcNow.AddSeconds(time);

            var data = string.Concat(email, ".", otp, ".", expireTime);
            string result = string.Concat(Encrypt(data), ".", otp, ".", expireTime);
            if (result == hashcode)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Encrypt
        /// <summary>
        /// method to Encrypt the Mobile number
        /// </summary>
        /// <param name="mobilerNumber"></param>
        /// <returns></returns>
        protected string Encrypt(string mobilerNumber)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(mobilerNumber));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        } 
        #endregion

        Expression<Func<TokenInfoUser, bool>> FilterByByUsername(string username)
        {
            return x => x.UserName == username;
        }
        Expression<Func<TokenInfoUser, bool>> FilterByById(Guid? Id)
        {
            return x => x.Id == Id;
        }

        public async Task<string> SetNewPassword(string password, string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if(user!=null)
            {
                PasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
                string passwordData = _passwordHasher.HashPassword(user, password);
                user.PasswordHash = passwordData;
                await _userManager.UpdateAsync(user);
                return passwordData;
            }
            return LoginResource.SomthingWentWrong;
        }

    }
}
