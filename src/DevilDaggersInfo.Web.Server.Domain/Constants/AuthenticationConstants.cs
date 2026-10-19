using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Web.Server.Domain.Constants;

// The request validation for this policy lives in the ApiSpec projects, which do not reference other projects. Keep it
// in sync with DevilDaggersInfo.Web.ApiSpec.Main.AuthenticationConstants and DevilDaggersInfo.Web.ApiSpec.Admin.UserConstants.
[SuppressMessage("Security", "S2068:Credentials should not be hard-coded", Justification = "This is the password policy, not a credential.")]
public static class AuthenticationConstants
{
	public const string PasswordRegex = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{12,40}$";
}
