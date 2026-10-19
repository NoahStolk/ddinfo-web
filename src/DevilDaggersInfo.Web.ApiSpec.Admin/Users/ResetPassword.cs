using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public sealed record ResetPassword
{
	[RegularExpression(UserConstants.PasswordRegex, ErrorMessage = UserConstants.PasswordValidation)]
	public required string NewPassword { get; init; }
}
