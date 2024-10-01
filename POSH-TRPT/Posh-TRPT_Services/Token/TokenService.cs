using AutoMapper;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Token;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posh_TRPT_Models.DTO.TokenDTO;
using Posh_TRPT_Utility.Resources;
using System.Net;
using System.Security.Claims;
using OtpNet;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Models.DTO.RegisterDTO;

namespace Posh_TRPT_Services.Token
{
    public class TokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenRepository _tokenRepository;
        public readonly IMapper _mapper;
		private readonly ILogger<TokenService> _logger;
		public TokenService(IUnitOfWork unitOfWork
            , ITokenRepository tokenRepository
            , IMapper mapper,
			ILogger<TokenService> logger)
        {
            _unitOfWork = unitOfWork;
            _tokenRepository = tokenRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<APIResponse<TokenInfoUserDTO>> AddUpdateToken(TokenInfoUserDTO tokenInfo)
        {
            APIResponse<TokenInfoUserDTO> _APIResponse = new APIResponse<TokenInfoUserDTO>();
            try
            {
                var tokenData = _mapper.Map<TokenInfoUser>(tokenInfo);
                var data=_tokenRepository.SaveNewRefreshToken(tokenData);
                if (await _unitOfWork.CommitAsync() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<TokenInfoUserDTO>(data);
                    _APIResponse.Message = LoginResource.LoginSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = LoginResource.LoginFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = LoginResource.LoginFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
        public async Task<APIResponse<ClaimsPrincipal>> GetPrincipleFromExpiredToken(string token)
        {
            APIResponse<ClaimsPrincipal> _APIResponse = new APIResponse<ClaimsPrincipal>();
            try
            {
                var data=_tokenRepository.GetPrincipleFromExpiredToken(token);
                if (await _unitOfWork.CommitAsync() > 0||data.Claims.Count()>0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = RefreshTokenResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.AddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
        public async Task<APIResponse<string>> GetRefreshToken()
        {
            APIResponse<string> _APIResponse = new APIResponse<string>();
            try
            {
                var data = _tokenRepository.GetRefreshToken();
                if (await _unitOfWork.CommitAsync() > 0||(!string.IsNullOrEmpty(data)))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = RefreshTokenResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.AddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }
        public async Task<APIResponse<TokenResponse>> GetToken(IEnumerable<Claim> claim)
        {
            APIResponse<TokenResponse> _APIResponse = new APIResponse<TokenResponse>();
            try
            {
                var data = _tokenRepository.GetToken(claim);
                if (await _unitOfWork.CommitAsync() > 0||!string.IsNullOrEmpty(data.TokenString))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = RefreshTokenResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.AddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }

		#region GetUserFromTokenInfo
		public async Task<APIResponse<TokenInfoUser>> GetUserFromTokenInfo(string username)
        {
            APIResponse<TokenInfoUser> _APIResponse = new APIResponse<TokenInfoUser>();
            try
            {
                var data = _tokenRepository.GetUserFromTokenInfo(username);
                if (await _unitOfWork.CommitAsync() > 0 || !(data is null))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data!;
                    _APIResponse.Message = RefreshTokenResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.AddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        } 
        #endregion

        #region GetVerificationCode
        public async Task<APIResponse<string>> GetVerificationCode(CodeGenerateDTO codeGenerateDTO)
        {
            APIResponse<string> _APIResponse = new APIResponse<string>();
            try
            {
                _logger.LogInformation("{0} InSide GenerateOTP Method of TokenService Started--   Contact Number : {1}  Email-Id : {2} ", DateTime.UtcNow, codeGenerateDTO.MobileNumber, codeGenerateDTO.EmailId);

                var data = _tokenRepository.GenerateVerificationCode(string.Concat(codeGenerateDTO.CountryCode, codeGenerateDTO.MobileNumber), codeGenerateDTO.EmailId!);

                _logger.LogInformation("{0} InSide GenerateOTP Method of TokenService End--   Contact Number : {1}  Email-Id : {2}  Code {3}", DateTime.UtcNow, codeGenerateDTO.MobileNumber, codeGenerateDTO.EmailId, data);

                if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(data))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = RefreshTokenResource.CodeGenerationSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.CodeGenerationFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }

            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.CodeGenerationFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                _logger.LogError("{0} InSide GenerateOTP Method of TokenService --   Contact Number : {1}  Email-Id : {2}  Error {3}", DateTime.UtcNow, codeGenerateDTO.MobileNumber, codeGenerateDTO.EmailId, _APIResponse.Error);

            }


            return _APIResponse;
        }
		#endregion

		#region GetVerificationCodeForForgotPassword
		public async Task<APIResponse<string>> GetVerificationCodeForForgotPassword(string email)
		{
			APIResponse<string> _APIResponse = new APIResponse<string>();
			try
			{
				_logger.LogInformation("{0} InSide GetVerificationCodeForForgotPassword Method of TokenService Started--   Email-Id : {1} ", DateTime.UtcNow, email);

				var data = await _tokenRepository.GetVerificationCodeForForgotPassword(email);

				_logger.LogInformation("{0} InSide GetVerificationCodeForForgotPassword Method of TokenService End--   Email-Id : {1} data: {2} ", DateTime.UtcNow, email, data);

				if (await _unitOfWork.CommitAsync() > 0 || !string.IsNullOrEmpty(data)&&!data.Equals(LoginResource.UserNotExists))
				{
					_APIResponse.Success = true;
					_APIResponse.Data = data;
					_APIResponse.Message = RefreshTokenResource.CodeGenerationSuccess;
					_APIResponse.Status = HttpStatusCode.OK;
				}
				else
				{
					_APIResponse.Success = false;
					_APIResponse.Message = LoginResource.UserNotExists;
					_APIResponse.Status = HttpStatusCode.InternalServerError;
				}

			}
			catch (Exception ex)
			{
				_APIResponse.Success = false;
				_APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
				_APIResponse.Message = RefreshTokenResource.CodeGenerationFailed;
				_APIResponse.Status = HttpStatusCode.InternalServerError;
				_logger.LogError("{0} InSide GetVerificationCodeForForgotPassword Method of TokenService --   Email-Id : {1}  Error {2}", DateTime.UtcNow, email, _APIResponse.Error);

			}


			return _APIResponse;
		}
		#endregion

		public async Task<APIResponse<bool>> VerifyCode(RiderCodeVerifyDTO riderCodeVerifyDTO)
        {
            APIResponse<bool> _APIResponse = new APIResponse<bool>();
            try
            {
				_logger.LogInformation("{0} InSide VerifyCode Method of TokenService Started--   Contact Number : {1}  Hashcode : {2} OTP: {3}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber!, riderCodeVerifyDTO.HashCode!, riderCodeVerifyDTO.Otp!);
				var data = _tokenRepository.VerificationGeneratedCode(String.Concat( riderCodeVerifyDTO.CountryCode,riderCodeVerifyDTO.MobileNumber), riderCodeVerifyDTO.HashCode!, riderCodeVerifyDTO.Otp!);
				_logger.LogInformation("{0} InSide VerifyCode Method of TokenService End--   Contact Number : {1}  Hashcode : {2} OTP: {3}, Data:{4}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber!, riderCodeVerifyDTO.HashCode!, riderCodeVerifyDTO.Otp!, data);
				if (await _unitOfWork.CommitAsync() > 0 || data)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = RefreshTokenResource.CodeVerificationSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide VerifyCode Method of TokenService End--   Contact Number : {1}  Hashcode : {2} OTP: {3}, Data:{4} Message:{5}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber!, riderCodeVerifyDTO.HashCode!, riderCodeVerifyDTO.Otp!, data, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.CodeVerificationFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                    _logger.LogInformation("{0} InSide VerifyCode Method of TokenService End--   Contact Number : {1}  Hashcode : {2} OTP: {3}, Data:{4} Message:{5}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber!, riderCodeVerifyDTO.HashCode!, riderCodeVerifyDTO.Otp!, data, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.CodeVerificationFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

				_logger.LogError("{0} InSide GenerateOTP Method of TokenService --   Contact Number : {1}  HashCode : {2}  Error : {3}", DateTime.UtcNow, riderCodeVerifyDTO.MobileNumber!, riderCodeVerifyDTO.HashCode!, _APIResponse.Error);
			}


            return _APIResponse;
        }
        public async Task<APIResponse<bool>> ForgotPasswordVerifyCode(ForgotPasswordVerifyCodeDTO forgotPasswordVerify)
        {
            APIResponse<bool> _APIResponse = new APIResponse<bool>();
            try
            {
                _logger.LogInformation("{0} InSide ForgotPasswordVerifyCode Method of TokenService Started--   Email : {1}  Hashcode : {2} OTP: {3}", DateTime.UtcNow, forgotPasswordVerify.Email!, forgotPasswordVerify.HashCode!, forgotPasswordVerify.Otp!);
                var data = _tokenRepository.ForgotPasswordVerificationGeneratedCode(forgotPasswordVerify.Email!, forgotPasswordVerify.HashCode!, forgotPasswordVerify.Otp!);
                _logger.LogInformation("{0} InSide ForgotPasswordVerifyCode Method of TokenService End--   Email : {1}  Hashcode : {2} OTP: {3}, Data:{4}", DateTime.UtcNow, forgotPasswordVerify.Email!, forgotPasswordVerify.HashCode!, forgotPasswordVerify.Otp!, data);
                if (await _unitOfWork.CommitAsync() > 0 || data)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = RefreshTokenResource.CodeVerificationSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide ForgotPasswordVerifyCode Method of TokenService End--   Email : {1}  Hashcode : {2} OTP: {3}, Data:{4} Message:{5}", DateTime.UtcNow, forgotPasswordVerify.Email!, forgotPasswordVerify.HashCode!, forgotPasswordVerify.Otp!, data, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = RefreshTokenResource.CodeVerificationFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                    _logger.LogInformation("{0} InSide ForgotPasswordVerifyCode Method of TokenService End--   Email : {1}  Hashcode : {2} OTP: {3}, Data:{4} Message:{5}", DateTime.UtcNow, forgotPasswordVerify.Email!, forgotPasswordVerify.HashCode!, forgotPasswordVerify.Otp!, data, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.CodeVerificationFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                _logger.LogError("{0} InSide ForgotPasswordVerifyCode Method of TokenService --   Email : {1}  HashCode : {2}  Error : {3}", DateTime.UtcNow, forgotPasswordVerify.Email!, forgotPasswordVerify.HashCode!, _APIResponse.Error);
            }


            return _APIResponse;
        }
        public async Task<APIResponse<string>> SetNewPassword(string password,string email)
        {
            APIResponse<string> _APIResponse = new APIResponse<string>();
            try
            {
                _logger.LogInformation("{0} InSide SetNewPassword Method of TokenService Started--  ", DateTime.UtcNow);
                var data = await _tokenRepository.SetNewPassword(password,email);
                _logger.LogInformation("{0} InSide SetNewPassword Method of TokenService End--    data: {1}", DateTime.UtcNow,  data);
                if (await _unitOfWork.CommitAsync() > 0 || data!=null&&!data.Equals(LoginResource.SomthingWentWrong))
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = data;
                    _APIResponse.Message = GlobalResourceFile.PasswordChangedSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                    _logger.LogInformation("{0} InSide SetNewPassword Method of TokenService End--   data: {1} Message:{2}", DateTime.UtcNow, data, _APIResponse.Message);
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = LoginResource.SomthingWentWrong;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                    _logger.LogInformation("{0} InSide SetNewPassword Method of TokenService End--  data: {1} Message:{2}", DateTime.UtcNow,  data, _APIResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = RefreshTokenResource.CodeVerificationFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                _logger.LogError("{0} InSide SetNewPassword Method of TokenService --    Error : {1}", DateTime.UtcNow,  _APIResponse.Error);
            }


            return _APIResponse;
        }


    }
}
