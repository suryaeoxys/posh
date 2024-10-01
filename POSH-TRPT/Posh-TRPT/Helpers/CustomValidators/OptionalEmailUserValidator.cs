using Microsoft.AspNetCore.Identity;

namespace Posh_TRPT.Helpers.CustomValidators
{
	public class OptionalEmailUserValidator<TUser> : UserValidator<TUser> where TUser : class
	{
		public OptionalEmailUserValidator(IdentityErrorDescriber errors = null!) : base(errors)
		{
		}
		#region ValidateAsync
		/// <summary>
		/// Method to validate email
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var result = await base.ValidateAsync(manager, user);
            if (!result.Succeeded && string.IsNullOrWhiteSpace(await manager.GetEmailAsync(user)))
            {
                var errors = result.Errors.Where(e => e.Code != "InvalidEmail");
                result = errors.Count() > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
            }
            return result;
        }
        #endregion

    }
}
