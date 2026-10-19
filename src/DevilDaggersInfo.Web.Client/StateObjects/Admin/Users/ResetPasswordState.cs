using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Users;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Users;

public sealed class ResetPasswordState : IStateObject<ResetPassword>
{
	[RegularExpression(UserConstants.PasswordRegex, ErrorMessage = UserConstants.PasswordValidation)]
	public string NewPassword { get; set; } = string.Empty;

	public ResetPassword ToModel()
	{
		return new ResetPassword
		{
			NewPassword = NewPassword,
		};
	}
}
