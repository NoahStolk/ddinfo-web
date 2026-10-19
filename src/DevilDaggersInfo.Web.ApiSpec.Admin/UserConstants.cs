using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Web.ApiSpec.Admin;

[SuppressMessage("Security", "S2068:Credentials should not be hard-coded", Justification = "These are the password policy and its message, not a credential.")]
public static class UserConstants
{
	public const string PasswordRegex = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{12,40}$";

	public const string PasswordValidation =
		"Password must be at least 12 characters in length (max 40), contain at least one uppercase letter, " +
		"contain at least one lowercase letter, and contain at least one digit.";
}
