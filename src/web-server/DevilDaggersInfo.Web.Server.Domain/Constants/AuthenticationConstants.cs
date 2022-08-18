namespace DevilDaggersInfo.Web.Server.Domain.Constants;

public static class AuthenticationConstants
{
	public const string PasswordRegex = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{12,40}$";
}
