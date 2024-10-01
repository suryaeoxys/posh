using Microsoft.AspNetCore.Mvc;
using OtpNet;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Token
{
    public interface ITokenRepository:IRepository<TokenInfoUser>
    {
        TokenResponse GetToken(IEnumerable<Claim> claim);
        public string GetRefreshToken();
        ClaimsPrincipal GetPrincipleFromExpiredToken(string token);
        public TokenInfoUser? GetUserFromTokenInfo(string username);
        TokenInfoUser SaveNewRefreshToken(TokenInfoUser tokenInfoUser);
        public string GenerateVerificationCode(string mobileNumber,string emailId);
        public bool VerificationGeneratedCode(string mobileNumber, string hashcode, string otp);
        public bool ForgotPasswordVerificationGeneratedCode(string email, string hashcode, string otp);
        Task<string> SetNewPassword(string password,string email);
        public Task<string> GetVerificationCodeForForgotPassword(string email);


	}
}
